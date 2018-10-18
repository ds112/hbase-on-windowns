using Microsoft.Spark.CSharp.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Syncfusion.CSharpRunner
{
    public class CSharpRunner
    {
        #region Variable Declaration
        private string studioHome;
        private string sparkHome;
        private string command ;
        public Process process;
        private BackgroundWorker outputworker = new BackgroundWorker();
        private BackgroundWorker errorworker = new BackgroundWorker();
        public bool IsCSharpRunnerStarted;
        private int port;
        private string assemblyLocation = string.Empty;
        private int defaultPort;
        private string configFile;
        private const string backendPortNumber = "1000";
        private const string workerPath = "AppPath";
        #endregion Variable Declaration

        #region Constructor
        /// <summary>
        /// 
        /// To initialize the variables
        /// </summary>
        public CSharpRunner(string sparkHome, string confFile)
        {
            IsCSharpRunnerStarted = false;
            this.sparkHome = sparkHome;
            assemblyLocation = System.Reflection.Assembly.GetExecutingAssembly().Location;
            defaultPort = 5567;
            command = "sparkclr-submit.cmd debug ";
            configFile = confFile;
        }

        #endregion Constructor
        
        #region Check Port availability

        /// <summary>
        /// Checks whether the port is listening or not.
        /// If not start command will be constructed with the port
        /// </summary>
        private void CheckPortAvailability()
        {
            string loopback = IPAddress.Loopback.ToString();
            port = defaultPort;
            for (int i=0;i<=5;i++)
            {
                if (!IsPortLive(loopback, defaultPort+i))
                {
                    port = defaultPort+i;
                    command = command + port;
                    break;
                }
            }
        }

        
        /// <summary>
        /// Checks whether the port is connected or not
        /// </summary>
        /// <param name="systemIp">Primary Namenode IP</param>
        /// <param name="port">Port Number</param>
        /// <returns>returns true,if the port is connected</returns>
        private static bool IsPortLive(string systemIp, int port)
        {
            try
            {
                using (var client = new TcpClient())
                {
                    var result = client.BeginConnect(systemIp, port, null, null);
                    var success = result.AsyncWaitHandle.WaitOne(10000);
                    if (!success)
                    {
                        return false;
                    }

                    client.EndConnect(result);
                }

            }
            catch(Exception ex)
            {
                return false;
            }
            return true;
        }

        #endregion Check Port availability

        #region StartCSharpRunner
        /// <summary>
        /// To start the CSharpRunner
        /// </summary>
        public void StartCsharpRunner()
        {
                if (process != null && !process.HasExited)
                    process.Kill();
                process = new Process();
                process.StartInfo.FileName = "cmd.exe";
                process.StartInfo.Arguments = command + "\n";
                process.StartInfo.WorkingDirectory = sparkHome;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardError = true;
                process.StartInfo.RedirectStandardInput = true;
                outputworker = new BackgroundWorker();
                errorworker = new BackgroundWorker();
                errorworker.DoWork += Errorworker_DoWork;
                outputworker.DoWork += Outputworker_DoWork;
                process.Start();
                outputworker.RunWorkerAsync();
                errorworker.RunWorkerAsync();
                process.StandardInput.WriteLine(command);
           
        }

        /// <summary>
        /// Called when OutputWorker background worker starts working
        /// </summary>
        /// <param name="sender">Object that holds the information about the sender</param>
        /// <param name="e">Object of the class DoWorkEventArgs</param>
        private void Outputworker_DoWork(object sender, DoWorkEventArgs e)
        {
            OutputReader();
        }

        /// <summary>
        /// Called when ErrorWorker background worker starts working
        /// </summary>
        /// <param name="sender">Object that holds the information about the sender</param>
        /// <param name="e">Object of the class DoWorkEventArgs</param>
        private void Errorworker_DoWork(object sender, DoWorkEventArgs e)
        {
            ErrorReader();
        }

        /// <summary>
        /// To check with the output logs from the command given
        /// </summary>
        private void OutputReader()
        {
            while (process != null && !process.HasExited)
            {
                byte[] buffer = new byte[1024];
                process.StandardOutput.BaseStream.Read(buffer, 0, buffer.Length);
                string response = System.Text.Encoding.Default.GetString(buffer).TrimEnd('\0');
                if (response.Contains("Backend running debug mode. Press enter to exit"))
                {
                    IsCSharpRunnerStarted = true;
                    errorworker.DoWork -= Errorworker_DoWork;
                    outputworker.DoWork -= Outputworker_DoWork;
                    break;
                }
            }

        }

        /// <summary>
        /// To check with the error logs from the command given
        /// </summary>
        private void ErrorReader()
        {
            while (process != null && !process.HasExited)
            {
                byte[] buffer = new byte[1024];
                process.StandardError.BaseStream.Read(buffer, 0, buffer.Length);
                string response = System.Text.Encoding.Default.GetString(buffer).TrimEnd('\0');
                if (response.Contains("Backend running debug mode. Press enter to exit"))
                {
                    IsCSharpRunnerStarted = true;
                    errorworker.DoWork -= Errorworker_DoWork;
                    outputworker.DoWork -= Outputworker_DoWork;
                    break;
                }
            }

        }

        #endregion StartCSharpRunner

        #region SetCSharpWorkerPath
        /// <summary>
        /// To set the CSharpWorkerPath in the config file
        /// </summary>
        private void SetCSharpWorkerPath()
        {
            string path = System.Configuration.ConfigurationManager.AppSettings["CSharpWorkerPath"];
            ExeConfigurationFileMap configFileMap = new ExeConfigurationFileMap();
            configFileMap.ExeConfigFilename = configFile;
            System.Configuration.Configuration config = ConfigurationManager.OpenMappedExeConfiguration(configFileMap, ConfigurationUserLevel.None);
            if (path != null)
            {
                if (path.Contains(workerPath))
                    path = path.Replace(workerPath, assemblyLocation.Remove(assemblyLocation.LastIndexOf('\\'))).ToString();
             
                config.AppSettings.Settings["CSharpWorkerPath"].Value = path.TrimEnd();
                config.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection("appSettings");
            }
        }

        #endregion SetCSharpWorkerPath

        #region SetCSharpBackendPortNumber
        /// <summary>
        /// To set the CSharpBackendPortNumber in the config file
        /// </summary>
        private void SetCSharpBackendPortNumber()
        {
            string cSharpBackendPortNumber = System.Configuration.ConfigurationManager.AppSettings["CSharpBackendPortNumber"];
            ExeConfigurationFileMap configFileMap = new ExeConfigurationFileMap();
            configFileMap.ExeConfigFilename = configFile;
            System.Configuration.Configuration config = ConfigurationManager.OpenMappedExeConfiguration(configFileMap, ConfigurationUserLevel.None);
            if (cSharpBackendPortNumber != null)
            {
                if (cSharpBackendPortNumber.Contains(backendPortNumber))
                    cSharpBackendPortNumber = cSharpBackendPortNumber.Replace(backendPortNumber, port.ToString()).ToString();
                else
                    cSharpBackendPortNumber = port.ToString();

                config.AppSettings.Settings["CSharpBackendPortNumber"].Value = cSharpBackendPortNumber.TrimEnd();
                config.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection("appSettings");
            }
        }

        #endregion SetCSharpBackendPortNumber

        #region UpdateConfigFile
        /// <summary>
        /// To update the config file with the key and value
        /// </summary>
        /// <param name="fileName"></param>
        public void UpdateConfigFile(string fileName)
        {
            CheckPortAvailability();
            SetCSharpWorkerPath();
            SetCSharpBackendPortNumber();
            if (File.Exists(fileName))
            {
                File.Delete(fileName);
                File.Copy(configFile, fileName);
            }
        }

        #endregion UpdateConfigFile
    }
}
