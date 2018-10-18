#region Copyright Syncfusion Inc. 2001-2016.
// Copyright Syncfusion Inc. 2001-2016. All rights reserved.
// Use of this code is subject to the terms of our license.
// A copy of the current license can be obtained at any time by e-mailing
// licensing@syncfusion.com. Any infringement will be prosecuted under
// applicable laws. 
#endregion
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessHDFS
{
    class Program
    {
        public static string commonHome;
        bool IsSuccess;
        static string hadoopHome;
        static void Main(string[] args)
        {
            Program p = new Program();
            Program.commonHome = Directory.GetParent("..\\..\\..\\..\\..\\..\\..\\").ToString();
            Program.hadoopHome = Program.commonHome + "\\SDK\\Hadoop";
            string hadoopConf = Program.hadoopHome + "\\etc\\hadoop";
            Console.WriteLine("Enter any operation");
            Console.WriteLine("1. Upload");
            Console.WriteLine("2. Delete");
            Console.WriteLine("3. MoveFile");
            Console.WriteLine("4. MakeDirectory");
            Console.WriteLine("5. SetReplication");
            Console.WriteLine("6. Exit");
            int i = int.Parse(Console.ReadLine());
            
            while (i != 6)
            {
                switch (i)
                {
                    case 1:
                        Console.WriteLine("Uploading file from '" + Program.commonHome + "\\Samples\\Data\\WarPeace.txt'" + " to hdfs location " + "'/Data/PracticeTemp.txt'");
                        if (p.UploadFile(hadoopConf, Program.commonHome + "\\Samples\\Data\\WarPeace.txt", "/Data/PracticeTemp.txt"))
                            Console.WriteLine("Upload operation completed");
                        else
                            Console.WriteLine("Upload operation Failed");
                        break;
                    case 2:
                        Console.WriteLine("Deleting file from hdfs location " + "'/Data/PracticeTemp.txt'");
                        if (p.DeleteFile(hadoopConf, "/Data/PracticeTemp.txt"))
                            Console.WriteLine("Delete operation completed");
                        else
                            Console.WriteLine("Delete operation Failed");
                        break;
                    case 3:
                        Console.WriteLine("Move file from hdfs location " + "'/Data/PracticeTemp.txt' to hdfs location " + "'/Data/PracticeTemp2.txt'");
                        if (p.MoveFile(hadoopConf, "/Data/PracticeTemp.txt", "/Data/PracticeTemp2.txt"))
                            Console.WriteLine("Move operation completed");
                        else
                            Console.WriteLine("Move operation Failed");
                        break;
                    case 4:
                        Console.WriteLine("Make directory from hdfs location " + "'/Data/PracticeDirectory'");
                        if (p.CreateDirectory(hadoopConf, "/Data/PracticeDirectory"))
                            Console.WriteLine("Make directory operation completed");
                        else
                            Console.WriteLine("Make directory operation Failed");
                        break;
                    case 5:
                        Console.WriteLine("set replication 5 for hdfs location " + "'/Data/PracticeTemp.txt'");
                        if (p.SetReplication(hadoopConf, "/Data/PracticeTemp.txt", 5))
                            Console.WriteLine("Set replication operation completed");
                        else
                            Console.WriteLine("Set Replication operation Failed");
                        break;
                    case 6:
                        break;
                }
                Console.WriteLine("Enter any operation");
                Console.WriteLine("1. Upload");
                Console.WriteLine("2. Delete");
                Console.WriteLine("3. MoveFile");
                Console.WriteLine("4. MakeDirectory");
                Console.WriteLine("5. SetReplication");
                Console.WriteLine("6. Exit");
                i = int.Parse(Console.ReadLine());
            }
        }

        private bool SetReplication(string ConfDirectories, string filePath, int replication)
        {
            try
            {
                string logText = string.Empty;
                string UploadLog = string.Empty;
                RunCommandProcess("hdfs --config " + ConfDirectories + " dfs -setrep 5 \"" + filePath + "\"");
                IsSuccess = true; 
            }
            catch (Exception e)
            {
                IsSuccess = false;
                Console.Error.WriteLine("/**SetReplication log*/" + e.Message);
            }
            return IsSuccess;
        }

        private bool CreateDirectory(string ConfDirectories, string filePath)
        {
            try
            {
                string logText = string.Empty;
                string UploadLog = string.Empty;
                RunCommandProcess("hdfs --config " + ConfDirectories + " dfs -mkdir \"" + filePath + "\"");
                IsSuccess = true;                               
            }
            catch (Exception e)
            {
                IsSuccess = false;
                Console.Error.WriteLine("/**CreateDirectory log*/" + e.Message);
            }
            return IsSuccess;
        }

        private bool MoveFile(string ConfDirectories, string filePath, string dstPath)
        {
            try
            {
                string logText = string.Empty;
                string UploadLog = string.Empty;
                RunCommandProcess("hdfs --config " + ConfDirectories + " dfs -mv \"" + filePath + "\" \""+ dstPath+"\"");
                IsSuccess = true;               
            }
            catch (Exception e)
            {
                IsSuccess = false;
                Console.Error.WriteLine("/**MoveFile log*/" + e.Message);
            }
            return IsSuccess;
        }

        private bool DeleteFile(string ConfDirectories, string filePath)
        {
            try
            {
                string logText = string.Empty;
                string UploadLog = string.Empty;
                RunCommandProcess("hdfs --config " + ConfDirectories + " dfs -rmr \"" + filePath + "\"");
                IsSuccess = true;               
            }
            catch (Exception e)
            {
                IsSuccess = false;
                Console.Error.WriteLine("/**Delete log*/" + e.Message);
            }
            return IsSuccess;
        }

        private bool UploadFile(string ConfDirectories, string file, string pathUpload)
        {
            try
            {
                string logText = string.Empty;
                string UploadLog = string.Empty;
                RunCommandProcess("hdfs --config " + ConfDirectories + " dfs -put \"" + file + "\" \"" + pathUpload + "\"");
                IsSuccess = true;               
            }
            catch (Exception e)
            {
                IsSuccess = false;
                Console.Error.WriteLine("/**Upload log*/" + e.Message);
            }
            return IsSuccess;
        }

        /// <summary>
        /// Starts a command process for given argument
        /// </summary>
        /// <param name="arg">Command process argument</param>
        private void RunCommandProcess(string command)
        {
            try
            {
                Process scriptProcess = new Process
                {
                    StartInfo =
                    {
                        FileName = "cmd.exe",
                        WorkingDirectory = hadoopHome.Trim('\\') + "\\bin",
                        UseShellExecute = false,
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        RedirectStandardInput = true,
                        CreateNoWindow = true,
                    }
                };
                scriptProcess.StartInfo.Arguments = command.StartsWith("/C ") ? command : "/C " + command;
                scriptProcess.OutputDataReceived += (sender, e) =>
                {
                    if (e.Data != null)
                        Console.WriteLine(e.Data.ToString() + "\n");
                };
                scriptProcess.ErrorDataReceived += (sender, e) =>
                {
                    if (e.Data != null)
                        Console.WriteLine(e.Data.ToString() + "\n");
                };
                scriptProcess.Start();
                scriptProcess.BeginOutputReadLine();
                scriptProcess.BeginErrorReadLine();
                scriptProcess.WaitForExit();
            }
            catch (Exception ex)
            {
                //TODO
            }
        }
    }
}
