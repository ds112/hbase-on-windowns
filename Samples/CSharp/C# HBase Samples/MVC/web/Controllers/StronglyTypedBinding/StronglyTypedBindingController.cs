#region Copyright Syncfusion Inc. 2001 - 2016

// Copyright Syncfusion Inc. 2001 - 2016. All rights reserved.
// Use of this code is subject to the terms of our license.
// A copy of the current license can be obtained at any time by e-mailing
// licensing@syncfusion.com. Any infringement will be prosecuted under
// applicable laws.

#endregion Copyright Syncfusion Inc. 2001 - 2016

using MVCSampleBrowser.Models;
using Syncfusion.ThriftHBase.Base;
using System.Web.Mvc;

namespace MVCSampleBrowser.Controllers
{
    public class StronglyTypedBindingController : Controller
    {
        //
        // GET: /StronglyBinded/
        public static string ErrorMessage
        { get; set; }

        public ActionResult StronglyTypedBindingDefault()
        {
            ErrorMessage = "";
            string path = new System.IO.DirectoryInfo(Request.PhysicalPath + "..\\..\\..\\..\\..\\..\\..\\Data\\AdventureWorks\\AdventureWorks_Person_Contact.csv").FullName;
            

            try
            {
                //Binding the result to the grid
                var DataSource = new StronglyBindedCustomersData().list(path);
                ViewBag.datasource = DataSource;
                ErrorMessage = "";
            }
            catch (HBaseConnectionException)
            {
                ErrorMessage = "Could not establish a connection to the HBaseServer. Please run HBaseServer from the Syncfusion service manager dashboard.";
            }
            catch (HBaseException ex)
            {
                ErrorMessage = ex.Message;
            }
            return View();
        }
    }
}