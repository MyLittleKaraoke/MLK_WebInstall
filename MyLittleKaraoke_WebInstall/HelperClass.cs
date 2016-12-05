using System;
using System.Text;
using System.Net;
using System.IO;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Windows.Forms;
using Microsoft.Win32;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Diagnostics;

namespace MyLittleKaraoke_WebInstall
{
    class HelperClass
    {
        private string SelfPath = System.Reflection.Assembly.GetEntryAssembly().Location;

        public void ShowErrorMessageDialog(string sErrorBasic, string sErrorStacktrace, string sLocation)
        {
            MessageBox.Show("Derpy is awfully sorry, but this MyLittleKaraoke Installer just encountered an error.\n" +
                "For help, see www.mylittlekaraoke.com/forum\n\n" +
                "Error description: " + sErrorBasic + "\n\nError location: "+ sLocation + "\n\nExtended information:\n" + sErrorStacktrace, "Error in " + sLocation, MessageBoxButtons.OK, MessageBoxIcon.Error);
            
            if (Application.MessageLoop)
            {
                Application.Exit();
            }
            else
            {
                Environment.Exit(1);
            };
        }

        public string[,] GetFileAddressesListFromWeb(Uri UpdaterFileAddressUrl)
        {
            string[] WebPageContentLines = GetWebPageContent(UpdaterFileAddressUrl.OriginalString).Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None);
            string[,] FileTableNx2 = new string[WebPageContentLines.Length / 2, 2];
            for (int i = 0; i < WebPageContentLines.Length -1; i++) //-1 because of the linux-typical line-break at file-end
            {
                try 
                {
                    FileTableNx2[i / 2, i % 2] = WebPageContentLines[i];
                }
                catch (Exception) { ;}
            }
            return FileTableNx2;
        }

        public string[,] GetFileAddressesListFromLocal(string LocalFilenameWeblist)
        {
            string[] LocalFileContentLines = File.ReadAllLines(LocalFilenameWeblist);
            string[,] FileTableNx2 = new string[LocalFileContentLines.Length / 2, 2];
            for (int i = 0; i < LocalFileContentLines.Length - 1; i++) //-1 because of the linux-typical line-break at file-end
            {
                try
                {
                    FileTableNx2[i / 2, i % 2] = LocalFileContentLines[i];
                }
                catch (Exception) { ;}
            }
            return FileTableNx2;
        }

        private string GetWebPageContent(string PageURL)
        {
            try
            {
                WebRequest Request = WebRequest.Create(PageURL);
                WebResponse Response = Request.GetResponse();
                StreamReader Reader = new StreamReader(Response.GetResponseStream());
                return Reader.ReadToEnd();
            }
            catch (WebException ex)
            {
                var statusCode = ((HttpWebResponse)ex.Response).StatusCode;
                ShowErrorMessageDialog("Downloading the filelist from web failed!", "Webserver returned status code: " + statusCode, "GetWebPageContent(string PageURL)");
                return "An error occurred, status code: " + statusCode;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool SetInstallLocationInRegistryKey(string InstallPath)
        {
            try
            {
                string regkey = "\"" + InstallPath + "\\" + "MLKHelperGUI.exe" + "\" " + "\"%1\"";
                Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Classes\derpymuffinsfactory.mlk.v1");
                Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Classes\derpymuffinsfactory.mlk.v1\shell");
                Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Classes\derpymuffinsfactory.mlk.v1\shell\open");
                Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Classes\derpymuffinsfactory.mlk.v1\shell\open\command");
                Registry.SetValue(@"HKEY_LOCAL_MACHINE\Software\Classes\derpymuffinsfactory.mlk.v1\shell\open\command", null, regkey);
                Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Classes\.mlk");
                Registry.SetValue(@"HKEY_LOCAL_MACHINE\Software\Classes\.mlk", null, "derpymuffinsfactory.mlk.v1");
                Registry.LocalMachine.CreateSubKey(@"SOFTWARE\\DerpyMuffinsFactory");
                Registry.LocalMachine.OpenSubKey("SOFTWARE\\DerpyMuffinsFactory", true).SetValue("MLK Path", (string)InstallPath , Microsoft.Win32.RegistryValueKind.String);
                return true;
            }
            catch (Exception ex)
            {
                //ShowErrorMessageDialog(ex.Message, ex.StackTrace, "HelperClass.SetInstallLocationInRegistryKey(string InstallPath)");
                return false;
            }
            
        }

        public string GetInstallLocationfromRegistryKey()
        {
            try
            {
                return (string)Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\DerpyMuffinsFactory", "MLK Path", null);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public bool IsAdministrator()
        {
            WindowsIdentity identity = WindowsIdentity.GetCurrent();
            WindowsPrincipal principal = new WindowsPrincipal(identity);
            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }

        public void SetWritePermissionForLoggedInUsers(string FolderPath)
        {
            try
            {
                // This gets the "Authenticated Users" group, no matter what it's called
                IdentityReference everybodyIdentity = new SecurityIdentifier(WellKnownSidType.AuthenticatedUserSid, null);

                FileSystemAccessRule writerule = new FileSystemAccessRule(
                    everybodyIdentity,
                    FileSystemRights.Modify,
                    InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit,
                    PropagationFlags.None,
                    AccessControlType.Allow);

                if (!string.IsNullOrEmpty(FolderPath) && Directory.Exists(FolderPath))
                {
                    // Get your file's ACL
                    DirectorySecurity fsecurity = Directory.GetAccessControl(FolderPath);

                    // Add the new rule to the ACL
                    fsecurity.AddAccessRule(writerule);

                    // Set the ACL back to the file
                    Directory.SetAccessControl(FolderPath, fsecurity);

                }
            }
            catch (Exception ex)
            {
                ShowErrorMessageDialog(ex.Message, ex.StackTrace, "SetWritePermissionForLoggedInUsers(string FolderPath)");
            }
        }

        public void CreateStartmenuShortcut(string ShortCutTarget, string ShortCutName)
        {

            try
            {
                StringBuilder path = new StringBuilder(260);
                SHGetSpecialFolderPath(IntPtr.Zero, path, CSIDL_COMMON_STARTMENU, false);
                string commonStartMenuPath = path.ToString();
                string appStartMenuPath = Path.Combine(Path.Combine(Path.Combine(commonStartMenuPath, "Programs"), "Derpy Muffins Factory"),"My Little Karaoke - Singing is Magic");

                if (!Directory.Exists(appStartMenuPath))
                    Directory.CreateDirectory(appStartMenuPath);


                IShellLink link = (IShellLink)new ShellLink();

                // setup shortcut information
                link.SetDescription(ShortCutName);
                link.SetPath(ShortCutTarget);

                // save it
                IPersistFile file = (IPersistFile)link;
                file.Save(Path.Combine(appStartMenuPath, ShortCutName + ".lnk"), false);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Was not able to create shortcuts in Windows startmenu. " + Environment.NewLine + ex.Message, "Error creating shortcuts.", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        [DllImport("shell32.dll")]
        static extern bool SHGetSpecialFolderPath(IntPtr hwndOwner,
           [Out] StringBuilder lpszPath, int nFolder, bool fCreate);
        const int CSIDL_COMMON_STARTMENU = 0x16;  // All Users\Start Menu
        
        public bool IsDVDInstallation()
        {
            try
            {
                string location = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName;
                var info = new DriveInfo(Path.GetPathRoot(location));
                return info.DriveType == DriveType.CDRom;
            }
            catch
            {
                return false;
            }
        }

        public void Run_MLK_SIM4_Uninstaller(string FolderPath)
        {
            
            if (MessageBox.Show("An old installation of MyLittleKaraoke has been detected and needs to be uninstalled first." + Environment.NewLine +
                "Do you want to automatically uninstall it now?", "Uninstall old version?",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try { Directory.Delete(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "SIM4toNew"), true); }
                catch (Exception) { ;}
                Boolean domanually = false;
                try
                {
                        Directory.Move(Path.Combine(FolderPath, "songs"), Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "SIM4toNew"));
                }
                catch (Exception ex)
                {
                    domanually = true;
                    MessageBox.Show("Was not able to automatically correctly backup songs folder or uninstall MLK SIM4. Please backup the songs folder manually to a different folder now and then click OK." + Environment.NewLine + "Error Message: " + ex.Message + Environment.NewLine + Environment.NewLine + ex.StackTrace);
                }
                try{ System.Diagnostics.Process.Start("cmd.exe", "/Q /C MsiExec.exe /x{590FE3A5-47DB-42C0-B868-D5E43F46DCBC} /passive /norestart"); }
                catch (Exception) { ;}
                if (MessageBox.Show("Please press yes when uninstall finished successfully.", "Confirm when uninstall completed", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) != DialogResult.OK)
                {
                    throw new InvalidOperationException("User did not confirm successfull uninstall of MLK SIM4");
                };
                if (domanually == false)
                { Directory.Move(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "SIM4toNew"), Path.Combine(FolderPath, "songs")); }
                else
                { MessageBox.Show("Please now restore the songs folder of the game to the correct path (-> in the MyLittleKaraoke folder) and then press OK to continue the installation."); }
                
            }
            else
            {
                ShowErrorMessageDialog("User aborted uninstall of previous version.", "", "Run_MLK_SIM4_Uninstaller");
            };
        }

        public void Run_Old_Uninstaller()
        {
            try
            {
                if (MessageBox.Show("An old installation of MyLittleKaraoke has been detected and needs to be uninstalled first." + Environment.NewLine +
                    "Do you want to automatically uninstall it now?", "Uninstall old version?",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                {
                    System.Diagnostics.Process.Start("cmd.exe", "/Q /C MsiExec.exe /x{590FE3A5-47DB-42C0-B868-D5E43F46DCBC} /passive /norestart");
                    if (MessageBox.Show("Please press yes when uninstall finished successfully.", "Confirm when uninstall completed", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) != DialogResult.OK)
                    { throw new InvalidOperationException("User did not confirm successfull uninstall of old MLK"); };
                }
                else
                {
                    ShowErrorMessageDialog("User aborted uninstall of previous version.", "", "Run_Old_Uninstaller");
                };
            }
            catch (Exception ex)
            {
                ShowErrorMessageDialog("Was not able to correctly backup songs folder or uninstall MLK SIM4: " + ex.Message, ex.StackTrace, "Run_Old_Uninstaller");
            }
        }

        [ComImport]
        [Guid("00021401-0000-0000-C000-000000000046")]
        internal class ShellLink
        {
        }

        [ComImport]
        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        [Guid("000214F9-0000-0000-C000-000000000046")]
        internal interface IShellLink
        {
            void GetPath([Out, MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszFile, int cchMaxPath, out IntPtr pfd, int fFlags);
            void GetIDList(out IntPtr ppidl);
            void SetIDList(IntPtr pidl);
            void GetDescription([Out, MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszName, int cchMaxName);
            void SetDescription([MarshalAs(UnmanagedType.LPWStr)] string pszName);
            void GetWorkingDirectory([Out, MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszDir, int cchMaxPath);
            void SetWorkingDirectory([MarshalAs(UnmanagedType.LPWStr)] string pszDir);
            void GetArguments([Out, MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszArgs, int cchMaxPath);
            void SetArguments([MarshalAs(UnmanagedType.LPWStr)] string pszArgs);
            void GetHotkey(out short pwHotkey);
            void SetHotkey(short wHotkey);
            void GetShowCmd(out int piShowCmd);
            void SetShowCmd(int iShowCmd);
            void GetIconLocation([Out, MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszIconPath, int cchIconPath, out int piIcon);
            void SetIconLocation([MarshalAs(UnmanagedType.LPWStr)] string pszIconPath, int iIcon);
            void SetRelativePath([MarshalAs(UnmanagedType.LPWStr)] string pszPathRel, int dwReserved);
            void Resolve(IntPtr hwnd, int fFlags);
            void SetPath([MarshalAs(UnmanagedType.LPWStr)] string pszFile);
        }

        public void SetAssociation(string Extension, string KeyName, string OpenWith, string FileDescription)
        {
            RegistryKey BaseKey;
            RegistryKey OpenMethod;
            RegistryKey Shell;

            BaseKey = Registry.ClassesRoot.CreateSubKey(Extension);
            BaseKey.SetValue("", KeyName);

            OpenMethod = Registry.ClassesRoot.CreateSubKey(KeyName);
            OpenMethod.SetValue("", FileDescription);
            OpenMethod.CreateSubKey("DefaultIcon").SetValue("", "\"" + OpenWith + "\",0");
            Shell = OpenMethod.CreateSubKey("Shell");
            Shell.CreateSubKey("edit").CreateSubKey("command").SetValue("", "\"" + OpenWith + "\"" + " \"%1\"");
            Shell.CreateSubKey("open").CreateSubKey("command").SetValue("", "\"" + OpenWith + "\"" + " \"%1\"");
            BaseKey.Close();
            OpenMethod.Close();
            Shell.Close();
        }

        public int GetFreeDiskSpaceGB(string DrivePath)
        {
            try
            {
                return Convert.ToInt32((new DriveInfo(Path.GetPathRoot(DrivePath))).AvailableFreeSpace / 1024d / 1024d / 1024d);
            }
            catch (Exception)
            {
                return 29999;
            }
        }

        public void StartProcessAsAdmin(string Path) {
            ProcessStartInfo startInfo = new ProcessStartInfo(Path);
            startInfo.Verb = "runas";
            Process.Start(startInfo);
        }

        public bool IsInGameDirectory() {
            string selffolder = Path.GetDirectoryName(SelfPath);
            //If both ultrastardx.exe and packageversion.txt exist or we are in temp dir, we can assume we're in the installdir
            return !(SelfPath.Contains(Path.GetTempPath())) && File.Exists(Path.Combine(selffolder, "ultrastardx.exe")) && File.Exists(Path.Combine(selffolder, "packageversion.txt"));
        }

        public bool IsProcessOpen(string name)
        {
            foreach (Process clsProcess in Process.GetProcesses())
            {
                if (clsProcess.ProcessName.Contains(name))
                {
                    Console.WriteLine(clsProcess.ProcessName);
                    return true;
                }
            }

            return false;
        }
    }
}
