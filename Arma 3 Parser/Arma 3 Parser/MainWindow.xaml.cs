﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Forms;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace Arma_3_Parser
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Log("Program Start");
        }

        private void Log(string text)
        {
            String s = String.Format("{0:HH:mm:ss.fff}: {1}\r\n", DateTime.Now, text);
            rtxtStatus.AppendText(s);
            rtxtStatus.ScrollToEnd();
        }

        private Microsoft.Win32.OpenFileDialog browseForFile(String path)
        {
            // Create OpenFileDialog
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            // Set filter for file extension and default file extension
            dlg.DefaultExt = ".txt";
            dlg.Filter = "Text documents (.txt)|*.txt";
            // If there is a path, open to that path first
            if (txtConfigPath.Text == "")
                ;
            else
                dlg.InitialDirectory = txtConfigPath.Text;
            
            return dlg;
        }

        private System.Windows.Forms.FolderBrowserDialog browseForDirectory(String path)
        {
            // Create OpenFileDialog
            System.Windows.Forms.FolderBrowserDialog dlg = new System.Windows.Forms.FolderBrowserDialog();
            
            // If there is a path, open to that path first
            if (txtConfigPath.Text == "")
                ;
            else
                dlg.SelectedPath = txtConfigPath.Text;

            return dlg;
        }

        private void btnBrowseConfig_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = browseForFile(txtConfigPath.Text);
            // Display OpenFileDialog by calling ShowDialog method
            Nullable<bool> result = dlg.ShowDialog();
            // Get the selected file name and display in a TextBox
            if (result == true)
            {
                // Open document
                string filename = dlg.FileName;
                txtConfigPath.Text = filename;
            }
        }

        private void btnBrowsePBO_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog dlg = browseForDirectory(txtPboPath.Text);
            
            // Open Dialog and verify it opens via opening via Showdialog and checking DialogResult Get the selected file name and display in a TextBox
            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                // Open document
                string filename = dlg.SelectedPath;
                txtPboPath.Text = filename;
            }
        }

        private void btnBrowseBIN_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog dlg = browseForDirectory(txtBinPath.Text);

            // Open Dialog and verify it opens via opening via Showdialog and checking DialogResult Get the selected file name and display in a TextBox
            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                // Open document
                string filename = dlg.SelectedPath;
                txtBinPath.Text = filename;
            }
        }

        private void btnBrowseCPP_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog dlg = browseForDirectory(txtCppPath.Text);

            // Open Dialog and verify it opens via opening via Showdialog and checking DialogResult Get the selected file name and display in a TextBox
            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                // Open document
                string filename = dlg.SelectedPath;
                txtCppPath.Text = filename;
            }
        }

        private void btnBrowseSerialized_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = browseForFile(txtSerialized.Text);
            // Display OpenFileDialog by calling ShowDialog method
            Nullable<bool> result = dlg.ShowDialog();
            // Get the selected file name and display in a TextBox
            if (result == true)
            {
                // Open document
                string filename = dlg.FileName;
                txtSerialized.Text = filename;
            }
        }

        private void btnBrowseOutput_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = browseForFile(txtOutputPath.Text);
            // Display OpenFileDialog by calling ShowDialog method
            Nullable<bool> result = dlg.ShowDialog();
            // Get the selected file name and display in a TextBox
            if (result == true)
            {
                // Open document
                string filename = dlg.FileName;
                txtOutputPath.Text = filename;
            }
        }

        private void btnBrowseBankRev_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = browseForFile(txtBankRevPath.Text);
            // Display OpenFileDialog by calling ShowDialog method
            Nullable<bool> result = dlg.ShowDialog();
            // Get the selected file name and display in a TextBox
            if (result == true)
            {
                // Open document
                string filename = dlg.FileName;
                txtBankRevPath.Text = filename;
            }
        }

        private void btnBrowseCFGConvert_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = browseForFile(txtCfgConvertPath.Text);
            // Display OpenFileDialog by calling ShowDialog method
            Nullable<bool> result = dlg.ShowDialog();
            // Get the selected file name and display in a TextBox
            if (result == true)
            {
                // Open document
                string filename = dlg.FileName;
                txtCfgConvertPath.Text = filename;
            }
        }

        private void initializeConfig()
        {
            Config con = new Config();

            IFormatter writer = new BinaryFormatter();
            Stream file = new FileStream(txtConfigPath.Text, FileMode.Create, FileAccess.Write, FileShare.None);

            writer.Serialize(file, con);
            file.Close();
            Log("Initialized Config File");
        }

        private void readConfig()
        {
            if (File.Exists(txtConfigPath.Text))
            {
                Config con = new Arma_3_Parser.Config();
                IFormatter reader = new BinaryFormatter();

                Stream file = new FileStream(txtConfigPath.Text, FileMode.Open, FileAccess.Read, FileShare.Read);
                con = (Config)reader.Deserialize(file);
                file.Close();

                txtPboPath.Text = con.PBOPath;
                txtBinPath.Text = con.BinPath;
                txtCppPath.Text = con.CPPPath;
                txtSerialized.Text = con.SerialPath;
                txtOutputPath.Text = con.OutputPath;
                txtBankRevPath.Text = con.UnpackPath;
                txtCfgConvertPath.Text = con.ConvertPath;
                txtNotContainPartClass.Text = con.NotContainPartClass;
                txtContainPartClass.Text = con.ContainPartClass;
                txtHasDirectParent.Text = con.HasDirectParent;
                txtHasParent.Text = con.HasOneOfParent;
                txtContainsFields.Text = con.AnyField;
                txtDisplayFields.Text = con.DispField;
            }
            else
            {
                initializeConfig();
                readConfig();
            }
        }

        private void writeConfig()
        {
            Config con = new Arma_3_Parser.Config();
            con.PBOPath = txtPboPath.Text;
            con.BinPath = txtBinPath.Text;
            con.CPPPath = txtCppPath.Text;
            con.SerialPath = txtSerialized.Text;
            con.OutputPath = txtOutputPath.Text;
            con.UnpackPath = txtBankRevPath.Text;
            con.ConvertPath = txtCfgConvertPath.Text;
            con.NotContainPartClass = txtNotContainPartClass.Text;
            con.ContainPartClass = txtContainPartClass.Text;
            con.HasDirectParent = txtHasDirectParent.Text;
            con.HasOneOfParent = txtHasParent.Text;
            con.AnyField = txtContainsFields.Text;
            con.DispField = txtDisplayFields.Text;

            IFormatter writer = new BinaryFormatter();
            Stream file = new FileStream(txtConfigPath.Text, FileMode.Create, FileAccess.Write, FileShare.None);

            writer.Serialize(file, con);
            file.Close();
            Log("Saved Config");
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            Log("Saving Configuration to " + txtConfigPath.Text);
            writeConfig();
            Log("Save Complete");
        }

        private void btnLoad_Click(object sender, RoutedEventArgs e)
        {
            Log("Loading Configuration from " + txtConfigPath.Text);
            readConfig();
            Log("Load Complete");
        }

        private void btnProcess_Click(object sender, RoutedEventArgs e)
        {
            String pboPath = txtPboPath.Text;
            String binPath = txtBinPath.Text;
            String cppPath = txtCppPath.Text;
            String serialPath = txtSerialized.Text;
            String outputPath = txtOutputPath.Text;

            //Extract
            List<String> binList = new List<String>();
            if (cbExtractIsNeeded.IsChecked.Value)
            {
                Log("Extracting Files At: " + pboPath);
                binList = GenLib.extract(pboPath, binPath, txtBankRevPath.Text);
                Log("Extracted Files To: " + binPath);
            }
            //Convert
            List<String> cppList = new List<String>();
            if (cbConvertIsNeeded.IsChecked.Value)
            {
                Log("Converting Files At: " + binPath);
                if (!cbExtractIsNeeded.IsChecked.Value)
                    binList = GenLib.binList(binPath);
                cppList = GenLib.convert(binList, binPath, txtCfgConvertPath.Text);
                Log("Converted Files To: " + cppPath);
            }
            //Serialize
            if (cbSerialize.IsChecked.Value)
            {
                Log("Serializing...");
                GenLib.serialize(cppList, serialPath);
                Log("Serialized");
            }
            ///Parse
            ///
            if (cbProcess.IsChecked.Value)
            {
                Log("Parsing...");
                //Apply Filters

                Log("Parsed");
            }

        }
    }
}
