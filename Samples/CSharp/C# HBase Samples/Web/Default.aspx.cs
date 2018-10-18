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
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Syncfusion.ThriftHBase.Base;

namespace WebSampleBrowser
{
    public partial class Default : System.Web.UI.Page
    {
        string path;
        protected void Page_Load(object sender, EventArgs e)
        {
            path = string.Format("{0}\\..\\Data\\AdventureWorks\\AdventureWorks_Person_Contact.csv", Request.PhysicalPath.ToLower().Split(new string[] { "\\c# hbase samples" }, StringSplitOptions.None));
            BindDataSource();
        }
        private void BindDataSource()
        {
            ErrorMessage.InnerText = "";
          
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
                    this.FlatGrid.DataSource = table;
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
                catch(HBaseException ex)
                {
                    ErrorMessage.InnerText = ex.Message;
                }

        }
    }
    #region csv
    public class csv
    {
        #region Variable Declaration

        //Declare the object array to store the values
        private static object[,] cells;

        #endregion Variable Declaration

        #region Properties

        /// <summary>
        /// Get the RowCount of the Table
        /// </summary>
        public static int RowCount
        {
            get;
            private set;
        }

        /// <summary>
        /// Get the ColumnCount of the Table
        /// </summary>
        public static int ColumnCount
        {
            get;
            private set;
        }

        /// <summary>
        /// Get and set the ColumnNames of the Table
        /// </summary>
        public static string[] ColumnNames
        {
            get;
            internal set;
        }

        #endregion Properties

        public object[,] Table(string filepath, bool containsHeaderRow, char separator)
        {
            string[] Lines = File.ReadAllLines(filepath);
            RowCount = Lines.Length - 1;

            ColumnCount = Lines[0].Split(new[] { separator }).Length;
            //Initialize string array to add column names
            if (ColumnNames == null && containsHeaderRow)
                ColumnNames = new string[ColumnCount];
            //Parse the csv file
            cells = ReadCsv(filepath, separator, containsHeaderRow);
            return cells;
        }

        #region Method to read the data from CSV

        /// <summary>
        ///  Method to parse a CSV file
        /// </summary>
        /// <param name="filepath"> file path</param>
        /// <param name="separator"> separator to be used</param>
        /// <param name="header"> boolean header</param>

        private static object[,] ReadCsv(string filepath, char separator, bool header)
        {
            if (!string.IsNullOrEmpty(filepath) && File.Exists(filepath))
            {
                //Read file
                FileStream csvStream = new FileStream((filepath), FileMode.Open, FileAccess.Read);
                //Create stream reader
                StreamReader reader = new StreamReader(csvStream);

                //Flag to read column
                bool isColumn = header;
                //initialize row index
                int rowindex = 0;
                //Loop if not end of stream
                while (!reader.EndOfStream)
                {
                    //Read line
                    string line = reader.ReadLine();
                    //Split line using separator. Default is ","
                    string[] fields = line.Split(new[] { separator });
                    ColumnCount = fields.Length;

                    //Checks whether the cells is null and initializes if it is null
                    if (cells == null)
                    {
                        cells = new object[RowCount, ColumnCount];
                    }
                    //Take first row csv values as column names
                    if (isColumn)
                    {
                        //Add column names if header is true
                        if (header)
                        {
                            //Copy columnNames
                            for (int i = 0; i < ColumnCount; i++)
                            {
                                ColumnNames[i] = fields[i];
                            }
                        }
                        //Set flag to false to indicate columns are already copied
                        isColumn = false;
                    }
                    else
                    {
                        //Checks if the rowindex exceeds the cells maximum size limit
                        if (rowindex != RowCount)
                        {
                            for (int i = 0; i < ColumnCount; i++)
                                cells[rowindex, i] = fields[i];
                            rowindex++;
                        }
                    }
                }
            }
            return cells;
        }

        #endregion Method to read the data from CSV
    }

    #endregion reading csv file
}