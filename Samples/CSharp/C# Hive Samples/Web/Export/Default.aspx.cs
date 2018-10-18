#region Copyright Syncfusion Inc. 2001 - 2016
// Copyright Syncfusion Inc. 2001 - 2016. All rights reserved.
// Use of this code is subject to the terms of our license.
// A copy of the current license can be obtained at any time by e-mailing
// licensing@syncfusion.com. Any infringement will be prosecuted under
// applicable laws. 
#endregion
using Syncfusion.DocIO;
using Syncfusion.DocIO.DLS;
using Syncfusion.ThriftHive.Base;
using Syncfusion.XlsIO;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebSampleBrowser.Export
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            ErrorMessage.InnerText = "";
            if (hdnGroup.Value == "Word")
            {
                CheckFileStatus FileStatus = new CheckFileStatus();
                string path = string.Format("{0}\\..\\Data\\AdventureWorks\\AdventureWorks_Person_Contact.csv", Request.PhysicalPath.ToLower().Split(new string[] { "\\c# hive samples" }, StringSplitOptions.None));
            ErrorMessage.InnerText = FileStatus.CheckFile(path);
            if (ErrorMessage.InnerText == "")
            {
                try
                {
                    //Create a new document
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

                    //Create a character format for declaring font color and style for the text inside the cell
                    WCharacterFormat charFormat = new WCharacterFormat(document);
                    charFormat.TextColor = System.Drawing.Color.White;
                    charFormat.Bold = true;

                    //Initializing the hive server connection
                    HqlConnection con = new HqlConnection("localhost", 10000, HiveServer.HiveServer2);

                    //To initialize a Hive server connection with secured cluster
                    //HqlConnection con = new HqlConnection("<Secured cluster Namenode IP>", 10000, HiveServer.HiveServer2,"<username>","<password>");

                    //To initialize a Hive server connection with Azure cluster
                    //HqlConnection con = new HqlConnection("<FQDN name of Azure cluster>", 8004, HiveServer.HiveServer2,"<username>","<password>");

                    con.Open();

                    //Create table for adventure person contacts
                    HqlCommand createCommand = new HqlCommand("CREATE EXTERNAL TABLE IF NOT EXISTS AdventureWorks_Person_Contact(ContactID int,FullName string,Age int,EmailAddress string,PhoneNo string,ModifiedDate string) ROW FORMAT DELIMITED FIELDS TERMINATED BY ',' LOCATION '/Data/AdventureWorks'", con);
                    createCommand.ExecuteNonQuery();
                    HqlCommand command = new HqlCommand("Select * from AdventureWorks_Person_Contact", con);

                    //Executing the query
                    HqlDataReader reader = command.ExecuteReader();
                    reader.FetchSize = 100;

                    //Fetches the result from the reader and store it in HiveResultSet
                    HiveResultSet result = reader.FetchResult();

                    //Adding headertext for the table
                    doctable.AddRow(true, false);
                    //Creating new cell
                    WTableCell cell = new WTableCell(document);
                    cell.AddParagraph().AppendText("Customer Id").ApplyCharacterFormat(charFormat);
                    cell.Width = 75;
                    //Adding cell to the row
                    doctable.Rows[0].Cells.Add(cell);
                    cell = new WTableCell(document);
                    cell.AddParagraph().AppendText("Full Name").ApplyCharacterFormat(charFormat);
                    cell.Width = 75;
                    doctable.Rows[0].Cells.Add(cell);
                    cell = new WTableCell(document);
                    cell.AddParagraph().AppendText("Age").ApplyCharacterFormat(charFormat);
                    cell.Width = 75;
                    doctable.Rows[0].Cells.Add(cell);
                    cell = new WTableCell(document);
                    cell.AddParagraph().AppendText("Email Id").ApplyCharacterFormat(charFormat);
                    cell.Width = 75;
                    doctable.Rows[0].Cells.Add(cell);
                    cell = new WTableCell(document);
                    cell.AddParagraph().AppendText("Phone Number").ApplyCharacterFormat(charFormat);
                    cell.Width = 75;
                    doctable.Rows[0].Cells.Add(cell);
                    cell = new WTableCell(document);
                    cell.AddParagraph().AppendText("Modified Date").ApplyCharacterFormat(charFormat);
                    cell.Width = 75;
                    doctable.Rows[0].Cells.Add(cell);

                    //Reading each row from the fetched result
                    for (int i = 0; i < result.Count(); i++)
                    {
                        HiveRecord records = result[i];
                        doctable.AddRow(true, false);

                        //Reading each data from the row
                        for (int j = 0; j < records.Count; j++)
                        {
                            Object fields = records[j];

                            //Adding new cell to the document
                            cell = new WTableCell(document);

                            //Adding each data to the cell
                            cell.AddParagraph().AppendText(fields.ToString());
                            cell.Width = 75;

                            //Adding cell to the table
                            doctable.Rows[i + 1].Cells.Add(cell);
                            doctable.Rows[0].Cells[j].CellFormat.BackColor = Color.FromArgb(51, 153, 51);
                        }
                    }
                    //Adding table to the section
                    section.Tables.Add(doctable);
                    //Save as word 2007 format
                  if(rBtnWord2003.Checked == true)
                  {
                      document.Save("Sample.doc", FormatType.Doc, Response, HttpContentDisposition.Attachment);
                  }
                    else if (rBtnWord2007.Checked == true)
                    {
                        document.Save("Sample.docx", FormatType.Word2007, Response, HttpContentDisposition.Attachment);
                    }
                    //Save as word 2010 format
                    else if (rbtnWord2010.Checked == true)
                    {
                        document.Save("Sample.docx", FormatType.Word2010, Response, HttpContentDisposition.Attachment);
                    }
                    //Save as word 2013 format
                    else if (rbtnWord2013.Checked == true)
                    {
                        document.Save("Sample.docx", FormatType.Word2013, Response, HttpContentDisposition.Attachment);
                    }

                    //Closing the hive connection
                    con.Close();
                }
                catch (HqlConnectionException)
                {
                    ErrorMessage.InnerText = "Could not establish a connection to the HiveServer. Please run HiveServer2 from the Syncfusion service manager dashboard.";
                }
            }
            }
            else if (hdnGroup.Value == "Excel")
            {
                ErrorMessage.InnerText = "";
                CheckFileStatus FileStatus = new CheckFileStatus();
                string path = string.Format("{0}\\..\\Data\\AdventureWorks\\AdventureWorks_Person_Contact.csv", Request.PhysicalPath.ToLower().Split(new string[] { "\\c# hive samples" }, StringSplitOptions.None));
            ErrorMessage.InnerText = FileStatus.CheckFile(path);
            if (ErrorMessage.InnerText == "")
            {
                try
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
                    worksheet[1, 1].Text = "ContactID";
                    worksheet[1, 2].Text = "FullName";
                    worksheet[1, 3].Text = "Age";
                    worksheet[1, 4].Text = "EmailAddress";
                    worksheet[1, 5].Text = "PhoneNo";
                    worksheet[1, 6].Text = "ModifiedDate";

                    //Initializing the hive server connection
                    HqlConnection con = new HqlConnection("localhost", 10000, HiveServer.HiveServer2);

                    //To initialize a Hive server connection with secured cluster
                    //HqlConnection con = new HqlConnection("<Secured cluster Namenode IP>", 10000, HiveServer.HiveServer2,"<username>","<password>");

                    con.Open();

                    //Create table for adventure person contacts
                    HqlCommand createCommand = new HqlCommand("CREATE EXTERNAL TABLE IF NOT EXISTS AdventureWorks_Person_Contact(ContactID int,FullName string,Age int,EmailAddress string,PhoneNo string,ModifiedDate string) ROW FORMAT DELIMITED FIELDS TERMINATED BY ',' LOCATION '/Data/AdventureWorks'", con);
                    createCommand.ExecuteNonQuery();

                    //Passing the hive query
                    HqlCommand command = new HqlCommand("Select * from AdventureWorks_Person_Contact", con);

                    //Executing the query
                    HqlDataReader reader = command.ExecuteReader();
                    reader.FetchSize = 100;

                    //Fetches the result from the reader and store it in a seperate set
                    HiveResultSet result = reader.FetchResult();

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
                    worksheet.Range["A1:F1"].CellStyle.Font.Color = Syncfusion.XlsIO.ExcelKnownColors.White;
                    worksheet.Range["A1:F1"].CellStyle.Font.Bold = true;
                    worksheet.Range["A1:F1"].CellStyle.Color = System.Drawing.Color.FromArgb(51, 153, 51);
                    worksheet.UsedRange.AutofitColumns();

                    //Saving workbook on user relevant name
                    string fileName = "Sample.xlsx";

                    //conditions for selecting version
                    //Save as Excel 97to2003 format
                    if (rBtn2003.Checked == true)
                    {
                        workbook.Version = ExcelVersion.Excel97to2003;
                        workbook.SaveAs("Sample.xls", Response, ExcelDownloadType.PromptDialog);
                    }
                    //Save as Excel 2007 formt
                    else if (rBtn2007.Checked == true)
                    {
                        workbook.Version = ExcelVersion.Excel2007;
                        workbook.SaveAs("Sample.xlsx", Response, ExcelDownloadType.PromptDialog);
                    }
                    //Save as Excel 2010 format
                    else if (rbtn2010.Checked == true)
                    {
                        workbook.Version = ExcelVersion.Excel2010;
                        workbook.SaveAs("Sample.xlsx", Response, ExcelDownloadType.PromptDialog);
                    }
                    //Save as Excel 2013 format
                    else if (rbtn2013.Checked == true)
                    {
                        workbook.Version = ExcelVersion.Excel2013;
                        workbook.SaveAs("Sample.xlsx", Response, ExcelDownloadType.PromptDialog);
                    }

                    //closing the workwook
                    workbook.Close();

                    //Closing the excel engine
                    excelEngine.Dispose();


                    //Closing the hive connection
                    con.Close();
                }
                catch (HqlConnectionException)
                {
                    ErrorMessage.InnerText = "Could not establish a connection to the HiveServer. Please run HiveServer2 from the Syncfusion service manager dashboard.";
                }
            }
            }
        }
        
    }
}