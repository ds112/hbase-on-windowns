using Microsoft.Spark.CSharp.Core;
using Microsoft.Spark.CSharp.Services;
using Syncfusion.CSharpRunner;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WordCount
{
    public class WordCountExample
    {
        #region Variable Declaration

        private static ILoggerService logger;
        private static SparkContext sparkContext;
        // To read data from the cluster. Modify the localhost with the hostName/IP
        private static string hdfsFile = "hdfs://localhost:9000//Data/WarPeace.txt";
        private static CSharpRunner cSharpRunner;
        private static string configFile = "WordCount.exe.config";
        #endregion Variable Declaration

        #region Main 
        /// <summary>
        /// Main method which is to be the starting point
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            LoggerServiceFactory.SetLoggerService(Log4NetLoggerService.Instance);
            logger = LoggerServiceFactory.GetLogger(typeof(WordCountExample));
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
                    WordCount();
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

        #region WordCount
        /// <summary>
        /// To calculate the wordcount for the Hdfs file
        /// </summary>
        private static void WordCount()
        {
            var sparkConf = new SparkConf();
            sparkConf.SetAppName("MobiusWordCountC#");
            sparkConf.SetMaster("yarn");
            sparkContext = new SparkContext(sparkConf);
            try
            {
                var lines = sparkContext.TextFile(hdfsFile);
                var counts = lines
                    .FlatMap(x => x.Split(' '))
                    .Map(w => new Tuple<string, int>(w, 1))
                    .ReduceByKey((x, y) => x + y);
                logger.LogInfo("**********************************************");

                foreach (var wordcount in counts.Collect())
                {
                    Console.WriteLine("{0}: {1}", wordcount.Item1, wordcount.Item2);
                }

                logger.LogInfo("**********************************************");
                logger.LogInfo("Executed Successfully.................");
            }
            catch (Exception ex)
            {
                logger.LogError("Error performing Word Count");
                logger.LogException(ex);
            }
        }

        #endregion WordCount

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
