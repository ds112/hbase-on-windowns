#region Copyright Syncfusion Inc. 2001 - 2016
// Copyright Syncfusion Inc. 2001 - 2016. All rights reserved.
// Use of this code is subject to the terms of our license.
// A copy of the current license can be obtained at any time by e-mailing
// licensing@syncfusion.com. Any infringement will be prosecuted under
// applicable laws. 
#endregion
using System;
using System.Globalization;
using System.Windows.Forms;
using System.Data;
using Syncfusion.Windows.Forms.Grid.Grouping;
using Syncfusion.Windows.Forms;
using Syncfusion.ThriftHive.Base;
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace Asynchronous
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class Form1 : MetroForm
    {
        private Label label1;
        private GridGroupingControl _gridGroupingControl2;
        
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
            ((System.ComponentModel.ISupportInitialize)(this._gridGroupingControl2)).BeginInit();
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
                #region Data
                //Initializing Hive connection
                HqlConnection con = new HqlConnection("localhost", 10000, HiveServer.HiveServer2);

                //To initialize a Hive server connection with secured cluster
                //HqlConnection con = new HqlConnection("<Secured cluster Namenode IP>", 10000, HiveServer.HiveServer2,"<username>","<password>");

                //To initialize a Hive server connection with Azure cluster
                //HqlConnection con = new HqlConnection("<FQDN name of Azure cluster>", 8004, HiveServer.HiveServer2,"<username>","<password>");

                //Opening the HQL connection
                con.Open();

                //Creating the adventure person contacts table in Hive Database
                HqlCommand createTable = new HqlCommand("create external table IF NOT EXISTS AdventureWorks_Person_Contact(contactid int,fullname string,age int,emailaddress string,phoneno string,modifieddate string) ROW FORMAT DELIMITED FIELDS TERMINATED BY ',' LOCATION '/Data/AdventureWorks'", con);
                createTable.ExecuteNonQuery();
                
                //Query to fetch data from Hive Database
                HqlCommand command = new HqlCommand("select * from AdventureWorks_Person_Contact", con);

                //Executes the Hql query
                HqlDataReader reader = command.ExecuteReader();

                //Assigning number of records to be fetched from Hive database
                reader.FetchSize = 5000;

               
                #endregion

                // Paging enabled in grid
                var pager = new Pager();
                pager.Wire(_gridGroupingControl2, reader);

                _gridGroupingControl2.TopLevelGroupOptions.ShowFilterBar = false;
                foreach (var col in _gridGroupingControl2.TableDescriptor.Columns)
                    col.AllowFilter = false;

                int topRow = this._gridGroupingControl2.TableControl.TopRowIndex;
                this._gridGroupingControl2.TableControl.CurrentCell.Activate(topRow, 1);
            }
            catch (HqlConnectionException)
            {

                if (MessageBox.Show("Could not establish a connection to the HiveServer. Please run HiveServer2 from Synfusion service manager dashboard.", "Could not establish a connection to the HiveServer", MessageBoxButtons.OK,MessageBoxIcon.Warning) == DialogResult.OK)
                {
                    Application.Exit();
                }
            }
        }
    }
}
