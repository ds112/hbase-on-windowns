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

namespace Pi
{
    public static class PiExample
    {
        #region Variable declaration

        private static ILoggerService logger;
        private static SparkContext sparkContext;
        private static CSharpRunner cSharpRunner;
        private static string configFile = "Pi.exe.config";

        #endregion Variable declaration

        #region Main

        /// <summary>
        /// Main method which is to be the starting point
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            LoggerServiceFactory.SetLoggerService(Log4NetLoggerService.Instance); 
            logger = LoggerServiceFactory.GetLogger(typeof(PiExample));

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
                    Pi();                   
                    DisplayEndInfo();
                    sparkContext.Stop();
                    cSharpRunner.process.Kill();
                }
                else
                {
                    DisplayEndInfo();
                }              
            }
            catch(Exception ex)
            {
                logger.LogError(ex.Message);
            }
        }

        #endregion Main

        #region Pi
        /// <summary>
        /// To calculate Pi value
        /// </summary>
        private static void Pi()
        {
            var sparkConf = new SparkConf();
            sparkConf.SetAppName("MobiusSimpleSamplePI");
            sparkConf.SetMaster("yarn");
            sparkContext = new SparkContext(sparkConf);
            try
            {
                const int slices = 3;
                var numberOfItems = (int)Math.Min(100000L * slices, int.MaxValue);
                var values = new List<int>(numberOfItems);
                for (var i = 0; i <= numberOfItems; i++)
                {
                    values.Add(i);
                }

                var rdd = sparkContext.Parallelize(values, slices);

                logger.LogInfo("Started Calculating Pi");

                CalculatePiUsingAnonymousMethod(numberOfItems, rdd);

                CalculatePiUsingSerializedClassApproach(numberOfItems, rdd);

                logger.LogInfo("Completed calculating the value of Pi");
                logger.LogInfo("Executed Successfully.................");
            }
            catch (Exception ex)
            {
                logger.LogError("Error calculating Pi");
                logger.LogException(ex);
            }
        }
		/// <summary>
        /// To calculate Pi value using Serialized approach
        /// </summary>
        private static void CalculatePiUsingSerializedClassApproach(int n, RDD<int> rdd)
        {
            logger.LogInfo("Started Calculating Pi using Serialized class approach");
            var count = rdd.Map(new PiHelper().Execute)
                            .Reduce((x, y) => x + y);

            logger.LogInfo("****************************************************");
            logger.LogInfo(string.Format("(serialized class approach) Pi is roughly {0}.", 4.0 * count / n));
            logger.LogInfo("****************************************************");
        }
		
		/// <summary>
        /// To calculate Pi value using AnonymousMethod
        /// </summary>
        private static void CalculatePiUsingAnonymousMethod(int n, RDD<int> rdd)
        {
            logger.LogInfo("Started Calculating Pi using Anonymous Method");
            var count = rdd
                            .Map(i =>
                            {
                                var random = new Random();
                                var x = random.NextDouble() * 2 - 1;
                                var y = random.NextDouble() * 2 - 1;

                                return (x * x + y * y) < 1 ? 1 : 0;
                            })
                            .Reduce((x, y) => x + y);
           logger.LogInfo("****************************************************");
           logger.LogInfo(string.Format("(anonymous method approach) Pi is roughly {0}.", 4.0 * count / n));
           logger.LogInfo("****************************************************");
        }

        #endregion Pi

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
                    val+=2;
                }
                else
                {
                    logger.LogInfo("Timed out starting CSharpRunner...");
                    break;
                }
            }
        }
        #endregion Helper

        #region Helper class

        /// <summary>
        /// Serialized class used in RDD Map Transformation
        /// </summary>
        [Serializable]
        private class PiHelper
        {
            private readonly Random random = new Random();
            public int Execute(int input)
            {
                var x = random.NextDouble() * 2 - 1;
                var y = random.NextDouble() * 2 - 1;

                return (x * x + y * y) < 1 ? 1 : 0;
            }
        }
        #endregion Helper class

    }
}
