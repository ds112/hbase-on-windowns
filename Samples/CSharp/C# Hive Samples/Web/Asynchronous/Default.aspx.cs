#region Copyright Syncfusion Inc. 2001 - 2016
// Copyright Syncfusion Inc. 2001 - 2016. All rights reserved.
// Use of this code is subject to the terms of our license.
// A copy of the current license can be obtained at any time by e-mailing
// licensing@syncfusion.com. Any infringement will be prosecuted under
// applicable laws. 
#endregion
using Syncfusion.JavaScript;
using Syncfusion.ThriftHive.Base;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebSampleBrowser.Asynchronous_Grid_Paging
{
    public partial class Default : System.Web.UI.Page
    {

        public class DataResult
        {
            public IEnumerable result { get; set; }
            public int count { get; set; }
            public string hasRows { get; set; }
        }

        //Declaring the variables used to perform hive query operation.
        static Dictionary<int, DataResult> DataCollection = new Dictionary<int, DataResult>();
        static HqlDataReader reader;
        static private int page = 0;
        static int Skip = -1;
        HqlConnection con= null;
        HqlCommand command = null;
     
        
        protected void Page_Load(object sender, EventArgs e)
        {
            ErrorMessage.InnerText = "";
            CheckFileStatus FileStatus = new CheckFileStatus();
            string path = path = string.Format("{0}\\..\\Data\\AdventureWorks\\AdventureWorks_Person_Contact.csv", Request.PhysicalPath.ToLower().Split(new string[] { "\\c# hive samples" }, StringSplitOptions.None));
            ErrorMessage.InnerText = FileStatus.CheckFile(path);
            if (ErrorMessage.InnerText == "")
            {
                try
                {
                    //initializing the connection
                    con = new HqlConnection("localhost", 10000, HiveServer.HiveServer2);

                    //To initialize a Hive server connection with secured cluster
                    //HqlConnection con = new HqlConnection("<Secured cluster Namenode IP>", 10000, HiveServer.HiveServer2,"<username>","<password>");

                    //To initialize a Hive server connection with Azure cluster
                    //HqlConnection con = new HqlConnection("<FQDN name of Azure cluster>", 8004, HiveServer.HiveServer2,"<username>","<password>");

                    //Open the hive server connection
                    con.Open();

                    //Create table for adventure person contacts
                    HqlCommand createCommand = new HqlCommand("CREATE EXTERNAL TABLE IF NOT EXISTS AdventureWorks_Person_Contact(ContactID int,FullName string,Age int,EmailAddress string,PhoneNo string,ModifiedDate string) ROW FORMAT DELIMITED FIELDS TERMINATED BY ',' LOCATION '/Data/AdventureWorks'", con);
                    createCommand.ExecuteNonQuery();

                    //Executing the query
                    command = new HqlCommand("select * from AdventureWorks_Person_Contact limit 100", con);
                    reader = command.ExecuteReader();

                    page = 0;
                    Skip = -1;

                    DataCollection = new Dictionary<int, DataResult>();
                }
                catch (HqlConnectionException)
                {
                    ErrorMessage.InnerText = "Could not establish a connection to the HiveServer. Please run HiveServer2 from the Syncfusion service manager dashboard.";
                }
            }
        }
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static object DataSource(int skip, int take)
        {
            DataResult resultSet = new DataResult();
            if (reader != null)
            {
                //View the data stored in next page
               if (Skip < skip)
                {
                   //Incrementing the page after the required amount of data is stored
                    page = page + 1;
                    if (DataCollection.ContainsKey(page))
                    {
                        resultSet = DataCollection[page];
                    }
                    else
                    {
                        //Fetches the result and store it in datasource
                        var DataSource = CustomerData.list(reader);
                        resultSet.result = DataSource.Take(take).ToList();
                        resultSet.count = DataSource.Count();

                        //Adds the page number and the data to the dictionary
                        DataCollection.Add(page, resultSet);
                        if (!reader.HasRows)
                        {
                            resultSet.hasRows = "Nill";
                        }
                    }
                }
               //View the data stored in previous page
                else
                {
                    page = page - 1;
                    resultSet = DataCollection[page];

                }
                Skip = skip;
            }
                return resultSet;
           
        }

    }
}



