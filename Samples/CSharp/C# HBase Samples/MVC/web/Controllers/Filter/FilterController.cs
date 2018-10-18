using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVCSampleBrowser.Models;
using Syncfusion.ThriftHBase.Base;
namespace MVCSampleBrowser.Controllers
{
    public class FilterController : Controller
    {
        public static List<List<Customers>> DataSource;
        public static string ErrorMessage
        { get; set; }
        // GET: Filter
        public ActionResult FilterDefault()
        {
            ErrorMessage = "";
            string path = new System.IO.DirectoryInfo(Request.PhysicalPath + "..\\..\\..\\..\\..\\..\\..\\Data\\AdventureWorks\\AdventureWorks_Person_Contact.csv").FullName;

            try
            {
                //Binding the result to the grid
                DataSource = new List<List<Customers>>();
                DataSource = FilterData.list(path);

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
