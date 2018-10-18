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

namespace StronglyTypedBinding
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

        public class PersonDetail
        {
            public int ContactId { get; set; }
            public string FullName { get; set; }
            public int Age { get; set; }
            public string EmailAddress { get; set; }
            public string PhoneNo { get; set; }
            public string ModifiedDate { get; set; }
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

                //Query to create AdventureWorks_Person_Contact table in Hive Database
                HqlCommand createCommand = new HqlCommand("create external table IF NOT EXISTS AdventureWorks_Person_Contact(contactid int,fullname string,age int,emailaddress string,phoneno string,modifieddate string) ROW FORMAT DELIMITED FIELDS TERMINATED BY ',' LOCATION '/Data/AdventureWorks'", con);
                createCommand.ExecuteNonQuery();
                HqlCommand command = new HqlCommand("Select * from AdventureWorks_Person_Contact", con);

                //Query to fetch data from Hive Database
                HqlDataReader reader = command.ExecuteReader();
                reader.FetchSize = 500;
                HiveResultSet resultCollection = reader.FetchResult();
                BindingList<PersonDetail> result = new BindingList<PersonDetail>();

                //Reading each row from the fetched result
                foreach (HiveRecord rows in resultCollection)
                {
                    //Adding each field value to the list
                    result = new BindingList<PersonDetail>(resultCollection.Select(row => new PersonDetail
                    {
                        ContactId = Convert.ToInt32(row["contactid"]),
                        FullName = row["fullname"].ToString(),
                        Age = Convert.ToInt32(row["age"]),
                        EmailAddress = row["emailaddress"].ToString(),
                        PhoneNo = row["phoneno"].ToString(),
                        ModifiedDate = row["modifieddate"].ToString()
                    }).ToList());
                }

                //Binding the result to the grid
                gridGroupingControl1.DataSource = result;

                gridGroupingControl1.TableDescriptor.Columns["ContactId"].HeaderText = "Contact Id";
                gridGroupingControl1.TableDescriptor.Columns["FullName"].HeaderText = "Full Name";
                gridGroupingControl1.TableDescriptor.Columns["Age"].HeaderText = "Age";
                gridGroupingControl1.TableDescriptor.Columns["EmailAddress"].HeaderText = "Email Address";
                gridGroupingControl1.TableDescriptor.Columns["PhoneNo"].HeaderText = "Phone No";
                gridGroupingControl1.TableDescriptor.Columns["ModifiedDate"].HeaderText = "Modified Date";
                gridGroupingControl1.TableDescriptor.Columns["ContactId"].Width = 85;
                gridGroupingControl1.TableDescriptor.Columns["FullName"].Width = 170;
                gridGroupingControl1.TableDescriptor.Columns["Age"].Width = 90;
                gridGroupingControl1.TableDescriptor.Columns["EmailAddress"].Width = 230;
                gridGroupingControl1.TableDescriptor.Columns["PhoneNo"].Width = 170;
                gridGroupingControl1.TableDescriptor.Columns["ModifiedDate"].Width = 170;

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
    }
}

         

       