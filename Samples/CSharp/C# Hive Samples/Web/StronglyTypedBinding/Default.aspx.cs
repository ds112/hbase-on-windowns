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
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebSampleBrowser.StronglyTypedView
{
    public partial class Default : System.Web.UI.Page
    {
        public class PersonDetail
        {
            public int ContactId { get; set; }
            public string FullName { get; set; }
            public int Age { get; set; }
            public string EmailAddress { get; set; }
            public string PhoneNo { get; set; }
            public string ModifiedDate { get; set; }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            BindDataSource();
        }
        private void BindDataSource()
        {
            
            ErrorMessage.InnerText = "";
            string path = string.Format("{0}\\..\\Data\\AdventureWorks\\AdventureWorks_Person_Contact.csv", Request.PhysicalPath.ToLower().Split(new string[] { "\\c# hive samples" }, StringSplitOptions.None));
            CheckFileStatus FileStatus = new CheckFileStatus();
            ErrorMessage.InnerText = FileStatus.CheckFile(path);
            if (ErrorMessage.InnerText == "")
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

                    //Create table for adventure person contacts
                    HqlCommand createCommand = new HqlCommand("CREATE EXTERNAL TABLE IF NOT EXISTS AdventureWorks_Person_Contact(ContactID int,FullName string,Age int,EmailAddress string,PhoneNo string,ModifiedDate string) ROW FORMAT DELIMITED FIELDS TERMINATED BY ',' LOCATION '/Data/AdventureWorks'", con);
                    createCommand.ExecuteNonQuery();
                    HqlCommand command = new HqlCommand("Select * from AdventureWorks_Person_Contact limit 100", con);

                    //Executing the query
                    HqlDataReader reader = command.ExecuteReader();
                    reader.FetchSize = int.MaxValue;

                    //Fetches the result from the reader and store it in a HiveResultSet
                    HiveResultSet result = reader.FetchResult();

                    //Initialize the list to add elements in each row
                    BindingList<PersonDetail> resultList = new BindingList<PersonDetail>();

                    //Read each row from the fetched result
                    foreach (HiveRecord rows in result)
                    {
                        //Adding element of each row to the list
                        resultList = new BindingList<PersonDetail>(result.Select(row => new PersonDetail
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
                    this.FlatGrid.DataSource = resultList;
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