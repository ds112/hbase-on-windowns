using Microsoft.Spark.CSharp.Core;
using Microsoft.Spark.CSharp.Services;
using Microsoft.Spark.CSharp.Sql;
using Syncfusion.CSharpRunner;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SparkXml
{
    public static class SparkXmlExample
    {
        #region Variable Declaration

        private static ILoggerService logger;
        private static SparkContext sparkContext;
        private static CSharpRunner cSharpRunner;
        private static string configFile = "SparkXml.exe.config";
        // To read data from the cluster. Modify the localhost with the hostName/IP
        private static string inputXmlFilePath = "hdfs://localhost:9000/Data/Books.xml";

        #endregion Variable Declaration

        #region Main
        /// <summary>
        /// Main method which is to be the starting point
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            LoggerServiceFactory.SetLoggerService(Log4NetLoggerService.Instance); //this is optional - DefaultLoggerService will be used if not set
            logger = LoggerServiceFactory.GetLogger(typeof(SparkXmlExample));
            try
            {
                string studioHome = Directory.GetParent("..\\..\\..\\..\\..\\..\\..\\..\\..\\").ToString();
                string sparkHome = studioHome + @"\BigDataSDK\SDK\Spark\bin";
                string appConfigFile = System.IO.Path.Combine(Directory.GetParent("..\\..\\").ToString(), "App.config");
                cSharpRunner = new CSharpRunner(sparkHome, appConfigFile);
                cSharpRunner.UpdateConfigFile(configFile);
                // Starting CSharpRunner is essential to execute a C# Spark samples
                StartCSharpRunner();

                if (cSharpRunner.IsCSharpRunnerStarted)
                {
                    logger.LogInfo("CSharpRunner Started.................");
                    SparkXml();
                    DisplayEndInfo();
                    sparkContext.Stop();
                    cSharpRunner.process.Kill();
                }
                else
                {
                    DisplayEndInfo();
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
            }
        }

        #endregion Main

        #region SparkXml
        /// <summary>
        /// 
        /// To read the data from the xml file and to retrieve the data
        /// </summary>
        private static void SparkXml()
        {
            var sparkConf = new SparkConf();
            sparkConf.SetMaster("yarn");
            sparkConf.SetAppName("SparkXmlMobius");
            sparkContext = new SparkContext(sparkConf);
            var sqlContext = new SqlContext(sparkContext);
            var dataframe = sqlContext.Read()
                                .Format("com.databricks.spark.xml")
                                  .Option("rowTag", "book")
                                .Load(inputXmlFilePath); 
          
            var rowCount = dataframe.Count();
            logger.LogInfo("****Row count is " + rowCount + "****");
            var rowCollections = dataframe.Collect();
            logger.LogInfo("**********************************************");
            foreach (var row in rowCollections)
            {
                Console.WriteLine("{0}", row);
            }
            logger.LogInfo("*********************************************");
            logger.LogInfo("Executed Successfully.................");
        }

        #endregion SparkXml

        #region Helper

        /// <summary>
        /// To display a sample's end information
        /// </summary>
        private static void DisplayEndInfo()
        {
            logger.LogInfo("Press any key to exit.................");
            Console.ReadKey();
        }

        /// <summary>
        /// To start the CSharpRunner
        /// </summary>
        private static void StartCSharpRunner()
        {
            logger.LogInfo("Starting CSharpRunner.................");
            cSharpRunner.StartCsharpRunner();
            int val = 0;
            while (!cSharpRunner.IsCSharpRunnerStarted)
            {
                if (val <= 20)
                {
                    Thread.Sleep(2000);
                    val += 2;
                }
                else
                {
                    logger.LogInfo("Timed out starting CSharpRunner...");
                    break;
                }
            }
        }
        #endregion Helper
    }
}
