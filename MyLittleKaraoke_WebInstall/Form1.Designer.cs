﻿namespace MyLittleKaraoke_WebInstall
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.TextBoxInstallPath = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.DownloadAndInstallButton = new System.Windows.Forms.Button();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.progressBar2 = new System.Windows.Forms.ProgressBar();
            this.label5 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.InstalledVersionLabel = new System.Windows.Forms.Label();
            this.InstalledPackageLabel = new System.Windows.Forms.Label();
            this.ActionNextLabel = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.progressBar3 = new System.Windows.Forms.ProgressBar();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::MyLittleKaraoke_WebInstall.Properties.Resources.HeaderSetup;
            this.pictureBox1.Location = new System.Drawing.Point(-6, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(780, 100);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 109);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(160, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "My Little Karaoke - Web Installer";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 122);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(324, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "This Webinstaller will download and install My Little Karaoke for you";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 160);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(229, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "Where do you want to install My Little Karaoke:";
            // 
            // TextBoxInstallPath
            // 
            this.TextBoxInstallPath.Location = new System.Drawing.Point(15, 177);
            this.TextBoxInstallPath.Name = "TextBoxInstallPath";
            this.TextBoxInstallPath.Size = new System.Drawing.Size(656, 20);
            this.TextBoxInstallPath.TabIndex = 4;
            this.TextBoxInstallPath.Text = "C:\\Program Files\\My Little Karaoke";
            this.TextBoxInstallPath.TextChanged += new System.EventHandler(this.TextBoxInstallPath_TextChanged);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(677, 176);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 5;
            this.button1.Text = "Browse";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(15, 211);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(594, 33);
            this.label4.TabIndex = 6;
            this.label4.Text = "If anything happened to your internet connection during the download, or, if the " +
    "downloaded file is corrupted, \r\nMy Little Karaoke will ask you to redownload onl" +
    "y the last part.";
            this.label4.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // DownloadAndInstallButton
            // 
            this.DownloadAndInstallButton.Location = new System.Drawing.Point(615, 211);
            this.DownloadAndInstallButton.Name = "DownloadAndInstallButton";
            this.DownloadAndInstallButton.Size = new System.Drawing.Size(137, 33);
            this.DownloadAndInstallButton.TabIndex = 7;
            this.DownloadAndInstallButton.Text = "Install the Game!";
            this.DownloadAndInstallButton.UseVisualStyleBackColor = true;
            this.DownloadAndInstallButton.Click += new System.EventHandler(this.DownloadAndInstallButton_Click);
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(18, 298);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(734, 23);
            this.progressBar1.TabIndex = 8;
            // 
            // progressBar2
            // 
            this.progressBar2.Location = new System.Drawing.Point(18, 354);
            this.progressBar2.Name = "progressBar2";
            this.progressBar2.Size = new System.Drawing.Size(734, 23);
            this.progressBar2.TabIndex = 9;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(20, 282);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(122, 13);
            this.label5.TabIndex = 10;
            this.label5.Text = "Download part progress:";
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(413, 324);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(339, 23);
            this.label7.TabIndex = 12;
            this.label7.Text = "Speed and File Size";
            this.label7.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label8
            // 
            this.label8.Location = new System.Drawing.Point(401, 379);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(351, 23);
            this.label8.TabIndex = 13;
            this.label8.Text = "Part 0 of ?";
            this.label8.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(21, 338);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(143, 13);
            this.label6.TabIndex = 14;
            this.label6.Text = "Download (Overall) progress:";
            // 
            // InstalledVersionLabel
            // 
            this.InstalledVersionLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.InstalledVersionLabel.Location = new System.Drawing.Point(455, 109);
            this.InstalledVersionLabel.Name = "InstalledVersionLabel";
            this.InstalledVersionLabel.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.InstalledVersionLabel.Size = new System.Drawing.Size(297, 13);
            this.InstalledVersionLabel.TabIndex = 18;
            this.InstalledVersionLabel.Text = "Installed version: unknown";
            this.InstalledVersionLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // InstalledPackageLabel
            // 
            this.InstalledPackageLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.InstalledPackageLabel.Location = new System.Drawing.Point(455, 122);
            this.InstalledPackageLabel.Name = "InstalledPackageLabel";
            this.InstalledPackageLabel.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.InstalledPackageLabel.Size = new System.Drawing.Size(297, 13);
            this.InstalledPackageLabel.TabIndex = 19;
            this.InstalledPackageLabel.Text = "- here extra package information -";
            this.InstalledPackageLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // ActionNextLabel
            // 
            this.ActionNextLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ActionNextLabel.Location = new System.Drawing.Point(455, 135);
            this.ActionNextLabel.Name = "ActionNextLabel";
            this.ActionNextLabel.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.ActionNextLabel.Size = new System.Drawing.Size(297, 13);
            this.ActionNextLabel.TabIndex = 20;
            this.ActionNextLabel.Text = "Action: unknown";
            this.ActionNextLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(21, 393);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(149, 13);
            this.label9.TabIndex = 23;
            this.label9.Text = "Package Installation progress:";
            // 
            // label10
            // 
            this.label10.Location = new System.Drawing.Point(401, 434);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(351, 23);
            this.label10.TabIndex = 22;
            this.label10.Text = "Part 0 of ?";
            this.label10.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // progressBar3
            // 
            this.progressBar3.Location = new System.Drawing.Point(18, 409);
            this.progressBar3.Name = "progressBar3";
            this.progressBar3.Size = new System.Drawing.Size(734, 23);
            this.progressBar3.TabIndex = 21;
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Checked = true;
            this.checkBox1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox1.Location = new System.Drawing.Point(198, 239);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(212, 17);
            this.checkBox1.TabIndex = 24;
            this.checkBox1.Text = "Delete installation packages after install";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AcceptButton = this.DownloadAndInstallButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(764, 261);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.progressBar3);
            this.Controls.Add(this.ActionNextLabel);
            this.Controls.Add(this.InstalledPackageLabel);
            this.Controls.Add(this.InstalledVersionLabel);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.progressBar2);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.DownloadAndInstallButton);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.TextBoxInstallPath);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pictureBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(780, 550);
            this.MinimumSize = new System.Drawing.Size(780, 300);
            this.Name = "Form1";
            this.Text = "My Little Karaoke - Web Installer";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox TextBoxInstallPath;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button DownloadAndInstallButton;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.ProgressBar progressBar2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label InstalledVersionLabel;
        private System.Windows.Forms.Label InstalledPackageLabel;
        private System.Windows.Forms.Label ActionNextLabel;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.ProgressBar progressBar3;
        private System.Windows.Forms.CheckBox checkBox1;
    }
}

