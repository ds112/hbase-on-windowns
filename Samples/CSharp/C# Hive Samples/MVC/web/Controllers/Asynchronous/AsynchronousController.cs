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
using Syncfusion.JavaScript;
using MVCSampleBrowser.Models;
using System.Collections;
using servesidePaging.Models;
using MVCSampleBrowser.Controllers.Grid;
using Thrift;
using Syncfusion.ThriftHive.Base;
using MVCSampleBrowser.Controllers;
namespace MVCSampleBrowser.Controllers
{
    public class AsynchronousController : Controller
    {
        static Dictionary<int, DataResult> DataCollection = new Dictionary<int, DataResult>();
        static HqlDataReader reader;
        static private int page = 0;
        static int skip = -1;
        public static string ErrorMessage
        { get; set; }
        public ActionResult AsynchronousDefault()
        {
            ErrorMessage = "";
            string path = new System.IO.DirectoryInfo(Request.PhysicalPath + "..\\..\\..\\..\\..\\..\\..\\Data\\AdventureWorks\\AdventureWorks_Person_Contact.csv").FullName;
            CheckFileStatus CheckFile = new CheckFileStatus();
            ErrorMessage = CheckFile.CheckFile(path);
            if (ErrorMessage == "")
            {
                try
                {
                    //Initializing the connection
                    HqlConnection con = new HqlConnection("localhost", 10000, HiveServer.HiveServer2);

                    //To initialize a Hive server connection with secured cluster
                    //HqlConnection con = new HqlConnection("<Secured cluster Namenode IP>", 10000, HiveServer.HiveServer2,"<username>","<password>");

                    //To initialize a Hive server connection with Azure cluster
                    //HqlConnection con = new HqlConnection("<FQDN name of Azure cluster>", 8004, HiveServer.HiveServer2,"<username>","<password>");

                    //Open the HiveServer connection
                    con.Open();

                    //Create a table with data
                    HqlCommand createCommand = new HqlCommand("create external table IF NOT EXISTS AdventureWorks_Person_Contact(contactid int,fullname string,age int,emailaddress string,phoneno string,modifieddate string) ROW FORMAT DELIMITED FIELDS TERMINATED BY ',' LOCATION '/Data/AdventureWorks'", con);
                    createCommand.ExecuteNonQuery();

                    //Select query with connection
                    HqlCommand command = new HqlCommand("select * from AdventureWorks_Person_Contact limit 100", con);

                    //Executing the query
                    reader = command.ExecuteReader();
                    page = 0;
                    skip = -1;
                    DataCollection = new Dictionary<int, DataResult>();
                    ErrorMessage = "";

                }
                catch (HqlConnectionException)
                {
                    ErrorMessage = "Could not establish a connection to the HiveServer. Please run HiveServer2 from the Syncfusion service manager dashboard.";
                }
            }

            return View();

        }
        public ActionResult Data(DataManager dm)
        {
            DataResult result = new DataResult();           
                if (ErrorMessage == "")
                {
                    //View the data stored in next page
                    if (skip < dm.Skip)
                    {
                        //Incrementing the page after the required amount of data is stored
                        page = page + 1;
                        if (DataCollection.ContainsKey(page))
                        {
                            result = DataCollection[page];
                        }
                        else
                        {
                            if (reader != null)
                            {
                                var DataSource = CustomerData.list(reader);
                                result.result = DataSource.Take(dm.Take).ToList();
                                result.count = DataSource.Count();
                                //Adds the page number and the data to the dictionary
                                DataCollection.Add(page, result);
                                if (!reader.HasRows)
                                {
                                    result.hasRows = "Nill";
                                }
                            }

                        }
                    }
                    //View the data stored in previous page
                    else
                    {
                        page = page - 1;
                        result = DataCollection[page];

                    }
                    skip = dm.Skip;
                }
        
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}
