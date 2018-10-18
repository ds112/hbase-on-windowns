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

namespace ProcessPig
{
    class Program
    {
        public static string commonHome;
        static string pigHome;
        static void Main(string[] args)
        {
            Program p = new Program();
            Program.commonHome = Directory.GetParent("..\\..\\..\\..\\..\\..\\..\\").ToString();
            Program.pigHome = Program.commonHome + "\\SDK\\Pig";
            int i;
            do
            {
                Console.WriteLine("Enter any operation");
                Console.WriteLine("1. UsingUDF");
                Console.WriteLine("2. Filter");
                Console.WriteLine("3. FindAverage");
                Console.WriteLine("4. FindDistinct");
                Console.WriteLine("5. FindMaximum");
                Console.WriteLine("6. FindMinimum");
                Console.WriteLine("7. Foreach");
                Console.WriteLine("8. GroupValues");
                Console.WriteLine("9. JoinValues");
                Console.WriteLine("10. LimitValues");
                Console.WriteLine("11. LoadStore");
                Console.WriteLine("12. Exit");
                i = int.Parse(Console.ReadLine());

                switch (i)
                {
                    case 1:
                        p.SubmitPigFile(Program.commonHome + "\\Samples\\Scripts\\Pig\\UsingUDF.pig", "UsingUDF");
                        break;
                    case 2:
                        p.SubmitPigFile(Program.commonHome + "\\Samples\\Scripts\\Pig\\Filter.pig", "Filter");
                        break;
                    case 3:
                        p.SubmitPigFile(Program.commonHome + "\\Samples\\Scripts\\Pig\\FindAverage.pig", "FindAverage");
                        break;
                    case 4:
                        p.SubmitPigFile(Program.commonHome + "\\Samples\\Scripts\\Pig\\FindDistinct.pig", "FindDistinct");
                        break;
                    case 5:
                        p.SubmitPigFile(Program.commonHome + "\\Samples\\Scripts\\Pig\\FindMaximum.pig", "FindMaximum");
                        break;
                    case 6:
                        p.SubmitPigFile(Program.commonHome + "\\Samples\\Scripts\\Pig\\FindMinimum.pig", "FindMinimum");
                        break;
                    case 7:
                        p.SubmitPigFile(Program.commonHome + "\\Samples\\Scripts\\Pig\\Foreach.pig", "ForEach");
                        break;
                    case 8:
                        p.SubmitPigFile(Program.commonHome + "\\Samples\\Scripts\\Pig\\GroupValues.pig", "GroupValues");
                        break;
                    case 9:
                        p.SubmitPigFile(Program.commonHome + "\\Samples\\Scripts\\Pig\\JoinValues.pig", "JoinValues");
                        break;
                    case 10:
                        p.SubmitPigFile(Program.commonHome + "\\Samples\\Scripts\\Pig\\LimitValues.pig", "LimitValues");
                        break;
                    case 11:
                        p.SubmitPigFile(Program.commonHome + "\\Samples\\Scripts\\Pig\\LoadStore.pig", "LoadStore");
                        break;
                }
            } while (i != 12);
        }

        private void SubmitPigFile(string file,string fileName)
        {
            try
            {
                Console.WriteLine(fileName+" execution started");
                RunCommandProcess("pig -f \"" + file + "\"");
                Console.WriteLine(fileName + " execution completed");
            }
            catch (Exception e)
            {
                Console.WriteLine(fileName + " execution failed");
                Console.Error.WriteLine("/**"+fileName+" log*/" + e.Message);
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
                        WorkingDirectory = pigHome.Trim('\\') + "\\bin",
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
