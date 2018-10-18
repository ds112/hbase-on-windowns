#region Copyright Syncfusion Inc. 2001 - 2016
// Copyright Syncfusion Inc. 2001 - 2016. All rights reserved.
// Use of this code is subject to the terms of our license.
// A copy of the current license can be obtained at any time by e-mailing
// licensing@syncfusion.com. Any infringement will be prosecuted under
// applicable laws. 
#endregion
using System;
using System.Collections.Generic;
using System.IO;
using System.Globalization;
using System.Windows.Forms;
using System.Data;
using Syncfusion.Windows.Forms.Grid.Grouping;
using Syncfusion.Windows.Forms;
using System.ComponentModel;
using System.Collections.ObjectModel;
using Syncfusion.ThriftHBase.Base;
namespace Asynchronous
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class Form1 : MetroForm
    {
        private Label label1;
        private GridGroupingControl _gridGroupingControl2;
        private static HBaseConnection con = null;
        private Panel commonLoaderPicPanel;
        private PictureBox commonLoaderPic;
        private static string tableName = string.Empty;

	    private Form1()
		{
            InitializeComponent();
		}

	    #region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this._gridGroupingControl2 = new Syncfusion.Windows.Forms.Grid.Grouping.GridGroupingControl();
            this.label1 = new System.Windows.Forms.Label();
            this.commonLoaderPicPanel = new System.Windows.Forms.Panel();
            this.commonLoaderPic = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this._gridGroupingControl2)).BeginInit();
            this.commonLoaderPicPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.commonLoaderPic)).BeginInit();
            this.SuspendLayout();
            // 
            // _gridGroupingControl2
            // 
            this._gridGroupingControl2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._gridGroupingControl2.BackColor = System.Drawing.SystemColors.Window;
            this._gridGroupingControl2.FreezeCaption = false;
            this._gridGroupingControl2.GridOfficeScrollBars = Syncfusion.Windows.Forms.OfficeScrollBars.Metro;
            this._gridGroupingControl2.GridVisualStyles = Syncfusion.Windows.Forms.GridVisualStyles.Metro;
            this._gridGroupingControl2.Location = new System.Drawing.Point(12, 59);
            this._gridGroupingControl2.Name = "_gridGroupingControl2";
            this._gridGroupingControl2.ShowGroupDropArea = true;
            this._gridGroupingControl2.Size = new System.Drawing.Size(956, 322);
            this._gridGroupingControl2.TabIndex = 0;
            this._gridGroupingControl2.TableDescriptor.AllowEdit = false;
            this._gridGroupingControl2.TableDescriptor.AllowNew = false;
            this._gridGroupingControl2.TableDescriptor.Appearance.AnyCell.Font.Facename = "Segoe UI";
            this._gridGroupingControl2.TableDescriptor.Appearance.AnyCell.TextColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this._gridGroupingControl2.TableDescriptor.Appearance.AnyGroupCell.Borders.Bottom = new Syncfusion.Windows.Forms.Grid.GridBorder(Syncfusion.Windows.Forms.Grid.GridBorderStyle.Solid, System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(234)))), ((int)(((byte)(234))))), Syncfusion.Windows.Forms.Grid.GridBorderWeight.ExtraThin);
            this._gridGroupingControl2.TableDescriptor.Appearance.AnyGroupCell.Borders.Right = new Syncfusion.Windows.Forms.Grid.GridBorder(Syncfusion.Windows.Forms.Grid.GridBorderStyle.Solid, System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(234)))), ((int)(((byte)(234))))), Syncfusion.Windows.Forms.Grid.GridBorderWeight.ExtraThin);
            this._gridGroupingControl2.TableDescriptor.Appearance.AnyGroupCell.Interior = new Syncfusion.Drawing.BrushInfo(System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(235)))), ((int)(((byte)(235))))));
            this._gridGroupingControl2.TableDescriptor.Appearance.AnyGroupCell.TextColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this._gridGroupingControl2.TableDescriptor.Appearance.AnyRecordFieldCell.Borders.Bottom = new Syncfusion.Windows.Forms.Grid.GridBorder(Syncfusion.Windows.Forms.Grid.GridBorderStyle.Solid, System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(234)))), ((int)(((byte)(234))))), Syncfusion.Windows.Forms.Grid.GridBorderWeight.ExtraThin);
            this._gridGroupingControl2.TableDescriptor.Appearance.AnyRecordFieldCell.Borders.Right = new Syncfusion.Windows.Forms.Grid.GridBorder(Syncfusion.Windows.Forms.Grid.GridBorderStyle.Solid, System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(234)))), ((int)(((byte)(234))))), Syncfusion.Windows.Forms.Grid.GridBorderWeight.ExtraThin);
            this._gridGroupingControl2.TableDescriptor.Appearance.AnySummaryCell.Borders.Right = new Syncfusion.Windows.Forms.Grid.GridBorder(Syncfusion.Windows.Forms.Grid.GridBorderStyle.Solid, System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208))))), Syncfusion.Windows.Forms.Grid.GridBorderWeight.ExtraThin);
            this._gridGroupingControl2.TableDescriptor.Appearance.AnySummaryCell.Borders.Top = new Syncfusion.Windows.Forms.Grid.GridBorder(Syncfusion.Windows.Forms.Grid.GridBorderStyle.Solid, System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208))))), Syncfusion.Windows.Forms.Grid.GridBorderWeight.ExtraThin);
            this._gridGroupingControl2.TableDescriptor.Appearance.AnySummaryCell.Interior = new Syncfusion.Drawing.BrushInfo(System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208))))));
            this._gridGroupingControl2.TableDescriptor.Appearance.ColumnHeaderCell.Font.Bold = true;
            this._gridGroupingControl2.TableDescriptor.Appearance.GroupCaptionCell.CellType = "ColumnHeader";
            this._gridGroupingControl2.TableDescriptor.Name = "Contacts";
            this._gridGroupingControl2.TableDescriptor.TableOptions.ColumnHeaderRowHeight = 25;
            this._gridGroupingControl2.TableDescriptor.TableOptions.RecordRowHeight = 20;
            this._gridGroupingControl2.Text = "_gridGroupingControl2";
            this._gridGroupingControl2.VersionInfo = "4.200.0.60";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(84)))), ((int)(((byte)(84)))), ((int)(((byte)(83)))));
            this.label1.Location = new System.Drawing.Point(8, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(584, 21);
            this.label1.TabIndex = 1;
            this.label1.Text = " This sample demonstrates the asynchronous access of records from the Hive table." +
    "";
            // 
            // commonLoaderPicPanel
            // 
            this.commonLoaderPicPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.commonLoaderPicPanel.AutoSize = true;
            this.commonLoaderPicPanel.Controls.Add(this.commonLoaderPic);
            this.commonLoaderPicPanel.Location = new System.Drawing.Point(14, 94);
            this.commonLoaderPicPanel.Name = "commonLoaderPicPanel";
            this.commonLoaderPicPanel.Size = new System.Drawing.Size(953, 285);
            this.commonLoaderPicPanel.TabIndex = 2;
            // 
            // commonLoaderPic
            // 
            this.commonLoaderPic.Dock = System.Windows.Forms.DockStyle.Fill;
            this.commonLoaderPic.Image = global::Asynchronous.Properties.Resources.preloader_48x48;
            this.commonLoaderPic.Location = new System.Drawing.Point(0, 0);
            this.commonLoaderPic.Name = "commonLoaderPic";
            this.commonLoaderPic.Size = new System.Drawing.Size(953, 285);
            this.commonLoaderPic.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.commonLoaderPic.TabIndex = 0;
            this.commonLoaderPic.TabStop = false;
            // 
            // Form1
            // 
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(252)))), ((int)(((byte)(252)))), ((int)(((byte)(252)))));
            this.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(102)))), ((int)(((byte)(102)))), ((int)(((byte)(101)))));
            this.BorderThickness = 2;
            this.CaptionAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.CaptionBarColor = System.Drawing.Color.FromArgb(((int)(((byte)(252)))), ((int)(((byte)(252)))), ((int)(((byte)(252)))));
            this.CaptionButtonColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(130)))), ((int)(((byte)(195)))));
            this.CaptionButtonHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(130)))), ((int)(((byte)(195)))));
            this.CaptionFont = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CaptionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(130)))), ((int)(((byte)(195)))));
            this.ClientSize = new System.Drawing.Size(980, 393);
            this.Controls.Add(this.commonLoaderPicPanel);
            this.Controls.Add(this.label1);
            this.Controls.Add(this._gridGroupingControl2);
            this.DropShadow = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Location = new System.Drawing.Point(12, 85);
            this.MetroColor = System.Drawing.Color.FromArgb(((int)(((byte)(252)))), ((int)(((byte)(252)))), ((int)(((byte)(252)))));
            this.MinimumSize = new System.Drawing.Size(600, 300);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Asynchronous";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this._gridGroupingControl2)).EndInit();
            this.commonLoaderPicPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.commonLoaderPic)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			Application.Run(new Form1());
        }

        private void Form1_Load(object sender, EventArgs e)
        {

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
            string path = System.AppDomain.CurrentDomain.BaseDirectory;
            cells = csvObj.Table(path + "..\\..\\..\\..\\..\\..\\..\\..\\Data\\AdventureWorks\\AdventureWorks_Person_Contact.csv", false, ',');

            #endregion parsing csv input file

            #region creating table
            tableName = "AdventureWorks_Person_Contact";
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


                ////Executes the Hql query
                //HqlDataReader reader = command.ExecuteReader();

                //Assigning number of records to be fetched from HBase
            HBaseOperation.FetchSize = 5000;

               
                //#endregion

                // Paging enabled in grid
                var pager = new Pager();
                pager.Wire(_gridGroupingControl2, tableName, con, commonLoaderPicPanel);

                _gridGroupingControl2.TopLevelGroupOptions.ShowFilterBar = false;
                foreach (var col in _gridGroupingControl2.TableDescriptor.Columns)
                    col.AllowFilter = false;

                int topRow = this._gridGroupingControl2.TableControl.TopRowIndex;
                this._gridGroupingControl2.TableControl.CurrentCell.Activate(topRow, 1);
            }
            catch (HBaseConnectionException)
            {

                if (MessageBox.Show("Could not establish a connection to the HBaseServer. Please run HBaseServer from Synfusion service manager dashboard.", "Could not establish a connection to the HiveServer", MessageBoxButtons.OK,MessageBoxIcon.Warning) == DialogResult.OK)
                {
                    Application.Exit();
                }
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
