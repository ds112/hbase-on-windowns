#region Copyright Syncfusion Inc. 2001 - 2016
// Copyright Syncfusion Inc. 2001 - 2016. All rights reserved.
// Use of this code is subject to the terms of our license.
// A copy of the current license can be obtained at any time by e-mailing
// licensing@syncfusion.com. Any infringement will be prosecuted under
// applicable laws. 
#endregion
using System;
using System.Web;
using MVCSampleBrowser.Utils;
using System.IO;

namespace MVCSampleBrowser.Handler
{
    /// <summary>
    /// Summary description for Handler2
    /// </summary>
    public class Handler2 : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            
            DirectoryInfo webFolder;
            try
            {

                webFolder = new DirectoryInfo(context.Request.PhysicalApplicationPath).Parent;
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Exception while tracking 'Web' folder -{0}", ex.Message));
            }

            if (context.Request.QueryString["product"] == null && context.Request.QueryString["product"] == null)
                throw new Exception("Requirred Query string [product] to handle the request.");

            string path = string.Format("{0}\\{1}", webFolder.FullName, context.Request.QueryString["product"]);
            string productname = context.Request.QueryString["product"].ToString().ToLower();
            if (!Directory.Exists(path))
            {
                context.Response.Write(string.Format("http://js.syncfusion.com/demos/", productname));
            }
            else
            {
                DirectoryInfo productFolder = new DirectoryInfo(path);

                string port = null;
                string physicalPath = string.Format("{0}", productFolder.FullName);
                string prefixURL = string.Empty;//.Format("mvcsamplebrowser/{0}", productFolder.Name);

                try
                {
                    string frameWorks = System.Configuration.ConfigurationManager.AppSettings["FrameWork"];
                    if (frameWorks == "4.5")
                    {
                        CassiniWebServer.StartVersion45xWebServer(physicalPath, prefixURL, out port);
                    }
                    else if (frameWorks == "4.0")
                    {
                        CassiniWebServer.StartVersion4xWebServer(physicalPath, prefixURL, out port);
                    }
                    else
                    {
                        CassiniWebServer.StartVersion3xWebServer(physicalPath, prefixURL, out port);
                    }

                    context.Response.Write(string.Format("http://localhost:{0}", port));
                }
                catch (Exception ex)
                {
                    throw new Exception(string.Format("Error: Unable to start Webserver.web.exe, Message:{0}", ex.Message));
                }
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}