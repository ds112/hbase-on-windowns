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

namespace JdbcDataFrame
{
    
    public static class JdbcDataFrameExample
    {
        #region Variable Declaration

        private static ILoggerService logger;
        private static SparkContext sparkContext;
        private static CSharpRunner cSharpRunner;

        //For SQL Server use the connection string formats below
        //"jdbc:sqlserver://localhost;databaseName=Temp;user=MyUserName;password=myPassword;"

        //Assign the connectionString and tableName properly then proceed.

        private static string connectionString= string.Empty;
        private static string tableName=string.Empty;
        private static string configFileName= "JdbcDataFrame.exe.config";

        #endregion Variable Declaration

        #region Main
        /// <summary>
        /// Main method which is to be the starting point
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            LoggerServiceFactory.SetLoggerService(Log4NetLoggerService.Instance); //this is optional - DefaultLoggerService will be used if not set
            logger = LoggerServiceFactory.GetLogger(typeof(JdbcDataFrameExample));

            try
            {
                string studioHome = Directory.GetParent("..\\..\\..\\..\\..\\..\\..\\..\\..\\").ToString();
                string sparkHome = studioHome + @"\BigDataSDK\SDK\Spark\bin";
                string appConfigFile = System.IO.Path.Combine(Directory.GetParent("..\\..\\").ToString(), "App.config");
                cSharpRunner = new CSharpRunner(sparkHome, appConfigFile);
                cSharpRunner.UpdateConfigFile(configFileName);
                // Starting CSharpRunner is essential to execute a C# Spark samples
                StartCSharpRunner();
                if (cSharpRunner.IsCSharpRunnerStarted)
                {
                    logger.LogInfo("CSharpRunner Started.................");
                    GetValues();
                    JdbcDataFrame();
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

        #region JdbcDataFrame
        /// <summary>
        /// 
        /// To process with the given connection string for the SQL
        /// </summary>
        private static void JdbcDataFrame()
        {

            if (!string.IsNullOrEmpty(connectionString) && !string.IsNullOrEmpty(tableName))
            {
                var sparkConf = new SparkConf();
                sparkConf.SetAppName("SqlConnectionFromMobius");
                sparkConf.SetMaster("yarn");
                sparkConf.Set("spark.sql.warehouse.dir", "/user/hive/warehouse");
                sparkContext = new SparkContext(sparkConf);
                var sqlContext = new SqlContext(sparkContext);

                var df = sqlContext
                        .Read()
                        .Jdbc(connectionString, tableName, new Dictionary<string, string>());
                var rowCount = df.Count();

                logger.LogInfo("****Row count is " + rowCount + "****");
                logger.LogInfo("Executed Successfully.................");
            }
            else
            {
                logger.LogInfo("****Please provide correct connectionstring and table name****");
                GetValues();
                JdbcDataFrame();
            }
        }

        /// <summary>
        /// 
        /// To get values from the user
        /// </summary>
        private static void GetValues()
        {
            Console.WriteLine("Enter the connectionstring:");
            connectionString = Console.ReadLine();

            Console.WriteLine("Enter the table name:");
            tableName = Console.ReadLine();

        }
        #endregion JdbcDataFrame

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
