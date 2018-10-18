#region Copyright Syncfusion Inc. 2001 - 2016
// Copyright Syncfusion Inc. 2001 - 2016. All rights reserved.
// Use of this code is subject to the terms of our license.
// A copy of the current license can be obtained at any time by e-mailing
// licensing@syncfusion.com. Any infringement will be prosecuted under
// applicable laws. 
#endregion
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Syncfusion.PMML;
namespace MVCSampleBrowser.Controllers
{
    public class IntroductionController : Controller
    {
       
        public ActionResult IntroductionDefault()
        {
            List<List<String>> Datasource = new List<List<String>>();

            KMeans outputData = new KMeans();
            String path = Request.PhysicalPath;
            Datasource = outputData.list(path);
            ViewBag.datasource = Datasource;
            ViewBag.source = System.IO.File.ReadAllText(path + "../../../Controllers/Introduction/IntroductionController.cs");
            ViewBag.spark = System.IO.File.ReadAllText(path + "../../../Models/KMeans/Spark.scala");
            ViewBag.PMML = System.IO.File.ReadAllText(path + "../../../Models/KMeans/kmeans_data.pmml");
            return View();
        }
    }
    public class KMeans
    {
        //Create Table instance for output table
        private Table outputTable = null;

        public List<List<String>> list(String path)
        {
            //Create instance
            KMeans program = new KMeans();
            string[] Lines = File.ReadAllLines(path + "../../../Models/KMeans/kmeans_data.txt");
            program.outputTable = program.PredictResult(Lines, path + "../../../Models/KMeans/kmeans_data.pmml");
            List<List<String>> result = new List<List<String>>();
            for (int i = 0; i < Lines.Length; i++)
            {
                List<String> tempRow = new List<String>();
                String[] temp = Lines[i].Split(' ');
                String tempValue = null;
                for (int j = 0; j < temp.Length; j++)
                {
                    tempValue = temp[j];
                    tempRow.Add(tempValue);
                }
                tempValue = program.outputTable[i][0].ToString();
                tempRow.Add(tempValue);
                result.Add(tempRow);
            }
            return result;
        }

        #region PredictResult

        /// <summary>
        /// Predicts the results for given PMML and CSV file and serialize the results in a CSV file
        /// </summary>
        public Table PredictResult(string[] input, string pmmlPath)
        {
            //Get PMML Evaluator instance
            PMMLEvaluator evaluator = new PMMLEvaluatorFactory().
              GetPMMLEvaluatorInstance(pmmlPath);

            string[] predictedCategories = null;
            int i = 0;
            foreach (string s in input)
            {
                string[] words = s.Split(' ');

                var record = new
                {
                    field_0 = words[0],
                    field_1 = words[1],
                    field_2 = words[2]
                };
                PredictedResult predictedResult = evaluator.GetResult(record, null);
                if (i == 0)
                {
                    //Get the predicted propability fields
                    predictedCategories = predictedResult.GetPredictedCategories();
                    //Initialize the output table

                    InitializeTable(input.Length, predictedCategories);

                }
                //Add predicted value
                outputTable[i, 0] = predictedResult.PredictedValue;
                i++;
            }

            return outputTable;
        }

        #endregion PredictResult

        #region Initialize OutputTable

        /// <summary>
        /// Initialize the outputTable
        /// </summary>
        /// <param name="rowCount">rowCount of output table</param>
        /// <param name="predictedfield">predictedfield name</param>
        /// <param name="predictedCategories">probableFields</param>
        private void InitializeTable(int rowCount, string[] predictedCategories)
        {
            //Create instance to hold evaluated results
            outputTable = new Table(rowCount, predictedCategories.Length + 1);
            //Add predicted column names
            outputTable.ColumnNames[0] = "Predicted";
        }

        #endregion Initialize OutputTable
    }

}
