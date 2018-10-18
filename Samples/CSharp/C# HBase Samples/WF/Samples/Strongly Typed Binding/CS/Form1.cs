#region Copyright Syncfusion Inc. 2001 - 2016

// Copyright Syncfusion Inc. 2001 - 2016. All rights reserved.
// Use of this code is subject to the terms of our license.
// A copy of the current license can be obtained at any time by e-mailing
// licensing@syncfusion.com. Any infringement will be prosecuted under
// applicable laws.

#endregion Copyright Syncfusion Inc. 2001 - 2016

using Syncfusion.ThriftHBase.Base;
using Syncfusion.Windows.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace StronglyTypedBinding
{
    public partial class Form1 : MetroForm
    {
        public Form1()
        {
            InitializeComponent();
        }

        public void InitializeComponenet()
        {
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                #region Creating connection

                HBaseConnection con = new HBaseConnection("localhost", 10003);
                con.Open();

                #endregion Creating connection

                #region Creating table

                string tableName = "AdventureWorks_Person_Contact";
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

                #region Parsing csv input file

                csv csvObj = new csv();
                object[,] cells;
                cells = null;
                string path = System.AppDomain.CurrentDomain.BaseDirectory;
                cells = csvObj.Table(path + "..\\..\\..\\..\\..\\..\\..\\..\\Data\\AdventureWorks\\AdventureWorks_Person_Contact.csv", false, ',');

                #endregion Parsing csv input file

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

                #region Fetch result

                HBaseOperation.FetchSize = 100;
                HBaseResultSet result = HBaseOperation.ScanTable(tableName, con);

                #endregion Fetch result

                BindingList<adventureworks_person_contact> resultList = new BindingList<adventureworks_person_contact>();

                //Read each row from the fetched result
                foreach (HBaseRecord row in result)
                {
                    resultList = new BindingList<adventureworks_person_contact>(result.Select(rowvalue => new adventureworks_person_contact
                    {
                        CONTACTID = rowvalue["rowKey"].ToString(),
                        FULLNAME = rowvalue["info:FULLNAME"] != null ? rowvalue["info:FULLNAME"].ToString() : "",
                        AGE = rowvalue["info:AGE"] != null ? rowvalue["info:AGE"].ToString() : "",
                        EMAILID = rowvalue["contact:EMAILID"] != null ? rowvalue["contact:EMAILID"].ToString() : "",
                        PHONE = rowvalue["contact:PHONE"] != null ? rowvalue["contact:PHONE"].ToString() : "",
                        MODIFIEDDATE = rowvalue["others:MODIFIEDDATE"] != null ? rowvalue["others:MODIFIEDDATE"].ToString() : "",
                    }).ToList());
                }

                //Binding the result to the grid
                gridGroupingControl1.DataSource = resultList;
                gridGroupingControl1.TableDescriptor.Columns["CONTACTID"].Width = 85;
                gridGroupingControl1.TableDescriptor.Columns["FULLNAME"].Width = 170;
                gridGroupingControl1.TableDescriptor.Columns["AGE"].Width = 90;
                gridGroupingControl1.TableDescriptor.Columns["EMAILID"].Width = 230;
                gridGroupingControl1.TableDescriptor.Columns["PHONE"].Width = 170;
                gridGroupingControl1.TableDescriptor.Columns["MODIFIEDDATE"].Width = 170;
                //Closing the HBase connection
                con.Close();
            }
            catch (HBaseConnectionException)
            {
                if (MessageBox.Show("Could not establish a connection to the HBase thrift server. Please run HBase thrift server from the Syncfusion service manager dashboard.", "Could not establish a connection to the HBase thrift server", MessageBoxButtons.OK, MessageBoxIcon.Warning) == DialogResult.OK)
                {
                    Application.Exit();
                }
            }
            catch (HBaseException hbase_ex)
            {
                if (MessageBox.Show(hbase_ex.Message.ToString(), "HBaseException", MessageBoxButtons.OK, MessageBoxIcon.Warning) == DialogResult.OK)
                {
                    Application.Exit();
                }
            }
            catch (Exception ex)
            {
                if (MessageBox.Show(ex.Message.ToString(), "Exception", MessageBoxButtons.OK, MessageBoxIcon.Warning) == DialogResult.OK)
                {
                    Application.Exit();
                }
            }
        }
    }

    public class adventureworks_person_contact
    {
        public string CONTACTID { get; set; }
        public string FULLNAME { get; set; }
        public string AGE { get; set; }
        public string EMAILID { get; set; }
        public string PHONE { get; set; }
        public string MODIFIEDDATE { get; set; }
    }

    #region reading csv file

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