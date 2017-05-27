﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using YoutubeExtractor;

namespace Youtube2
{
    public partial class DownloadForm : Form
    {
        public DownloadForm()
        {
            InitializeComponent();
            procentLabel.Hide();
        }

        private void downloadButton_Click(object sender, EventArgs e)
        {
            progressBar.Minimum = 0;
            progressBar.Maximum = 100;
            procentLabel.Show();
            IEnumerable<VideoInfo> videos = DownloadUrlResolver.GetDownloadUrls(textBox.Text);
            VideoInfo video = videos.First(p => p.VideoType == VideoType.Mp4 && p.Resolution == Convert.ToInt32(comboBox.Text));

            if (video.RequiresDecryption)
                DownloadUrlResolver.DecryptDownloadUrl(video);
            var videoDownloader = new VideoDownloader(video, Path.Combine("D:/Downloads", video.Title + video.VideoExtension));

            videoDownloader.DownloadProgressChanged += Downloader_DownloadProgressChanged;
            videoDownloader.DownloadFinished += Downloader_DownloadFinished;
            Thread thread = new Thread(() => { videoDownloader.Execute(); }) { IsBackground = true };
            thread.Start();
        }

        private void Downloader_DownloadFinished(object sender, EventArgs e)
        {
            const string message = "Do you want one more download?";
            const string caption = "Download successfully completed";
            var result = MessageBox.Show(message, caption,
                                         MessageBoxButtons.YesNo,
                                         MessageBoxIcon.Information);
            if (result == DialogResult.No)
            {
                Application.Exit();
                //Close();
            }
            else
            {
                MethodInvoker action = delegate
                {
                    textBox.Clear();
                    progressBar.Value = 0;
                    procentLabel.Text = "0%";
                    procentLabel.Hide();
                };
                textBox.BeginInvoke(action);
            }
        }

        private void Downloader_DownloadProgressChanged(object sender, ProgressEventArgs e)
        {
            Invoke(new MethodInvoker(delegate ()
            {
                progressBar.Value = (int)e.ProgressPercentage;
                procentLabel.Text = $"{string.Format("{0:0}", e.ProgressPercentage)}%";
                progressBar.Update();
            }));
        }

        private void DownloadForm_Load(object sender, EventArgs e)
        {
            comboBox.SelectedIndex = 0;
        }
    }
}