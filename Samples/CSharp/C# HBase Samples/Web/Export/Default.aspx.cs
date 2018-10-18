#region Copyright Syncfusion Inc. 2001 - 2016
// Copyright Syncfusion Inc. 2001 - 2016. All rights reserved.
// Use of this code is subject to the terms of our license.
// A copy of the current license can be obtained at any time by e-mailing
// licensing@syncfusion.com. Any infringement will be prosecuted under
// applicable laws. 
#endregion
using Syncfusion.DocIO;
using Syncfusion.DocIO.DLS;
using Syncfusion.ThriftHBase.Base;
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

                string path = string.Format("{0}\\..\\Data\\AdventureWorks\\AdventureWorks_Person_Contact.csv", Request.PhysicalPath.ToLower().Split(new string[] { "\\c# hbase samples" }, StringSplitOptions.None));
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

                    //Adding headertext for the table
                    doctable.AddRow(true, false);
                    //Creating new cell
                    WTableCell cell = new WTableCell(document);
                    cell.AddParagraph().AppendText("ContactId").ApplyCharacterFormat(charFormat);
                    cell.Width = 100;
                    //Adding cell to the row
                    doctable.Rows[0].Cells.Add(cell);
                    cell = new WTableCell(document);
                    cell.AddParagraph().AppendText("contact:EmailId").ApplyCharacterFormat(charFormat);
                    cell.Width = 200;
                    doctable.Rows[0].Cells.Add(cell);
                    cell = new WTableCell(document);
                    cell.AddParagraph().AppendText("contact:PhoneNo").ApplyCharacterFormat(charFormat);
                    cell.Width = 150;
                    doctable.Rows[0].Cells.Add(cell);
                    cell = new WTableCell(document);
                    cell.AddParagraph().AppendText("info:Age").ApplyCharacterFormat(charFormat);
                    cell.Width = 100;
                    doctable.Rows[0].Cells.Add(cell);
                    cell = new WTableCell(document);
                    cell.AddParagraph().AppendText("info:FullName").ApplyCharacterFormat(charFormat);
                    cell.Width = 150;
                    doctable.Rows[0].Cells.Add(cell);
                    cell = new WTableCell(document);
                    cell.AddParagraph().AppendText("others:ModifiedDate").ApplyCharacterFormat(charFormat);
                    cell.Width = 200;
                    doctable.Rows[0].Cells.Add(cell);
                   

                    //Reading each row from the fetched result
                    for (int i = 0; i < table.Count(); i++)
                    {
                        HBaseRecord records = table[i];
                        doctable.AddRow(true, false);

                        //Reading each data from the row
                        for (int j = 0; j < records.Count; j++)
                        {
                            Object fields = records[j];

                            //Adding new cell to the document
                            cell = new WTableCell(document);

                            //Adding each data to the cell
                            cell.AddParagraph().AppendText(fields.ToString());
                            if (j != 1 && j != 2 && j != 4 && j != 5)
                                cell.Width = 100;
                            else if (j == 2 || j == 4)
                                cell.Width = 150;
                            else
                                cell.Width = 200;

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
                    #endregion scan values

                  #region close connection
                  //Closing the hive connection
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
            
            else if (hdnGroup.Value == "Excel")
            {

                string path = string.Format("{0}\\..\\Data\\AdventureWorks\\AdventureWorks_Person_Contact.csv", Request.PhysicalPath.ToLower().Split(new string[] { "\\c# hbase samples" }, StringSplitOptions.None));
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
                    worksheet[1, 1].Text = "ContactId";
                    worksheet[1, 2].Text = "contact:EmailId";
                    worksheet[1, 3].Text = "contact:PhoneNo";
                    worksheet[1, 4].Text = "info:Age";
                    worksheet[1, 5].Text = "info:FullName";
                    worksheet[1, 6].Text = "others:ModifiedDate";
                   

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

                    //Reading each row from the fetched result
                    for (int i = 0; i < table.Count(); i++)
                    {
                        HBaseRecord records = table[i];

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
                    #endregion scan values

                    //Closing the hive connection
                    con.Close();
                }
                catch (HBaseConnectionException)
                {
                    ErrorMessage.InnerText = "Could not establish a connection to the HBase Server. Please run HBase Server from the Syncfusion service manager dashboard.";
                }
                catch (HBaseException ex)
                {
                    ErrorMessage.InnerText = ex.Message;
                }
            
            }
        }
        
    }
}