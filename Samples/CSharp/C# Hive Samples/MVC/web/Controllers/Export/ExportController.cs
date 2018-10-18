#region Copyright Syncfusion Inc. 2001 - 2016
// Copyright Syncfusion Inc. 2001 - 2016. All rights reserved.
// Use of this code is subject to the terms of our license.
// A copy of the current license can be obtained at any time by e-mailing
// licensing@syncfusion.com. Any infringement will be prosecuted under
// applicable laws. 
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Syncfusion.XlsIO;
using System.Drawing;
using System.Data;
using Syncfusion.DocIO;
using Syncfusion.DocIO.DLS;
using MVCSampleBrowser.Models;
using Syncfusion.ThriftHive.Base;
namespace MVCSampleBrowser.Controllers
{
    public class ExportController : Controller
    {
        public static string ErrorMessage
        { get; set; }
        #region Getting Started
        public ActionResult ExportDefault(string SaveOption)
        {
            if (SaveOption == null)
            {
                ErrorMessage = "";
                return View();
            }
            else if (SaveOption == "Excel 2013" || SaveOption == "Excel 2010" || SaveOption == "Excel 2007" || SaveOption == "Excel 2003" || SaveOption == "CSV")
            {
                ErrorMessage = "";
                string path = new System.IO.DirectoryInfo(Request.PhysicalPath + "..\\..\\..\\..\\..\\..\\..\\Data\\AdventureWorks\\AdventureWorks_Person_Contact.csv").FullName;
                CheckFileStatus CheckFile = new CheckFileStatus();
                ErrorMessage = CheckFile.CheckFile(path);
                if (ErrorMessage == "")
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
                        var result = CustomersData.list();
                        ErrorMessage = null;

                        //Adding header text for worksheet
                        worksheet[1, 1].Text = "Contact Id";
                        worksheet[1, 2].Text = "Full Name";
                        worksheet[1, 3].Text = "Age";
                        worksheet[1, 4].Text = "Phone Number";
                        worksheet[1, 5].Text = "Email Id";
                        worksheet[1, 6].Text = "Modified Date";

                        int i = 1;
                        //Reading each row from the fetched result
                        foreach (PersonDetail records in result)
                        {
                            //Reading each data from the row
                            //Assigning each data to the worksheet based on index
                            worksheet[i + 1, 1].Text = records.ContactId;
                            worksheet[i + 1, 2].Text = records.FullName;
                            worksheet[i + 1, 3].Text = records.Age;
                            worksheet[i + 1, 4].Text = records.PhoneNumber;
                            worksheet[i + 1, 5].Text = records.EmailId;
                            worksheet[i + 1, 6].Text = records.ModifiedDate;
                            i++;
                        }
                        //Assigning Header text with cell color
                        worksheet.Range["A1:F1"].CellStyle.Color = System.Drawing.Color.FromArgb(51, 153, 51);
                        worksheet.Range["A1:F1"].CellStyle.Font.Color = Syncfusion.XlsIO.ExcelKnownColors.White;
                        worksheet.Range["A1:F1"].CellStyle.Font.Bold = true;
                        worksheet.UsedRange.AutofitColumns();
        #endregion

                        //Save as .xls format
                        if (SaveOption == "Excel 2003")
                        {
                            return excelEngine.SaveAsActionResult(workbook, "Sample.xls", HttpContext.ApplicationInstance.Response, ExcelDownloadType.PromptDialog, ExcelHttpContentType.Excel97);
                        }
                        //Save as .xlsx format
                        else if (SaveOption == "Excel 2007")
                        {
                            workbook.Version = ExcelVersion.Excel2007;
                            return excelEngine.SaveAsActionResult(workbook, "Sample.xlsx", HttpContext.ApplicationInstance.Response, ExcelDownloadType.PromptDialog, ExcelHttpContentType.Excel2007);
                        }
                        //Save as .xlsx format
                        else if (SaveOption == "Excel 2010")
                        {
                            workbook.Version = ExcelVersion.Excel2010;
                            return excelEngine.SaveAsActionResult(workbook, "Sample.xlsx", HttpContext.ApplicationInstance.Response, ExcelDownloadType.PromptDialog, ExcelHttpContentType.Excel2010);
                        }
                        //Save as .xlsx format
                        else if (SaveOption == "Excel 2013")
                        {
                            workbook.Version = ExcelVersion.Excel2013;
                            return excelEngine.SaveAsActionResult(workbook, "Sample.xlsx", HttpContext.ApplicationInstance.Response, ExcelDownloadType.PromptDialog, ExcelHttpContentType.Excel2013);
                        }
                        //Save as .csv format
                        else if (SaveOption == "CSV")
                        {

                            return excelEngine.SaveAsActionResult(workbook, "Sample.csv", ",", HttpContext.ApplicationInstance.Response, ExcelDownloadType.PromptDialog, ExcelHttpContentType.CSV);
                        }
                        //Close the workbook
                        workbook.Close();

                        //Close the excelengine
                        excelEngine.Dispose();
                    }
                    catch (HqlConnectionException)
                    {
                        ErrorMessage = "Could not establish a connection to the HiveServer. Please run HiveServer2 from the Syncfusion service manager dashboard.";

                    }
                }

                return View();
            }
            else
            {
                ErrorMessage = "";
                string path = new System.IO.DirectoryInfo(Request.PhysicalPath + "..\\..\\..\\..\\..\\..\\..\\Data\\AdventureWorks\\AdventureWorks_Person_Contact.csv").FullName;
                CheckFileStatus CheckFile = new CheckFileStatus();
                ErrorMessage = CheckFile.CheckFile(path);
                //A new document is created.
                if (ErrorMessage == "")
                {
                    try
                    {
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

                        //Reading the data from the table
                        var result = CustomersData.list();
                        doctable.AddRow(true, false);

                        //creating new cell
                        WTableCell cell = new WTableCell(document);

                        //Create a character format for declaring font color and style for the text inside the cell
                        WCharacterFormat charFormat = new WCharacterFormat(document);
                        charFormat.TextColor = System.Drawing.Color.White;
                        charFormat.Bold = true;

                        //Adding header text for the table
                        cell.AddParagraph().AppendText("Customer Id").ApplyCharacterFormat(charFormat);
                        cell.Width = 75;
                        cell.CellFormat.BackColor = System.Drawing.Color.FromArgb(51, 153, 51);
                        //Adding cell to rows
                        doctable.Rows[0].Cells.Add(cell);
                        cell = new WTableCell(document);
                        cell.AddParagraph().AppendText("Full Name").ApplyCharacterFormat(charFormat);
                        cell.Width = 90;
                        cell.CellFormat.BackColor = System.Drawing.Color.FromArgb(51, 153, 51);
                        doctable.Rows[0].Cells.Add(cell);
                        cell = new WTableCell(document);
                        cell.AddParagraph().AppendText("Age").ApplyCharacterFormat(charFormat);
                        cell.Width = 75;
                        cell.CellFormat.BackColor = System.Drawing.Color.FromArgb(51, 153, 51);
                        doctable.Rows[0].Cells.Add(cell);
                        cell = new WTableCell(document);
                        cell.AddParagraph().AppendText("Phone Number").ApplyCharacterFormat(charFormat);
                        cell.Width = 90;
                        cell.CellFormat.BackColor = System.Drawing.Color.FromArgb(51, 153, 51);
                        doctable.Rows[0].Cells.Add(cell);
                        cell = new WTableCell(document);
                        cell.AddParagraph().AppendText("Email Id").ApplyCharacterFormat(charFormat);
                        cell.Width = 180;
                        cell.CellFormat.BackColor = System.Drawing.Color.FromArgb(51, 153, 51);
                        doctable.Rows[0].Cells.Add(cell);
                        cell = new WTableCell(document);
                        cell.AddParagraph().AppendText("Modified Date").ApplyCharacterFormat(charFormat);
                        cell.Width = 125;
                        cell.CellFormat.BackColor = System.Drawing.Color.FromArgb(51, 153, 51);
                        doctable.Rows[0].Cells.Add(cell);

                        int i = 1;
                        //Reading each row from the fetched result 
                        foreach (PersonDetail records in result)
                        {
                            doctable.AddRow(true, false);

                            //Assigning fetched result to the cell
                            cell = new WTableCell(document);
                            cell.AddParagraph().AppendText(records.ContactId.ToString());
                            cell.Width = 75;
                            doctable.Rows[i].Cells.Add(cell);
                            cell = new WTableCell(document);
                            cell.AddParagraph().AppendText(records.FullName.ToString());
                            cell.Width = 90;
                            doctable.Rows[i].Cells.Add(cell);
                            cell = new WTableCell(document);
                            cell.AddParagraph().AppendText(records.Age.ToString());
                            cell.Width = 75;
                            doctable.Rows[i].Cells.Add(cell);
                            cell = new WTableCell(document);
                            cell.AddParagraph().AppendText(records.PhoneNumber.ToString());
                            cell.Width = 90;
                            doctable.Rows[i].Cells.Add(cell);
                            cell = new WTableCell(document);
                            cell.AddParagraph().AppendText(records.EmailId.ToString());
                            cell.Width = 180;
                            doctable.Rows[i].Cells.Add(cell);
                            cell = new WTableCell(document);
                            cell.AddParagraph().AppendText(records.ModifiedDate.ToString());
                            cell.Width = 125;
                            doctable.Rows[i].Cells.Add(cell);

                            i++;
                        }
                        //Adding table to the section
                        section.Tables.Add(doctable);

                        #region saveOption
                        //Save as .doc Word 97-2003 format
                        if (SaveOption == "Word 97-2003")
                        {
                            return document.ExportAsActionResult("Sample.doc", FormatType.Doc, HttpContext.ApplicationInstance.Response, HttpContentDisposition.Attachment);
                        }
                        //Save as .docx Word 2007 format
                        else if (SaveOption == "Word 2007")
                        {
                            return document.ExportAsActionResult("Sample.docx", FormatType.Word2007, HttpContext.ApplicationInstance.Response, HttpContentDisposition.Attachment);
                        }
                        //Save as .docx Word 2010 format
                        else if (SaveOption == "Word 2010")
                        {
                            return document.ExportAsActionResult("Sample.docx", FormatType.Word2010, HttpContext.ApplicationInstance.Response, HttpContentDisposition.Attachment);
                        }
                        //Save as .docx Word 2013 format
                        else if (SaveOption == "Word 2013")
                        {
                            return document.ExportAsActionResult("Sample.docx", FormatType.Word2013, HttpContext.ApplicationInstance.Response, HttpContentDisposition.Attachment);
                        }

                        #endregion saveOption
                    }

                    catch (HqlConnectionException)
                    {
                        ErrorMessage = "Could not establish a connection to the HiveServer. Please run HiveServer2 from the Syncfusion service manager dashboard.";

                    }
                }
                return View();
            }
        }
        protected string ResolveApplicationDataPath(string fileName, string folderName)
        {
            string dataPath = new System.IO.DirectoryInfo(Request.PhysicalApplicationPath).FullName;
            if (folderName != string.Empty)
                dataPath += folderName;
            return string.Format("{0}\\{1}", dataPath, fileName);
        }
    }

    #region ExcelResult
    public class ExcelResult : ActionResult
    {
        private IWorkbook m_source;
        private ExcelEngine m_engine;
        private string m_filename;
        private HttpResponse m_response;
        private ExcelDownloadType m_downloadType;
        private ExcelHttpContentType m_contentType;
        private string m_separator;
        public string FileName
        {
            get
            {
                return m_filename;
            }
            set
            {
                m_filename = value;
            }
        }
        public IWorkbook Source
        {
            get
            {
                return m_source as IWorkbook;
            }
        }
        public ExcelEngine Engine
        {
            get
            {
                return m_engine as ExcelEngine;
            }
        }
        public HttpResponse Response
        {
            get
            {
                return m_response;
            }
        }
        public ExcelDownloadType DownloadType
        {
            set
            {
                m_downloadType = value;
            }
            get
            {
                return m_downloadType;
            }
        }
        public ExcelHttpContentType ContentType
        {
            set
            {
                m_contentType = value;
            }
            get
            {
                return m_contentType;
            }
        }
        public string Separator
        {
            set
            {
                m_separator = value;
            }
            get
            {
                return m_separator;
            }
        }
        public ExcelResult(ExcelEngine engine, IWorkbook source, string fileName, HttpResponse response, ExcelDownloadType downloadType, ExcelHttpContentType contentType)
        {
            this.FileName = fileName;
            this.m_source = source;
            this.m_engine = engine;
            m_response = response;
            DownloadType = downloadType;
            ContentType = contentType;
        }
        public ExcelResult(ExcelEngine engine, IWorkbook source, string fileName, string separator, HttpResponse response, ExcelDownloadType downloadType, ExcelHttpContentType contentType)
        {
            this.FileName = fileName;
            this.m_source = source;
            this.m_engine = engine;
            m_response = response;
            DownloadType = downloadType;
            ContentType = contentType;
            Separator = separator;
        }
        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
                throw new ArgumentNullException("Context");
            if (m_contentType == ExcelHttpContentType.CSV)
            {
                this.m_source.SaveAs(FileName, Separator, Response, DownloadType, ContentType);
                this.m_source.Close();
                this.m_engine.Dispose();
            }
            else
            {
                this.m_source.SaveAs(FileName, Response, DownloadType, ContentType);
                this.m_source.Close();
                this.m_engine.Dispose();
            }
        }
    }
    #endregion
    #region XlsIOExtension
    public static class XlsIOExtension
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="_workbook"></param>
        /// <param name="filename"></param>
        /// <param name="response"></param>
        /// <returns></returns>
        public static ExcelResult SaveAsActionResult(this ExcelEngine _engine, IWorkbook _workbook, string filename, HttpResponse response)
        {
            ExcelHttpContentType contentType = ExcelHttpContentType.Excel2007;
            if (_workbook.Version == ExcelVersion.Excel2007)
                contentType = ExcelHttpContentType.Excel2007;
            else if (_workbook.Version == ExcelVersion.Excel97to2003)
                contentType = ExcelHttpContentType.Excel2000;
            return new ExcelResult(_engine, _workbook, filename, response, ExcelDownloadType.PromptDialog, contentType);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="_workbook"></param>
        /// <param name="filename"></param>
        /// <param name="response"></param>
        /// <param name="DownloadType"></param>
        /// <returns></returns>
        public static ExcelResult SaveAsActionResult(this ExcelEngine _engine, IWorkbook _workbook, string filename, HttpResponse response, ExcelDownloadType DownloadType)
        {
            ExcelHttpContentType contentType = ExcelHttpContentType.Excel2007;
            if (_workbook.Version == ExcelVersion.Excel2007)
                contentType = ExcelHttpContentType.Excel2007;
            else if (_workbook.Version == ExcelVersion.Excel97to2003)
                contentType = ExcelHttpContentType.Excel2000;
            return new ExcelResult(_engine, _workbook, filename, response, DownloadType, contentType);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="_workbook"></param>
        /// <param name="filename"></param>
        /// <param name="response"></param>
        /// <param name="contentType"></param>
        /// <returns></returns>
        public static ExcelResult SaveAsActionResult(this ExcelEngine _engine, IWorkbook _workbook, string filename, HttpResponse response, ExcelHttpContentType contentType)
        {
            return new ExcelResult(_engine, _workbook, filename, response, ExcelDownloadType.PromptDialog, contentType);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="_workbook"></param>
        /// <param name="filename"></param>
        /// <param name="response"></param>
        /// <param name="DownloadType"></param>
        /// <param name="contentType"></param>
        /// <returns></returns>
        public static ExcelResult SaveAsActionResult(this ExcelEngine _engine, IWorkbook _workbook, string filename, HttpResponse response, ExcelDownloadType DownloadType, ExcelHttpContentType contentType)
        {
            return new ExcelResult(_engine, _workbook, filename, response, DownloadType, contentType);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="_workbook"></param>
        /// <param name="filename"></param>
        /// <param name="saveType"></param>
        /// <param name="response"></param>
        /// <param name="DownloadType"></param>
        /// <param name="contentType"></param>
        /// <returns></returns>
        public static ExcelResult SaveAsActionResult(this ExcelEngine _engine, IWorkbook _workbook, string filename, ExcelSaveType saveType, HttpResponse response, ExcelDownloadType DownloadType, ExcelHttpContentType contentType)
        {
            return new ExcelResult(_engine, _workbook, filename, response, DownloadType, contentType);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="_workbook"></param>
        /// <param name="filename"></param>
        /// <param name="saveType"></param>
        /// <param name="response"></param>
        /// <param name="DownloadType"></param>
        /// <param name="contentType"></param>
        /// <returns></returns>
        public static ExcelResult SaveAsActionResult(this ExcelEngine _engine, IWorkbook _workbook, string filename, string separator, HttpResponse response, ExcelDownloadType DownloadType, ExcelHttpContentType contentType)
        {
            return new ExcelResult(_engine, _workbook, filename, separator, response, DownloadType, contentType);
        }
    }


    #region DocumentResult
    /// <summary>
    /// This Class represents the Custom ActionResult for WordDocument.
    /// </summary>
    public class DocumentResult : ActionResult
    {
        #region Fields
        private string m_filename;
        private IWordDocument m_document;
        private FormatType m_formatType;
        private HttpResponse m_response;
        private HttpContentDisposition m_contentDisposition;
        #endregion Fields
        #region Properties
        /// <summary>
        /// Gets/Sets the Name of the file.
        /// </summary>
        /// <value>Name of the file</value>
        public string FileName
        {
            get
            {
                return m_filename;
            }
            set
            {
                m_filename = value;
            }
        }
        /// <summary>
        /// Gets the WordDocument
        /// </summary>
        /// <value>The WordDocument</value>
        public IWordDocument Document
        {
            get
            {
                if (m_document != null)
                    return m_document;
                return null;
            }
        }
        /// <summary>
        /// Gets/Sets the Format Type
        /// </summary>
        /// <value>The FormatType</value>
        public FormatType formatType
        {
            get
            {
                return m_formatType;
            }
            set
            {
                m_formatType = value;
            }
        }
        /// <summary>
        /// Gets/Sets the type of ContentDisposition
        /// </summary>
        /// <value>The type of the ContentDisposition.</value>
        public HttpContentDisposition ContentDisposition
        {
            get
            {
                return m_contentDisposition;
            }
            set
            {
                m_contentDisposition = value;
            }
        }
        /// <summary>
        /// Gets the response
        /// </summary>
        /// <value>The Response.</value>
        public HttpResponse Response
        {
            get
            {
                return m_response;
            }
        }
        #endregion Properties
        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentResult"/> class.
        /// </summary>
        /// <param name="document">The Word Document</param>
        /// <param name="filename">The Filename</param>
        /// <param name="formattype">The FormatType</param>
        /// <param name="response">The Resposne</param>
        /// <param name="contentDisposition">The Type of ContentDisposition</param>
        public DocumentResult(IWordDocument document, string filename, FormatType formattype, HttpResponse response, HttpContentDisposition contentDisposition)
        {
            FileName = filename;
            m_document = document;
            this.formatType = formattype;
            m_response = response;
            this.ContentDisposition = contentDisposition;
        }
        #endregion Constructor
        #region Implementation
        /// <summary>
        /// Executes the result.
        /// </summary>
        /// <param name="context">The Context.</param>
        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
                throw new ArgumentNullException("Context");
            this.Document.Save(FileName, formatType, Response, ContentDisposition);
        }
        #endregion Implementation
    }
    #endregion
    #region DocIOExtension
    /// <summary>
    /// DocIO Extension
    /// </summary>
    public static class DocIOExtension
    {
        /// <summary>
        /// Export the document as ActionResult, returns the DocResult
        /// </summary>
        /// <param name="document">WordDocument to serialize</param>
        /// <param name="filename">Name of the File</param>
        /// <param name="formattype">Format type of the document</param>
        /// <param name="response">Response</param>
        /// <param name="contentDisposition">HttpContentDisposition</param>
        /// <returns></returns>
        public static DocumentResult ExportAsActionResult(this IWordDocument document, string filename, FormatType formattype, HttpResponse response, HttpContentDisposition contentDisposition)
        {
            return new DocumentResult(document, filename, formattype, response, contentDisposition);
        }
    }
}
#endregion



    #endregion