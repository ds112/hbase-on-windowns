#region Copyright Syncfusion Inc. 2001 - 2016
// Copyright Syncfusion Inc. 2001 - 2016. All rights reserved.
// Use of this code is subject to the terms of our license.
// A copy of the current license can be obtained at any time by e-mailing
// licensing@syncfusion.com. Any infringement will be prosecuted under
// applicable laws. 
#endregion
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using Syncfusion.JavaScript;
using Syncfusion.ThriftHBase.Base;

namespace WebSampleBrowser.Asynchronous_Grid_Paging
{
    public partial class Default : System.Web.UI.Page
    {

        public class DataResult
        {
            public IEnumerable result { get; set; }
            public int count { get; set; }
            public string hasRows { get; set; }
        }

        //Declaring the variables used to perform hive query operation.
        static Dictionary<int, DataResult> DataCollection = new Dictionary<int, DataResult>();       
        static private int page = 0;
        static int Skip = -1;
        static HBaseConnection con = null;
        static string error = null;
        private static string tableName = string.Empty;

            protected void Page_Load(object sender, EventArgs e)
            {
                string path = string.Format("{0}\\..\\Data\\AdventureWorks\\AdventureWorks_Person_Contact.csv", Request.PhysicalPath.ToLower().Split(new string[] { "\\c# hbase samples" }, StringSplitOptions.None));
               
               try
               {
                #region creating connection
                con = new HBaseConnection("localhost", 10003);
                con.Open();
                #endregion creating connection

                #region parsing csv input file
                csv csvObj = new csv();
                object[,] cells;
                cells = null;
                cells = csvObj.Table(path, false, ',');

                #endregion parsing csv input file

                #region creating table
                tableName = "asyncHbaseCustomer3";
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
                        error = "ERROR: Table must have at least one column family";
                        throw new HBaseException(error);
                    }
                }
               
                # endregion

                #region Inserting Values
                string[] column = new string[] { "CONTACTID", "FULLNAME", "AGE", "EMAILID", "PHONE", "MODIFIEDDATE"};
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

                        page = 0;
                        Skip = -1;

                        DataCollection = new Dictionary<int, DataResult>();
                    }
                    catch (HBaseConnectionException)
                    {
                        ErrorMessage.InnerText = "Could not establish a connection to the HiveServer. Please run HiveServer2 from the Syncfusion service manager dashboard.";
                    }
               catch (HBaseException ex)
               {
                   ErrorMessage.InnerText = ex.Message;
               }
             }

            [WebMethod]
            [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
            public static object DataSource(int skip, int take)
            {
                DataResult resultSet = new DataResult();
                if (error == null)
                { 
                    if (Skip < skip)
                    {
                        //Incrementing the page after the required amount of data is stored
                        page = page + 1;
                        if (DataCollection.ContainsKey(page))
                        {
                            resultSet = DataCollection[page];
                        }
                        else
                        {
                            var DataSource = CustomerData.list(tableName, con);
                            resultSet.result = DataSource;
                            resultSet.count = DataSource.Count;
                            //Adds the page number and the data to the dictionary
                            DataCollection.Add(page, resultSet);
                            if (!HBaseOperation.HasRows)
                            {
                                resultSet.hasRows = "Nill";
                            }
                        }
                    }
                    //View the data stored in previous page
                    else
                    {
                        page = page - 1;
                        resultSet = DataCollection[page];

                    }
                    Skip = skip;
                }
                return resultSet;
            }

        }

    }




