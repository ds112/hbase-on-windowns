#region Copyright Syncfusion Inc. 2001 - 2016

// Copyright Syncfusion Inc. 2001 - 2016. All rights reserved.
// Use of this code is subject to the terms of our license.
// A copy of the current license can be obtained at any time by e-mailing
// licensing@syncfusion.com. Any infringement will be prosecuted under
// applicable laws.

#endregion Copyright Syncfusion Inc. 2001 - 2016

using Microsoft.Win32;
using MVCSampleBrowser.Helpers;
using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;

/// <summary>
/// Summary description for Utils
/// </summary>
///
namespace MVCSampleBrowser.Utils
{
    #region Class StartDevelopmentWebServer

    public class CassiniWebServer
    {
        private const string iisExpressExename = "iisexpress.exe";
        private const string webServerExeName = "WebDev.WebServer.EXE";
        private const string Netfx20PathRegKeyName = "Software\\Microsoft\\.NETFramework";
        private const string Netfx20PathRegValueName = "InstallRoot";
        private const string SharedFolderPathRegKeyName = "Software\\Microsoft\\Shared Tools";
        private const string SharedFolderPathRegValueName = "SharedFilesDir";

        #region Public Methods

        public static void StartVersion20WebServer(string path, string vDirName, out string port)
        {
            if (!Directory.Exists(path))
                throw new Exception("Unable to locate sample directory.");
            string WebserverPath = string.Format("{0}\\{1}", GetMSBuildPath(), webServerExeName);
            if (File.Exists(WebserverPath))
            {
                StartWebServer(WebserverPath, path, vDirName, out port);
            }
            else
                throw new NullReferenceException("Unable to locate WebDev.WebServer.EXE");
        }

        public static void StartVersion3xWebServer(string path, string vDirName, out string port)
        {
            string WebserverPath = string.Format("{0}\\{1}", GetSharedFolderPath(), webServerExeName);
            if (File.Exists(WebserverPath))
            {
                StartWebServer(WebserverPath, path, vDirName, out port);
            }
            else
                throw new NullReferenceException("Unable to locate WebDev.WebServer.EXE");
        }

        public static void StartVersion4xWebServer(string path, string vDirName, out string port)
        {
            string webServerDir = string.Empty;
            webServerDir = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) + @"\Common Files\Microsoft Shared\DevServer\10.0";
            if (webServerDir == null)
            {
                webServerDir = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) + @"\Microsoft Shared\DevServer\10.0";
            }
            string WebserverPath = string.Format("{0}\\{1}", webServerDir, "WebDev.WebServer40.EXE");
            if (File.Exists(WebserverPath))
            {
                StartWebServer(WebserverPath, path, vDirName, out port);
            }
            else
                throw new NullReferenceException("Unable to locate WebDev.WebServer40.EXE");
        }

        public static void StartVersion45xWebServer(string path, string vDirName, out string port)
        {
            string webServerDir = string.Empty;
            webServerDir = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) + @"\Common Files\Microsoft Shared\DevServer\11.0";
            if (webServerDir == null)
            {
                webServerDir = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) + @"\Microsoft Shared\DevServer\11.0";
            }
            string WebserverPath = string.Format("{0}\\{1}", webServerDir, "WebDev.WebServer40.EXE");
            if (MvcLaunchPageExt.RequestedServer() != ServerMode.IISExpress && File.Exists(WebserverPath))
            {
                StartWebServer(WebserverPath, path, vDirName, out port);
            }
            else
            {
                string iisExpressPath = string.Format("{0}\\{1}", GetIISExpressPath(), iisExpressExename);
                if (File.Exists(iisExpressPath))
                {
                    StartIISExpress(iisExpressPath, path, vDirName, out port);
                }
                else
                {
                    throw new NullReferenceException("Unable to locate WebDev.WebServer.EXE");
                }
            }
        }

        #endregion Public Methods

        #region Private methods

        private static void StartWebServer(string WebserverPath, string samplepath, string vDirName, out string port)
        {
            string commandArgs = string.Empty;

            Random r = new Random();
            port = r.Next(1024, 9000).ToString();

            //grab the original path
            commandArgs += " /path:\"" + samplepath + "\"";
            commandArgs += " /port:";
            commandArgs += port;
            commandArgs += " /vpath:/" + vDirName;

            Process mDOSProcess = new Process();
            mDOSProcess.StartInfo.FileName = WebserverPath;
            mDOSProcess.StartInfo.Arguments = commandArgs;

            //if you dont want to see the dos black screen
            mDOSProcess.StartInfo.CreateNoWindow = true;
            mDOSProcess.StartInfo.UseShellExecute = false;

            //now start the process
            mDOSProcess.Start();
        }

        private static void StartIISExpress(string iisExpressPath, string samplepath, string vDirName, out string port)
        {
            string commandArgs = string.Empty;

            Random r = new Random();
            port = r.Next(1024, 9000).ToString();

            //grab the original path
            commandArgs += " /path:\"" + samplepath + "\"";
            commandArgs += " /port:";
            commandArgs += port;
            //commandArgs += " /vpath:/" + vDirName;

            Process mDOSProcess = new Process();
            mDOSProcess.StartInfo.FileName = iisExpressPath;
            mDOSProcess.StartInfo.Arguments = commandArgs;

            //////if you dont want to see the dos black screen
            mDOSProcess.StartInfo.CreateNoWindow = true;
            mDOSProcess.StartInfo.UseShellExecute = false;

            //now start the process
            mDOSProcess.Start();
        }

        private static string GetMSBuildPath()
        {
            string regValue = string.Empty;
            if (FrameworkVersionDetection.GetRegistryValue(RegistryHive.LocalMachine, Netfx20PathRegKeyName, Netfx20PathRegValueName, RegistryValueKind.String, out regValue))
            {
                System.Version curVersion = FrameworkVersionDetection.GetExactVersion(FrameworkVersion.Fx20);
                return string.Format("{0}{1}", regValue, string.Format("v{0}.{1}.{2}", curVersion.Major, curVersion.Minor, curVersion.Build));
            }
            else
                throw new NullReferenceException("Unable to find .NET Framework Root path.");
        }

        private static string GetIISExpressPath()
        {
            string programFilesFolder = Environment.GetEnvironmentVariable("PROGRAMFILES");
            return programFilesFolder + @"\IIS Express";
        }

        private static string GetSharedFolderPath()
        {
            string regValue = string.Empty;
            if (FrameworkVersionDetection.GetRegistryValue(RegistryHive.LocalMachine, SharedFolderPathRegKeyName, SharedFolderPathRegValueName, RegistryValueKind.String, out regValue))
            {
                return string.Format("{0}{1}", regValue, "DevServer\\9.0");
            }
            else
                throw new NullReferenceException("Unable to find Windows Shared folder path.");
        }

        #endregion Private methods
    }

    #endregion Class StartDevelopmentWebServer

    #region enum FrameworkVersion

    /// <summary>
    /// Specifies the .NET Framework versions
    /// </summary>
    public enum FrameworkVersion
    {
        /// <summary>
        /// .NET Framework 1.0
        /// </summary>
        Fx10,

        /// <summary>
        /// .NET Framework 1.1
        /// </summary>
        Fx11,

        /// <summary>
        /// .NET Framework 2.0
        /// </summary>
        Fx20,

        /// <summary>
        /// .NET Framework 3.0
        /// </summary>
        Fx30,

        /// <summary>
        /// .NET Framework 3.5 (Orcas)
        /// </summary>
        Fx35,

        /// <summary>
        /// .NET Framework 3.5 (SP1)
        /// </summary>
        Fx35Sp1,
    }

    #endregion enum FrameworkVersion

    #region class FrameworkVersionDetection

    /// <summary>
    /// Provides support for determining if a specific version of the .NET
    /// Framework runtime is installed and the service pack level for the
    /// runtime version.
    /// </summary>
    public static class FrameworkVersionDetection
    {
        #region class-wide fields

        private const string Netfx10RegKeyName = "Software\\Microsoft\\.NETFramework\\Policy\\v1.0";
        private const string Netfx10RegKeyValue = "3705";
        private const string Netfx10SPxMSIRegKeyName = "Software\\Microsoft\\Active Setup\\Installed Components\\{78705f0d-e8db-4b2d-8193-982bdda15ecd}";
        private const string Netfx10SPxOCMRegKeyName = "Software\\Microsoft\\Active Setup\\Installed Components\\{FDC11A6F-17D1-48f9-9EA3-9051954BAA24}";
        private const string Netfx10SPxRegValueName = "Version";
        private const string Netfx11RegKeyName = "Software\\Microsoft\\NET Framework Setup\\NDP\\v1.1.4322";
        private const string Netfx20RegKeyName = "Software\\Microsoft\\NET Framework Setup\\NDP\\v2.0.50727";
        private const string Netfx30RegKeyName = "Software\\Microsoft\\NET Framework Setup\\NDP\\v3.0\\Setup";
        private const string Netfx35RegKeyName = "Software\\Microsoft\\NET Framework Setup\\NDP\\v3.5";
        private const string Netfx11PlusRegValueName = "Install";
        private const string Netfx30PlusRegValueName = "InstallSuccess";
        private const string Netfx11PlusSPxRegValueName = "SP";
        private const string Netfx35PlusSPxRegValueName = "SP";
        private const string Netfx20PlusBuildRegValueName = "Increment";
        private const string Netfx30PlusVersionRegValueName = "Version";
        private const string Netfx35PlusBuildRegValueName = "Build";
        private const string Netfx30PlusWCFRegKeyName = Netfx30RegKeyName + "\\Windows Communication Foundation";
        private const string Netfx30PlusWPFRegKeyName = Netfx30RegKeyName + "\\Windows Presentation Foundation";
        private const string Netfx30PlusWFRegKeyName = Netfx30RegKeyName + "\\Windows Workflow Foundation";
        private const string Netfx30PlusWFPlusVersionRegValueName = "FileVersion";
        private const string CardSpaceServicesRegKeyName = "System\\CurrentControlSet\\Services\\idsvc";
        private const string CardSpaceServicesPlusImagePathRegName = "ImagePath";

        #endregion class-wide fields

        #region private and internal properties and methods

        #region methods

        #region GetRegistryValue

        public static bool GetRegistryValue<T>(RegistryHive hive, string key, string value, RegistryValueKind kind, out T data)
        {
            bool success = false;
            data = default(T);

            using (RegistryKey baseKey = RegistryKey.OpenRemoteBaseKey(hive, String.Empty))
            {
                if (baseKey != null)
                {
                    using (RegistryKey registryKey = baseKey.OpenSubKey(key, RegistryKeyPermissionCheck.ReadSubTree))
                    {
                        if (registryKey != null)
                        {
                            // If the key was opened, try to retrieve the value.
                            RegistryValueKind kindFound = registryKey.GetValueKind(value);
                            if (kindFound == kind)
                            {
                                object regValue = registryKey.GetValue(value, null);
                                if (regValue != null)
                                {
                                    data = (T)Convert.ChangeType(regValue, typeof(T), CultureInfo.InvariantCulture);
                                    success = true;
                                }
                            }
                        }
                    }
                }
            }
            return success;
        }

        #endregion GetRegistryValue

        #region GetNetfxExactVersion functions

        #region GetNetfx20ExactVersion

        private static System.Version GetNetfx20ExactVersion()
        {
            string regValue = String.Empty;

            // We can only get -1 if the .NET Framework is not
            // installed or there was some kind of error retrieving
            // the data from the registry
            System.Version fxVersion = new System.Version();

            if (GetRegistryValue(RegistryHive.LocalMachine, Netfx20RegKeyName, Netfx20PlusBuildRegValueName, RegistryValueKind.String, out regValue))
            {
                if (!String.IsNullOrEmpty(regValue))
                {
                    // In the strict sense, we are cheating here, but the registry key name itself
                    // contains the version number.
                    string[] versionTokens = Netfx20RegKeyName.Split(new string[] { "NDP\\v" }, StringSplitOptions.None);
                    if (versionTokens.Length == 2)
                    {
                        string[] tokens = versionTokens[1].Split('.');
                        if (tokens.Length == 3)
                        {
                            fxVersion = new System.Version(Convert.ToInt32(tokens[0], NumberFormatInfo.InvariantInfo), Convert.ToInt32(tokens[1], NumberFormatInfo.InvariantInfo), Convert.ToInt32(tokens[2], NumberFormatInfo.InvariantInfo), Convert.ToInt32(regValue, NumberFormatInfo.InvariantInfo));
                        }
                    }
                }
            }

            return fxVersion;
        }

        #endregion GetNetfx20ExactVersion

        #region GetNetfx30ExactVersion

        private static System.Version GetNetfx30ExactVersion()
        {
            string regValue = String.Empty;

            // We can only get the default version if the .NET Framework
            // is not installed or there was some kind of error retrieving
            // the data from the registry
            System.Version fxVersion = new System.Version();

            if (GetRegistryValue(RegistryHive.LocalMachine, Netfx30RegKeyName, Netfx30PlusVersionRegValueName, RegistryValueKind.String, out regValue))
            {
                if (!String.IsNullOrEmpty(regValue))
                {
                    fxVersion = new System.Version(regValue);
                }
            }

            return fxVersion;
        }

        #endregion GetNetfx30ExactVersion

        #region GetNetfx35ExactVersion

        private static System.Version GetNetfx35ExactVersion()
        {
            string regValue = String.Empty;

            // We can only get the default version if the .NET Framework
            // is not installed or there was some kind of error retrieving
            // the data from the registry
            System.Version fxVersion = new System.Version();

            if (GetRegistryValue(RegistryHive.LocalMachine, Netfx35RegKeyName, Netfx30PlusVersionRegValueName, RegistryValueKind.String, out regValue))
            {
                if (!String.IsNullOrEmpty(regValue))
                {
                    fxVersion = new System.Version(regValue);
                }
            }

            return fxVersion;
        }

        #endregion GetNetfx35ExactVersion

        #endregion GetNetfxExactVersion functions

        #region IsNetfxInstalled functions

        #region IsNetfx20Installed

        private static bool IsNetfx20Installed()
        {
            bool found = false;
            int regValue = 0;

            if (GetRegistryValue(RegistryHive.LocalMachine, Netfx20RegKeyName, Netfx11PlusRegValueName, RegistryValueKind.DWord, out regValue))
            {
                if (regValue == 1)
                {
                    found = true;
                }
            }

            return found;
        }

        public static bool IsNetfx35SP1Installed()
        {
            bool found = false;
            int regValue = 0;

            if (GetRegistryValue(RegistryHive.LocalMachine, Netfx35RegKeyName, Netfx35PlusSPxRegValueName, RegistryValueKind.DWord, out regValue))
            {
                if (regValue == 1)
                {
                    found = true;
                }
            }

            return found;
        }

        #endregion IsNetfx20Installed

        #region IsNetfx30Installed

        private static bool IsNetfx30Installed()
        {
            bool found = false;
            int regValue = 0;

            if (GetRegistryValue(RegistryHive.LocalMachine, Netfx30RegKeyName, Netfx30PlusRegValueName, RegistryValueKind.DWord, out regValue))
            {
                if (regValue == 1)
                {
                    found = true;
                }
            }

            return found;
        }

        #endregion IsNetfx30Installed

        #region IsNetfx35Installed

        private static bool IsNetfx35Installed()
        {
            bool found = false;
            int regValue = 0;

            if (GetRegistryValue(RegistryHive.LocalMachine, Netfx35RegKeyName, Netfx11PlusRegValueName, RegistryValueKind.DWord, out regValue))
            {
                if (regValue == 1)
                {
                    found = true;
                }
            }

            return found;
        }

        #endregion IsNetfx35Installed

        #endregion IsNetfxInstalled functions

        #endregion methods

        #endregion private and internal properties and methods

        #region public properties and methods

        #region methods

        #region IsInstalled

        #region IsInstalled(FrameworkVersion frameworkVersion)

        /// <summary>
        /// Determines if the specified .NET Framework version is installed
        /// on the local computer.
        /// </summary>
        /// <param name="frameworkVersion">One of the
        /// <see cref="FrameworkVersion"/> values.</param>
        /// <returns><see langword="true"/> if the specified .NET Framework
        /// version is installed; otherwise <see langword="false"/>.</returns>
        public static bool IsInstalled(FrameworkVersion frameworkVersion)
        {
            bool ret = false;

            switch (frameworkVersion)
            {
                case FrameworkVersion.Fx20:
                    ret = IsNetfx20Installed();
                    break;

                case FrameworkVersion.Fx30:
                    ret = IsNetfx30Installed();
                    break;

                case FrameworkVersion.Fx35:
                    ret = IsNetfx35Installed();
                    break;

                case FrameworkVersion.Fx35Sp1:
                    ret = IsNetfx35SP1Installed();
                    break;

                default:
                    break;
            }

            return ret;
        }

        #endregion IsInstalled(FrameworkVersion frameworkVersion)

        #endregion IsInstalled

        #region GetExactVersion

        #region GetExactVersion(FrameworkVersion frameworkVersion)

        /// <summary>
        /// Retrieves the exact version number for the specified .NET Framework
        /// version.
        /// </summary>
        /// <param name="frameworkVersion">One of the
        /// <see cref="FrameworkVersion"/> values.</param>
        /// <returns>A <see cref="Version">version</see> representing
        /// the exact version number for the specified .NET Framework version.
        /// If the specified .NET Frameowrk version is not found, a
        /// <see cref="Version"/> is returned that represents a 0.0.0.0 version
        /// number.
        /// </returns>
        public static System.Version GetExactVersion(FrameworkVersion frameworkVersion)
        {
            System.Version fxVersion = new System.Version();

            switch (frameworkVersion)
            {
                case FrameworkVersion.Fx20:
                    fxVersion = GetNetfx20ExactVersion();
                    break;

                case FrameworkVersion.Fx30:
                    fxVersion = GetNetfx30ExactVersion();
                    break;

                case FrameworkVersion.Fx35:
                    fxVersion = GetNetfx35ExactVersion();
                    break;

                default:
                    break;
            }

            return fxVersion;
        }

        #endregion GetExactVersion(FrameworkVersion frameworkVersion)

        #endregion GetExactVersion

        #endregion methods

        #endregion public properties and methods
    }

    #endregion class FrameworkVersionDetection

    public enum SampleVersion
    {
        v35
    }

    public class Utility
    {
        public static string GetSampleVersionFolderName(SampleVersion version)
        {
            string ver = string.Empty;
            switch (version)
            {
                case SampleVersion.v35:
                    ver = "3.5";
                    break;

                default:
                    ver = "3.5";
                    break;
            }
            return ver;
        }
    }

    public class ReportHelper
    {
        public static string GetReportPath(string path)
        {
            string serverPath = System.Web.HttpContext.Current.Server.MapPath("");
            string serverSamplePath = serverPath.Substring(0, serverPath.IndexOf("MVC"));
            serverSamplePath += @"Common\Data\EjReportTemplate\" + path;
            return serverSamplePath;
        }
    }
}