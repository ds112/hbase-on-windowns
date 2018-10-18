#region Copyright Syncfusion Inc. 2001 - 2016
// Copyright Syncfusion Inc. 2001 - 2016. All rights reserved.
// Use of this code is subject to the terms of our license.
// A copy of the current license can be obtained at any time by e-mailing
// licensing@syncfusion.com. Any infringement will be prosecuted under
// applicable laws. 
#endregion
using System;
using System.Collections.Generic;
using System.Text;
using Syncfusion.ThriftHive.Base;
using System.Data;
using System.Collections.ObjectModel;
using System.Collections;
using System.ComponentModel;
using System.Security.Permissions;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Syncfusion.Windows.Forms.Grid;
using Syncfusion.Windows.Forms;
using Syncfusion.Windows.Forms.Tools;
using Syncfusion.DocIO;
using Syncfusion.DocIO.DLS;
using Syncfusion.XlsIO;


namespace Export
{
    public partial class Form1 : MetroForm
    {
        //Declaring the variables used to perform hive operations
        public HqlConnection con;
        public HqlCommand command;
        public HqlDataReader reader;
        public HiveResultSet result;

        public Form1()
        {
            InitializeComponent();
           
        }
        public void InitializeComponenet()
        {

        }
       public void Form1_Load(object sender, EventArgs e)
        {
            rdbExcel.Checked = true;
            groupBox1.Visible = true;
            groupBox2.Visible = false;
            button1.Visible = true;
            try
            {
                //Initializing the hive server connection
                con = new HqlConnection("localhost", 10000, HiveServer.HiveServer2);

                //To initialize a Hive server connection with secured cluster
                //HqlConnection con = new HqlConnection("<Secured cluster Namenode IP>", 10000, HiveServer.HiveServer2,"<username>","<password>");

                //To initialize a Hive server connection with Azure cluster
                //HqlConnection con = new HqlConnection("<FQDN name of Azure cluster>", 8004, HiveServer.HiveServer2,"<username>","<password>");

                //Open the hive server connection
                con.Open();

                //Creating query to create AdventureWorks_Person_Contact table in Hive Database
                HqlCommand createCommand = new HqlCommand("create external table IF NOT EXISTS AdventureWorks_Person_Contact(contactid int,fullname string,age int,emailaddress string,phoneno string,modifieddate string) ROW FORMAT DELIMITED FIELDS TERMINATED BY ',' LOCATION '/Data/AdventureWorks'", con);
                createCommand.ExecuteNonQuery();

                //Creating query to fetch data from Hive Database
                command = new HqlCommand(" Select * from AdventureWorks_Person_Contact ", con);
                
                //Execute query to fetch data from Hive Database
                reader = command.ExecuteReader();

                //Assigning number of records to be fetched from Hive database
                reader.FetchSize = 1000;
                
                //Fetches the result from the reader and store it in a HiveResultSet
                result = reader.FetchResult();

                //Closing the hive connection
                con.Close();
            }
            catch (HqlConnectionException)
            {
                if (MessageBox.Show("Could not establish a connection to the HiveServer. Please run HiveServer2 from the Syncfusion service manager dashboard.", "Could not establish a connection to the HiveServer", MessageBoxButtons.OK, MessageBoxIcon.Warning) == DialogResult.OK)
                {
                    Application.Exit();
                }
            }
        }

        //Enabling the requirements used for excel
       private void rdbExcel_CheckedChanged(object sender, EventArgs e)
       {
           groupBox1.Visible = true;
           groupBox2.Visible = false;
           button1.Visible = true;
           this.groupBox1.Location = new System.Drawing.Point(25, 147);
       }

       //Enabling the requirements used for word
       private void rdbWord_CheckedChanged(object sender, EventArgs e)
       {
           groupBox1.Visible = false;
           groupBox2.Visible = true;
           button1.Visible = true;
           this.groupBox2.Location = new System.Drawing.Point(25, 147);
       }
       
       //Exporting data to Excel or Word based on condition
        private void button1_Click(object sender, EventArgs e)
        {
            if (rdbExcel.Checked)
                ExportToExcel();
            else if (rdbWord.Checked)
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

            worksheet[1, 1].Text = "Contact Id";
            worksheet[1, 2].Text = "Full Name";
            worksheet[1, 3].Text = "Age";
            worksheet[1, 4].Text = "Email Id";
            worksheet[1, 5].Text = "Phone Number";
            worksheet[1, 6].Text = "Modified Date";

            //Reading each row from the fetched result
            for (int i = 0; i < result.Count(); i++)
            {
                HiveRecord records = result[i];

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
            if (rdbExcel97.Checked)
            {
                fileName = "Sample.xls";
                workbook.SaveAs(fileName);
            }
            // Save as Excel 2007 format
            else if (rdbExcel2007.Checked)
            {
                fileName = "Sample.xlsx";
                workbook.Version = ExcelVersion.Excel2007;
                workbook.SaveAs(fileName);
            }
            //Save as Excel 2010 format
            else if (rdbExcel2010.Checked)
            {
                fileName = "Sample.xlsx";
                workbook.Version = ExcelVersion.Excel2010;
                workbook.SaveAs(fileName);
            }
            //Save as Excel 2013 format
            else if (rdbExcel2013.Checked)
            {
                fileName = "Sample.xlsx";
                workbook.Version = ExcelVersion.Excel2013;
                workbook.SaveAs(fileName);
            }
            //Save as CSV format
            else if (rdbCsv.Checked)
            {
                fileName = "Sample.csv";
                worksheet.SaveAs(fileName, ",");
            }

            //Close the workbook.
            workbook.Close();

            //Closing the excel engine
            excelEngine.Dispose();

            if (MessageBox.Show("Do you want to view the workbook?", "Workbook has been created",
                MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
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

            //Creating Paragraph styles
            WParagraphStyle style = document.AddParagraphStyle("Normal") as WParagraphStyle;
            style.CharacterFormat.FontName = "Calibri";
            style.CharacterFormat.FontSize = 11f;

            //Creating text styles in cell
            WCharacterFormat charFormat = new WCharacterFormat(document);
            charFormat.TextColor = System.Drawing.Color.White;
            charFormat.Bold = true;
            
            //Assigning each cell with a data value
            doctable.AddRow(true, false);
            WTableCell cell = new WTableCell(document);
            cell.AddParagraph().AppendText("Customer Id").ApplyCharacterFormat(charFormat);
            cell.Width = 90;
            doctable.Rows[0].Cells.Add(cell);
            cell = new WTableCell(document);
            cell.AddParagraph().AppendText("Full Name").ApplyCharacterFormat(charFormat);
            cell.Width = 90;
            doctable.Rows[0].Cells.Add(cell);
            cell = new WTableCell(document);
            cell.AddParagraph().AppendText("Age").ApplyCharacterFormat(charFormat);
            cell.Width = 90;
            doctable.Rows[0].Cells.Add(cell);
            cell = new WTableCell(document);
            cell.AddParagraph().AppendText("Email Id").ApplyCharacterFormat(charFormat);
            cell.Width = 180;
            doctable.Rows[0].Cells.Add(cell);
            cell = new WTableCell(document);
            cell.AddParagraph().AppendText("Phone Number").ApplyCharacterFormat(charFormat);
            cell.Width = 90;
            doctable.Rows[0].Cells.Add(cell);
            cell = new WTableCell(document);
            cell.AddParagraph().AppendText("Modified Date").ApplyCharacterFormat(charFormat);
            cell.Width = 90;
            doctable.Rows[0].Cells.Add(cell);

            //Reading each row from the fetched result
            for (int i = 0; i < result.Count(); i++)
            {
                HiveRecord records = result[i];
                doctable.AddRow(true, false);

                //Reading each field from the row
                for (int j = 0; j < records.Count; j++)
                {
                    Object fields = records[j];

                    //Adding new cell to the document
                    cell = new WTableCell(document);

                    //Adding each field to the cell
                    cell.AddParagraph().AppendText(fields.ToString());
                    if (j == 3)
                    {
                        cell.Width = 180;
                    }
                    else
                    {
                        cell.Width = 90;
                    }

                    //Adding cell to the table
                    doctable.Rows[i + 1].Cells.Add(cell);
                    doctable.Rows[0].Cells[j].CellFormat.BackColor = Color.FromArgb(51, 153, 51);
                }
            }

            //Adding table to the section
            section.Tables.Add(doctable);
            //Save as word 2003 format
            if (rdbWord2003.Checked)
            {
                //Saves the document in doc format.
                document.Save("Sample.doc");

                //Message box confirmation to view the created document.
                if (MessageBox.Show("Do you want to view the MS Word document?", "Document has been created", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                {
                    //Launching the MS Word file using the default Application.[MS Word Or Free WordViewer]
                    System.Diagnostics.Process.Start("Sample.doc");
                    //Exit
                    this.Close();
                }
            }
            //Save as word 2007 format
            else if (rdbWord2007.Checked)
            {
                //Saving the document as .docx
                document.Save("Sample.docx", FormatType.Word2007);
                //Message box confirmation to view the created document.
                if (MessageBox.Show("Do you want to view the MS Word document?", "Document has been created", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
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
            else if (rdbWord2010.Checked)
            {
                //Saving the document as .docx
                document.Save("Sample.docx", FormatType.Word2010);
                //Message box confirmation to view the created document.
                if (MessageBox.Show("Do you want to view the MS Word document?", "Document has been created", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
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
            else if (rdbWord2013.Checked)
            {
                //Saving the document as .docx
                document.Save("Sample.docx", FormatType.Word2013);
                //Message box confirmation to view the created document.
                if (MessageBox.Show("Do you want to view the MS Word document?", "Document has been created", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
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
      
    }
}
