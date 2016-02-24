using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    }
}
