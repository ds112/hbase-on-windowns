using Microsoft.Spark.CSharp.Services;
using Microsoft.Spark.CSharp.Sql;
using Syncfusion.CSharpRunner;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HiveDataFrame
{
    public static class HiveDataFrameExample
    {
        #region Variable Declaration
        private static ILoggerService logger;
        // To read data in the cluster. Modify the localhost with the hostName/IP
        private static string jsonFilePath = "hdfs://localhost:9000//Data/Spark/Resources/People.json";
        private static string dbName = "SampleNewHiveDataBaseForMobius";
        private static string tableName = "people";
        private static SparkSession session;
        private static CSharpRunner cSharpRunner;
        private static string configFile= "HiveDataFrame.exe.config";
        #endregion Variable Declaration

        #region Main

        /// <summary>
        /// Main method which is to be the starting point
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            LoggerServiceFactory.SetLoggerService(Log4NetLoggerService.Instance); //this is optional - DefaultLoggerService will be used if not set
            logger = LoggerServiceFactory.GetLogger(typeof(HiveDataFrameExample));
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
                    HiveDataFrame();
                    DisplayEndInfo();
                    session.SparkContext.Stop();
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

        #region HiveDataFrame
        /// <summary>
        /// To integrate with Hive operations
        /// </summary>
        private static void HiveDataFrame()
        {
            var builder = SparkSession.Builder().EnableHiveSupport();
            builder = builder.Config("spark.master", "yarn");
            builder = builder.Config("spark.app.name", "HiveDataFrame");
            builder = builder.Config("spark.sql.warehouse.dir", "/user/hive/warehouse");
            session = builder.GetOrCreate();
            var peopleDataFrame = session.Read().Json(jsonFilePath);
            logger.LogInfo("****Create table if not exists****");
            session.Sql(string.Format("CREATE DATABASE IF NOT EXISTS {0}", dbName)); // create database if not exists
            logger.LogInfo("****Database Created****");
            session.Sql(string.Format("USE {0}", dbName));

            logger.LogInfo("****Create Table operation started****");
            peopleDataFrame.Write().Mode(SaveMode.Overwrite).SaveAsTable(tableName); // create table
            logger.LogInfo("****Table Created successfully****");
            var tablesDataFrame = session.Table(tableName); 
            logger.LogInfo(string.Format("****Table count in database {0}: {1}", dbName, tablesDataFrame.Count()) + "****");
            var rowCollections = tablesDataFrame.Collect();
            logger.LogInfo("**********************************************");
            foreach (var row in rowCollections)
            {
                Console.WriteLine("{0}", row);
            }
            logger.LogInfo("*********************************************");
            logger.LogInfo("Executed Successfully.................");
        }

        #endregion HiveDataFrame

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
