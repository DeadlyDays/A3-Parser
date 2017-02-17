using System;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            return System.IO.Directory.GetFiles(toPath, "config.cpp", System.IO.SearchOption.AllDirectories).ToList();
        }

        public static void serialize(List<String> fromList, String toFile)
        {


        }
    }
}
