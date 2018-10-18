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
using Syncfusion.Windows.Forms;

namespace Iteration
{
    public partial class Form1 : MetroForm
    {
        HqlConnection con;
        HiveResultSet result;
        HqlDataReader reader;
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
                con = new HqlConnection("localhost", 10000, HiveServer.HiveServer2);

                //To initialize a Hive server connection with secured cluster
                //HqlConnection con = new HqlConnection("<Secured cluster Namenode IP>", 10000, HiveServer.HiveServer2,"<username>","<password>");

                //To initialize a Hive server connection with Azure cluster
                //HqlConnection con = new HqlConnection("<FQDN name of Azure cluster>", 8004, HiveServer.HiveServer2,"<username>","<password>");

                con.Open();

                //Creating query to create AdventureWorks_Person_Contact table in Hive Database
                HqlCommand createCommand = new HqlCommand("create external table IF NOT EXISTS AdventureWorks_Person_Contact(contactid int,fullname string,age int,emailaddress string,phoneno string,modifieddate string) ROW FORMAT DELIMITED FIELDS TERMINATED BY ',' LOCATION '/Data/AdventureWorks'", con);
                createCommand.ExecuteNonQuery();

                //Query to fetch data from Hive Database
                HqlCommand command = new HqlCommand("Select * from AdventureWorks_Person_Contact", con);

                //Execute command to fetch data from Hive Database
                reader = command.ExecuteReader();
                reader.FetchSize = int.MaxValue;
                result = reader.FetchResult();

                //Closing the hive connection
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

        private void button1_Click(object sender, EventArgs e)
        {
            richTextBox1.Clear();

           
            StringBuilder resultCollection = new StringBuilder();

            #region Adding header text
            resultCollection.Append("Contact ID".PadRight(25).Substring(0, 25));
            resultCollection.Append("Full Name".PadRight(73).Substring(0, 73));
            resultCollection.Append("Age".PadRight(28).Substring(0, 28));
            resultCollection.Append("Email Address".PadRight(88).Substring(0, 88));
            resultCollection.Append("Phone No".PadRight(72).Substring(0, 72));
            resultCollection.Append("Modified Date \n");
            #endregion

            //Reading each rows from the fetched result
            foreach (HiveRecord rows in result)
            {
                int i = 0;
                //Reading each data from the rows
                foreach (Object fields in rows)
                {
                    string records = fields.ToString();
                    if (i == 0 || i == 2)
                    {
                        //Collecting each data and store it in result collection
                        resultCollection.Append(records.PadRight(20).Substring(0, 20));
                    }
                    else
                    {
                        //Collecting each data and store it in result collection
                        resultCollection.Append(records.PadRight(65).Substring(0, 65));
                    }
                    i += 1;
                    resultCollection.Append("\t");    
                }
                resultCollection.Append("\n");
            }
            //Bind the resultant collection to the rich textbox
            richTextBox1.Text = resultCollection.ToString();
            richTextBox1.Visible = true;

          
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            if (WindowState==FormWindowState.Maximized)
            {
                this.label1.Height = 34;
                this.label1.Width = 1000;

            }
            else
            {
                this.label1.Height = 47;
                this.label1.Width = 678;

            }
        }
    }
}
