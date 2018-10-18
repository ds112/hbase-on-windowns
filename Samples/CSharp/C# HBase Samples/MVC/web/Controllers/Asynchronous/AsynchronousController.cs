#region Copyright Syncfusion Inc. 2001 - 2016

// Copyright Syncfusion Inc. 2001 - 2016. All rights reserved.
// Use of this code is subject to the terms of our license.
// A copy of the current license can be obtained at any time by e-mailing
// licensing@syncfusion.com. Any infringement will be prosecuted under
// applicable laws.

#endregion Copyright Syncfusion Inc. 2001 - 2016

using MVCSampleBrowser.Controllers.Grid;
using System.Collections.Generic;
using System.Web.Mvc;
using Syncfusion.ThriftHBase.Base;
using Syncfusion.JavaScript;
using MVCSampleBrowser.Models;
using servesidePaging.Models;


namespace MVCSampleBrowser.Controllers
{
    public class AsynchronousController : Controller
    {
        
        private static HBaseConnection con = null;
        private static Dictionary<int, DataResult> DataCollection = new Dictionary<int, DataResult>();
        private static string tableName = string.Empty;
        private static int page = 0;
        private static int skip = -1;
        public static string ErrorMessage
        { get; set; }

        public ActionResult AsynchronousDefault()
        {
            
            ErrorMessage = "";
            string path = new System.IO.DirectoryInfo(Request.PhysicalPath + "..\\..\\..\\..\\..\\..\\..\\Data\\AdventureWorks\\AdventureWorks_Person_Contact.csv").FullName;

            try
            {
                #region creating connection
                con = new HBaseConnection("localhost", 10003);
                con.Open();
                #endregion creating connection

                #region parsing csv input file
                csv csvObj = new csv();
                object[,] cells;
                cells = null;
                cells = csvObj.Table(path, false, ',');

                #endregion parsing csv input file

                #region creating table
                tableName = "adventureworks_person_contact";
                List<string> columnFamilies = new List<string>();
                columnFamilies.Add("info");
                columnFamilies.Add("contact");
                columnFamilies.Add("others");
                if (!HBaseOperation.IsTableExists(tableName, con))
                {
                    if (columnFamilies.Count > 0)
                    {
                        HBaseOperation.CreateTable(tableName, columnFamilies, con);
                    }
                    else
                    {
                       
                        throw new HBaseException( "ERROR: Table must have at least one column family");
                    }
                }
              
                # endregion

                #region Inserting Values
                string[] column = new string[] { "CONTACTID", "FULLNAME", "AGE", "EMAILID", "PHONE", "MODIFIEDDATE" };
                Dictionary<string, IList<HMutation>> rowCollection = new Dictionary<string, IList<HMutation>>();
                string rowKey;
                for (int i = 0; i < cells.GetLength(0); i++)
                {
                    List<HMutation> mutations = new List<HMutation>();
                    rowKey = cells[i, 0].ToString();
                    for (int j = 1; j < column.Length; j++)
                    {
                        HMutation mutation = new HMutation();
                        mutation.ColumnFamily = j < 3 ? "info" : j < 5 ? "contact" : "others";
                        mutation.ColumnName = column[j];
                        mutation.Value = cells[i, j].ToString();
                        mutations.Add(mutation);
                    }
                    rowCollection[rowKey] = mutations;
                }
                HBaseOperation.InsertRows(tableName, rowCollection, con);
                #endregion Inserting Values

                page = 0;
                skip = -1;
                DataCollection = new Dictionary<int, DataResult>();
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
                            var DataSource = CustomerData.list(tableName, con);
                            result.result = DataSource;
                            result.count = DataSource.Count;
                            //Adds the page number and the data to the dictionary
                            DataCollection.Add(page, result);
                            if (!HBaseOperation.HasRows)
                            {
                                result.hasRows = "Nill";
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