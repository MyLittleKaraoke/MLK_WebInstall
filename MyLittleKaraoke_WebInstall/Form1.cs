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
        public string legacy;
        private HelperClass cHelper = new HelperClass();
        public Form1()
        {
            try
            {
                InitializeComponent();
                throw new FileNotFoundException("errormessage");
                // We determine if the user run Windows XP as path aren't the same.
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

        //File Size
        private string ByteBase1 = "473128448";
        private string ByteBase2 = "457838592";
        private string ByteBase3 = "485589504";
        private string ByteBase4 = "477571072";
        private string ByteBase5 = "497623040";
        private string ByteBase6 = "490698240";
        private string ByteBase7 = "451117056";
        private string ByteBase8 = "499034624";
        private string ByteBase9 = "497562112";
        private string ByteBase10 = "493263872";
        private string ByteBase11 = "492700160";
        private string ByteBase12 = "493468160";
        private string ByteBase13 = "498331648";
        private string ByteBase14 = "459000320";
        private string ByteBase15 = "495222784";
        private string ByteBase16 = "469262336";
        private string ByteBase17 = "489236992";
        private string ByteBase18 = "495268352";
        private string ByteBase19 = "493790720";
        private string ByteBase20 = "118261760";
        private string ByteBase21 = "58684928";
        private string ByteBase22 = "33291264";



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

        public void CheckTimeout(object source, object e)
        {
            if (Canard != null && Downloaded == Downloaded2) { Canard.CancelAsync(); }
            else { Downloaded2 = Downloaded; }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Height = 450;
            String Server = textBox2.Text;
            Timeout = new System.Windows.Forms.Timer();
            FileDL = "base1.tar.mlk";
            Timeout.Tick += new EventHandler(CheckTimeout);
            Timeout.Interval = 30000;
            Timeout.Start();
            if (File.Exists(TempPath + @"\base1.tar.mlk")) { DL2(null, null); }
            else
            {
                Canard = new WebClient();
                Canard.DownloadFileCompleted += new AsyncCompletedEventHandler(DL2);
                Canard.DownloadProgressChanged += new DownloadProgressChangedEventHandler(ProgressChanged);
                Canard.DownloadFileAsync(new Uri(Server + FileDL), TempPath + @"\" + FileDL);
                progressBar2.Value = 0;
                label8.Text = "Part 1 of 22";
            }
            sw.Start();
            //System.Windows.Forms.Timer Timeout = new System.Windows.Forms.Timer();

        }

        public void DL2(object sender, AsyncCompletedEventArgs e)
        {
            sw.Stop();
            //string GenByte = MD5.MD5.GetChecksum(TempPath + @"\\" + FileDL, MD5.MD5.Algorithms.MD5);
            FileDL = "base1.tar.mlk";
            string GenByte = new System.IO.FileInfo(TempPath + @"\" + FileDL).Length.ToString();
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

        public void DL4(object sender, AsyncCompletedEventArgs e)
        {
            sw.Stop();
            FileDL = "base3.tar.mlk";
            string GenByte = new System.IO.FileInfo(TempPath + @"\" + FileDL).Length.ToString();
            if (ByteBase3.Equals(GenByte))
            {
                String Server = textBox2.Text;
                Timeout.Start();
                FileDL = "base4.tar.mlk";
                if (File.Exists(TempPath + @"\" + FileDL)) { DL5(null, null); }
                else {
                    Canard = new WebClient();
                    Canard.DownloadFileCompleted += new AsyncCompletedEventHandler(DL5);
                    Canard.DownloadProgressChanged += new DownloadProgressChangedEventHandler(ProgressChanged);
                    Canard.DownloadFileAsync(new Uri(Server + FileDL), TempPath + @"\" + FileDL);
                    progressBar2.Value = 15;
                    label8.Text = "Part 4 of 22";
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
                    DL3(null, null);
                }
                else if (dialogResult == DialogResult.No)
                {
                    //here be poni
                }
            }
        }

        public void DL5(object sender, AsyncCompletedEventArgs e)
        {
            sw.Stop();
            FileDL = "base4.tar.mlk";
            string GenByte = new System.IO.FileInfo(TempPath + @"\" + FileDL).Length.ToString();
            if (ByteBase4.Equals(GenByte))
            {
                String Server = textBox2.Text;
                Timeout.Start();
                FileDL = "base5.tar.mlk";
                if (File.Exists(TempPath + @"\" + FileDL)) { DL6(null, null); }
                else {
                    Canard = new WebClient();
                    Canard.DownloadFileCompleted += new AsyncCompletedEventHandler(DL6);
                    Canard.DownloadProgressChanged += new DownloadProgressChangedEventHandler(ProgressChanged);
                    Canard.DownloadFileAsync(new Uri(Server + FileDL), TempPath + @"\" + FileDL);
                    progressBar2.Value = 20;
                    label8.Text = "Part 5 of 22";
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
                    DL4(null, null);
                }
                else if (dialogResult == DialogResult.No)
                {
                    //here be poni
                }
            }
        }

        public void DL6(object sender, AsyncCompletedEventArgs e)
        {
            sw.Stop();
            FileDL = "base5.tar.mlk";
            string GenByte = new System.IO.FileInfo(TempPath + @"\" + FileDL).Length.ToString();
            if (ByteBase5.Equals(GenByte))
            {
                String Server = textBox2.Text;
                Timeout.Start();
                FileDL = "base6.tar.mlk";
                if (File.Exists(TempPath + @"\" + FileDL)) { DL7(null, null); }
                else {
                    Canard = new WebClient();
                    Canard.DownloadFileCompleted += new AsyncCompletedEventHandler(DL7);
                    Canard.DownloadProgressChanged += new DownloadProgressChangedEventHandler(ProgressChanged);
                    Canard.DownloadFileAsync(new Uri(Server + FileDL), TempPath + @"\" + FileDL);
                    progressBar2.Value = 24;
                    label8.Text = "Part 6 of 22";
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
                    DL5(null, null);
                }
                else if (dialogResult == DialogResult.No)
                {
                    //here be poni
                }
            }
        }

        public void DL7(object sender, AsyncCompletedEventArgs e)
        {
            sw.Stop();
            FileDL = "base6.tar.mlk";
            string GenByte = new System.IO.FileInfo(TempPath + @"\" + FileDL).Length.ToString();
            if (ByteBase6.Equals(GenByte))
            {
                String Server = textBox2.Text;
                Timeout.Start();
                FileDL = "base7.tar.mlk";
                if (File.Exists(TempPath + @"\" + FileDL)) { DL8(null, null); }
                else {
                    Canard = new WebClient();
                    Canard.DownloadFileCompleted += new AsyncCompletedEventHandler(DL8);
                    Canard.DownloadProgressChanged += new DownloadProgressChangedEventHandler(ProgressChanged);
                    Canard.DownloadFileAsync(new Uri(Server + FileDL), TempPath + @"\" + FileDL);
                    progressBar2.Value = 29;
                    label8.Text = "Part 7 of 22";
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
                    DL6(null, null);
                }
                else if (dialogResult == DialogResult.No)
                {
                    //here be poni
                }
            }
        }

        public void DL8(object sender, AsyncCompletedEventArgs e)
        {
            sw.Stop();
            FileDL = "base7.tar.mlk";
            string GenByte = new System.IO.FileInfo(TempPath + @"\" + FileDL).Length.ToString();
            if (ByteBase7.Equals(GenByte))
            {
                String Server = textBox2.Text;
                Timeout.Start();
                FileDL = "base8.tar.mlk";
                if (File.Exists(TempPath + @"\" + FileDL)) { DL9(null, null); }
                else {
                    Canard = new WebClient();
                    Canard.DownloadFileCompleted += new AsyncCompletedEventHandler(DL9);
                    Canard.DownloadProgressChanged += new DownloadProgressChangedEventHandler(ProgressChanged);
                    Canard.DownloadFileAsync(new Uri(Server + FileDL), TempPath + @"\" + FileDL);
                    progressBar2.Value = 34;
                    label8.Text = "Part 8 of 22";
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
                    DL7(null, null);
                }
                else if (dialogResult == DialogResult.No)
                {
                    //here be poni
                }
            }
        }

        public void DL9(object sender, AsyncCompletedEventArgs e)
        {
            sw.Stop();
            FileDL = "base8.tar.mlk";
            string GenByte = new System.IO.FileInfo(TempPath + @"\" + FileDL).Length.ToString();
            if (ByteBase8.Equals(GenByte))
            {
                String Server = textBox2.Text;
                Timeout.Start();
                FileDL = "base9.tar.mlk";
                if (File.Exists(TempPath + @"\" + FileDL)) { DL10(null, null); }
                else {
                    Canard = new WebClient();
                    Canard.DownloadFileCompleted += new AsyncCompletedEventHandler(DL10);
                    Canard.DownloadProgressChanged += new DownloadProgressChangedEventHandler(ProgressChanged);
                    Canard.DownloadFileAsync(new Uri(Server + FileDL), TempPath + @"\" + FileDL);
                    progressBar2.Value = 39;
                    label8.Text = "Part 9 of 22";
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
                    DL8(null, null);
                }
                else if (dialogResult == DialogResult.No)
                {
                    //here be poni
                }
            }
        }

        public void DL10(object sender, AsyncCompletedEventArgs e)
        {
            sw.Stop();
            FileDL = "base9.tar.mlk";
            string GenByte = new System.IO.FileInfo(TempPath + @"\" + FileDL).Length.ToString();
            if (ByteBase9.Equals(GenByte))
            {
                String Server = textBox2.Text;
                Timeout.Start();
                FileDL = "base10.tar.mlk";
                if (File.Exists(TempPath + @"\" + FileDL)) { DL11(null, null); }
                else {
                    Canard = new WebClient();
                    Canard.DownloadFileCompleted += new AsyncCompletedEventHandler(DL11);
                    Canard.DownloadProgressChanged += new DownloadProgressChangedEventHandler(ProgressChanged);
                    Canard.DownloadFileAsync(new Uri(Server + FileDL), TempPath + @"\" + FileDL);
                    progressBar2.Value = 44;
                    label8.Text = "Part 10 of 22";
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
                    DL9(null, null);
                }
                else if (dialogResult == DialogResult.No)
                {
                    //here be poni
                }
            }
        }

        public void DL11(object sender, AsyncCompletedEventArgs e)
        {
            sw.Stop();
            FileDL = "base10.tar.mlk";
            string GenByte = new System.IO.FileInfo(TempPath + @"\" + FileDL).Length.ToString();
            if (ByteBase10.Equals(GenByte))
            {
                String Server = textBox2.Text;
                Timeout.Start();
                FileDL = "base11.tar.mlk";
                if (File.Exists(TempPath + @"\" + FileDL)) { DL12(null, null); }
                else {
                    Canard = new WebClient();
                    Canard.DownloadFileCompleted += new AsyncCompletedEventHandler(DL12);
                    Canard.DownloadProgressChanged += new DownloadProgressChangedEventHandler(ProgressChanged);
                    Canard.DownloadFileAsync(new Uri(Server + FileDL), TempPath + @"\" + FileDL);
                    progressBar2.Value = 49;
                    label8.Text = "Part 11 of 22";
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
                    DL10(null, null);
                }
                else if (dialogResult == DialogResult.No)
                {
                    //here be poni
                }
            }
        }

        public void DL12(object sender, AsyncCompletedEventArgs e)
        {
            sw.Stop();
            FileDL = "base11.tar.mlk";
            string GenByte = new System.IO.FileInfo(TempPath + @"\" + FileDL).Length.ToString();
            if (ByteBase11.Equals(GenByte))
            {
                String Server = textBox2.Text;
                Timeout.Start();
                FileDL = "base12.tar.mlk";
                if (File.Exists(TempPath + @"\" + FileDL)) { DL13(null, null); }
                else {
                    Canard = new WebClient();
                    Canard.DownloadFileCompleted += new AsyncCompletedEventHandler(DL13);
                    Canard.DownloadProgressChanged += new DownloadProgressChangedEventHandler(ProgressChanged);
                    Canard.DownloadFileAsync(new Uri(Server + FileDL), TempPath + @"\" + FileDL);
                    progressBar2.Value = 54;
                    label8.Text = "Part 12 of 22";
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
                    DL11(null, null);
                }
                else if (dialogResult == DialogResult.No)
                {
                    //here be poni
                }
            }
        }

        public void DL13(object sender, AsyncCompletedEventArgs e)
        {
            sw.Stop();
            FileDL = "base12.tar.mlk";
            string GenByte = new System.IO.FileInfo(TempPath + @"\" + FileDL).Length.ToString();
            if (ByteBase12.Equals(GenByte))
            {
                String Server = textBox2.Text;
                Timeout.Start();
                FileDL = "base13.tar.mlk";
                if (File.Exists(TempPath + @"\" + FileDL)) { DL14(null, null); }
                else {
                    Canard = new WebClient();
                    Canard.DownloadFileCompleted += new AsyncCompletedEventHandler(DL14);
                    Canard.DownloadProgressChanged += new DownloadProgressChangedEventHandler(ProgressChanged);
                    Canard.DownloadFileAsync(new Uri(Server + FileDL), TempPath + @"\" + FileDL);
                    progressBar2.Value = 59;
                    label8.Text = "Part 13 of 22";
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
                    DL12(null, null);
                }
                else if (dialogResult == DialogResult.No)
                {
                    //here be poni
                }
            }
        }

        public void DL14(object sender, AsyncCompletedEventArgs e)
        {
            sw.Stop();
            FileDL = "base13.tar.mlk";
            string GenByte = new System.IO.FileInfo(TempPath + @"\" + FileDL).Length.ToString();
            if (ByteBase13.Equals(GenByte))
            {
                String Server = textBox2.Text;
                Timeout.Start();
                FileDL = "base14.tar.mlk";
                if (File.Exists(TempPath + @"\" + FileDL)) { DL15(null, null); }
                else {
                    Canard = new WebClient();
                    Canard.DownloadFileCompleted += new AsyncCompletedEventHandler(DL15);
                    Canard.DownloadProgressChanged += new DownloadProgressChangedEventHandler(ProgressChanged);
                    Canard.DownloadFileAsync(new Uri(Server + FileDL), TempPath + @"\" + FileDL);
                    progressBar2.Value = 64;
                    label8.Text = "Part 14 of 22";
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
                    DL13(null, null);
                }
                else if (dialogResult == DialogResult.No)
                {
                    //here be poni
                }
            }
        }

        public void DL15(object sender, AsyncCompletedEventArgs e)
        {
            sw.Stop();
            FileDL = "base14.tar.mlk";
            string GenByte = new System.IO.FileInfo(TempPath + @"\" + FileDL).Length.ToString();
            if (ByteBase14.Equals(GenByte))
            {
                String Server = textBox2.Text;
                Timeout.Start();
                FileDL = "base15.tar.mlk";
                if (File.Exists(TempPath + @"\" + FileDL)) { DL16(null, null); }
                else {
                    Canard = new WebClient();
                    Canard.DownloadFileCompleted += new AsyncCompletedEventHandler(DL16);
                    Canard.DownloadProgressChanged += new DownloadProgressChangedEventHandler(ProgressChanged);
                    Canard.DownloadFileAsync(new Uri(Server + FileDL), TempPath + @"\" + FileDL);
                    progressBar2.Value = 69;
                    label8.Text = "Part 15 of 22";
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
                    DL14(null, null);
                }
                else if (dialogResult == DialogResult.No)
                {
                    //here be poni
                }
            }
        }

        public void DL16(object sender, AsyncCompletedEventArgs e)
        {
            sw.Stop();
            FileDL = "base15.tar.mlk";
            string GenByte = new System.IO.FileInfo(TempPath + @"\" + FileDL).Length.ToString();
            if (ByteBase15.Equals(GenByte))
            {
                String Server = textBox2.Text;
                Timeout.Start();
                FileDL = "base16.tar.mlk";
                if (File.Exists(TempPath + @"\" + FileDL)) { DL17(null, null); }
                else {
                    Canard = new WebClient();
                    Canard.DownloadFileCompleted += new AsyncCompletedEventHandler(DL17);
                    Canard.DownloadProgressChanged += new DownloadProgressChangedEventHandler(ProgressChanged);
                    Canard.DownloadFileAsync(new Uri(Server + FileDL), TempPath + @"\" + FileDL);
                    progressBar2.Value = 74;
                    label8.Text = "Part 16 of 22";
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
                    DL15(null, null);
                }
                else if (dialogResult == DialogResult.No)
                {
                    //here be poni
                }
            }
        }

        public void DL17(object sender, AsyncCompletedEventArgs e)
        {
            sw.Stop();
            FileDL = "base16.tar.mlk";
            string GenByte = new System.IO.FileInfo(TempPath + @"\" + FileDL).Length.ToString();
            if (ByteBase16.Equals(GenByte))
            {
                String Server = textBox2.Text;
                Timeout.Start();
                FileDL = "base17.tar.mlk";
                if (File.Exists(TempPath + @"\" + FileDL)) { DL18(null, null); }
                else {
                    Canard = new WebClient();
                    Canard.DownloadFileCompleted += new AsyncCompletedEventHandler(DL18);
                    Canard.DownloadProgressChanged += new DownloadProgressChangedEventHandler(ProgressChanged);
                    Canard.DownloadFileAsync(new Uri(Server + FileDL), TempPath + @"\" + FileDL);
                    progressBar2.Value = 79;
                    label8.Text = "Part 17 of 22";
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
                    DL16(null, null);
                }
                else if (dialogResult == DialogResult.No)
                {
                    //here be poni
                }
            }
        }

        public void DL18(object sender, AsyncCompletedEventArgs e)
        {
            sw.Stop();
            FileDL = "base17.tar.mlk";
            string GenByte = new System.IO.FileInfo(TempPath + @"\" + FileDL).Length.ToString();
            if (ByteBase17.Equals(GenByte))
            {
                String Server = textBox2.Text;
                Timeout.Start();
                FileDL = "base18.tar.mlk";
                if (File.Exists(TempPath + @"\" + FileDL)) { DL19(null, null); }
                else {
                    Canard = new WebClient();
                    Canard.DownloadFileCompleted += new AsyncCompletedEventHandler(DL19);
                    Canard.DownloadProgressChanged += new DownloadProgressChangedEventHandler(ProgressChanged);
                    Canard.DownloadFileAsync(new Uri(Server + FileDL), TempPath + @"\" + FileDL);
                    progressBar2.Value = 84;
                    label8.Text = "Part 18 of 22";
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
                    DL17(null, null);
                }
                else if (dialogResult == DialogResult.No)
                {
                    //here be poni
                }
            }
        }

        public void DL19(object sender, AsyncCompletedEventArgs e)
        {
            sw.Stop();
            FileDL = "base18.tar.mlk";
            string GenByte = new System.IO.FileInfo(TempPath + @"\" + FileDL).Length.ToString();
            if (ByteBase18.Equals(GenByte))
            {
                String Server = textBox2.Text;
                Timeout.Start();
                FileDL = "base19.tar.mlk";
                if (File.Exists(TempPath + @"\" + FileDL)) { DL20(null, null); }
                else {
                    Canard = new WebClient();
                    Canard.DownloadFileCompleted += new AsyncCompletedEventHandler(DL20);
                    Canard.DownloadProgressChanged += new DownloadProgressChangedEventHandler(ProgressChanged);
                    Canard.DownloadFileAsync(new Uri(Server + FileDL), TempPath + @"\" + FileDL);
                    progressBar2.Value = 89;
                    label8.Text = "Part 19 of 22";
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
                    DL18(null, null);
                }
                else if (dialogResult == DialogResult.No)
                {
                    //here be poni
                }
            }
        }

        public void DL20(object sender, AsyncCompletedEventArgs e)
        {
            sw.Stop();
            FileDL = "base19.tar.mlk";
            string GenByte = new System.IO.FileInfo(TempPath + @"\" + FileDL).Length.ToString();
            if (ByteBase19.Equals(GenByte))
            {
                String Server = textBox2.Text;
                Timeout.Start();
                FileDL = "base20.tar.mlk";
                if (File.Exists(TempPath + @"\" + FileDL)) { DL21(null, null); }
                else {
                    Canard = new WebClient();
                    Canard.DownloadFileCompleted += new AsyncCompletedEventHandler(DL21);
                    Canard.DownloadProgressChanged += new DownloadProgressChangedEventHandler(ProgressChanged);
                    Canard.DownloadFileAsync(new Uri(Server + FileDL), TempPath + @"\" + FileDL);
                    progressBar2.Value = 94;
                    label8.Text = "Part 20 of 22";
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
                    DL19(null, null);
                }
                else if (dialogResult == DialogResult.No)
                {
                    //here be poni
                }
            }
        }

        public void DL21(object sender, AsyncCompletedEventArgs e)
        {
            sw.Stop();
            FileDL = "base20.tar.mlk";
            string GenByte = new System.IO.FileInfo(TempPath + @"\" + FileDL).Length.ToString();
            if (ByteBase20.Equals(GenByte))
            {
                String Server = textBox2.Text;
                Timeout.Start();
                FileDL = "usdx1.mlu";
                if (File.Exists(TempPath + @"\" + FileDL)) { DL22(null, null); }
                else {
                    Canard = new WebClient();
                    Canard.DownloadFileCompleted += new AsyncCompletedEventHandler(DL22);
                    Canard.DownloadProgressChanged += new DownloadProgressChangedEventHandler(ProgressChanged);
                    Canard.DownloadFileAsync(new Uri(Server + FileDL), TempPath + @"\" + FileDL);
                    progressBar2.Value = 98;
                    label8.Text = "Part 21 of 22";
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
                    DL20(null, null);
                }
                else if (dialogResult == DialogResult.No)
                {
                    //here be poni
                }
            }
        }

        public void DL22(object sender, AsyncCompletedEventArgs e)
        {
            sw.Stop();
            FileDL = "usdx1.mlu";
            string GenByte = new System.IO.FileInfo(TempPath + @"\" + FileDL).Length.ToString();
            if (ByteBase21.Equals(GenByte))
            {
                String Server = textBox2.Text;
                Timeout.Start();
                FileDL = "theme1.mlt";
                if (File.Exists(TempPath + @"\" + FileDL)) { Over(null, null); }
                else {
                    Canard = new WebClient();
                    Canard.DownloadFileCompleted += new AsyncCompletedEventHandler(Over);
                    Canard.DownloadProgressChanged += new DownloadProgressChangedEventHandler(ProgressChanged);
                    Canard.DownloadFileAsync(new Uri(Server + FileDL), TempPath + @"\" + FileDL);
                    progressBar2.Value = 99;
                    label8.Text = "Part 22 of 22";
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
                    DL21(null, null);
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
            if (ByteBase22.Equals(GenByte))
            {
                progressBar1.Value = 100;
                progressBar2.Value = 100;
                label7.Text = " ";
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
            status = "Installation (3 of 22)";
            barvalue = 5;
            if (this.label5.InvokeRequired) { SetTextCallback d = new SetTextCallback(SetText); this.Invoke(d, new object[] { status }); } 
            if (this.progressBar1.InvokeRequired) { SetValueCallback d = new SetValueCallback(SetValue); this.Invoke(d, new object[] { barvalue }); }
            Stream inStream = File.OpenRead(TempPath + @"\base1.tar.mlk");
            TarArchive tarArchive = TarArchive.CreateInputTarArchive(inStream);
            tarArchive.ExtractContents(textBox1.Text + @"\songs");
            tarArchive.Close();
            inStream.Close();
            status = "Installation (4 of 22)";
            barvalue = 10;
            if (this.label5.InvokeRequired) { SetTextCallback d = new SetTextCallback(SetText); this.Invoke(d, new object[] { status }); }
            if (this.progressBar1.InvokeRequired) { SetValueCallback d = new SetValueCallback(SetValue); this.Invoke(d, new object[] { barvalue }); }
            Stream inStream2 = File.OpenRead(TempPath + @"\base2.tar.mlk");
            TarArchive tarArchive2 = TarArchive.CreateInputTarArchive(inStream2);
            tarArchive2.ExtractContents(textBox1.Text + @"\songs");
            tarArchive2.Close();
            inStream2.Close();
            status = "Installation (5 of 22)";
            barvalue = 15;
            if (this.label5.InvokeRequired) { SetTextCallback d = new SetTextCallback(SetText); this.Invoke(d, new object[] { status }); }
            if (this.progressBar1.InvokeRequired) { SetValueCallback d = new SetValueCallback(SetValue); this.Invoke(d, new object[] { barvalue }); }
            Stream inStream3 = File.OpenRead(TempPath + @"\base3.tar.mlk");
            TarArchive tarArchive3 = TarArchive.CreateInputTarArchive(inStream3);
            tarArchive3.ExtractContents(textBox1.Text + @"\songs");
            tarArchive3.Close();
            inStream3.Close();
            status = "Installation (6 of 22)";
            barvalue = 20;
            if (this.label5.InvokeRequired) { SetTextCallback d = new SetTextCallback(SetText); this.Invoke(d, new object[] { status }); }
            if (this.progressBar1.InvokeRequired) { SetValueCallback d = new SetValueCallback(SetValue); this.Invoke(d, new object[] { barvalue }); }
            Stream inStream4 = File.OpenRead(TempPath + @"\base4.tar.mlk");
            TarArchive tarArchive4 = TarArchive.CreateInputTarArchive(inStream4);
            tarArchive4.ExtractContents(textBox1.Text + @"\songs");
            tarArchive4.Close();
            inStream4.Close();
            status = "Installation (7 of 22)";
            barvalue = 25;
            if (this.label5.InvokeRequired) { SetTextCallback d = new SetTextCallback(SetText); this.Invoke(d, new object[] { status }); }
            if (this.progressBar1.InvokeRequired) { SetValueCallback d = new SetValueCallback(SetValue); this.Invoke(d, new object[] { barvalue }); }
            Stream inStream5 = File.OpenRead(TempPath + @"\base5.tar.mlk");
            TarArchive tarArchive5 = TarArchive.CreateInputTarArchive(inStream5);
            tarArchive5.ExtractContents(textBox1.Text + @"\songs");
            tarArchive5.Close();
            inStream5.Close();
            status = "Installation (8 of 22)";
            barvalue = 30;
            if (this.label5.InvokeRequired) { SetTextCallback d = new SetTextCallback(SetText); this.Invoke(d, new object[] { status }); }
            if (this.progressBar1.InvokeRequired) { SetValueCallback d = new SetValueCallback(SetValue); this.Invoke(d, new object[] { barvalue }); }
            Stream inStream6 = File.OpenRead(TempPath + @"\base6.tar.mlk");
            TarArchive tarArchive6 = TarArchive.CreateInputTarArchive(inStream6);
            tarArchive6.ExtractContents(textBox1.Text + @"\songs");
            tarArchive6.Close();
            inStream6.Close();
            status = "Installation (9 of 22)";
            barvalue = 35;
            if (this.label5.InvokeRequired) { SetTextCallback d = new SetTextCallback(SetText); this.Invoke(d, new object[] { status }); }
            if (this.progressBar1.InvokeRequired) { SetValueCallback d = new SetValueCallback(SetValue); this.Invoke(d, new object[] { barvalue }); }
            Stream inStream7 = File.OpenRead(TempPath + @"\base7.tar.mlk");
            TarArchive tarArchive7 = TarArchive.CreateInputTarArchive(inStream7);
            tarArchive7.ExtractContents(textBox1.Text + @"\songs");
            tarArchive7.Close();
            inStream7.Close();
            status = "Installation (10 of 22)";
            barvalue = 40;
            if (this.label5.InvokeRequired) { SetTextCallback d = new SetTextCallback(SetText); this.Invoke(d, new object[] { status }); }
            if (this.progressBar1.InvokeRequired) { SetValueCallback d = new SetValueCallback(SetValue); this.Invoke(d, new object[] { barvalue }); }
            Stream inStream8 = File.OpenRead(TempPath + @"\base8.tar.mlk");
            TarArchive tarArchive8 = TarArchive.CreateInputTarArchive(inStream8);
            tarArchive8.ExtractContents(textBox1.Text + @"\songs");
            tarArchive8.Close();
            inStream8.Close();
            status = "Installation (11 of 22)";
            barvalue = 45;
            if (this.label5.InvokeRequired) { SetTextCallback d = new SetTextCallback(SetText); this.Invoke(d, new object[] { status }); }
            if (this.progressBar1.InvokeRequired) { SetValueCallback d = new SetValueCallback(SetValue); this.Invoke(d, new object[] { barvalue }); }
            Stream inStream9 = File.OpenRead(TempPath + @"\base9.tar.mlk");
            TarArchive tarArchive9 = TarArchive.CreateInputTarArchive(inStream9);
            tarArchive9.ExtractContents(textBox1.Text + @"\songs");
            tarArchive9.Close();
            inStream9.Close();
            status = "Installation (12 of 22)";
            barvalue = 50;
            if (this.label5.InvokeRequired) { SetTextCallback d = new SetTextCallback(SetText); this.Invoke(d, new object[] { status }); }
            if (this.progressBar1.InvokeRequired) { SetValueCallback d = new SetValueCallback(SetValue); this.Invoke(d, new object[] { barvalue }); }
            Stream inStream10 = File.OpenRead(TempPath + @"\base10.tar.mlk");
            TarArchive tarArchive10 = TarArchive.CreateInputTarArchive(inStream10);
            tarArchive10.ExtractContents(textBox1.Text + @"\songs");
            tarArchive10.Close();
            inStream10.Close();
            status = "Installation (13 of 22)";
            barvalue = 55;
            if (this.label5.InvokeRequired) { SetTextCallback d = new SetTextCallback(SetText); this.Invoke(d, new object[] { status }); }
            if (this.progressBar1.InvokeRequired) { SetValueCallback d = new SetValueCallback(SetValue); this.Invoke(d, new object[] { barvalue }); }
            Stream inStream11 = File.OpenRead(TempPath + @"\base11.tar.mlk");
            TarArchive tarArchive11 = TarArchive.CreateInputTarArchive(inStream11);
            tarArchive11.ExtractContents(textBox1.Text + @"\songs");
            tarArchive11.Close();
            inStream11.Close();
            status = "Installation (14 of 22)";
            barvalue = 60;
            if (this.label5.InvokeRequired) { SetTextCallback d = new SetTextCallback(SetText); this.Invoke(d, new object[] { status }); }
            if (this.progressBar1.InvokeRequired) { SetValueCallback d = new SetValueCallback(SetValue); this.Invoke(d, new object[] { barvalue }); }
            Stream inStream12 = File.OpenRead(TempPath + @"\base12.tar.mlk");
            TarArchive tarArchive12 = TarArchive.CreateInputTarArchive(inStream12);
            tarArchive12.ExtractContents(textBox1.Text + @"\songs");
            tarArchive12.Close();
            inStream12.Close();
            status = "Installation (15 of 22)";
            barvalue = 65;
            if (this.label5.InvokeRequired) { SetTextCallback d = new SetTextCallback(SetText); this.Invoke(d, new object[] { status }); }
            if (this.progressBar1.InvokeRequired) { SetValueCallback d = new SetValueCallback(SetValue); this.Invoke(d, new object[] { barvalue }); }
            Stream inStream13 = File.OpenRead(TempPath + @"\base13.tar.mlk");
            TarArchive tarArchive13 = TarArchive.CreateInputTarArchive(inStream13);
            tarArchive13.ExtractContents(textBox1.Text + @"\songs");
            tarArchive13.Close();
            inStream13.Close();
            status = "Installation (16 of 22)";
            barvalue = 70;
            if (this.label5.InvokeRequired) { SetTextCallback d = new SetTextCallback(SetText); this.Invoke(d, new object[] { status }); }
            if (this.progressBar1.InvokeRequired) { SetValueCallback d = new SetValueCallback(SetValue); this.Invoke(d, new object[] { barvalue }); }
            Stream inStream14 = File.OpenRead(TempPath + @"\base14.tar.mlk");
            TarArchive tarArchive14 = TarArchive.CreateInputTarArchive(inStream14);
            tarArchive14.ExtractContents(textBox1.Text + @"\songs");
            tarArchive14.Close();
            inStream14.Close();
            status = "Installation (17 of 22)";
            barvalue = 75;
            if (this.label5.InvokeRequired) { SetTextCallback d = new SetTextCallback(SetText); this.Invoke(d, new object[] { status }); }
            if (this.progressBar1.InvokeRequired) { SetValueCallback d = new SetValueCallback(SetValue); this.Invoke(d, new object[] { barvalue }); }
            Stream inStream15 = File.OpenRead(TempPath + @"\base15.tar.mlk");
            TarArchive tarArchive15 = TarArchive.CreateInputTarArchive(inStream15);
            tarArchive15.ExtractContents(textBox1.Text + @"\songs");
            tarArchive15.Close();
            inStream15.Close();
            status = "Installation (18 of 22)";
            barvalue = 80;
            if (this.label5.InvokeRequired) { SetTextCallback d = new SetTextCallback(SetText); this.Invoke(d, new object[] { status }); }
            if (this.progressBar1.InvokeRequired) { SetValueCallback d = new SetValueCallback(SetValue); this.Invoke(d, new object[] { barvalue }); }
            Stream inStream16 = File.OpenRead(TempPath + @"\base16.tar.mlk");
            TarArchive tarArchive16 = TarArchive.CreateInputTarArchive(inStream16);
            tarArchive16.ExtractContents(textBox1.Text + @"\songs");
            tarArchive16.Close();
            inStream16.Close();
            status = "Installation (19 of 22)";
            barvalue = 85;
            if (this.label5.InvokeRequired) { SetTextCallback d = new SetTextCallback(SetText); this.Invoke(d, new object[] { status }); }
            if (this.progressBar1.InvokeRequired) { SetValueCallback d = new SetValueCallback(SetValue); this.Invoke(d, new object[] { barvalue }); }
            Stream inStream17 = File.OpenRead(TempPath + @"\base17.tar.mlk");
            TarArchive tarArchive17 = TarArchive.CreateInputTarArchive(inStream17);
            tarArchive17.ExtractContents(textBox1.Text + @"\songs");
            tarArchive17.Close();
            inStream17.Close();
            status = "Installation (20 of 22)";
            barvalue = 90;
            if (this.label5.InvokeRequired) { SetTextCallback d = new SetTextCallback(SetText); this.Invoke(d, new object[] { status }); }
            if (this.progressBar1.InvokeRequired) { SetValueCallback d = new SetValueCallback(SetValue); this.Invoke(d, new object[] { barvalue }); }
            Stream inStream18 = File.OpenRead(TempPath + @"\base18.tar.mlk");
            TarArchive tarArchive18 = TarArchive.CreateInputTarArchive(inStream18);
            tarArchive18.ExtractContents(textBox1.Text + @"\songs");
            tarArchive18.Close();
            inStream18.Close();
            status = "Installation (21 of 22)";
            barvalue = 95;
            if (this.label5.InvokeRequired) { SetTextCallback d = new SetTextCallback(SetText); this.Invoke(d, new object[] { status }); }
            if (this.progressBar1.InvokeRequired) { SetValueCallback d = new SetValueCallback(SetValue); this.Invoke(d, new object[] { barvalue }); }
            Stream inStream19 = File.OpenRead(TempPath + @"\base19.tar.mlk");
            TarArchive tarArchive19 = TarArchive.CreateInputTarArchive(inStream19);
            tarArchive19.ExtractContents(textBox1.Text + @"\songs");
            tarArchive19.Close();
            inStream19.Close();
            status = "Installation (22 of 22)";
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
