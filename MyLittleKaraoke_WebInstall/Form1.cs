﻿// Wouldnt work without @gameleon_eqd, think to add credits! (Network timeout)
// also Tucker @ irc.ponychat.net #geek (Resume function)

using System;
using System.ComponentModel;
using System.Windows.Forms;
using System.Net;
using System.Diagnostics;
using OperatingSystemInfo;
using System.IO;
using ICSharpCode.SharpZipLib.Tar;
using Microsoft.Win32;

namespace MyLittleKaraoke_WebInstall
{
    public partial class Form1 : Form
    {
        private Uri WebFileList = new Uri("https://www.mylittlekaraoke.com/store/webinst/windows.webinst");
        private string LocalFilenameWeblist = "windows.dvdinst";
        private string[,] FileAddressList;
        private string InstallFolderPath = "";
        private string InstalledVersion = "none";
        private string InstalledPackage = "none";
        private int sucessfullyDownloadedCount = 0;
        public string legacy;
        private HelperClass cHelper = new HelperClass();
        private VersionStuff cVersion = new VersionStuff();
        private string Downloaded;
        private string Downloaded2;
        private string status;
        private string TempPath = Registry.GetValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Shell Folders", "{374DE290-123F-4565-9164-39C4925E467B}", String.Empty).ToString();
        delegate void SetTextCallback(string status);
        delegate void SetValueCallback(int barvalue);
        private short DownloadRetryCount = 0;
        private string CurrentFileDLName;
        private WebClient Canard;
        private Timer Timeout;
        Stopwatch sw = new Stopwatch();
        private string SelfPath = System.Reflection.Assembly.GetEntryAssembly().Location;
        private string SelfName = Path.GetFileName(System.Reflection.Assembly.GetEntryAssembly().Location);

        public Form1()
        {
            try
            {
                InitializeComponent();
                // We determine if the user run Windows XP as path for configurations aren't the same.
                OSInfo osInfo = new OSInfo();
                string OS = string.Format("{0}", osInfo.GetOSName);
                string wxp = "Windows XP";
                if (OS.Equals(wxp)) { legacy = "1"; }
                else { legacy = "0"; }
            }
            catch (Exception ex)
            {
                cHelper.ShowErrorMessageDialog(ex.Message, ex.StackTrace, "MyLittleKaraoke_WebInstall.Form1");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                // Create a new instance of FolderBrowserDialog.
                FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
                // A new folder button will display in FolderBrowserDialog.
                folderBrowserDialog.ShowNewFolderButton = true;
                try {folderBrowserDialog.SelectedPath = TextBoxInstallPath.Text;}
                catch (Exception) { ;};
                //Show FolderBrowserDialog
                DialogResult dlgResult = folderBrowserDialog.ShowDialog();
                if (dlgResult.Equals(DialogResult.OK))
                {
                    //Send content to string
                    string path = folderBrowserDialog.SelectedPath;
                    if (path.Contains("MyLittleKaraoke") == false)
                    {
                        path += "\\MyLittleKaraoke";
                    }
                    //Show selected folder path in textbox1.
                    TextBoxInstallPath.Text = path;
                    InstallFolderPath = path; 
                    //Browsing start from root folder.
                    Environment.SpecialFolder rootFolder = folderBrowserDialog.RootFolder;
                    RefreshInitialization();
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
            string downloadspeed = string.Format("{0} Mbit/s", (e.BytesReceived / 1024d / 1024d * 8d / sw.Elapsed.TotalSeconds).ToString("0.00"));
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
                //check if user wants to install to folder which contains the installer already
                if (SelfPath.Contains(TextBoxInstallPath.Text))
                {
                    // Copy program to %temp% and run as admin
                    string tempPath = Path.GetTempPath() + SelfName;

                    File.Copy(SelfPath, tempPath, true);
                    MessageBox.Show("We detected that you try to install to where this insaller is located. Installer will now restart from a temporary location. \n" +
                        "Please then continue the installation.", "Installer restart from temporary folder required");
                    cHelper.StartProcessAsAdmin(tempPath);
                }
                //check for enough free disk-space
                if (cHelper.GetFreeDiskSpaceGB(TempPath) < 16 && (ActionNextLabel.Text == "Action: uninstall + new installation" || ActionNextLabel.Text ==  "Action: new installation"))
                {
                    MessageBox.Show("Warning, your harddrive which contains the users 'Downloads' folder does not have enough free disk space. Please choose a different temporary downloads folder after clicking OK and then click the install button again.");
                    // Create a new instance of FolderBrowserDialog.
                    FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
                    // A new folder button will display in FolderBrowserDialog.
                    folderBrowserDialog.ShowNewFolderButton = true;
                    try { folderBrowserDialog.SelectedPath = TempPath; }
                    catch (Exception) { ;};
                    //Show FolderBrowserDialog
                    DialogResult dlgResult = folderBrowserDialog.ShowDialog();
                    if (dlgResult.Equals(DialogResult.OK))
                    {
                        //Send content to string
                        string path = folderBrowserDialog.SelectedPath;
                        //Show selected folder path in textbox1.
                        TempPath = folderBrowserDialog.SelectedPath;
                    }
                    return;
                }
                if (cHelper.GetFreeDiskSpaceGB(InstallFolderPath) < 16 && (ActionNextLabel.Text == "Action: uninstall + new installation" || ActionNextLabel.Text == "Action: new installation"))
                {
                    MessageBox.Show("Warning, your harddrive installation destination folder does not have enough free disk space. Please choose a different installation folder and click the install button again.");
                    return;
                }
                if (cHelper.IsProcessOpen("ultrastardx") || cHelper.IsProcessOpen("My Little Karaoke Launcher") || cHelper.IsProcessOpen("MLKHelperGUI"))
                {
                    MessageBox.Show("Please close any other programs from this game and then click on the install button again. Aborting.", "Please close all other MLK applications.");
                    return;
                }
                DownloadAndInstallButton.Enabled = false;
                button1.Enabled = false;
                TextBoxInstallPath.Enabled = false;
                Height = 500;
                Timeout = new Timer();
                Application.DoEvents();
                // TIMER - Launch code to check if download is still active every 30 seconds)
                Timeout.Tick += new EventHandler(CheckTimeout);
                Timeout.Interval = 30000;
                Timeout.Start();
                // TIMER - End.
                Application.DoEvents();
                if (cHelper.IsDVDInstallation())
                    FileAddressList = cHelper.GetFileAddressesListFromLocal(Path.Combine(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "content"),LocalFilenameWeblist));
                else
                    FileAddressList = cHelper.GetFileAddressesListFromWeb(WebFileList);
                if (InstalledPackage.Equals("none") == false)
                    FileAddressList = cVersion.FilterPackageList(FileAddressList, InstalledPackage);
                if (FileAddressList.GetLength(0) < 1 || FileAddressList[0, 0] == null || FileAddressList[0, 0].Equals(""))
                {
                    MessageBox.Show("It seems you already have all updates successfully installed! Nothing to do here.", "No updates available!", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    Application.Exit();
                    return;
                }
                label10.Text = "Part 0 of " + FileAddressList.GetLength(0);
                Application.DoEvents();
                if (cHelper.IsDVDInstallation())
                {
                    Application.DoEvents();
                    InstallationFunctionThread();
                }
                else
                    DownloadFiles(null, null);
            }
            catch (Exception ex)
            {
                cHelper.ShowErrorMessageDialog(ex.Message, ex.StackTrace, "Form1.DownloadAndInstallButton_Click");
            }
        }

        private void PrepareForSetup()
        {
            try
            {
                if (ActionNextLabel.Text == "Action: uninstall, but keep songs, then install updates")
                {
                    cHelper.Run_MLK_SIM4_Uninstaller(InstallFolderPath);
                }
                else if (ActionNextLabel.Text == "Action: uninstall + new installation")
                {
                    cHelper.Run_Old_Uninstaller();
                };
                if (Directory.Exists(InstallFolderPath))
                {
                    if (Directory.Exists(Path.Combine(InstallFolderPath, "songs")) == false)
                    {
                        Directory.CreateDirectory(Path.Combine(InstallFolderPath,"songs"));
                    };
                }
                else
                {
                    Directory.CreateDirectory(InstallFolderPath);
                }
                cHelper.SetWritePermissionForLoggedInUsers(InstallFolderPath);
            }
            catch (Exception ex)
            {
                cHelper.ShowErrorMessageDialog(ex.Message, ex.StackTrace, "PrepareForSetup()");
            }
        }

        public void DownloadFiles(object sender, AsyncCompletedEventArgs e)
        {
            //We stop the stopwatch.
            //sw.Stop();
            try
            {
                for (int intCurrFile = sucessfullyDownloadedCount; intCurrFile < FileAddressList.GetLength(0); intCurrFile++)
                {
                    sw.Reset();
                    Application.DoEvents();
                    if (FileAddressList[intCurrFile,0] == null)
                    {
                        break;
                    }
                    CurrentFileDLName = Path.GetFileName((new Uri(FileAddressList[intCurrFile, 0])).AbsolutePath);
                    label8.Text = "Part " + (intCurrFile + 1) + " of " + FileAddressList.GetLength(0);
                    progressBar2.Value = 100 * intCurrFile / FileAddressList.GetLength(0);
                    if (cVersion.IsMlkSimAC3Package(CurrentFileDLName) && ActionNextLabel.Text == "Action: uninstall, but keep songs, then install updates")
                        continue;
                    if (File.Exists(Path.Combine(TempPath, CurrentFileDLName)))
                    {
                        string GenByte = new System.IO.FileInfo(Path.Combine(TempPath, CurrentFileDLName)).Length.ToString();
                        //if it match, then we download the next file!
                        if (FileAddressList[intCurrFile, 1].Equals(GenByte) == false && GenByte.Equals("483491840") ==false)
                        {
                            // Delete the file if it exist.
                            if (File.Exists(Path.Combine(TempPath, CurrentFileDLName))) { File.Delete(Path.Combine(TempPath, CurrentFileDLName)); }
                            // Ask user to redownload or cancel.
                            if (DownloadRetryCount < 5)
                            {
                                Downloaded = null;
                                Downloaded2 = null;
                                GenByte = null;
                                Timeout.Stop();
                                DownloadRetryCount += 1;
                                DownloadAndInstallButton_Click(null, null);
                                break;
                            }
                            else
                            {
                                DownloadRetryCount = 3;
                                DialogResult dialogResult = MessageBox.Show("the downloaded file " + CurrentFileDLName + " is corrupted. Please make sure you use a stable internet connection. Do you want to retry downloading?", "File verification FAILED", MessageBoxButtons.YesNo);
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
                        
                        sw.Start();
                        Canard.DownloadFileAsync(new Uri(FileAddressList[intCurrFile, 0]), Path.Combine(TempPath, CurrentFileDLName));
                        progressBar1.Value = 0;
                        break;
                    }
                }
                if (sucessfullyDownloadedCount == FileAddressList.GetLength(0))
                {
                    //all files are now successfully downloaded.
                    progressBar2.Value = 100;
                    InstallationFunctionThread();
                }
                //We start the stopwatch to calculate progress.
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

                Application.DoEvents();
                PrepareForSetup();
                Application.DoEvents();
                for (int intCurrFile = 0; intCurrFile < FileAddressList.GetLength(0); intCurrFile++)
                {
                    Application.DoEvents();
                    progressBar3.Value = ( 100 * intCurrFile ) / FileAddressList.GetLength(0);
                    CurrentFileDLName = Path.GetFileName((new Uri(FileAddressList[intCurrFile, 0])).AbsolutePath);
                    status = "Installation (" + (intCurrFile+1) + " of " + FileAddressList.GetLength(0) + ")";
                    label10.Text = "Part " + (intCurrFile + 1) + " of " + FileAddressList.GetLength(0);
                    if (cVersion.IsMlkSimAC3Package(CurrentFileDLName) && ActionNextLabel.Text == "Action: uninstall, but keep songs, then install updates")
                        continue;
                    if (CurrentFileDLName.Contains("clean_downloads"))
                    {
                        try { Directory.Delete(Path.Combine(Path.Combine(InstallFolderPath, "songs"), "Downloads"), true); }
                        catch (Exception) { ;}
                        try { Directory.Delete(Path.Combine(Path.Combine(InstallFolderPath, "songs"), "Fixes"), true); }
                        catch (Exception) { ;}
                        try { Directory.Delete(Path.Combine(Path.Combine(InstallFolderPath, "songs"), "Duets"), true); }
                        catch (Exception) { ;}
                        continue;
                    }
                    Stream inStream21;
                    if (cHelper.IsDVDInstallation())
                    {
                        String targetPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "content");
                        inStream21 = File.OpenRead(Path.Combine(targetPath, CurrentFileDLName)); 
                    }
                    else
                        inStream21 = File.OpenRead(Path.Combine(TempPath, CurrentFileDLName)); // TempPath
                    TarArchive tarArchive21 = TarArchive.CreateInputTarArchive(inStream21);

                    if (CurrentFileDLName.EndsWith(".mlk"))
                        tarArchive21.ExtractContents(Path.Combine(InstallFolderPath, "songs"));
                    else if (CurrentFileDLName.EndsWith(".mlt"))
                        tarArchive21.ExtractContents(Path.Combine(InstallFolderPath, "themes"));
                    else if (CurrentFileDLName.EndsWith(".mlu"))
                        tarArchive21.ExtractContents(InstallFolderPath);
                    else
                        cHelper.ShowErrorMessageDialog("Invalid filetype found in FileAddressList!", CurrentFileDLName, "InstallationFunctionThread()");
                    tarArchive21.Close();
                    inStream21.Close();
                    if (checkBox1.Checked && cHelper.IsDVDInstallation() == false && File.Exists(Path.Combine(TempPath, CurrentFileDLName)))
                    {
                            Application.DoEvents();
                            File.Delete(Path.Combine(TempPath, CurrentFileDLName));
                    }
                }
                progressBar3.Value = 100;
                if (cHelper.SetInstallLocationInRegistryKey(TextBoxInstallPath.Text) == false)
                {
                    MessageBox.Show("Installation was successfull but setting the installation path setting failed.", "Setting registry key failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                //cVersion.SetSongVersion(InstallFolderPath, NewMlkSimVersion);
                cVersion.SetPackageVersion(InstallFolderPath, Path.GetFileName((new Uri(FileAddressList[FileAddressList.GetLength(0)-1, 0])).AbsolutePath));
                cHelper.CreateStartmenuShortcut(Path.Combine(TextBoxInstallPath.Text, "MLK Instruction Manual.pdf"), "My Little Karaoke Instruction Manual");
                cHelper.CreateStartmenuShortcut(Path.Combine(TextBoxInstallPath.Text, "My Little Karaoke Launcher.exe"), "My Little Karaoke - Singing is Magic");
                cHelper.CreateStartmenuShortcut("https://www.mylittlekaraoke.com/highscores/", "MLK - Online Leaderboard");
                cHelper.SetAssociation(".mlk", "MLK_Installer_Package", Path.Combine(TextBoxInstallPath.Text, "MLKHelperGUI.exe"), "MLK Song Package");
                status = "Installation is done!";
                RefreshInitialization();
            }
            catch (Exception ex)
            {
                cHelper.ShowErrorMessageDialog(ex.Message, ex.StackTrace, "InstallationFunctionThread");
            }

            status = "Installation is done!";
            MessageBox.Show("Installation of MyLittleKaraoke was successfull! Have fun!");
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
            try
            {
                if (cHelper.IsInGameDirectory()) {
                    // Copy program to %temp% and run as admin
                    string tempPath = Path.GetTempPath() + SelfName;

                    File.Copy(SelfPath, tempPath, true);
                    cHelper.StartProcessAsAdmin(tempPath);
                    Application.Exit();
                    return;
                }

                if (!cHelper.IsAdministrator())
                {
                    // Restart program as admin
                    cHelper.StartProcessAsAdmin(SelfPath);
                    Application.Exit();
                    return;
                }

                string InstLocation = cHelper.GetInstallLocationfromRegistryKey();
                if (InstLocation != null && InstLocation.Equals("") != true && Directory.Exists(InstLocation))
                {
                    InstallFolderPath = InstLocation;
                }
                else if (8 == IntPtr.Size || (!string.IsNullOrEmpty(Environment.GetEnvironmentVariable("PROCESSOR_ARCHITEW6432"))))
                {
                    InstallFolderPath = Path.Combine(Environment.GetEnvironmentVariable("ProgramFiles(x86)"), "MyLittleKaraoke");
                }
                else
                {
                    InstallFolderPath = Path.Combine(Environment.GetEnvironmentVariable("ProgramFiles"), "MyLittleKaraoke");
                }

                TextBoxInstallPath.Text = InstallFolderPath;
                RefreshInitialization();
            }
            catch (Exception ex)
            {
                cHelper.ShowErrorMessageDialog(ex.Message, ex.StackTrace, "Form1_Load");
            }
            try
            {
                if (!cHelper.IsDVDInstallation())
                {
                    AutoUpdater.Start("https://www.mylittlekaraoke.com/store/webinst/WebInstall.xml");
                }
            }
            catch (Exception ex)
            {
                cHelper.ShowErrorMessageDialog(ex.Message, ex.StackTrace, "Form1_Load::Updater");
            }
        }

        private void RefreshInitialization()
        {
            //Get installed Version
            InstalledVersion = cVersion.GetSongVersion(InstallFolderPath);
            InstalledPackage = cVersion.GetPackageVersion(InstallFolderPath);
            DownloadAndInstallButton.Text = "Install the Game!";
            if (InstalledVersion.Equals("none") == true)
                ActionNextLabel.Text = "Action: new installation";
            else if (InstalledPackage.Equals("none") == false)
            {
                ActionNextLabel.Text = "Action: only install updates";
                DownloadAndInstallButton.Text = "Install Updates";
            }
            else
                ActionNextLabel.Text = "Action: uninstall, but keep songs, then install updates";
            if (InstalledVersion.StartsWith("2.") || InstalledVersion.StartsWith("3.") || InstalledVersion.StartsWith("4."))
            {
                MessageBox.Show("Your installed version of MyLittleKaraoke is too old to be automatically updated." + Environment.NewLine +
                    "Thus, a full new installation will be required.", "Very old version detected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                ActionNextLabel.Text = "Action: uninstall + new installation";
            }
            InstalledVersionLabel.Text = "Installed version: " + InstalledVersion;
            InstalledPackageLabel.Text = "Installed package: " + InstalledPackage;
        }

        private void TextBoxInstallPath_TextChanged(object sender, EventArgs e)
        {
            try
            {
                InstallFolderPath = TextBoxInstallPath.Text;
                RefreshInitialization();
            }
            catch (Exception ex)
            {
                cHelper.ShowErrorMessageDialog(ex.Message, ex.StackTrace, "BrowseInstallerLocationButton_Click");
            }
        }
    }
}
