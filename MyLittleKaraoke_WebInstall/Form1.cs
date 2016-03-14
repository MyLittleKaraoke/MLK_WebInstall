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
        private string[,] FileAddressList;
        private int sucessfullyDownloadedCount = 0;
        public string legacy;
        private HelperClass cHelper = new HelperClass();
        private string Downloaded;
        private string Downloaded2;
        private string status;
        private int barvalue;
        private string TempPath = Registry.GetValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Shell Folders", "{374DE290-123F-4565-9164-39C4925E467B}", String.Empty).ToString();
        delegate void SetTextCallback(string status);
        delegate void SetValueCallback(int barvalue);
        private string CurrentFileDLName;
        private WebClient Canard;
        private System.Windows.Forms.Timer Timeout;
        Stopwatch sw = new Stopwatch();

        public Form1()
        {
            try
            {
                InitializeComponent();
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
                    TextBoxInstallPath.Text = folderBrowserDialog.SelectedPath;
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

        private void DownloadAndInstallButton_Click(object sender, EventArgs e)
        {
            try
            {
                this.Height = 450;
                String Server = textBox2.Text;
                Timeout = new System.Windows.Forms.Timer();
                Application.DoEvents();
                // TIMER - Launch code to check if download is still active every 30 seconds)
                Timeout.Tick += new EventHandler(CheckTimeout);
                Timeout.Interval = 30000;
                Timeout.Start();
                // TIMER - End.
                FileAddressList = cHelper.GetFileAddressesListFromWeb(WebFileList);
                DownloadFiles(null, null);
            }
            catch (Exception ex)
            {
                cHelper.ShowErrorMessageDialog(ex.Message, ex.StackTrace, "Form1.DownloadAndInstallButton_Click");
            }

        }

        public void DownloadFiles(object sender, AsyncCompletedEventArgs e)
        {
            //We stop the stopwatch.
            sw.Stop();
            try
            {
                for (int intCurrFile = sucessfullyDownloadedCount; intCurrFile < FileAddressList.GetLength(0); intCurrFile++)
                {
                    CurrentFileDLName = Path.GetFileName((new Uri(FileAddressList[intCurrFile, 0])).AbsolutePath);
                    label8.Text = "Part " + (intCurrFile + 1) + " of " + FileAddressList.GetLength(0);
                    if (File.Exists(TempPath + @"\" + CurrentFileDLName))
                    {
                        string GenByte = new System.IO.FileInfo(TempPath + @"\" + CurrentFileDLName).Length.ToString();
                        //if it match, then we download the next file!
                        if (FileAddressList[intCurrFile, 1].Equals(GenByte) == false)
                        {
                            // Delete the file if it exist.
                            if (File.Exists(TempPath + @"\" + CurrentFileDLName)) { File.Delete(TempPath + @"\" + CurrentFileDLName); }
                            // Ask user to redownload or cancel.
                            DialogResult dialogResult = MessageBox.Show("the downloaded file " + CurrentFileDLName + " is corrupted. Do you want to retry downloading?", "File verification FAILED", MessageBoxButtons.YesNo);
                            if (dialogResult == DialogResult.Yes)
                            {
                                Downloaded = null;
                                Downloaded2 = null;
                                GenByte = null;
                                Timeout.Stop();
                                DownloadAndInstallButton_Click(null, null);
                                break;
                            }
                            else if (dialogResult == DialogResult.No)
                            {
                                cHelper.ShowErrorMessageDialog("User aborted after corruped file downloads. Closing application now.", "", "");
                                break;
                            }
                        }
                        else
                        {
                            sucessfullyDownloadedCount = Math.Max(sucessfullyDownloadedCount, (intCurrFile+1));
                        }
                    }
                    // If the file doesn't exist or has been detected as incorrect and deleted, we QUACK a copy!
                    else
                    {
                        Canard = new WebClient();
                        Canard.DownloadFileCompleted += new AsyncCompletedEventHandler(DownloadFiles);
                        Canard.DownloadProgressChanged += new DownloadProgressChangedEventHandler(ProgressChanged);
                        Canard.DownloadFileAsync(new Uri(FileAddressList[intCurrFile, 0]), TempPath + @"\" + CurrentFileDLName);
                        progressBar2.Value = 0;
                        break;
                    }
                    if (sucessfullyDownloadedCount == FileAddressList.GetLength(0))
                    {
                        //all files are now successfully downloaded.
                        InstallationFunctionThread();
                    }
                    //We start the stopwatch to calculate progress.
                    sw.Start();
                }
            }
            catch (Exception ex)
            {
                cHelper.ShowErrorMessageDialog(ex.Message, ex.StackTrace, "DownloadFiles");
            }
        }

        public void InstallationFunctionThread()
        {
            try
            {
                for (int intCurrFile = 0; intCurrFile < FileAddressList.GetLength(0); intCurrFile++)
                {
                    barvalue = ( 100 * intCurrFile ) / FileAddressList.GetLength(0);
                    CurrentFileDLName = Path.GetFileName((new Uri(FileAddressList[intCurrFile, 0])).AbsolutePath);
                    status = "Installation (" + (intCurrFile+1) + " of " + FileAddressList.GetLength(0) + ")";
                    if (this.label5.InvokeRequired) { SetTextCallback d = new SetTextCallback(SetText); this.Invoke(d, new object[] { status }); }
                    if (this.progressBar1.InvokeRequired) { SetValueCallback d = new SetValueCallback(SetValue); this.Invoke(d, new object[] { barvalue }); }
                    Stream inStream21 = File.OpenRead(TempPath + @"\" + CurrentFileDLName);
                    TarArchive tarArchive21 = TarArchive.CreateInputTarArchive(inStream21);
                    tarArchive21.ExtractContents(TextBoxInstallPath.Text);
                    tarArchive21.Close();
                    inStream21.Close();
                }
            
                status = "Installation (Registering component)";
                barvalue = 100;
                if (this.label5.InvokeRequired) { SetTextCallback d = new SetTextCallback(SetText); this.Invoke(d, new object[] { status }); }
                if (this.progressBar1.InvokeRequired) { SetValueCallback d = new SetValueCallback(SetValue); this.Invoke(d, new object[] { barvalue }); }
                if (cHelper.SetInstallLocationInRegistryKey(TextBoxInstallPath.Text) == false)
                {
                    MessageBox.Show("Installation was successfull but setting the installation path setting failed.", "Setting registry key failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

                status = "Installation is done!";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Installation was successfull but setting the installation path setting failed.", "Setting registry key failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            status = "Installation is done!";

            MessageBox.Show("Sing!");

        }

        private void SetText(string text)
        {
            this.label5.Text = status;
        }

        private void SetValue(int barvalue)
        {
            this.progressBar1.Value = barvalue;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (cHelper.IsAdministrator() == false)
            {
                // Restart program and run as admin
                var exeName = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName;
                ProcessStartInfo startInfo = new ProcessStartInfo(exeName);
                startInfo.Verb = "runas";
                System.Diagnostics.Process.Start(startInfo);
                Application.Exit();
                return;
            }


        }
    }
}
