using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Threading;
using System.Security.Permissions;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using Microsoft.Win32;
using IWshRuntimeLibrary;
using System.Runtime.InteropServices;

namespace MyLittleKaraoke_WebInstall
{
    class HelperClass
    {
        public void ShowErrorMessageDialog(string sErrorBasic, string sErrorStacktrace, string sLocation)
        {
            MessageBox.Show("Derpy is awfully sorry, but this MyLittleKaraoke Installer just encountered an error.\n" +
                "Please submit an error report at www.mylittlekaraoke.com/forum or visit our IRC Channel #karaoke at irc.ponychat.net\n\n" +
                "Error description: " + sErrorBasic + "\n\nError location: "+ sLocation + "\n\nExtended information:\n" + sErrorStacktrace, "Error in " + sLocation, MessageBoxButtons.OK, MessageBoxIcon.Error);
            
            if (System.Windows.Forms.Application.MessageLoop)
            {
                System.Windows.Forms.Application.Exit();
            }
            else
            {
                System.Environment.Exit(1);
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

        public Boolean SetInstallLocationInRegistryKey(string InstallPath)
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
                Registry.LocalMachine.OpenSubKey("SOFTWARE\\DerpyMuffinsFactory", true).SetValue("MLK Path", (string)InstallPath + "\\", Microsoft.Win32.RegistryValueKind.String);
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
                return Registry.LocalMachine.OpenSubKey("SOFTWARE\\DerpyMuffinsFactory", true).getValue("MLK Path");;
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        public bool IsAdministrator()
        {
            WindowsIdentity identity = WindowsIdentity.GetCurrent();
            WindowsPrincipal principal = new WindowsPrincipal(identity);
            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }

        public bool IsMlkSimAC3(string InstallPath)
        {
            try
            {
                return System.IO.File.ReadAllLines(InstallPath + @"\songs\version.txt").First().Contains("5.0 Final");
            }
            catch (Exception)
            {
                return false;
            }
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
                string appStartMenuPath = Path.Combine(Path.Combine(Path.Combine(commonStartMenuPath, "Programs"), "Derpy Muffins Test Factory"),"My Little Karaoke - Singing is Magic");

                if (!Directory.Exists(appStartMenuPath))
                    Directory.CreateDirectory(appStartMenuPath);

                string shortcutLocation = Path.Combine(appStartMenuPath, ShortCutName + ".lnk");
                WshShell shell = new WshShell();
                IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut(shortcutLocation);

                shortcut.Description = ShortCutName;
                //shortcut.IconLocation = @"C:\Program Files (x86)\TestApp\TestApp.ico"; //uncomment to set the icon of the shortcut
                shortcut.TargetPath = ShortCutTarget;
                shortcut.Save(); 
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
    }
}
