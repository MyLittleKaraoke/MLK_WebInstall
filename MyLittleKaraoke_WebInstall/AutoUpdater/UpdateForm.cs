using System;
using System.Diagnostics;
using System.Globalization;
using System.Windows.Forms;
using Microsoft.Win32;

namespace MyLittleKaraoke_WebInstall
{
    internal partial class UpdateForm : Form
    {
        private System.Timers.Timer _timer;

        public UpdateForm(bool remindLater = false)
        {
            if (!remindLater)
            {
                InitializeComponent();
                var resources = new System.ComponentModel.ComponentResourceManager(typeof(UpdateForm));
                Text = AutoUpdater.DialogTitle;
                labelUpdate.Text = string.Format(resources.GetString("labelUpdate.Text", CultureInfo.CurrentCulture), AutoUpdater.AppTitle);
                labelDescription.Text =
                    string.Format(resources.GetString("labelDescription.Text", CultureInfo.CurrentCulture),
                        AutoUpdater.AppTitle, AutoUpdater.CurrentVersion, AutoUpdater.InstalledVersion);
            }
        }

        public override sealed string Text
        {
            get { return base.Text; }
            set { base.Text = value; }
        }


        private void ButtonUpdateClick(object sender, EventArgs e)
        {
            if (AutoUpdater.OpenDownloadPage)
            {
                var processStartInfo = new ProcessStartInfo(AutoUpdater.DownloadURL);

                Process.Start(processStartInfo);
            }
            else
            {
                AutoUpdater.DownloadUpdate();
            }
        }

        private void ButtonSkipClick(object sender, EventArgs e)
        {
            
        }

        public void SetTimer(DateTime remindLater)
        {
            TimeSpan timeSpan = remindLater - DateTime.Now;
            _timer = new System.Timers.Timer
                {
                    Interval = (int) timeSpan.TotalMilliseconds
                };
            _timer.Elapsed += TimerElapsed;
            _timer.Start();
        }

        private void TimerElapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            _timer.Stop();
            AutoUpdater.Start();
        }

        private void UpdateForm_Load(object sender, EventArgs e)
        {

        }
    }
}
