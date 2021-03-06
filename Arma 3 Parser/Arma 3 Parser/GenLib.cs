﻿using System;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
//using MySql.Data.MySqlClient;
using System.Windows.Forms;

namespace Arma_3_Parser
{
    static class GenLib//General Library of Methods
    {
        /*
        public static MySqlConnection openDB(String serverAddr, String UID, String Password, String DB)
        {
            MySqlConnection db = new MySqlConnection();
            try { 
            db = new MySqlConnection("Server=" + serverAddr + ";Uid=" + UID + ";Pwd=" + Password + ";Database=" + DB);
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                MessageBox.Show("DB Connection Error");
                db.Close();
                return new MySqlConnection();
            }
            return db;
        }
        public static void closeDB(MySqlConnection db)
        {
            db.Close();
        }*/

        public static List<String> binList(String path)//create a list of bin files
        {
            path = path.Replace("config.bin", "");
            if (System.IO.Directory.Exists(path))
            {
                return System.IO.Directory.GetFiles(path, "config.bin", System.IO.SearchOption.AllDirectories).ToList();
            }  
            else
            {
                System.IO.Directory.CreateDirectory(path);
                return System.IO.Directory.GetFiles(path, "config.bin", System.IO.SearchOption.AllDirectories).ToList();
            }
            
        }
        
        public static List<String> pboList(String path)//create a list of pbo files
        {

            if(System.IO.Directory.Exists(path))//verify path exists
                return System.IO.Directory.GetFiles(path, "*pbo", System.IO.SearchOption.AllDirectories).ToList();//grab all pbo's at path
            else
            {
                System.IO.Directory.CreateDirectory(path);//if path doesn't exist, create that path
                return System.IO.Directory.GetFiles(path, "*pbo", System.IO.SearchOption.AllDirectories).ToList();//grab all PBO's on that path (none)
            }
            
        }

        public static List<String> cppList(String path)//create a list of cpp files
        {
            path = path.Replace("config.cpp", "");
            if (System.IO.Directory.Exists(path))
            {
                return System.IO.Directory.GetFiles(path, "config.cpp", System.IO.SearchOption.AllDirectories).ToList();
            }
            else
            {
                System.IO.Directory.CreateDirectory(path);
                return System.IO.Directory.GetFiles(path, "config.cpp", System.IO.SearchOption.AllDirectories).ToList();
            }
        }

        /// <summary>
        /// Async method to run commandline apps
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static async Task<int> RunProcessAsync(string fileName, string args)
        {
            using (var process = new Process
            {
                StartInfo =
        {
            FileName = fileName, Arguments = args,
            UseShellExecute = false, CreateNoWindow = true,
            RedirectStandardOutput = true, RedirectStandardError = true
        },
                EnableRaisingEvents = true
            })
            {
                return await RunProcessAsync(process).ConfigureAwait(false);
            }
        }
        /// <summary>
        /// Async method to run commandline apps
        /// </summary>
        /// <param name="process"></param>
        /// <returns></returns>
        private static Task<int> RunProcessAsync(Process process)
        {
            var tcs = new TaskCompletionSource<int>();

            process.Exited += (s, ea) => tcs.SetResult(process.ExitCode);
            process.OutputDataReceived += (s, ea) => Console.WriteLine(ea.Data);
            process.ErrorDataReceived += (s, ea) => Console.WriteLine("ERR: " + ea.Data);

            bool started = process.Start();
            if (!started)
            {
                //you may allow for the process to be re-used (started = false) 
                //but I'm not sure about the guarantees of the Exited event in such a case
                throw new InvalidOperationException("Could not start process: " + process);
            }

            process.BeginOutputReadLine();
            process.BeginErrorReadLine();

            return tcs.Task;
        }

        /// <summary>
        /// extract a list of .pbo's into .bin's at a location from a location(and all subfolders)
        /// </summary>
        /// <param name="fromPath"></param>
        /// <param name="toPath"></param>
        /// <param name="BankRev"></param>
        /// <returns></returns>
        public static List<String> extract(String fromPath, String toPath, String BankRev)
        {
            //fromPath is a Path to the parent folder of a bunch of PBO's
            //toPath is where we are extracting to
            //Bankrev is the path to the Arma3 Tool

            //We are returning a List of Paths which point to where we put the extracted PBO's(folder paths)

            List<String> PBOFilePathList = pboList(fromPath);//This creates the list of PBO's
            Parallel.ForEach(PBOFilePathList, (thisPBOPath) =>
            {
                ProcessStartInfo alpha = new ProcessStartInfo();

                alpha.FileName = BankRev;
                //Launch process in background
                alpha.CreateNoWindow = true;
                alpha.WindowStyle = ProcessWindowStyle.Hidden;
                alpha.Arguments = "-f \"" + toPath + "\" \"" + thisPBOPath + "\"";
                Process trackit = Process.Start(alpha);
                trackit.WaitForExit();
            });

            return binList(toPath);
        }


        /// <summary>
        /// convert from a list of .bin's to a location as .cpp's
        /// </summary>
        /// <param name="gui"></param>
        /// <param name="fromList"></param>
        /// <param name="toThisPath"></param>
        /// <param name="CfgConvert"></param>
        /// <returns></returns>
        public static List<String> convert(List<String> fromList, String toThisPath, String CfgConvert)
        {

            Parallel.ForEach(fromList, (thisThing) =>
            {
                //txtProgress.Text = "UnBinarizing\n" + x + "\nto Output Location";
                String toPath = thisThing.Replace(".bin", ".cpp");

                ///Identify currently set Path
                String topPath = toThisPath + "\\" + new DirectoryInfo(thisThing).Parent;

                ///Replace the current path with the proper path
                toPath = toThisPath + "\\" + new DirectoryInfo(thisThing).Parent + "\\" + Path.GetFileName(toPath);
                
                ///Create the directory if it doesn't exist
                System.IO.Directory.CreateDirectory(topPath);

                //Process.Start(FilePathToConverterTool, "-txt -dst " + OutputFolder + " " + x);//spits out to folder
                ProcessStartInfo alpha = new ProcessStartInfo();
                alpha.FileName = CfgConvert;
                alpha.CreateNoWindow = true;
                alpha.WindowStyle = ProcessWindowStyle.Hidden;
                alpha.Arguments = "-txt -dst \"" + toPath + "\" \"" + thisThing + "\"";
                Process trackit = Process.Start(alpha);
                trackit.WaitForExit();
            });
            /*
            //fromList is a List of Strings of PBO Paths
            for (int i = 0; i < fromList.Count; i++)
            {
                //txtProgress.Text = "UnBinarizing\n" + x + "\nto Output Location";
                String toPath = fromList[i].Replace("bin", "cpp").Replace("BIN", "CPP");
                int test = toPath.LastIndexOf('\\', toPath.Length - 1);
                String topPath = toPath.Remove(toPath.LastIndexOf('\\'), (toPath.Length - toPath.LastIndexOf('\\', toPath.Length - 1)));//Grab the Path
                System.IO.Directory.CreateDirectory(topPath);

                //Process.Start(FilePathToConverterTool, "-txt -dst " + OutputFolder + " " + x);//spits out to folder
                ProcessStartInfo alpha = new ProcessStartInfo();
                alpha.FileName = CfgConvert;
                alpha.CreateNoWindow = true;
                alpha.WindowStyle = ProcessWindowStyle.Hidden;
                alpha.Arguments = "-txt -dst " + toPath + " " + fromList[i];
                Process trackit = Process.Start(alpha);
                trackit.WaitForExit();
            }
            */
            
            return cppList(toThisPath);
        }

        public static A3CppFile parseFile(String path)//process a cpp file into a A3CppFile object
        {

            A3CppFile file = new A3CppFile();

            file.FilePath = path;
            file.OriginalCodeString = System.IO.File.ReadAllText(path, System.Text.Encoding.UTF8);//read file contents into String
            file.splitOCode();//split string into list by line

            //Parse all Class's
            file = parseFileForClasses(file);//Find and seperate the classes with their content
            //Parse all Variables
            file = parseFileClassesForVariables(file);
            //Build Inheritance Tree -- PERFORMANCE BOTTLENECK
            file.buildTrees();
            return file;
        }

        public static A3CppFile parseFileForClasses(A3CppFile file)//process a filepath listed in a file for all the classes and sort them into the file
        {
            file.stripClasses();
            return file;
        }

        public static A3CppFile parseFileClassesForVariables(A3CppFile file)//build list of variables
        {
            file.stripVariables();
            return file;
        }

        public static A3CppFile processFileClassesForInheritanceTree(A3CppFile file)//build the nested and extended tree lists and combine into inheritance tree
        {
            file.buildTrees();
            return file;
        }
        /*
        public static A3CppFile actualizeInheritanceForClassesInFile(A3CppFile file)//import variables from extended tree
        {
            file.actualizeInheritance();
            return file;
        }
        */
        public static void serialize(List<A3CppFile> fileList, String path)
        {
            List<String> pathList = new List<String>();
            if (System.IO.Directory.Exists(path))
            {
                pathList = System.IO.Directory.GetFiles(path, "*.dat", System.IO.SearchOption.AllDirectories).ToList();
            }
            else
            {
                System.IO.Directory.CreateDirectory(path);
            }
            if (pathList.Count > 0)
            foreach(String x in pathList)//remove all these Serial*.dat's if they exist
            {
                System.IO.File.Delete(x);
            }
            for(int i = 0; i < fileList.Count; i++)//create a seperate .dat file for each cpp
            {
                if (fileList[i] == null)
                    continue;
                String fileName = path + "\\serial" + i + ".dat";
                IFormatter form = new BinaryFormatter();
                Stream stream = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.None);
                form.Serialize(stream, fileList[i]);
                stream.Close();
            }
            
        }

        public static List<A3CppFile> deserialize(String filePath)
        {
            List<String> pathList = new List<String>();
            if (System.IO.Directory.Exists(filePath))
            {
                pathList = System.IO.Directory.GetFiles(filePath, "*.dat", System.IO.SearchOption.AllDirectories).ToList();
            }
            else;
            List<A3CppFile> list = new List<A3CppFile>();
            for(int i = 0; i < pathList.Count; i++)
            {
                IFormatter form = new BinaryFormatter();
                Stream stream = new FileStream(pathList[i], FileMode.Open, FileAccess.Read, FileShare.Read);

                if (stream != null) list.Add((A3CppFile)form.Deserialize(stream));
                stream.Close();
            }
            return list;
        }

        public static int endOfWord(String line, int startOfWord)
        {
            int end = line.Length;
            for (int i = startOfWord; i < line.Length; i++)
            {
                if (line[i].Equals(' ') || line[i].Equals(':') || line[i].Equals(';') || line[i].Equals('\\') || line[i].Equals('/') || line[i].Equals('{') || line[i].Equals('['))
                    return i;
            }
            return end;
        }

        public static int startOfNextWord(String line, int startLoc)
        {
            
            int result = startLoc;
            for (int i = startLoc; i < line.Length; i++)
            {
                if (line[i].Equals(' ') || line[i].Equals(':') || line[i].Equals(';') || line[i].Equals('\\') || line[i].Equals('/') || line[i].Equals('{') || line[i].Equals('['))
                    ;
                else
                    return i;
            }

            return result;
        }

        public static String stripFormating(String var)
        {
            var = var.Replace("\r", "").Replace("\t", "").Replace("\n", "");
            return var;
        }

    }
}
