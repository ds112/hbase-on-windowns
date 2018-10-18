using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using Microsoft.Hadoop.MapReduce;
//using Microsoft.Hadoop.WebClient.WebHCatClient;
//using Microsoft.WindowsAzure.Storage;
//using Microsoft.WindowsAzure.Storage.Auth;
//using Microsoft.WindowsAzure.Storage.Blob;
using System.Diagnostics;
using System.Net;
using Newtonsoft.Json;
using System.Threading;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Xml;
using System.Web.Mvc;
using System.Text.RegularExpressions;

namespace MVCSampleBrowser
{
    //DateTime Operation.
    //Implementation of DateTime operations in native MapReduce through C#.
    //Result : Emit the number of logs recorded for each hour.

   public class CheckFileStatus 
    {
        public static string ErrorMessage
        { get; set; }
        string path;
        public string CheckFile(string path)
        {
            ErrorMessage = "";
            this.path = path;
            if (CheckFile(string.Empty, "Data"))
            {
                if (!CheckFile("Data/AdventureWorks/", "AdventureWorks_Person_Contact.csv"))
                {
                    UploadFile("Data/AdventureWorks/AdventureWorks_Person_Contact.csv");
                }
            }
            else
            {
                UploadFile("Data/AdventureWorks/AdventureWorks_Person_Contact.csv");
            }
            return ErrorMessage;
        }

        private string ip_address = "localhost";
        private WebRequest getUrl;
        private Stream objStream;
        private bool CheckFile(string address, string item)
        {
            string userName = Environment.UserName;
            bool exists = false;

            //Checks whether the username has space or special characters
            //In case if username contains space or special character, we use SYSTEM as the username
            var regexItem = new Regex("^[a-zA-Z0-9_-]*$");
            if (!regexItem.IsMatch(userName))
                userName = "SYSTEM";
            String url = "http://" + ip_address + ":50070/webhdfs/v1/" + address + "/?user.name=" + userName + "&op=LISTSTATUS";
            getUrl = WebRequest.Create(url);
          
            try
            {
                objStream = getUrl.GetResponse().GetResponseStream();
                StreamReader objReader = new StreamReader(objStream);
                string json = string.Empty, line = string.Empty;
                string sLine = "";

                while (sLine != null)
                {
                    sLine = objReader.ReadLine();
                    line += sLine;
                }

                dynamic results = JsonConvert.DeserializeObject(line);
                JArray jsonStr = results["FileStatuses"]["FileStatus"];
                JToken temp2 = null;

                foreach (JObject root in jsonStr)
                {

                    root.TryGetValue("pathSuffix", out temp2);
                    if (temp2.ToString() == item)
                    {
                        exists = true;
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex.Message == "Unable to connect to the remote server")
                {
                    ErrorMessage = "Please ensure that the Hadoop services are running and you can start the Hadoop services through Syncfusion service manager";
                }
                else if (ex.Message == "The remote server returned an error: (404) Not Found.")
                    exists = false;               
            }
            return exists;
        }
        private void UploadFile(string address)
        {
            string userName = Environment.UserName;
            var regexItem = new Regex("^[a-zA-Z0-9_-]*$");
            if (!regexItem.IsMatch(userName))
                userName = "SYSTEM";           
            string sURL;
            sURL = "http://" + ip_address + ":50070/webhdfs/v1/" + address + "/?user.name=" + userName + "&Op=CREATE";

            WebRequest wrGETURL;

            wrGETURL = WebRequest.Create(sURL);
            String line = "";
            wrGETURL.Method = "PUT";
            ControllerContext controller = new ControllerContext();
           
            using (StreamReader sr = new StreamReader(path))
            {
                line = sr.ReadToEnd();

            }
            byte[] data = System.Text.Encoding.ASCII.GetBytes(line);
            wrGETURL.ContentType = "application/octet-stream";
            wrGETURL.ContentLength = data.Length;

            try
            {
                Stream objStream = wrGETURL.GetRequestStream();
                objStream.Write(data, 0, data.Length);
                objStream.Close();
                objStream = wrGETURL.GetResponse().GetResponseStream();
            }
            catch (Exception e)
            {

            }



        }

    }
}