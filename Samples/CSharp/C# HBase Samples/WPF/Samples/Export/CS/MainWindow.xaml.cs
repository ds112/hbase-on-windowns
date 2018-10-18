#region Copyright Syncfusion Inc. 2001 - 2016

// Copyright Syncfusion Inc. 2001 - 2016. All rights reserved.
// Use of this code is subject to the terms of our license.
// A copy of the current license can be obtained at any time by e-mailing
// licensing@syncfusion.com. Any infringement will be prosecuted under
// applicable laws.

#endregion Copyright Syncfusion Inc. 2001 - 2016

using Syncfusion.DocIO;
using Syncfusion.DocIO.DLS;
using Syncfusion.ThriftHBase.Base;
using Syncfusion.XlsIO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Documents;

namespace Export
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public HBaseConnection con;
        public HBaseResultSet result;

        public MainWindow()
        {
            InitializeComponent();
            groupBox1.Visibility = Visibility.Visible;
            groupBox2.Visibility = Visibility.Hidden;
            button1.Visibility = Visibility.Visible;

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

                # endregion

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

                //Assiging number of rows to be fetched from HBase Database
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

        //Enabling the requirements used for excel
        private void rdbWord_Unchecked(object sender, RoutedEventArgs e)
        {
            groupBox1.Visibility = Visibility.Visible;
            groupBox2.Visibility = Visibility.Hidden;
            button1.Visibility = Visibility.Visible;
        }

        //Enabling the requirements used for word
        private void rdbWord_Checked(object sender, RoutedEventArgs e)
        {
            groupBox1.Visibility = Visibility.Hidden;
            groupBox2.Visibility = Visibility.Visible;
            button1.Visibility = Visibility.Visible;
        }

        //Exporting data to Excel or Word based on condition
        private void button1_Click(object sender, RoutedEventArgs e)
        {
            if (rdbExcel.IsChecked.Value)
                ExportToExcel();
            else if (rdbWord.IsChecked.Value)
                ExportToWord();
        }

        //Exporting data to excel
        public void ExportToExcel()
        {
            //Instantiate the spreadsheet creation engine.
            ExcelEngine excelEngine = new ExcelEngine();

            //Instantiate the excel application object.
            IApplication application = excelEngine.Excel;

            //A new workbook is created.[Equivalent to creating a new workbook in MS Excel]
            //The new workbook will have 1 worksheets
            IWorkbook workbook = application.Workbooks.Create(1);

            //The first worksheet object in the worksheets collection is accessed.
            IWorksheet worksheet = workbook.Worksheets[0];

            //Adding header text for worksheet
            worksheet[1, 1].Text = "CONTACT ID";
            worksheet[1, 2].Text = "contact:EMAILID";
            worksheet[1, 3].Text = "contact:PHONE";
            worksheet[1, 4].Text = "info:AGE";
            worksheet[1, 5].Text = "info:FULLNAME";
            worksheet[1, 6].Text = "others:MODIFIEDDATE";

            //Reading each row from the fetched result
            for (int i = 0; i < result.Count(); i++)
            {
                HBaseRecord records = result[i];

                //Reading each field from the row
                for (int j = 0; j < records.Count; j++)
                {
                    Object fields = records[j];
                    //Assigning each field value to the worksheet based on index
                    worksheet[i + 2, j + 1].Text = fields.ToString();
                }
            }
            worksheet.Range["A1:F1"].CellStyle.Color = System.Drawing.Color.FromArgb(51, 153, 51);
            worksheet.Range["A1:F1"].CellStyle.Font.Color = Syncfusion.XlsIO.ExcelKnownColors.White;
            worksheet.Range["A1:F1"].CellStyle.Font.Bold = true;
            worksheet.UsedRange.AutofitColumns();

            string fileName = "";

            //Save as Excel 97 format
            if (rdbExcel97.IsChecked.Value)
            {
                fileName = "Sample.xls";
                workbook.SaveAs(fileName);
            }
            // Save as Excel 2007 format
            else if (rdbExcel2007.IsChecked.Value)
            {
                fileName = "Sample.xlsx";
                workbook.Version = ExcelVersion.Excel2007;
                workbook.SaveAs(fileName);
            }
            //Save as Excel 2010 format
            else if (rdbExcel2010.IsChecked.Value)
            {
                fileName = "Sample.xlsx";
                workbook.Version = ExcelVersion.Excel2010;
                workbook.SaveAs(fileName);
            }
            //Save as Excel 2013 format
            else if (rdbExcel2013.IsChecked.Value)
            {
                fileName = "Sample.xlsx";
                workbook.Version = ExcelVersion.Excel2013;
                workbook.SaveAs(fileName);
            }
            //Save as CSV format
            else if (rdbCsv.IsChecked.Value)
            {
                fileName = "Sample.csv";
                worksheet.SaveAs(fileName, ",");
            }

            //Close the workbook.
            workbook.Close();

            //Closing the excel engine
            excelEngine.Dispose();

            if (MessageBox.Show("Do you want to view the workbook?", "Workbook has been created",
                MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes)
            {
                try
                {
                    //Launching the Excel file using the default Application.[MS Excel Or Free ExcelViewer]
                    System.Diagnostics.Process.Start(fileName);

                    //Exit
                    this.Close();
                }
                catch (Win32Exception ex)
                {
                    MessageBox.Show("Ms Excel is not installed in this system");
                    Console.WriteLine(ex.ToString());
                }
            }
        }

        //Exporting data to word
        public void ExportToWord()
        {
            //A new document is created.
            WordDocument document = new WordDocument();

            //Adding new table to the document
            WTable doctable = new WTable(document);

            //Adding a new section to the document.
            WSection section = document.AddSection() as WSection;

            //Set Margin of the section
            section.PageSetup.Margins.All = 72;

            //Set page size of the section
            section.PageSetup.PageSize = new SizeF(800, 792);

            //Create Paragraph styles
            WParagraphStyle style = document.AddParagraphStyle("Normal") as WParagraphStyle;
            style.CharacterFormat.FontName = "Calibri";
            style.CharacterFormat.FontSize = 11f;

            doctable.AddRow(true, false);

            //creating new cell
            WTableCell cell = new WTableCell(document);

            //Create a character format for declaring font color and style for the text inside the cell
            WCharacterFormat charFormat = new WCharacterFormat(document);
            charFormat.TextColor = System.Drawing.Color.White;
            charFormat.Bold = true;

            //Adding header text for the table
            cell.AddParagraph().AppendText("CONTACT ID").ApplyCharacterFormat(charFormat);
            cell.Width = 100;
            cell.CellFormat.BackColor = System.Drawing.Color.FromArgb(51, 153, 51);
            //Adding cell to rows
            doctable.Rows[0].Cells.Add(cell);

            cell = new WTableCell(document);
            cell.AddParagraph().AppendText("contact:EMAILID").ApplyCharacterFormat(charFormat);
            cell.Width = 200;
            cell.CellFormat.BackColor = System.Drawing.Color.FromArgb(51, 153, 51);
            doctable.Rows[0].Cells.Add(cell);

            cell = new WTableCell(document);
            cell.AddParagraph().AppendText("contact:PHONE").ApplyCharacterFormat(charFormat);
            cell.Width = 150;
            cell.CellFormat.BackColor = System.Drawing.Color.FromArgb(51, 153, 51);
            doctable.Rows[0].Cells.Add(cell);

            cell = new WTableCell(document);
            cell.AddParagraph().AppendText("info:AGE").ApplyCharacterFormat(charFormat);
            cell.Width = 100;
            cell.CellFormat.BackColor = System.Drawing.Color.FromArgb(51, 153, 51);
            doctable.Rows[0].Cells.Add(cell);

            cell = new WTableCell(document);
            cell.AddParagraph().AppendText("info:FULLNAME").ApplyCharacterFormat(charFormat);
            cell.Width = 150;
            cell.CellFormat.BackColor = System.Drawing.Color.FromArgb(51, 153, 51);
            doctable.Rows[0].Cells.Add(cell);

            cell = new WTableCell(document);
            cell.AddParagraph().AppendText("others:MODIFIEDDATE").ApplyCharacterFormat(charFormat);
            cell.Width = 200;
            cell.CellFormat.BackColor = System.Drawing.Color.FromArgb(51, 153, 51);
            doctable.Rows[0].Cells.Add(cell);

            //Reading each row from the fetched result
            for (int i = 0; i < result.Count(); i++)
            {
                HBaseRecord records = result[i];
                doctable.AddRow(true, false);

                //Reading each field from the row
                for (int j = 0; j < records.Count; j++)
                {
                    Object fields = records[j];

                    //Adding new cell to the document
                    cell = new WTableCell(document);

                    //Adding each field to the cell
                    cell.AddParagraph().AppendText(fields.ToString());
                    if (j != 1 && j != 2 && j != 4 && j != 5)
                        cell.Width = 100;
                    else if (j == 4 || j==2)
                        cell.Width = 150;
                    else
                        cell.Width = 200;
                   

                    //Adding cell to the table
                    doctable.Rows[i + 1].Cells.Add(cell);
                    doctable.Rows[0].Cells[j].CellFormat.BackColor = System.Drawing.Color.FromArgb(51, 153, 51);
                }
            }

            //Adding table to the section
            section.Tables.Add(doctable);
            //Save as word 2003 format
            if (rdbWord2003.IsChecked.Value)
            {
                //Saving the document to disk.
                document.Save("Sample.doc");

                //Message box confirmation to view the created document.
                if (MessageBox.Show("Do you want to view the MS Word document?", "Document has been created", MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes)
                {
                    //Launching the MS Word file using the default Application.[MS Word Or Free WordViewer]
                    System.Diagnostics.Process.Start("Sample.doc");
                    //Exit
                    this.Close();
                }
            }
            //Save as word 2007 format
            else if (rdbWord2007.IsChecked.Value)
            {
                //Saving the document as .docx
                document.Save("Sample.docx", FormatType.Word2007);
                //Message box confirmation to view the created document.
                if (MessageBox.Show("Do you want to view the MS Word document?", "Document has been created", MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes)
                {
                    try
                    {
                        //Launching the MS Word file using the default Application.[MS Word Or Free WordViewer]
                        System.Diagnostics.Process.Start("Sample.docx");
                        //Exit
                        this.Close();
                    }
                    catch (Win32Exception ex)
                    {
                        MessageBox.Show("Word 2007 is not installed in this system");
                        Console.WriteLine(ex.ToString());
                    }
                }
            }
            //Save as word 2010  format
            else if (rdbWord2010.IsChecked.Value)
            {
                //Saving the document as .docx
                document.Save("Sample.docx", FormatType.Word2010);
                //Message box confirmation to view the created document.
                if (MessageBox.Show("Do you want to view the MS Word document?", "Document has been created", MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes)
                {
                    try
                    {
                        //Launching the MS Word file using the default Application.[MS Word Or Free WordViewer]
                        System.Diagnostics.Process.Start("Sample.docx");
                        //Exit
                        this.Close();
                    }
                    catch (Win32Exception ex)
                    {
                        MessageBox.Show("Word 2010 is not installed in this system");
                        Console.WriteLine(ex.ToString());
                    }
                }
            }
            //Save as word 2013  format
            else if (rdbWord2013.IsChecked.Value)
            {
                //Saving the document as .docx
                document.Save("Sample.docx", FormatType.Word2013);
                //Message box confirmation to view the created document.
                if (MessageBox.Show("Do you want to view the MS Word document?", "Document has been created", MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes)
                {
                    try
                    {
                        //Launching the MS Word file using the default Application.[MS Word Or Free WordViewer]
                        System.Diagnostics.Process.Start("Sample.docx");
                        //Exit
                        this.Close();
                    }
                    catch (Win32Exception ex)
                    {
                        MessageBox.Show("Word 2013 is not installed in this system");
                        Console.WriteLine(ex.ToString());
                    }
                }
            }
            else
            {
                // Exit
                this.Close();
            }
        }

        private void Minimize(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void Maximize(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Maximized;
        }

        private void Restore(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Normal;
        }

        private void Close(object sender, RoutedEventArgs e)
        {
            this.Close();
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