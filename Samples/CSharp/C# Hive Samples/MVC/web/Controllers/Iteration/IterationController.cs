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
using MVCSampleBrowser.Models;
using Syncfusion.ThriftHive.Base;
namespace MVCSampleBrowser.Controllers
{
    public class IterationController : Controller
    {
        public static string ErrorMessage
        { get; set; }
        public ActionResult IterationDefault()
        {
            ErrorMessage = "";
            return View();
         }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult IterationDefault(string dumpbutton)
        {
            ErrorMessage="";
            string path = new System.IO.DirectoryInfo(Request.PhysicalPath + "..\\..\\..\\..\\..\\..\\..\\Data\\AdventureWorks\\AdventureWorks_Person_Contact.csv").FullName;
             CheckFileStatus CheckFile = new CheckFileStatus();
            ErrorMessage = CheckFile.CheckFile(path);
            if (ErrorMessage == "")
            {
                try
                {
                    //Binding the result to the rich text editor
                    var DataSource = CustomersData.list();
                    ViewBag.datasource = DataSource;
                    ErrorMessage = "";
                }
                catch (HqlConnectionException)
                {
                    ErrorMessage = "Could not establish a connection to the HiveServer. Please run HiveServer2 from the Syncfusion service manager dashboard.";

                }
            }
            return View();
        }
    }
}
