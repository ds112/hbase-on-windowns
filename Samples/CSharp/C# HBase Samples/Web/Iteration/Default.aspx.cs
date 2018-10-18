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
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Syncfusion.ThriftHBase.Base;

namespace WebSampleBrowser.RichTextBox
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
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
                    

                    StringWriter stringWriter = new StringWriter();
                    //Htmlwriter for creating table to append the HiveResults
                    using (HtmlTextWriter writer = new HtmlTextWriter(stringWriter))
                    {
                        writer.AddAttribute(HtmlTextWriterAttribute.Border, "1");
                        writer.AddStyleAttribute(HtmlTextWriterStyle.BorderCollapse, "collapse");
                        writer.RenderBeginTag(HtmlTextWriterTag.Table);

                        writer.RenderBeginTag(HtmlTextWriterTag.Tr);

                        writer.RenderBeginTag(HtmlTextWriterTag.Th);
                        writer.Write("ContactId");
                        writer.RenderEndTag();

                        writer.RenderBeginTag(HtmlTextWriterTag.Th);
                        writer.Write("contact:EmailId");
                        writer.RenderEndTag();

                        writer.RenderBeginTag(HtmlTextWriterTag.Th);
                        writer.Write("contact:PhoneNo");
                        writer.RenderEndTag();

                        writer.RenderBeginTag(HtmlTextWriterTag.Th);
                        writer.Write("info:Age");
                        writer.RenderEndTag();

                        writer.RenderBeginTag(HtmlTextWriterTag.Th);
                        writer.Write("info:FullName");
                        writer.RenderEndTag();

                        writer.RenderBeginTag(HtmlTextWriterTag.Th);
                        writer.Write("others:ModifiedDate");
                        writer.RenderEndTag();
                                            
                        writer.RenderEndTag();
                        int count = table.Count();
                        for (int i = 0; i < count; i++)
                        {

                            writer.RenderBeginTag(HtmlTextWriterTag.Tr);
                            HBaseRecord records = table[i];

                            for (int j = 0; j < records.Count; j++)
                            {
                                Object fields = records[j];
                                writer.RenderBeginTag(HtmlTextWriterTag.Td);
                                writer.Write(fields);
                                writer.RenderEndTag();
                            }
                            writer.RenderEndTag();

                        }
                        writer.RenderEndTag();
                    }
                    //Binding the result to the RTE control
                    string results = stringWriter.ToString();
                    rteControl.RTEContent.InnerHtml = results;
                    #endregion scan values

                    #region close connection
                    //Closing the hive connection
                    con.Close();
                    #endregion close connection
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
       
    }
}