using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Windows.Forms;

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
                FileTableNx2[i / 2, i % 2] = WebPageContentLines[i];
            }
            return FileTableNx2;
        }

        public string GetWebPageContent(string PageURL)
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
    }
}
