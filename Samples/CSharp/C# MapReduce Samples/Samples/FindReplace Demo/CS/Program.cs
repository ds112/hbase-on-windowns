#region Copyright Syncfusion Inc. 2001 - 2016
// Copyright Syncfusion Inc. 2001 - 2016. All rights reserved.
// Use of this code is subject to the terms of our license.
// A copy of the current license can be obtained at any time by e-mailing
// licensing@syncfusion.com. Any infringement will be prosecuted under
// applicable laws. 
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Hadoop.MapReduce;
using Microsoft.Hadoop.WebClient.WebHCatClient;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using System.Diagnostics;
using System.Net;
using Newtonsoft.Json;
using System.Threading;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;
using System.Xml;
using Group.Helpers;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
namespace FindandReplace
{
//Find and Replace Operation.
    //Implementation of Find and Replace operations in native MapReduce through C#
   
    class Program
    {
        #region Variables
        static WebRequest getUrl;
        static Stream objStream;
        static string uName = string.Empty;
        #endregion

         public class FindReplace : HadoopJob<SimpleMapper, SimpleReducer, SimpleReducer>
        {
            public static string _input1HDFS = "/Data/NASA_Access_Log";
            public static string s_outputFolderHDFS = "/output";


            public override HadoopJobConfiguration Configure(ExecutorContext context)
            {
                HadoopJobConfiguration config = new HadoopJobConfiguration();
                config.Verbose = true;
                config.InputPath = _input1HDFS;
                config.OutputFolder = s_outputFolderHDFS;
                config.AdditionalGenericArguments.Add("-D \"mapred.map.tasks=3\""); // example of controlling arbitrary hadoop options.
                return config;
            }
        }
        public class SimpleMapper : MapperBase
        {
            public override void Map(string inputLine, MapperContext context)
            {
                List<string> split_input;
                // Replace '- ' with empty value, so input lines are separated only by white space
                inputLine = inputLine.Replace("- ", "");

                // Using Regex to split input lines by white space unless text enclosed with in double quotes and '[]' and store each field as list 
                split_input = System.Text.RegularExpressions.Regex.Matches(inputLine, @"[\""].+?[\""]|[[].+?[]]|[^ ]+")
                    .Cast<System.Text.RegularExpressions.Match>()
                    .Select(m => m.Value)
                    .ToList();

                string val = string.Empty;
                // Record without host name and Date are not taken into consideration
                if (split_input.Count > 1)
                {
                    // Parsing date string to DateTime format with date alone, excluding time
                    DateTime date = DateTime.Parse(split_input[1].Substring(1, split_input[1].IndexOf(':') - 1));
                    foreach (string s in split_input)
                    {
                        val += s + "\t";
                    }
                    if (!string.IsNullOrEmpty(split_input[4]))
                        context.EmitKeyValue(split_input[0] + "\t", val);
                }
            }
        }

        public class SimpleReducer : ReducerCombinerBase
        {
            public override void Reduce(string key, IEnumerable<string> values, ReducerCombinerContext context)
            {
                // Finding the special charaters and non-alphabatic characters from input and replace it with empty string value. 
                foreach(string val in values)
                {
                    string st = Regex.Replace(val, @"[^0-9a-zA-Z]+", "");
                    context.EmitKeyValue(key, st.ToString());
                }
            }
        }
        static void Main(string[] args)
        {
            //To submit jobs in HDInsight cluster
            //DoCustomMapReduce();

            uName = Environment.UserName;//Checks whether the username has space or special characters
            //In case if username contains space or special character, we use SYSTEM as the username
            var regexItem = new Regex("^[a-zA-Z0-9_-]*$");
            if (!regexItem.IsMatch(uName))
                uName = "SYSTEM";

            //To submit jobs in Local cluster
            DoMapReduce();
        }


        #region Job Submission in Local cluster

        static string ActiveNameNode = "localhost";
        static bool IsRemote = false;
        static Credential credentials;
        static bool IsSecured = false;


        private static void DoMapReduce()
        {
            try
            {
                SetEnvironment();
                UserInteraction();
                var config = GetHadoopConfiguration();

                IHadoop myCluster = null;
                if (IsRemote)
                {
                    if (!CreateConfigurationDirectory())
                        Console.WriteLine("Failed to create configuration directory");
                    else
                    {
                        if (IsSecured)
                        {
                            myCluster = ConnectToSecuredRemoteCluster();
                        }
                        else
                        {
                            myCluster = ConnectToRemoteCluster();
                        }
                    }
                }
                else
                {
                    myCluster = ConnectToLocalPseudoCluster();
                }

                if (myCluster != null)
                {
                    //execute mapreduce job
                    Console.WriteLine("\n\nExecution begins......\n");

                    //passing the Mapper and Reducer
                    var jobResult = myCluster.MapReduceJob.Execute<SimpleMapper, SimpleReducer>(config);
                    UpdateExecutionStatus(jobResult.Info.ExitCode);
                }
                UpdateExecutionStatus(1);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.Read();
            }
        }

        private static void SetEnvironment()
        {
            Environment.SetEnvironmentVariable("HADOOP_HOME", @"C:\Syncfusion\BigData\3.2.0.20\BigDataSDK\\SDK\Hadoop");
            Environment.SetEnvironmentVariable("JAVA_HOME", @"C:\Syncfusion\BigData\3.2.0.20\BigDataSDK\\Java\jdk1.7.0_51");
        }

        private static void UserInteraction()
        {
            Console.WriteLine(
                "Do you want to connect to Remote Hadoop ? (y - Yes, Press any key to connect to Pseudo node) ");

            IsRemote = Console.ReadKey().Key == ConsoleKey.Y;
            if (IsRemote)
            {
                Console.WriteLine();
                Console.WriteLine("Enter active name node IP/Host name of remote machine : ");
                ActiveNameNode = Console.ReadLine().Trim();

                Console.WriteLine(
                "Is cluster secured Kerberos ? (y - Yes, Press any key to connect to provided name node) ");

                IsSecured = Console.ReadKey().Key == ConsoleKey.Y;
                if (IsSecured)
                {
                    credentials = new Credential();
                    Console.WriteLine();
                    Console.WriteLine("Enter active directory IP : ");
                    credentials.ActiveDirectoryIp = Console.ReadLine().Trim();
                    Console.WriteLine("Enter user name : ");
                    credentials.Username = Console.ReadLine().Trim();
                    Console.WriteLine("Enter password : ");
                    credentials.Password = Console.ReadLine().Trim();
                }
            }
        }

        private static HadoopJobConfiguration GetHadoopConfiguration()
        {
            HadoopJobConfiguration config = new HadoopJobConfiguration();
            config.InputPath = "/Data/NASA_Access_Log";
            config.OutputFolder = "/output";

            Console.WriteLine("\n\n\nInput Path :" + config.InputPath);
            Console.WriteLine("\nOutput Folder :" + config.OutputFolder);
            return config;
        }

        private static bool CreateConfigurationDirectory()
        {
            string hadoopHome = Environment.GetEnvironmentVariable("HADOOP_HOME");

            HadoopConfiguration config;
            if (IsSecured)
            {
                config = new HadoopConfiguration(ActiveNameNode, IsSecured, credentials);
                credentials.DomainName = config.GetDomainName(credentials.ActiveDirectoryIp, credentials.Username, credentials.Password);
            }
            else
                config = new HadoopConfiguration(ActiveNameNode);
            if (!config.IsConfigurationExist())
            {
                return config.GenerateConfigFiles(true);
            }
            else
            {
                return true;
            }
        }

        private static IHadoop ConnectToLocalPseudoCluster()
        {
            LoadInpuFiles();
            IHadoop myCluster;
            myCluster = Hadoop.Connect();
            return myCluster;
        }

        private static IHadoop ConnectToRemoteCluster()
        {
            LoadInpuFiles();
            IHadoop myCluster;
            Uri uri = new Uri("http://" + ActiveNameNode);
            myCluster = Hadoop.Connect(uri);
            return myCluster;
        }

        private static IHadoop ConnectToSecuredRemoteCluster()
        {
            LoadInpuFiles();
            IHadoop myCluster;
            Uri uri = new Uri("http://" + ActiveNameNode);
            Uri uri2 = new Uri("http://" + credentials.ActiveDirectoryIp);
            myCluster = Hadoop.Connect(uri, uri2, credentials.Username, credentials.Password);
            return myCluster;
        }

        private static void UpdateExecutionStatus(int exitCode)
        {
            //write job result to console
            string exitStatus = (exitCode == 0) ? "Success" : "Failure";

            exitStatus = exitCode + " (" + exitStatus + ")";

            Console.WriteLine();

            Console.Write("Exit Code = " + exitStatus);
            Console.Read();
        }

        private static void LoadInpuFiles()
        {
            if (CheckFile("Data"))
            {
                if (!CheckFile("Data/NASA_Access_Log"))
                {
                    UploadFile();
                }
            }
            else
            {
                CreateFolder("Data");
                UploadFile();
            }

            if (CheckFile("user"))
            {
                if (!CheckFile("user/" + uName))
                {
                    CreateFolder("user/" + uName);
                }
            }
            else
            {
                CreateFolder("user/" + uName);
            }
        }

        private static void UploadFile()
        {
            try
            {
                string logText = string.Empty;
                string arg = string.Empty;
                Process proc;
                string commonHome = Directory.GetParent("..\\..\\..\\..\\..\\..\\..\\..\\").ToString();
                string ConfDirectories = string.Empty;
                if (IsRemote)
                {
                    ConfDirectories = commonHome + "\\SDK\\Hadoop\\etc\\hadoopclusterHA_" + ActiveNameNode;
                }
                else
                    ConfDirectories = commonHome + "\\SDK\\Hadoop\\etc\\hadoop";
                arg = "hdfs --config " + ConfDirectories + " dfs -put \"" + commonHome + "\\Samples\\Data\\NASA_Access_Log" + "" + "\" \\Data\"" + "/" + "\"";
                proc = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = "cmd.exe",
                        WorkingDirectory = commonHome + "\\SDK\\Hadoop\\bin",
                        Arguments = "/c " + arg,
                        UseShellExecute = false,
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        CreateNoWindow = true
                    }
                };
                if (IsSecured)
                {
                    proc.StartInfo.RedirectStandardInput = true;
                    proc.StartInfo.Arguments = "";
                    ConfDirectories = commonHome + "\\SDK\\Hadoop\\etc\\hadoopSecured_" + ActiveNameNode;
                    proc.Start();
                    proc.StandardInput.WriteLine("SET KRB5CCNAME=" + ConfDirectories + "\\UserTickets");
                    proc.StandardInput.WriteLine("hdfs --config " + ConfDirectories + " dfs -put \"" + commonHome + "\\Samples\\Data\\NASA_Access_Log" + "" + "\" \\Data\"" + "/" + "\"");
                    proc.StandardInput.WriteLine("exit");
                }
                else
                {
                    proc.Start();
                }
                if (!proc.StandardError.EndOfStream)
                {
                    logText += proc.StandardError.ReadToEnd() + "\n";
                }
                if (string.IsNullOrEmpty(logText))
                    Console.WriteLine("Input data uploaded");
                proc.WaitForExit();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            Console.WriteLine("Enter any key to continue....");
            Console.ReadKey();
        }

        private static void CreateFolder(string address)
        {
            if (IsSecured)
            {
                IPHostEntry ipHostEntry = Dns.GetHostEntry(ActiveNameNode);
                string hostName = ipHostEntry.HostName.ToString();
                if (!SendSecuredWebRequestAndResponse("https://" + hostName + ":50470/webhdfs/v1/" + address.TrimStart('/') + "?op=MKDIRS", "PUT", credentials))
                    Console.WriteLine("Unable to create directory " + address);
            }
            else
            {
                string sURL;
                sURL = "http://" + ActiveNameNode + ":50070/webhdfs/v1/" + address + "/?user.name=" + uName + "&op=MKDIRS";
                sURL = sURL.Replace("#", "%23");
                WebRequest wrGETURL;
                wrGETURL = WebRequest.Create(sURL);
                wrGETURL.Method = "PUT";
                try
                {
                    Stream objStream;
                    objStream = wrGETURL.GetResponse().GetResponseStream();
                }
                catch (Exception e)
                {
                    Console.WriteLine("Folder Creation Failed.\n" + e.Message);
                }
            }
        }

        private static bool CheckFile(string hdfsPath)
        {
            string response;
            if (IsSecured)
            {
                IPHostEntry ipHostEntry = Dns.GetHostEntry(ActiveNameNode);
                string hostName = ipHostEntry.HostName.ToString();
                return SendSecuredWebRequestAndResponse(("https://" + hostName + ":50470/webhdfs/v1/" + hdfsPath.TrimStart('/') + "?op=LISTSTATUS"), "GET", credentials);
            }
            else
                return TryGetResponse(("http://" + ActiveNameNode + ":50070/webhdfs/v1/" + hdfsPath.TrimStart('/') + "/?user.name=" + uName + "&op=LISTSTATUS").Replace("#", "%23").Replace("+", "%2b"), out response);
        }

        private static bool TryGetResponse(string url, out string response)
        {
            response = string.Empty;
            try
            {
                WebRequest request = WebRequest.Create(url);
                using (Stream objStream = request.GetResponse().GetResponseStream())
                {
                    if (objStream != null)
                    {
                        using (StreamReader objReader = new StreamReader(objStream))
                        {
                            response = objReader.ReadToEnd();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }

        private static bool SendSecuredWebRequestAndResponse(string sURL, string protocol, Credential credential)
        {
            HttpWebRequest connectionReq = null;
            try
            {
                ServicePointManager.ServerCertificateValidationCallback += ValidateServerCertificate;
                if (!string.IsNullOrEmpty(sURL))
                {
                    connectionReq = (HttpWebRequest)WebRequest.Create(sURL);
                    connectionReq.Credentials = new NetworkCredential(credential.Username, credential.Password, credential.DomainName);
                    connectionReq.Timeout = 10000;
                    connectionReq.Method = protocol;

                    using (var httpResponse = (HttpWebResponse)connectionReq.GetResponse())
                    {
                        httpResponse.Close();
                    }
                    return true;
                }
            }
            catch (Exception e)
            {

            }
            return false;
        }

        public static bool ValidateServerCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }
        #endregion

        #region Job Submission in HDInsight cluster

        public static void DoCustomMapReduce()
        {
            //The credentials entered below are dummy values. Please input valid credentials and submit jobs
            Environment.SetEnvironmentVariable("HADOOP_HOME", @"C:\Syncfusion\BigData\3.2.0.20\BigDataSDK\\SDK\Hadoop");
            Environment.SetEnvironmentVariable("JAVA_HOME", @"C:\Syncfusion\BigData\3.2.0.20\BigDataSDK\\Java\jdk1.7.0_51");
            //Pass the cluster name
            string clusterName = "https://{clustername}.azurehdinsight.net:";
            Uri azureCluster = new Uri(clusterName);
            string clusterUserName = "{username}"; // default - admin
            string clusterPassword = "{password}";

            //// This is the name of the account under which Hadoop will execute jobs.
            //// Normally this is just "Hadoop".
            string hadoopUserName = "{hadoopusername}";

            //// Azure Storage Information.
            string azureStorageAccount = "{storagename}.blob.core.windows.net";
            string azureStorageKey = "{storagekey}";
            string azureStorageContainer = "{storagecontainer}";



            //Console.WriteLine("Starting MapReduce job. Remote login to your Name Node and check progress from JobTracker portal with the returned JobID...");

            IHadoop hadoop = Hadoop.Connect(azureCluster, clusterUserName,
                            hadoopUserName, clusterPassword, azureStorageAccount,
                            azureStorageKey, azureStorageContainer, true);
            // Create or overwrite the "myblob" blob with contents from a local file.
            var fileStream = File.ReadAllText(@"..//..//data/NASA_Access_Log");
            hadoop.StorageSystem.WriteAllText(FindReplace._input1HDFS, fileStream);
            Console.WriteLine("Input file uploaded.......\n\n");
           Console.WriteLine("Find and Replace Operation.\n\nImplementation of Find and Replace operations in native MapReduce through C#");
            Console.WriteLine("Execution begins......\n");

            //connect to HDInsightcluster      
            MapReduceResult result = hadoop.MapReduceJob.ExecuteJob<FindReplace>();
            Console.WriteLine();
            Console.WriteLine("Job Run Information");
            Console.WriteLine();
            Console.WriteLine("Job Id: {0}", result.Id);
            Console.WriteLine("Exit Code: {0}", result.Info.ExitCode);
            Console.WriteLine("Standard Out");
            Console.WriteLine(result.Info.StandardOut);
            Console.WriteLine();
            Console.WriteLine("Standard Err");
            Console.WriteLine(result.Info.StandardError);
            Console.ReadKey();
        }

        #endregion
    }

 
}

