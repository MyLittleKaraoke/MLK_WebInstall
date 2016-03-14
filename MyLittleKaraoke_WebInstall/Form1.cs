// Wouldnt work without @gameleon_eqd, think to add credits! (Network timeout)
// also Tucker @ irc.ponychat.net #geek (Resume function)

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Diagnostics;
using OperatingSystemInfo;
using System.IO;
using ICSharpCode.SharpZipLib.Tar;
using System.Threading;
using System.Security.AccessControl;
using Microsoft.Win32;

namespace MyLittleKaraoke_WebInstall
{
    public partial class Form1 : Form
    {
        private Uri WebFileList = new Uri("https://yp.coco-pommel.org/mlk-web-test/windows.webinst");
        public string legacy;
        private HelperClass cHelper = new HelperClass();
        private string Downloaded;
        private string Downloaded2;
        private string status;
        private int barvalue;
        private string TempPath = "downloads";
        delegate void SetTextCallback(string status);
        delegate void SetValueCallback(int barvalue);
        private string FileDL;
        private WebClient Canard;
        private System.Windows.Forms.Timer Timeout;
        Stopwatch sw = new Stopwatch();

        public Form1()
        {
            try
            {
                InitializeComponent();
                throw new FileNotFoundException("errormessage");
                // We determine if the user run Windows XP as path for configurations aren't the same.
                OSInfo osInfo = new OSInfo();
                string OS = String.Format("{0}", osInfo.GetOSName);
                string wxp = "Windows XP";
                if (OS.Equals(wxp)) { legacy = "1"; }
                else { legacy = "0"; }
            }
            catch (Exception ex)
            {
                cHelper.ShowErrorMessageDialog(ex.Message, ex.StackTrace, "MyLittleKaraoke_WebInstall.Form1");
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            textBox2.ReadOnly = false;
            checkBox1.Enabled = false;
            this.Height = 550;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {// Create a new instance of FolderBrowserDialog.
                FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
                // A new folder button will display in FolderBrowserDialog.
                folderBrowserDialog.ShowNewFolderButton = true;
                //Show FolderBrowserDialog
                DialogResult dlgResult = folderBrowserDialog.ShowDialog();
                if (dlgResult.Equals(DialogResult.OK))
                {
                    //Send content to string
                    string path = folderBrowserDialog.SelectedPath;
                    //Show selected folder path in textbox1.
                    textBox1.Text = folderBrowserDialog.SelectedPath;
                    //Browsing start from root folder.
                    Environment.SpecialFolder rootFolder = folderBrowserDialog.RootFolder;
                }
            }
            catch (Exception ex)
            {
                cHelper.ShowErrorMessageDialog(ex.Message, ex.StackTrace, "BrowseInstallerLocationButton_Click");
            }
        }


        // We calculate and display Progress and Speed.
        public void ProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;
            Downloaded = e.BytesReceived.ToString();
            string downloadsize = string.Format("{0} MB of {1} MB",
                (e.BytesReceived / 1024d / 1024d).ToString("0.00"),
                (e.TotalBytesToReceive / 1024d / 1024d).ToString("0.00"));
            string downloadspeed = string.Format("{0} kb/s", (e.BytesReceived / 1024d / sw.Elapsed.TotalSeconds).ToString("0.00"));
            label7.Text = downloadsize + " at " + downloadspeed;
        }

        //We make sure the connection is still active, by comparing the value of "Downloaded" (see ProgressChanged) with Downloaded 2 that we update only if the download is still active
        public void CheckTimeout(object source, object e)
        {
            if (Canard != null && Downloaded == Downloaded2) { Canard.CancelAsync(); }
            else { Downloaded2 = Downloaded; }
        }

        private void button2_Click(object sender, EventArgs e) //this is the Download&Install button
        {
            this.Height = 450;
            String Server = textBox2.Text;
            Timeout = new System.Windows.Forms.Timer();
            FileDL = "base1.tar.mlk";
            // TIMER - Launch code to check if download is still active every 30 seconds)
            Timeout.Tick += new EventHandler(CheckTimeout);
            Timeout.Interval = 30000;
            Timeout.Start();
            // TIMER - End.
            //
            // If the file exist, we go to the next one.
            if (File.Exists(TempPath + @"\base1.tar.mlk")) { DL2(null, null); }
            // If the file doesn't exist or has been detected as incorrect and deleted, we QUACK a copy!
            else
            {
                Canard = new WebClient();
                Canard.DownloadFileCompleted += new AsyncCompletedEventHandler(DL2);
                Canard.DownloadProgressChanged += new DownloadProgressChangedEventHandler(ProgressChanged);
                Canard.DownloadFileAsync(new Uri(Server + FileDL), TempPath + @"\" + FileDL);
                progressBar2.Value = 0;
                label8.Text = "Part 1 of 22";
            }
            //We start the stopwatch to calculate progress.
            sw.Start();
            //System.Windows.Forms.Timer Timeout = new System.Windows.Forms.Timer();

        }

        public void DL2(object sender, AsyncCompletedEventArgs e)
        {
            //We stop the stopwatch.
            sw.Stop();
            //string GenByte = MD5.MD5.GetChecksum(TempPath + @"\\" + FileDL, MD5.MD5.Algorithms.MD5);
            //We check if size is similar to the one preset earlier in the code
            FileDL = "base1.tar.mlk";
            string GenByte = new System.IO.FileInfo(TempPath + @"\" + FileDL).Length.ToString();
            //if it match, then we download the next file!
            if (ByteBase1.Equals(GenByte))
            {
                String Server = textBox2.Text;
                Timeout.Start();
                FileDL = "base2.tar.mlk";
                if (File.Exists(TempPath + @"\" + FileDL)) { DL3(null, null); }
                else {
                    Canard = new WebClient();
                    Canard.DownloadFileCompleted += new AsyncCompletedEventHandler(DL3);
                    Canard.DownloadProgressChanged += new DownloadProgressChangedEventHandler(ProgressChanged);
                    Canard.DownloadFileAsync(new Uri(Server + FileDL), TempPath + @"\" + FileDL);
                    progressBar2.Value = 5;
                    label8.Text = "Part 2 of 22";
                }
            }
            //Else, we ask the user if he want to redownload or cancel
            else {
                // Delete the file if it exist.
                if (File.Exists(TempPath + @"\" + FileDL)) { File.Delete(TempPath + @"\" + FileDL); }
                // Ask user to redownload or cancel.
                DialogResult dialogResult = MessageBox.Show(FileDL + " is corrupted, retry downloading?", "File verification FAILED", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    Downloaded = null;
                    Downloaded2 = null;
                    GenByte = null;
                    Timeout.Stop();
                    button2_Click(null, null);
                }
                else if (dialogResult == DialogResult.No)
                {
                    //here be poni
                }
            }
        }

        public void DL3(object sender, AsyncCompletedEventArgs e)
        {
            sw.Stop();
            FileDL = "base2.tar.mlk";
            string GenByte = new System.IO.FileInfo(TempPath + @"\" + FileDL).Length.ToString();
            if (ByteBase2.Equals(GenByte))
            {
                String Server = textBox2.Text;
                Timeout.Start();
                FileDL = "base3.tar.mlk";
                if (File.Exists(TempPath + @"\" + FileDL)) { DL4(null, null); }
                else {
                    Canard = new WebClient();
                    Canard.DownloadFileCompleted += new AsyncCompletedEventHandler(DL4);
                    Canard.DownloadProgressChanged += new DownloadProgressChangedEventHandler(ProgressChanged);
                    Canard.DownloadFileAsync(new Uri(Server + FileDL), TempPath + @"\" + FileDL);
                    progressBar2.Value = 10;
                    label8.Text = "Part 3 of 22";
                }
            }
            else {
                // Delete the file if it exist.
                if (File.Exists(TempPath + @"\" + FileDL)) { File.Delete(TempPath + @"\" + FileDL); }
                // Ask user to redownload or cancel.
                DialogResult dialogResult = MessageBox.Show(FileDL + " is corrupted, retry downloading?", "File verification FAILED", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    Downloaded = null;
                    Downloaded2 = null;
                    GenByte = null;
                    Timeout.Stop();
                    DL2(null, null);
                }
                else if (dialogResult == DialogResult.No)
                {
                    //here be poni
                }
            }
        }


        public void Over(object sender, AsyncCompletedEventArgs e)
        {
            sw.Stop();
            FileDL = "theme1.mlt";
            string GenByte = new System.IO.FileInfo(TempPath + @"\" + FileDL).Length.ToString();
            // IF the download matched, we edit the UI for the installation and we launch the install thread!
            if (ByteBase22.Equals(GenByte))
            {
                progressBar1.Value = 100;
                progressBar2.Value = 100;
                label7.Text = " ";
                label6.Text = " ";
                label8.Text = "Download finished!";
                this.Height = 370;
                //thread = new ThreadStart(WorkThreadFunction);
                Thread thread = new Thread(new ThreadStart(WorkThreadFunction));
                thread.Start();
        }
    
            else {
                // Delete the file if it exist.
                if (File.Exists(TempPath + @"\" + FileDL)) { File.Delete(TempPath + @"\" + FileDL); }
                // Ask user to redownload or cancel.
                DialogResult dialogResult = MessageBox.Show(FileDL + " is corrupted, retry downloading?", "File verification FAILED", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    Downloaded = null;
                    Downloaded2 = null;
                    GenByte = null;
                    Timeout.Stop();
                    DL22(null, null);
                }
                else if (dialogResult == DialogResult.No)
                {
                    //here be poni
                }
            }
        }

        public void WorkThreadFunction()
        {
            status = "Installation (1 of 22)";
            barvalue = 1;
            if (this.label5.InvokeRequired) { SetTextCallback d = new SetTextCallback(SetText); this.Invoke(d, new object[] { status }); }
            if (this.progressBar1.InvokeRequired) { SetValueCallback d = new SetValueCallback(SetValue); this.Invoke(d, new object[] { barvalue }); }
            Stream inStream21 = File.OpenRead(TempPath + @"\usdx1.mlu");
            TarArchive tarArchive21 = TarArchive.CreateInputTarArchive(inStream21);
            tarArchive21.ExtractContents(textBox1.Text);
            tarArchive21.Close();
            inStream21.Close();
            status = "Installation (2 of 22)";
            barvalue = 2;
            if (this.label5.InvokeRequired) { SetTextCallback d = new SetTextCallback(SetText); this.Invoke(d, new object[] { status }); }
            if (this.progressBar1.InvokeRequired) { SetValueCallback d = new SetValueCallback(SetValue); this.Invoke(d, new object[] { barvalue }); }
            Stream inStream22 = File.OpenRead(TempPath + @"\theme1.mlt");
            TarArchive tarArchive22 = TarArchive.CreateInputTarArchive(inStream22);
            tarArchive22.ExtractContents(textBox1.Text);
            tarArchive22.Close();
            inStream22.Close();
            
            barvalue = 99;
            if (this.label5.InvokeRequired) { SetTextCallback d = new SetTextCallback(SetText); this.Invoke(d, new object[] { status }); }
            if (this.progressBar1.InvokeRequired) { SetValueCallback d = new SetValueCallback(SetValue); this.Invoke(d, new object[] { barvalue }); }
            Stream inStream20 = File.OpenRead(TempPath + @"\base20.tar.mlk");
            TarArchive tarArchive20 = TarArchive.CreateInputTarArchive(inStream20);
            tarArchive20.ExtractContents(textBox1.Text + @"\songs");
            tarArchive20.Close();
            inStream20.Close();
            status = "Installation (Registering component)";
            barvalue = 100;
            if (this.label5.InvokeRequired) { SetTextCallback d = new SetTextCallback(SetText); this.Invoke(d, new object[] { status }); }
            if (this.progressBar1.InvokeRequired) { SetValueCallback d = new SetValueCallback(SetValue); this.Invoke(d, new object[] { barvalue }); }
            string regkey = "\"" + textBox1.Text + "\\" + "MLKHelperGUI.exe" + "\" " + "\"%1\"";
            Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Classes\derpymuffinsfactory.mlk.v1");
            Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Classes\derpymuffinsfactory.mlk.v1\shell");
            Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Classes\derpymuffinsfactory.mlk.v1\shell\open");
            Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Classes\derpymuffinsfactory.mlk.v1\shell\open\command");
            Registry.SetValue(@"HKEY_LOCAL_MACHINE\Software\Classes\derpymuffinsfactory.mlk.v1\shell\open\command", null, regkey);
            Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Classes\.mlk");
            Registry.SetValue(@"HKEY_LOCAL_MACHINE\Software\Classes\.mlk", null, "derpymuffinsfactory.mlk.v1");
            Registry.LocalMachine.CreateSubKey(@"SOFTWARE\\DerpyMuffinsFactory");
            Registry.LocalMachine.OpenSubKey("SOFTWARE\\DerpyMuffinsFactory", true).SetValue("MLK Path", (string)textBox1.Text + "\\", Microsoft.Win32.RegistryValueKind.String);
            status = "Installation is done!";


        }

        private void SetText(string text)
        {
            this.label5.Text = status;
        }

        private void SetValue(int barvalue)
        {
            this.progressBar1.Value = barvalue;
        }
    }
}
