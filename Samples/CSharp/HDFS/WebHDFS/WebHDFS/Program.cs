#region Copyright Syncfusion Inc. 2001-2016.
// Copyright Syncfusion Inc. 2001-2016. All rights reserved.
// Use of this code is subject to the terms of our license.
// A copy of the current license can be obtained at any time by e-mailing
// licensing@syncfusion.com. Any infringement will be prosecuted under
// applicable laws. 
#endregion
using System;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Timers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace WebHDFS
{
    class Program
    {
        #region Variable declaration
        static string userName ,password,exitOption,domainName, hostName;
        static Timer checkClusterState;
        static string primaryNameNode = "localhost", standbyNameNode = "localhost";
        static ClusterType clusterType;
        static bool isHadoopServiceAlertShow = false;
        #endregion

        static void Main(string[] args)
        {
            try
            {
                clusterType = ClusterType.Normal;
                userName = password = exitOption = domainName = hostName = string.Empty;
                checkClusterState = new Timer();
                checkClusterState.Interval = 10000;
                checkClusterState.Elapsed += CheckClusterState_Elapsed;
                checkClusterState.Enabled = true;
                var commonHome = Directory.GetParent("..\\..\\..\\..\\..\\..\\..\\..\\").ToString();

                //To initialize normal cluster connection
                //InitializeNormalCluster("<Normal Cluster PrimaryNamenode IP>", "<Normal Cluster SecondaryNameNode IP>");

                //To initialize secured cluster connection
                //InitializeSecureClusterConnection("<Secured cluster PrimaryNameNode IP>", "<Secured cluster StandbyNameNode IP>", "<userName>", "<password>", "<domainName>");

                hostName = primaryNameNode;
                int hdfsOperation;
                do
                {
                    Console.WriteLine("Enter any operation");
                    Console.WriteLine("1. Upload");
                    Console.WriteLine("2. Delete");
                    Console.WriteLine("3. MoveFile");
                    Console.WriteLine("4. MakeDirectory");
                    Console.WriteLine("5. SetReplication");
                    Console.WriteLine("6. Exit");
                    hdfsOperation = int.Parse(Console.ReadLine());
                    var program = new Program();
                    switch (hdfsOperation)
                    {
                        case 1:
                            Console.WriteLine("Uploading file from '" + commonHome +
                                              "\\BigDataSDK\\Samples\\Data\\WarPeace.txt'" +
                                              " to hdfs location " + "'/Data/PracticeTemp.txt'");
                            program.UploadFile(hostName, clusterType,
                                commonHome + "\\BigDataSDK\\Samples\\Data\\WarPeace.txt",
                                "/Data/PracticeTemp.txt");
                            break;
                        case 2:
                            Console.WriteLine("Deleting file from hdfs location " + "'/Data/PracticeTemp.txt'");
                            if (program.DeleteFile(hostName, clusterType, "/Data/PracticeTemp.txt"))
                                Console.WriteLine("Delete operation completed");
                            else
                                Console.WriteLine("Delete operation Failed");
                            break;
                        case 3:
                            Console.WriteLine("Move file from hdfs location " +
                                              "'/Data/PracticeTemp.txt' to hdfs location " +
                                              "'/Data/PracticeTemp2.txt'");
                            if (program.MoveFile(hostName, clusterType, "/Data/PracticeTemp.txt",
                                "/Data/PracticeTemp2.txt"))
                                Console.WriteLine("Move operation completed");
                            else
                                Console.WriteLine("Move operation Failed");
                            break;
                        case 4:
                            Console.WriteLine("Make directory from hdfs location " + "'/Data/PracticeDirectory'");
                            if (program.CreateDirectory(hostName, clusterType, "/Data/PracticeDirectory"))
                                Console.WriteLine("Make directory operation completed");
                            else
                                Console.WriteLine("Make directory operation Failed");
                            break;
                        case 5:
                            Console.WriteLine("set replication 5 for hdfs location " +
                                              "'/Data/PracticeTemp.txt'");
                            if (program.SetReplication(hostName, clusterType, "/Data/PracticeTemp.txt", 5))
                                Console.WriteLine("Set replication operation completed");
                            else
                                Console.WriteLine("Set Replication operation Failed");
                            break;
                        case 6:
                            break;
                        default:
                            Console.WriteLine("Please select existing hdfs operation number.");
                            break;
                    }
                } while (hdfsOperation != 6);
            }
            catch
            {
                //TODO
            }
        }

        /// <summary>
        /// Upload file operation
        /// </summary>
        /// <param name="host"></param>
        /// <param name="clusterType"></param>
        /// <param name="fileToUpload"></param>
        /// <param name="hdfspathToUpload"></param>
        private void UploadFile(string host, ClusterType clusterType,string fileToUpload, string hdfspathToUpload)
        {
            try
            {
                string url = string.Empty;
                hdfspathToUpload = hdfspathToUpload.TrimEnd('/').TrimStart('/');

                //Creating the url
                url = (clusterType.Equals(ClusterType.Secure))
                    ? ("https://" + host + ":50470/webhdfs/v1/" + hdfspathToUpload + "/?op=CREATE&data=true").Replace(
                        "#", "%23")
                    : (clusterType.Equals(ClusterType.Azure))
                        ? ("https://" + host + ":8000/webhdfs/v1/" + hdfspathToUpload + "/?user.name=" + userName +
                           "&op=CREATE&data=true").Replace("#", "%23").Replace("+", "%2b")
                        : ("http://" + host + ":50070/webhdfs/v1/" + hdfspathToUpload +
                           "/?user.name=SYSTEM&op=CREATE&data=true").Replace("#", "%23");

                var webRequest = WebRequest.Create(url);
                if (clusterType.Equals(ClusterType.Secure))
                    webRequest.Credentials = new NetworkCredential(userName, password);
                else if (clusterType.Equals(ClusterType.Azure))
                {
                    webRequest.Headers.Add("Username", userName);
                    webRequest.Headers.Add("Password", password);
                }
                webRequest.Method = "PUT";
                const int chunkSize = 1024 * 1024; // read the file by chunks of 10 MB
                using (var file = new FileStream(fileToUpload, FileMode.Open, FileAccess.Read))
                {
                    var buffer = new byte[chunkSize];
                    long fileLength = file.Length;
                    if (fileLength <= chunkSize)
                        buffer = new byte[file.Length];
                    webRequest.ContentType = "application/octet-stream";
                    webRequest.ContentLength = buffer.Length;
                    file.Read(buffer, 0, buffer.Length);
                    Stream objStream = webRequest.GetRequestStream();
                    objStream.Write(buffer, 0, buffer.Length);

                    objStream.Close();
                    objStream = webRequest.GetResponse().GetResponseStream();
                    url = (clusterType.Equals(ClusterType.Secure))
                        ? ("https://" + host + ":50470/webhdfs/v1/" + hdfspathToUpload +
                           "/?op=SETREPLICATION&replication=1").Replace("#", "%23")
                        : (clusterType.Equals(ClusterType.Azure))
                            ? ("https://" + host + ":8000/webhdfs/v1/" + hdfspathToUpload + "/?user.name=SYSTEM" +
                               "&op=SETREPLICATION&replication=1").Replace("#", "%23")
                            : ("http://" + host + ":50070/webhdfs/v1/" + hdfspathToUpload +
                               "/?user.name=SYSTEM" + "&op=SETREPLICATION&replication=1").Replace("#", "%23");
                    SendWebRequest(url, "PUT",clusterType);

                    buffer = (file.Length - file.Position) > chunkSize ? new byte[chunkSize] : new byte[(file.Length - file.Position)];
                    while (file.Read(buffer, 0, buffer.Length) > 0)
                    {
                        //Append File
                        url = (clusterType.Equals(ClusterType.Secure))
                            ? ("https://" + host + ":50470/webhdfs/v1/" +
                               hdfspathToUpload +
                               "/?Op=APPEND&buffersize=" + webRequest.ContentLength)
                            : (clusterType.Equals(ClusterType.Azure))
                                ? ("https://" + host + ":8000/webhdfs/v1/" + hdfspathToUpload + "/?user.name=" +
                                   userName + "&op=APPEND&data=true")
                                : ("http://" + host + ":50070/webhdfs/v1/" +
                                   hdfspathToUpload +
                                   "/?user.name=SYSTEM&Op=APPEND&buffersize=" + webRequest.ContentLength);
                        WebRequest webRequest1 = WebRequest.Create(url);
                        webRequest1.Credentials = new NetworkCredential(userName, password);
                        webRequest1.Method = "POST";
                        webRequest1.ContentType = "application/octet-stream";
                        webRequest1.ContentLength = buffer.Length;
                        Stream objStreamAppend = webRequest1.GetRequestStream();
                        objStreamAppend.Write(buffer, 0, buffer.Length);
                        objStreamAppend.Close();
                        webRequest1.GetResponse().GetResponseStream();
                        buffer = (file.Length - file.Position) > chunkSize ? new byte[chunkSize] : new byte[(file.Length - file.Position)];
                    }
                }
                if (host == "localhost" || host == "127.0.0.1")
                    url = ("http://" + host + ":50070/webhdfs/v1/" + hdfspathToUpload +
                         "/?user.name=SYSTEM" + "&op=SETREPLICATION&replication=1").Replace("#", "%23");
                else
                {
                    url = (clusterType.Equals(ClusterType.Secure))
                        ? ("https://" + host + ":50470/webhdfs/v1/" + hdfspathToUpload +
                           "/?op=SETREPLICATION&replication=3").Replace("#", "%23")
                        : (clusterType.Equals(ClusterType.Azure))
                            ? ("https://" + host + ":8000/webhdfs/v1/" + hdfspathToUpload + "/?user.name=SYSTEM" +
                               "&op=SETREPLICATION&replication=3").Replace("#", "%23")
                            : ("http://" + host + ":50070/webhdfs/v1/" + hdfspathToUpload +
                               "/?user.name=SYSTEM" + "&op=SETREPLICATION&replication=3").Replace("#", "%23");
                }
                SendWebRequest(url, "PUT",clusterType);
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(
                    "/**UploadHdfsFile(string nameNodeIP, string fileToUpload, string hdfspathToUpload)**/" +
                    e.Message + "\n" + e.StackTrace);
            }
        }

        /// <summary>
        /// Deletes a selected file
        /// </summary>
        /// <param name="clusterType"></param>
        /// <param name="filePath">HDFS path of file needs to be deleted</param>
        /// <param name="host"></param>
        /// <returns>If Delete succeeds returns true else returns false</returns>
        public bool DeleteFile(string host, ClusterType clusterType, string filePath)
        {
            string url = (clusterType.Equals(ClusterType.Secure))
                ? ("https://" + host + ":50470/webhdfs/v1/" + filePath.TrimStart('/') + "/?op=DELETE&recursive=true")
                    .Replace("#", "%23")
                : (clusterType.Equals(ClusterType.Azure))
                    ? ("https://" + host + ":8000/webhdfs/v1/" + filePath.TrimStart('/') + "/?user.name=" + userName +
                       "&op=DELETE&recursive=true").Replace("#", "%23").Replace("+", "%2b")
                    : ("http://" + host + ":50070/webhdfs/v1/" + filePath.TrimStart('/') +
                       "/?user.name=SYSTEM&op=DELETE&recursive=true").Replace("#", "%23").Replace("+", "%2b");
            return SendWebRequest(url, "DELETE", clusterType);
        }

        /// <summary>
        /// Creates a new folder
        /// </summary>
        /// <param name="clusterType"></param>
        /// <param name="folderPath">HDFS Path of folder to be created</param>
        /// <param name="host"></param>
        /// <returns>If folder is created returns true else returns false</returns>
        public bool CreateDirectory(string host, ClusterType clusterType, string folderPath)
        {
            string url = (clusterType.Equals(ClusterType.Secure))
                ? ("https://" + host + ":50470/webhdfs/v1/" + folderPath.TrimStart('/') + "/?op=MKDIRS").Replace("#",
                    "%23")
                : (clusterType.Equals(ClusterType.Azure))
                    ? ("https://" + host + ":8000/webhdfs/v1/" + folderPath.TrimStart('/') + "/?user.name=" + userName +
                       "&op=MKDIRS").Replace("#", "%23").Replace("+", "%2b")
                    : ("http://" + host + ":50070/webhdfs/v1/" + folderPath.TrimStart('/') +
                       "/?user.name=SYSTEM&op=MKDIRS")
                        .Replace("#", "%23");
            return SendWebRequest(url, "PUT",clusterType);

        }

        /// <summary>
        ///  Renames Selected file/folder(s)
        /// </summary>
        /// <param name="host"></param>
        /// <param name="clusterType"></param>
        /// <param name="sourceName">Current file name</param>
        /// <param name="targetName">New name for the file</param>
        /// <returns>If Rename succeeds returns true else returns false</returns>
        public bool MoveFile(string host, ClusterType clusterType,string sourceName, string targetName)
        {
            string commonHome = Directory.GetParent("..\\..\\..\\..\\..\\..\\..\\..\\").ToString();
            UploadFile(host, clusterType, commonHome + "\\BigDataSDK\\Samples\\Data\\WarPeace.txt", targetName);
            DeleteFile(host, clusterType, "/Data/PracticeTemp.txt");
            return true;
        }

        /// <summary>
        ///  Renames Selected file/folder(s)
        /// </summary>
        /// <param name="host"></param>
        /// <param name="clusterType"></param>
        /// <param name="sourceName">Current file name</param>
        /// <param name="replication"></param>
        /// <returns>If Rename succeeds returns true else returns false</returns>
        public bool SetReplication(string host, ClusterType clusterType, string sourceName, int replication)
        {
            string url = (clusterType.Equals(ClusterType.Secure))
                ? ("https://" + host + ":50470/webhdfs/v1" + sourceName + "/?op=SETREPLICATION&replication=" +
                   replication).Replace("#", "%23")
                : (clusterType.Equals(ClusterType.Azure))
                    ? ("https://" + host + ":8000/webhdfs/v1/" + sourceName + "/?user.name=SYSTEM" +
                       "&op=SETREPLICATION&replication=" + replication).Replace("#", "%23")
                    : ("http://" + host + ":50070/webhdfs/v1" + sourceName +
                       "/?user.name=SYSTEM&op=SETREPLICATION&replication=" + replication).Replace("#", "%23");
            return SendWebRequest(url, "PUT", clusterType);
        }

        /// <summary>
        /// Send webrequest for given URL and protocol
        /// </summary>
        /// <param name="url">URL</param>
        /// <param name="protocol">Protocol</param>
        /// <param name="clusterType"></param>
        /// <returns>Returns success status of web request <true>return true if success</true></returns>
        private bool SendWebRequest(string url, string protocol, ClusterType clusterType)
        {
            try
            {
                WebRequest request = WebRequest.Create(url);
                if (clusterType.Equals(ClusterType.Secure))
                    request.Credentials = new NetworkCredential(userName, password);
                else if (clusterType.Equals(ClusterType.Azure))
                {
                    request.Headers.Add("Username", userName);
                    request.Headers.Add("Password", password);
                }
                request.Method = protocol;
                using (var response = request.GetResponse())
                {
                    response.Close();
                }
                return true;
            }
            catch (Exception e)
            {
                Console.Error.WriteLine("/**SendWebRequest(string url, string protocol)**/" + e.Message + "\n" + e.StackTrace);
                return false;
            }
        }

        /// <summary>
        /// Certificate validation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="certificate"></param>
        /// <param name="chain"></param>
        /// <param name="sslPolicyErrors"></param>
        /// <returns></returns>
        public static bool ValidateServerCertificate(object sender, X509Certificate certificate,
            X509Chain chain,
            SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }

        /// <summary>
        /// Used to get host name
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static string GetHostName(string ip)
        {
            try
            {
                var hostEntry = Dns.GetHostByAddress(ip);
                return hostEntry.HostName;
            }
            catch
            {
                return ip;
            }
        }


        #region Initialize cluster details

        /// <summary>
        /// To initialize secure cluster details
        /// </summary>
        /// <param name="primaryNameNode">It define the primary NameNode IP</param>
        /// <param name="secondaryNameNode">It define the secondary NameNode IP</param>
        /// <param name="userName">It define the secure cluster username</param>
        /// <param name="password">It define the secure cluster password</param>
        /// <param name="domainName">It define the secure cluster domainName</param>
        private static void InitializeSecureClusterConnection(string primaryNameNode, string secondaryNameNode, string userName, string password, string domainName)
        {
            Program.primaryNameNode = primaryNameNode;
            Program.standbyNameNode = secondaryNameNode;
            Program.clusterType = ClusterType.Secure;
            Program.userName = userName;
            Program.password = password;
            Program.domainName = domainName;
            Program.userName = GetUsernameWithDomain(userName, domainName);
            ServicePointManager.ServerCertificateValidationCallback += ValidateServerCertificate;
            Program.hostName = GetHostName(hostName);
        }

        /// <summary>
        /// To initialize normal cluster details.
        /// </summary>
        /// <param name="primaryNameNode">It define the normal cluster primaryNameNode IP</param>
        /// <param name="secondaryNameNode">It define the normal cluster secondaryNameNode IP</param>
        private static void InitializeNormalCluster(string primaryNameNode, string secondaryNameNode)
        {
            Program.primaryNameNode = primaryNameNode;
            Program.standbyNameNode = secondaryNameNode;
            Program.clusterType = ClusterType.Normal;
        }

        #endregion

        #region Get Response methods
        /// <summary>
        /// Triggerd when check cluster status timer getting enabled in every 10seconds.
        /// </summary>
        /// <param name="sender">Object that holds the information about the sender</param>
        /// <param name="e">Object of the class ElapsedEventArgs</param>
        private static void CheckClusterState_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (IsNameNodeActive(hostName) != "active")
            {
                if (IsNameNodeActive(standbyNameNode) == "active")
                {
                    hostName = GetHostName(standbyNameNode);
                    standbyNameNode = primaryNameNode;
                    primaryNameNode = hostName;
                    isHadoopServiceAlertShow = false;
                }
                else
                {
                    if (!isHadoopServiceAlertShow)
                    {
                        isHadoopServiceAlertShow = true;
                        if (primaryNameNode == "localhost")
                            Console.WriteLine("Hadoop services are not running. Please start the services to continue.");
                        else
                            Console.WriteLine("Unable to communicate with the Hadoop cluster. Please ensure that the cluster is active.");
                    }
                }
            }
        }

        /// <summary>
        /// Check the status of the NameNode
        /// </summary>
        /// <param name="hostName">It defines the host name of the connected cluster</param>
        /// <returns>returns the namonode status.</returns>
        private static string IsNameNodeActive(string hostName)
        {
            try
            {
                string nodeStatus = "";
                string nameSystem = GetNameSystemResponse(hostName);
                if (!string.IsNullOrEmpty(nameSystem))
                {
                    JToken status = null;
                    dynamic results = JsonConvert.DeserializeObject(nameSystem);
                    JArray clusterDetailsArray = results["beans"];
                    foreach (var jToken in clusterDetailsArray)
                    {
                        var root = (JObject)jToken;
                        root.TryGetValue("tag.HAState", out status);
                    }
                    nodeStatus = status.ToString();
                }
                return nodeStatus;
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Set the Url based on the cluster and returns the response
        /// </summary>
        /// <param name="hostName">It defines the host name of the connected cluster</param>
        /// <returns>returns the system response.</returns>
        private static string GetNameSystemResponse(string hostName)
        {
            string namenodeStatus = string.Empty;
            if (clusterType == ClusterType.Normal)
                namenodeStatus = GetResponse("http://" + hostName + ":50070/jmx?qry=Hadoop:service=NameNode,name=FSNamesystem");
            else if (clusterType == ClusterType.Secure)
                namenodeStatus = GetSecuredWebRequestAndResponse("https://" + hostName + ":50470/jmx?qry=Hadoop:service=NameNode,name=FSNamesystem");
            else if (clusterType == ClusterType.Azure)
                namenodeStatus = GetResponse("https://" + hostName + ":8000/jmx?qry=Hadoop:service=NameNode,name=FSNamesystem");
            return namenodeStatus;
        }

        #endregion

        #region To get Response for the url

        /// <summary>
        /// Gets response for given url
        /// </summary>
        /// <param name="url">It define name node status check url</param>
        /// <returns>Returns data from URL</returns>
        private static string GetResponse(string url)
        {
            try
            {
                WebRequest request = WebRequest.Create(url);
                request.Method = "GET";
                using (Stream objStream = request.GetResponse().GetResponseStream())
                {
                    using (StreamReader objReader = new StreamReader(objStream))
                    {
                        return objReader.ReadToEnd();
                    }
                }
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Gets response for given secured url
        /// </summary>
        /// <param name="url">It define name node status check url</param>
        /// <returns>Returns data from URL</returns>
        private static string GetSecuredWebRequestAndResponse(string sURL)
        {
            string error = string.Empty;
            HttpWebRequest connectionReq = null;
            try
            {
                if (!string.IsNullOrEmpty(sURL))
                {
                    connectionReq = (HttpWebRequest)WebRequest.Create(sURL);
                    connectionReq.Credentials = new NetworkCredential(userName, password);
                    connectionReq.Timeout = 100000;
                    var httpResponse = (HttpWebResponse)connectionReq.GetResponse();
                    var dataStream = httpResponse.GetResponseStream();
                    var reader = new StreamReader(dataStream);
                    var responseFromServer = reader.ReadToEnd();
                    return responseFromServer;
                }
                return string.Empty;
            }
            catch (WebException wex)
            {
                var Pagecontent = string.Empty;
                var response = (HttpWebResponse)wex.Response;
                if (wex.Response != null)
                {
                    Pagecontent = new StreamReader(wex.Response.GetResponseStream()).ReadToEnd();
                }
                try
                {
                    if (response.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        try
                        {
                            connectionReq = (HttpWebRequest)WebRequest.Create(wex.Response.ResponseUri.AbsoluteUri);
                            connectionReq.Credentials = new NetworkCredential(userName, password, domainName);
                            connectionReq.Timeout = 100000;
                            var httpResponse = (HttpWebResponse)connectionReq.GetResponse();
                            var dataStream = httpResponse.GetResponseStream();
                            var reader = new StreamReader(dataStream);
                            var responseFromServer = reader.ReadToEnd();
                            return responseFromServer;
                        }
                        catch (WebException internalWebEx)
                        {

                            if (internalWebEx.Response != null)
                            {
                                Pagecontent = new StreamReader(internalWebEx.Response.GetResponseStream()).ReadToEnd();
                            }
                        }
                    }
                }
                catch
                {
                }
                return "Fail; " + Pagecontent;
            }
            catch (Exception e)
            {
                return "Fail; " + e;
            }
        }

        /// <summary>
        /// Get the Username of the cluster with domain name.
        /// </summary>
        /// <param name="username">It defines the username of the cluster.</param>
        /// <param name="domain">It defines the domain name of the cluster name.</param>
        /// <returns>It returns the username with domain name.</returns>
        private static string GetUsernameWithDomain(string username, string domain)
        {
            if (string.IsNullOrEmpty(domain))
                return username;
            if (username.Contains("@") && username.Contains(domain))
                return username;
            return username + "@" + domain;
        }

        #endregion
    }

    public enum ClusterType
    {
        Normal,
        Secure,
        Azure
    }
}
