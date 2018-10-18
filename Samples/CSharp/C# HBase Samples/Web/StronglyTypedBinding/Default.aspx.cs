#region Copyright Syncfusion Inc. 2001 - 2016
// Copyright Syncfusion Inc. 2001 - 2016. All rights reserved.
// Use of this code is subject to the terms of our license.
// A copy of the current license can be obtained at any time by e-mailing
// licensing@syncfusion.com. Any infringement will be prosecuted under
// applicable laws. 
#endregion
using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Syncfusion.ThriftHBase.Base;

namespace WebSampleBrowser.StronglyTypedView
{
    public partial class Default : System.Web.UI.Page
    {
        public class Customers
        {
            public string ContactId { get; set; }
            public string FullName { get; set; }
            public string Age { get; set; }
            public string EmailId { get; set; }
            public string PhoneNumber { get; set; }
            public string ModifiedDate { get; set; }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            BindDataSource();
        }
        private void BindDataSource()
        {
            
            ErrorMessage.InnerText = "";
            string path = string.Format("{0}\\..\\Data\\AdventureWorks\\AdventureWorks_Person_Contact.csv", Request.PhysicalPath.ToLower().Split(new string[] { "\\c# hbase samples" }, StringSplitOptions.None));
           
            try
                {
                    #region creating connection

                    HBaseConnection con = new HBaseConnection("localhost", 10003);
                    con.Open();

                    #endregion creating connection

                    #region parsing csv input file

                    csv csvObj = new csv();
                    object[,] cells;
                    cells = null;

                    cells = csvObj.Table(path, false, ',');

                    #endregion parsing csv input file

                    #region creating table
                    String tableName = "AdventureWorks_Person_Contact";
                    List<string> columnFamilies = new List<string>();
                    columnFamilies.Add("info");
                    columnFamilies.Add("contact");
                    columnFamilies.Add("others");
                    if (!HBaseOperation.IsTableExists(tableName, con))
                    {
                        if (columnFamilies.Count > 0)
                        {
                            HBaseOperation.CreateTable(tableName, columnFamilies, con);
                        }
                        else
                        {
                            throw new HBaseException("ERROR: Table must have at least one column family");
                        }
                    }
                    # endregion

                    #region Inserting Values
                    string[] column = new string[] { "CONTACTID", "FULLNAME", "AGE", "EMAILID", "PHONE", "MODIFIEDDATE" };
                    Dictionary<string, IList<HMutation>> rowCollection = new Dictionary<string, IList<HMutation>>();
                    string rowKey;
                    for (int i = 0; i < cells.GetLength(0); i++)
                    {
                        List<HMutation> mutations = new List<HMutation>();
                        rowKey = cells[i, 0].ToString();
                        for (int j = 1; j < column.Length; j++)
                        {
                            HMutation mutation = new HMutation();
                            mutation.ColumnFamily = j < 3 ? "info" : j < 5 ? "contact" : "others";
                            mutation.ColumnName = column[j];
                            mutation.Value = cells[i, j].ToString();
                            mutations.Add(mutation);
                        }
                        rowCollection[rowKey] = mutations;
                    }
                    HBaseOperation.InsertRows(tableName, rowCollection, con);
                    #endregion Inserting Values

                    #region scan values

                    HBaseOperation.FetchSize = 100;
                    HBaseResultSet table = HBaseOperation.ScanTable(tableName, con);

                    //Initialize the list to add elements in each row
                    BindingList<Customers> resultList = new BindingList<Customers>();

                    //Read each row from the fetched result
                    foreach (HBaseRecord rows in table)
                    {
                        //Adding element of each row to the list
                        resultList = new BindingList<Customers>(table.Select(row => new Customers
                        {
                            ContactId = row["rowKey"]!=null?row["rowKey"].ToString():"",
                            FullName = row["info:FULLNAME"] != null ? row["info:FULLNAME"].ToString() : "",
                            Age = row["info:AGE"] != null ? row["info:AGE"].ToString() : "",
                            EmailId = row["contact:EMAILID"] != null ? row["contact:EMAILID"].ToString() : "",
                            PhoneNumber = row["contact:PHONE"] != null ? row["contact:PHONE"].ToString() : "",
                            ModifiedDate = row["others:MODIFIEDDATE"] != null ? row["others:MODIFIEDDATE"].ToString() : "",                            
                        }).ToList());
                    }
                    //Binding the result to the grid
                    this.FlatGrid.DataSource = resultList;
                    this.FlatGrid.DataBind();
                    #endregion scan values

                    #region close connection
                    //Closing the hbase connection
                    con.Close();
                    #endregion close connection
                }
                catch (HBaseConnectionException)
                {
                    ErrorMessage.InnerText = "Could not establish a connection to the HBaseServer. Please run HBaseServer from the Syncfusion service manager dashboard.";
                }
            catch (HBaseException ex)
            {
                ErrorMessage.InnerText = ex.Message;
            }

        }
    }
}