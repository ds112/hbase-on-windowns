#region Copyright Syncfusion Inc. 2001-2016.
// Copyright Syncfusion Inc. 2001-2016. All rights reserved.
// Use of this code is subject to the terms of our license.
// A copy of the current license can be obtained at any time by e-mailing
// licensing@syncfusion.com. Any infringement will be prosecuted under
// applicable laws. 
#endregion
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace RestOozie
{
    class Program
    {
        public static string commonHome;
        public static bool isSecured=false;
        public static string exitOption,userName,password;

        static void Main(string[] args)
        {
            do
            {
                string option;
                //Ensure if hostname connect with cluster, need to change 'localhost' to cluster IP in above property file
                Console.Write("Enter Namenode IP : ");
                string hostname = Console.ReadLine();
                Program.commonHome = Directory.GetParent("..\\..\\..\\..\\..\\..\\..\\").ToString();
                Console.WriteLine("Is Secured Cluster(Y/N):");
                option = Console.ReadLine();
                if (option.ToUpper() == "Y" || option.ToUpper() == "N")
                {
                    isSecured = (option.ToUpper() == "Y") ? true : false;
                    if (isSecured)
                    {
                        Console.WriteLine("Please enter the UserName(username@domainname) and Password\nUserName:");
                        userName = Console.ReadLine();
                        Console.WriteLine("password:");
                        password = Console.ReadLine();
                        ServicePointManager.ServerCertificateValidationCallback += ValidateServerCertificate;
                        hostname = GetHostName(hostname);
                    }
                    int i;
                    do
                    {
                        Console.WriteLine("Enter any operation");
                        Console.WriteLine("1. Submit oozie job");
                        Console.WriteLine("2. Manage oozie job");
                        Console.WriteLine("3. Exit");
                        i = int.Parse(Console.ReadLine());
                        switch (i)
                        {
                            case 1:
                                if (SubmitOozieJob(hostname, GetOozieProperties(hostname), ""))
                                    Console.WriteLine("Oozie job submitted");
                                else
                                    Console.WriteLine("Oozie job failed to submit");
                                break;
                            case 2:
                                Console.WriteLine("Enter Job ID");
                                string jobID = Console.ReadLine();
                                Console.WriteLine("Enter Job Action [KILL, SUSPEND, RESUME]");
                                string action = Console.ReadLine();
                                ManageOozieJob(hostname, jobID, action);
                                break;
                            case 3:
                                break;
                        }
                    } while (i != 3);
                }
                else
                {
                    Console.WriteLine("Invalid option");
                }
                Console.WriteLine("Press 'Y' to continue by connecting new cluster or Press any key to exit:");
                exitOption = Console.ReadLine();
                if (exitOption.ToUpper() != "Y")
                    break;
            } while (exitOption.ToUpper() == "Y");
        }

        public static bool SubmitOozieJob(string OozieServerIp, string properties, string error)
        {
            try
            {
                string url = !isSecured ? "http://" + OozieServerIp + ":11000/oozie/v1/jobs?action=start" :
                    "https://" + OozieServerIp + ":11443/oozie/v1/jobs?action=start";
                WebRequest request = WebRequest.Create(url);
                if (isSecured)
                    request.Credentials = new NetworkCredential(userName, password);
                request.Method = "POST";
                request.ContentType = "application/xml";
                if (!string.IsNullOrEmpty(properties))
                {
                    byte[] config = Encoding.UTF8.GetBytes(properties);
                    request.GetRequestStream().Write(config, 0, config.Length);
                }
                byte[] responseBytes = new byte[500];
                using (var response = request.GetResponse())
                {
                    response.GetResponseStream().Read(responseBytes, 0, responseBytes.Length);
                    response.Close();
                }
                error = DateTime.Now + "\nJob submitted successfully \nJob Id :" +
                        Encoding.UTF8.GetString(responseBytes).TrimEnd('\0');
                return true;
            }
            catch (WebException e)
            {
                HttpWebResponse response = (HttpWebResponse)e.Response;
                string responseHeaders = string.Empty;
                if (response != null)
                {
                    WebHeaderCollection headers = response.Headers;
                    foreach (var header in headers)
                    {
                        if (header.ToString() == "oozie-error-message")
                            responseHeaders += header + " : " + response.GetResponseHeader(header.ToString()) + "\n";
                    }
                }
                error = "\nJob submission failed. \n" + (string.IsNullOrEmpty(responseHeaders) ? "This may occur if the specified properties file is not valid or the Oozie server is not running." : responseHeaders);
                return false;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex);
                error = "Job submission failed. This may occur if the specified properties file is not valid or the Oozie server is not running.";
                return false;
            }
        }


        public static void ManageOozieJob(string OozieServerIp, string jobId, string jobAction)
        {
            try
            {
                string url = !isSecured ? "http://" + OozieServerIp + ":11000/oozie/v1/job/" + jobId + "?action=" + jobAction :
                    "https://" + OozieServerIp + ":11443/oozie/v1/job/" + jobId + "?action=" + jobAction;
                WebRequest request = WebRequest.Create(url);
                if(isSecured)
                    request.Credentials=new NetworkCredential(userName,password);
                request.Method = "PUT";
                using (var response = request.GetResponse())
                {
                    response.Close();
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex);
            }
        }

        public static string GetHostName(string ip)
        {
            try
            {
                System.Net.IPHostEntry hostEntry = System.Net.Dns.GetHostByAddress(ip);
                return hostEntry.HostName;
            }
            catch (Exception ex)
            {
                return ip;
            }
        }

        public static bool ValidateServerCertificate(object sender, X509Certificate certificate,
           X509Chain chain,
           SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }

        private static string GetOozieProperties(string hostName)
        {            
            try
            {
                return "<configuration>\n" +
                "<property>\n" +
                "        <name>user.name</name>\n" +
                "        <value>SYSTEM</value>\n" +
                "</property>\n" +
                "<property>\n" +
                "        <name>oozie.wf.application.path</name>\n" +
                "        <value>${nameNode}/Data/Oozie/Apps/Java-Mainworkflow.xml</value>\n" +
                "</property>\n" +
                "<property>\n" +
                "        <name>queueName</name>\n" +
                "        <value>default</value>\n" +
                "</property>\n" +
                "<property>\n" +
                "        <name>nameNode</name>\n" +
                "        <value>hdfs://" + hostName + ":9000</value>\n" +
                "</property>   \n" +
                "<property>\n" +
                "        <name>jobTracker</name>\n" +
                "        <value>" + hostName + ":8032</value>\n" +
                "</property>   \n" +
                "<property>\n" +
                "        <name>examplesRoot</name>\n" +
                "        <value>examples</value>\n" +
                "</property>\n" +
                "<property>\n" +
                "        <name>oozie.use.system.libpath</name>\n" +
                "        <value>true</value>\n" +
                "</property>\n" +
                "</configuration>";                
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }
    }
}
