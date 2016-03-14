using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using Microsoft.Win32;

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
            for (int i = 0; i < WebPageContentLines.Length -1; i++)
            {
                try {
                    FileTableNx2[i / 2, i % 2] = WebPageContentLines[i]; }
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
            catch (Exception)
            {
                return "";
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
    }
}
