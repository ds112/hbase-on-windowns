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

namespace ProcessSqoop
{
    class Program
    {
        public static string commonHome;
        bool IsSuccess;
        static string sqoopHome;
        static void Main(string[] args)
        {
            Program p = new Program();
            Program.commonHome = Directory.GetParent("..\\..\\..\\..\\..\\..\\..\\").ToString();
            Program.sqoopHome = Program.commonHome + "\\SDK\\Sqoop";
            String command = String.Empty;
            //Import command
            //ConnectionString = [jdbc:mysql://<host>:port, jdbc:sqlserver://<host>:port]
            //sqoop import --connect jdbc:mysql://localhost/userdb --username root --table emp --m 1            
            command = "sqoop import --connect <ConnectionString>/<database> --username <userName> --password <password> --table <tableName> --target-dir <hdfsDirectory> --m 1";
            p.SqoopSubmit(command);
            

            //Export command
            //ConnectionString = [jdbc:mysql://<host>:port, jdbc:sqlserver://<host>:port]
            //sqoop export --connect jdbc:mysql://localhost/db --username root --table employee --export-dir /emp/emp_data
            command = "sqoop export --connect <ConnectionString>/<database> --username <userName> --password <password> --table <tableName> --export-dir <hdfsDirectory>";
            p.SqoopSubmit(command);
            Console.Read();
        }

        private void SqoopSubmit(string argument)
        {
            try
            {
                Console.WriteLine("Sqoop job execution started");
                RunCommandProcess(argument);                
                Console.WriteLine("Sqoop job execution completed");
            }
            catch (Exception e)
            {
                Console.WriteLine("Sqoop job execution failed");
                Console.Error.WriteLine("/**Sqoop log*/" + e.Message);
            }
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
                        WorkingDirectory = sqoopHome.Trim('\\') + "\\bin",
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
                        Console.Write(e.Data.ToString() + "\n");
                };
                scriptProcess.ErrorDataReceived += (sender, e) =>
                {
                    if (e.Data != null)
                        Console.Write(e.Data.ToString() + "\n");
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
