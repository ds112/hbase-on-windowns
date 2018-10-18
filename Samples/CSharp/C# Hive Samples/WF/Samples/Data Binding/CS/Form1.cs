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
using System.Data.SqlClient;
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

namespace DataBinding
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
                //Initializing the hive server connection
                HqlConnection con = new HqlConnection("localhost", 10000, HiveServer.HiveServer2);

                //To initialize a Hive server connection with secured cluster
                //HqlConnection con = new HqlConnection("<Secured cluster Namenode IP>", 10000, HiveServer.HiveServer2,"<username>","<password>");

                //To initialize a Hive server connection with Azure cluster
                //HqlConnection con = new HqlConnection("<FQDN name of Azure cluster>", 8004, HiveServer.HiveServer2,"<username>","<password>");

                con.Open();

                //Creating the adventure person contacts table 
                HqlCommand createCommand = new HqlCommand("create external table IF NOT EXISTS AdventureWorks_Person_Contact(contactid int,fullname string,age int,emailaddress string,phoneno string,modifieddate string) ROW FORMAT DELIMITED FIELDS TERMINATED BY ',' LOCATION '/Data/AdventureWorks'", con);
                createCommand.ExecuteNonQuery();

                //Creating Query to fetch all data set from Hive Database
                HqlCommand command = new HqlCommand("Select * from AdventureWorks_Person_Contact", con);

                //Execution query to fetch data from Hive Database 
                HqlDataReader reader = command.ExecuteReader();
                reader.FetchSize = int.MaxValue;

                //Fetches the result from the reader and store it in a object
                HiveResultSet result = reader.FetchResult();

                //Binding the fetched result to the grid
                gridGroupingControl1.DataSource = result;

                //Assigning Header text to Grid
                gridGroupingControl1.TableDescriptor.Columns["contactid"].HeaderText = "Contact Id";
                gridGroupingControl1.TableDescriptor.Columns["fullname"].HeaderText = "Full Name";
                gridGroupingControl1.TableDescriptor.Columns["age"].HeaderText = "Age";
                gridGroupingControl1.TableDescriptor.Columns["emailaddress"].HeaderText = "Email Address";
                gridGroupingControl1.TableDescriptor.Columns["phoneno"].HeaderText = "Phone No";
                gridGroupingControl1.TableDescriptor.Columns["modifieddate"].HeaderText = "Modified Date";
                gridGroupingControl1.TableDescriptor.Columns["contactid"].Width = 85;
                gridGroupingControl1.TableDescriptor.Columns["fullname"].Width = 170;
                gridGroupingControl1.TableDescriptor.Columns["age"].Width = 90;
                gridGroupingControl1.TableDescriptor.Columns["emailaddress"].Width = 230;
                gridGroupingControl1.TableDescriptor.Columns["phoneno"].Width = 170;
                gridGroupingControl1.TableDescriptor.Columns["modifieddate"].Width = 170;

                //closing the hive connection
                con.Close();
            }
            catch(HqlConnectionException)
            {
                if (MessageBox.Show("Could not establish a connection to the HiveServer. Please run HiveServer2 from the Syncfusion service manager dashboard.", "Could not establish a connection to the HiveServer", MessageBoxButtons.OK,MessageBoxIcon.Warning) == DialogResult.OK)
                {
                    Application.Exit();
                }
            }
        }

    }
}
       
        