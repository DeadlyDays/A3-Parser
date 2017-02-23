using System;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace Arma_3_Parser
{
    static class GenLib//General Library of Methods
    {
        public static List<String> binList(String path)//create a list of bin files
        {
            return System.IO.Directory.GetFiles(path, "config.bin", System.IO.SearchOption.AllDirectories).ToList();
        }
        
        public static List<String> pboList(String path)//create a list of pbo files
        {
            return System.IO.Directory.GetFiles(path, "*pbo", System.IO.SearchOption.AllDirectories).ToList();
        }

        public static List<String> cppList(String path)//create a list of cpp files
        {
            return System.IO.Directory.GetFiles(path, "config.cpp", System.IO.SearchOption.AllDirectories).ToList();
        }

        public static List<String> extract(String fromPath, String toPath, String BankRev)//extract a list of .pbo's into .bin's at a location from a location(and all subfolders)
        {
            List<String> PBOFilePathList = pboList(fromPath);
            foreach (String x in PBOFilePathList)
            {
                ProcessStartInfo alpha = new ProcessStartInfo();
                alpha.FileName = BankRev;
                //Launch process in background
                alpha.CreateNoWindow = true;
                alpha.WindowStyle = ProcessWindowStyle.Hidden;
                alpha.Arguments = "-f " + toPath + " \"" + x + "\"";
                Process.Start(alpha);
            }
            return binList(toPath);
        }

        public static List<String> convert(List<String> fromList, String toPath, String CfgConvert)
        {

            foreach (String x in fromList)
            {
                //txtProgress.Text = "UnBinarizing\n" + x + "\nto Output Location";
                toPath = x.Replace("bin", "cpp").Replace("BIN", "CPP");
                int test = toPath.LastIndexOf('\\', toPath.Length - 1);
                String topPath = toPath.Remove(toPath.LastIndexOf('\\'), (toPath.Length - toPath.LastIndexOf('\\', toPath.Length - 1)));//Grab the Path
                System.IO.Directory.CreateDirectory(topPath);

                //Process.Start(FilePathToConverterTool, "-txt -dst " + OutputFolder + " " + x);//spits out to folder
                ProcessStartInfo alpha = new ProcessStartInfo();
                alpha.FileName = CfgConvert;
                alpha.CreateNoWindow = true;
                alpha.WindowStyle = ProcessWindowStyle.Hidden;
                alpha.Arguments = "-txt -dst " + toPath + " " + x;
                Process.Start(alpha);
            }
            return cppList(toPath);
        }//convert from a list of .bin's to a location as .cpp's

        public static A3CppFile parseFile(String path)//process a cpp file into a A3CppFile object
        {

            A3CppFile file = new A3CppFile();

            file.FilePath = path;
            file.OriginalCodeString = System.IO.File.ReadAllText(path, System.Text.Encoding.UTF8);//read file contents into String
            file.splitOCode();//split string into list by line

            //Parse all Class's
            file = parseFileForClasses(file);//Find and seperate the classes with their content
            //Build Inheritance Tree

            //Parse all Variables
            file = parseFileClassesForVariables(file);
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
        public static void serialize(List<A3CppFile> fileList, String toFile)
        {
            IFormatter form = new BinaryFormatter();
            Stream stream = new FileStream(toFile, FileMode.Create, FileAccess.Write, FileShare.None);
            form.Serialize(stream, fileList);
            stream.Close();
        }

        public static List<A3CppFile> deserialize(String filePath)
        {
            IFormatter form = new BinaryFormatter();
            Stream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            List<A3CppFile> list = (List<A3CppFile>)form.Deserialize(stream);
            stream.Close();
            return list;
        }

        public static int endOfWord(String line, int startOfWord)
        {
            int end = line.Length;
            for (int i = startOfWord; i < line.Length; i++)
            {
                if (line[i].Equals(" ") || line[i].Equals(":") || line[i].Equals(";") || line[i].Equals("\\") || line[i].Equals("/") || line[i].Equals("{") || line[i].Equals("["))
                    return i;
            }
            return end;
        }

        public static int startOfNextWord(String line, int startLoc)
        {
            int result = startLoc;
            for (int i = startLoc; i < line.Length; i++)
            {
                if (line[i].Equals(" ") || line[i].Equals(":") || line[i].Equals(";") || line[i].Equals("\\") || line[i].Equals("/") || line[i].Equals("{") || line[i].Equals("["))
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
