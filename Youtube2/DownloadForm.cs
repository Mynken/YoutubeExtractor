using System;
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
            if (Data.Url == string.Empty)
            {

            }
            else
            {
                textBox.Text = Data.Url;
            }
            procentLabel.Hide();
            downloadButton.Hide();
            progressBar.Hide();
            label2.Hide();
            comboBox.Hide();
        }
        private void findButton_Click(object sender, EventArgs e)
        {
            IEnumerable<VideoInfo> videos = DownloadUrlResolver.GetDownloadUrls(textBox.Text);
            List<int> resolution = new List<int>();
            foreach (var item in videos)
            {
                if (item.Resolution != 0 && !resolution.Contains(item.Resolution))
                {
                    resolution.Add(item.Resolution);
                }
            }
            resolution.Sort();
            foreach (var item in resolution)
            {
                comboBox.Items.Add(item);
            }
            findButton.Hide();
            downloadButton.Show();
            progressBar.Show();
            label2.Show();
            comboBox.Show();
        }
        private void downloadButton_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                Data.Path = folderBrowserDialog1.SelectedPath.ToString();
            }
            progressBar.Minimum = 0;
            progressBar.Maximum = 100;
            procentLabel.Show();
            IEnumerable<VideoInfo> videos = DownloadUrlResolver.GetDownloadUrls(textBox.Text);
            VideoInfo video = videos.First(p => p.VideoType == VideoType.Mp4 && p.Resolution == Convert.ToInt32(comboBox.Text));

            if (video.RequiresDecryption)
                DownloadUrlResolver.DecryptDownloadUrl(video);
            var videoDownloader = new VideoDownloader(video, Path.Combine(Data.Path, video.Title + video.VideoExtension));

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
          //  comboBox.SelectedIndex = 0;
        }


    }
}
