using Syncfusion.PMML;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kmeans_data
{
    /// <summary>
    /// Console program to demonstrate PMML execution engine
    /// </summary>
    public class Program
    {
        //Create Table instance for output table
        private Table outputTable = null;

        public static void Main(string[] args)
        {
            //Create instance
            Program program = new Program();
            string[] Lines = File.ReadAllLines("../../Model/kmeans_data.txt");
            program.outputTable = program.PredictResult(Lines, "../../Model/kmeans_data.pmml");
            //Write the Result as CSV File
            program.outputTable.WriteToCSV("../../Model/PredictedOutput.csv", true, ',');
            //Dispose the output Table
            program.outputTable.Dispose();
            //Display the result saved location
            Console.WriteLine("\nResult saved in : " + Path.GetFullPath("../../Model/PredictedOutput.csv"));
            Console.ReadKey();
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
