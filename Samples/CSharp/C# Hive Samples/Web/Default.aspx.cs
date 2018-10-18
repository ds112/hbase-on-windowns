#region Copyright Syncfusion Inc. 2001 - 2016
// Copyright Syncfusion Inc. 2001 - 2016. All rights reserved.
// Use of this code is subject to the terms of our license.
// A copy of the current license can be obtained at any time by e-mailing
// licensing@syncfusion.com. Any infringement will be prosecuted under
// applicable laws. 
#endregion
using Syncfusion.ThriftHive.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace WebSampleBrowser
{
    public partial class Default : System.Web.UI.Page
    {
        string path;
        protected void Page_Load(object sender, EventArgs e)
        {
            path = string.Format("{0}\\..\\Data\\AdventureWorks\\AdventureWorks_Person_Contact.csv", Request.PhysicalPath.ToLower().Split(new string[] { "\\c# hive samples" }, StringSplitOptions.None));
            BindDataSource();
        }
        private void BindDataSource()
        {
            ErrorMessage.InnerText = "";
            CheckFileStatus FileStatus = new CheckFileStatus();
            ErrorMessage.InnerText = FileStatus.CheckFile(path);
            if (ErrorMessage.InnerText == "")
            {
                try
                {
                    //Connecting to Hiveserver
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

                    //Fetches the result from the reader and store it in a HiveResultSet
                    HiveResultSet result = reader.FetchResult();

                    //Binding the result to the Grid
                    this.FlatGrid.DataSource = result;
                    this.FlatGrid.DataBind();

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