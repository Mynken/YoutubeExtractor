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
    public partial class MusicForm : Form
    {
        public MusicForm()
        {
            InitializeComponent();
            MessageBox.Show("Sorry but this add isn`t supported now, now you can only watch video here");
            button1.Hide();
            progressBar1.Hide();
            textBox1.Hide();
            label1.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Sorry but this add isn`t supported now");
            Close();

          //  if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
          //  {
          //      Data.Path = folderBrowserDialog1.SelectedPath.ToString();
          //  }
          //  IEnumerable<VideoInfo> videos = DownloadUrlResolver.GetDownloadUrls(textBox1.Text);

          //  VideoInfo video = videos.First();
          ////  VideoInfo video = videos.First(p => p.VideoType == VideoType.Mp4 && p.Resolution == 0);
          //  //VideoInfo video = videos
          //  //    .Where(info => info.CanExtractAudio)
          //  //    .OrderByDescending(info => info.AudioBitrate)
          //  //    .First();


          //  if (video.RequiresDecryption)
          //      DownloadUrlResolver.DecryptDownloadUrl(video);

          //  var audioDownloader = new AudioDownloader(video, Path.Combine(Data.Path, video.Title + video.AudioExtension));

          //      audioDownloader.DownloadProgressChanged += Downloader_DownloadProgressChanged;
          //      audioDownloader.DownloadFinished += Downloader_DownloadFinished;
            
          //  Thread thread = new Thread(() => { audioDownloader.Execute(); }) { IsBackground = true };
          //  thread.Start();
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
                    textBox1.Clear();
                    progressBar1.Value = 0;
                    label1.Text = "0%";
                    label1.Hide();
                };
                textBox1.BeginInvoke(action);
            }
        }

        private void Downloader_DownloadProgressChanged(object sender, ProgressEventArgs e)
        {
            Invoke(new MethodInvoker(delegate ()
            {
                progressBar1.Value = (int)e.ProgressPercentage;
                label1.Text = $"{string.Format("{0:0}", e.ProgressPercentage)}%";
                progressBar1.Update();
            }));
        }

        private void watchButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog file = new OpenFileDialog();
            if (file.ShowDialog() == DialogResult.OK)
            {
               // textBox1.Text = file.FileName;
                axWindowsMediaPlayer1.URL = file.FileName;
                axWindowsMediaPlayer1.Ctlcontrols.play();
            }
        }
           
    }
}
