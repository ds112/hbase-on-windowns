#region Copyright Syncfusion Inc. 2001 - 2016

// Copyright Syncfusion Inc. 2001 - 2016. All rights reserved.
// Use of this code is subject to the terms of our license.
// A copy of the current license can be obtained at any time by e-mailing
// licensing@syncfusion.com. Any infringement will be prosecuted under
// applicable laws.

#endregion Copyright Syncfusion Inc. 2001 - 2016

using Syncfusion.ThriftHBase.Base;
using Syncfusion.Windows.Tools.Controls;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Documents;

namespace Iteration
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private HBaseConnection con;
        private HBaseResultSet result;

        public MainWindow()
        {
            InitializeComponent();
            try
            {
                string path = System.AppDomain.CurrentDomain.BaseDirectory;
                path = path + "..\\..\\..\\..\\..\\..\\..\\..\\Data\\AdventureWorks\\AdventureWorks_Person_Contact.csv";

                #region Creating connection

                con = new HBaseConnection("localhost", 10003);
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

                #endregion Creating table

                #region Inserting Values

                #region Parsing csv input file

                csv csvObj = new csv();
                object[,] cells = csvObj.Table(path, false, ',');

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

                //Fetches the result from the reader and store it in a object
                HBaseOperation.FetchSize = 100;
                result = HBaseOperation.ScanTable(tableName, con);

                //Closing the HBase connection
                con.Close();
            }
            catch (HBaseConnectionException)
            {
                if (MessageBox.Show("Could not establish a connection to the HBase thrift server. Please run HBase thrift server from the Syncfusion service manager dashboard.", "Could not establish a connection to the HBase thrift server", MessageBoxButton.OK, MessageBoxImage.Warning) == MessageBoxResult.OK)
                {
                    Application.Current.Shutdown();
                }
            }
            catch (HBaseException hbase_ex)
            {
                if (MessageBox.Show(hbase_ex.Message.ToString(), "HBaseException", MessageBoxButton.OK, MessageBoxImage.Warning) == MessageBoxResult.OK)
                {
                    Application.Current.Shutdown();
                }
            }
            catch (Exception ex)
            {
                if (MessageBox.Show(ex.Message.ToString(), "Exception", MessageBoxButton.OK, MessageBoxImage.Warning) == MessageBoxResult.OK)
                {
                    Application.Current.Shutdown();
                }
            }
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            //Creating new table
            TableAdv table = new TableAdv();
            table.BorderThickness = 0;
            //Creating a row
            TableRowAdv row = new TableRowAdv();
            //Creating cell
            TableCellAdv cell = new TableCellAdv();

            #region Adding the header text

            ParagraphAdv paragraph = new ParagraphAdv();
            paragraph.BeforeSpacing = 5;
            SpanAdv span = new SpanAdv();
            span.FontWeight = FontWeights.Bold;
            span.FontSize = 10;
            span.Text = "CONTACT ID";
            paragraph.Inlines.Add(span);
            cell.Blocks.Add(paragraph);
            row.Cells.Add(cell);

            cell = new TableCellAdv();
            paragraph = new ParagraphAdv();
            paragraph.BeforeSpacing = 5;
            span = new SpanAdv();
            span.FontWeight = FontWeights.Bold;
            span.FontSize = 10;
            span.Text = "info:FULLNAME";
            paragraph.Inlines.Add(span);
            cell.Blocks.Add(paragraph);
            row.Cells.Add(cell);

            cell = new TableCellAdv();
            paragraph = new ParagraphAdv();
            paragraph.BeforeSpacing = 5;
            span = new SpanAdv();
            span.FontWeight = FontWeights.Bold;
            span.FontSize = 10;
            span.Text = "info:AGE";
            paragraph.Inlines.Add(span);
            cell.Blocks.Add(paragraph);
            row.Cells.Add(cell);

            cell = new TableCellAdv();
            paragraph = new ParagraphAdv();
            paragraph.BeforeSpacing = 5;
            span = new SpanAdv();
            span.FontWeight = FontWeights.Bold;
            span.FontSize = 10;
            span.Text = "contact:EMAILID";
            paragraph.Inlines.Add(span);
            cell.Blocks.Add(paragraph);
            row.Cells.Add(cell);

            cell = new TableCellAdv();
            paragraph = new ParagraphAdv();
            paragraph.BeforeSpacing = 5;
            span = new SpanAdv();
            span.FontWeight = FontWeights.Bold;
            span.FontSize = 10;
            span.Text = "contact:PHONE";
            paragraph.Inlines.Add(span);
            cell.Blocks.Add(paragraph);
            row.Cells.Add(cell);

            cell = new TableCellAdv();
            paragraph = new ParagraphAdv();
            paragraph.BeforeSpacing = 5;
            span = new SpanAdv();
            span.FontWeight = FontWeights.Bold;
            span.FontSize = 10;
            span.Text = "others:MODIFIEDDATE";
            paragraph.Inlines.Add(span);
            cell.Blocks.Add(paragraph);
            row.Cells.Add(cell);

            table.Rows.Add(row);

            #endregion Adding the header text

            //Reading each rows from the fetched result
            foreach (HBaseRecord rows in result)
            {
                //Creating new row
                row = new TableRowAdv();

                //Reading each data from the rows
                foreach (Object fields in rows)
                {
                    //Creating a Cell
                    cell = new TableCellAdv();

                    //Creating a paragraph
                    paragraph = new ParagraphAdv();
                    paragraph.BeforeSpacing = 5;
                    string records = fields.ToString();
                    span = new SpanAdv() { Text = records };
                    span.FontSize = 8;
                    paragraph.Inlines.Add(span);

                    //Adding field value to cell
                    cell.Blocks.Add(paragraph);

                    //Adding the cell to row
                    row.Cells.Add(cell);
                }

                //Adding the row to table
                table.Rows.Add(row);
            }

            //Adding  table to section
            richTextBox1.Document.Sections[0].Blocks.Add(table);
            richTextBox1.UpdateEditorLayout();

            //Adding Scrollbar to RichTextEditor
            richTextBox1.VerticalScrollBarVisibility = true;
            richTextBox1.HorizontalScrollBarVisibility = true;
        }

        private void Minimize(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void Maximize(object sender, RoutedEventArgs e)
        {
            btnRestore.Visibility = Visibility.Visible;
            btnMaximize.Visibility = Visibility.Collapsed;
            this.WindowState = WindowState.Maximized;
        }

        private void Restore(object sender, RoutedEventArgs e)
        {
            btnRestore.Visibility = Visibility.Collapsed;
            btnMaximize.Visibility = Visibility.Visible;
            this.WindowState = WindowState.Normal;
        }

        private void Close(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (this.WindowState == WindowState.Maximized)
            {
                textBlock1.Width = 1100;
                btnRestore.Visibility = Visibility.Visible;
                btnMaximize.Visibility = Visibility.Collapsed;
                this.WindowState = WindowState.Maximized;
            }
            else if (this.WindowState == WindowState.Normal)
            {
                textBlock1.Width = 705;
                btnRestore.Visibility = Visibility.Collapsed;
                btnMaximize.Visibility = Visibility.Visible;
                this.WindowState = WindowState.Normal;
            }
        }
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