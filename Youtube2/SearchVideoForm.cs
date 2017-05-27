using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using YoutubeExtractor;
using YoutubeSearch;

namespace Youtube2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            dataGridView1.RowTemplate.Height = 110;
            dataGridView1.Visible = false;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            VideoSearch items = new VideoSearch();
            List<Video> list = new List<Video>();
            foreach (var item in items.SearchQuery(textBox1.Text, 1))
            {
                Video video = new Video();
                video.Title = item.Title;
                video.Author = item.Author;
                video.Url = item.Url;
                byte[] imagebytes = new WebClient().DownloadData(item.Thumbnail);
                using (MemoryStream ms = new MemoryStream(imagebytes))
                {
                    video.Thumbnail = Image.FromStream(ms);
                }
                list.Add(video);
            }
            videoBindingSource.DataSource = list;
            dataGridView1.Visible = true;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                IEnumerable<VideoInfo> videos = DownloadUrlResolver.GetDownloadUrls(dataGridView1.SelectedCells.ToString());
                VideoInfo video = videos.First(p => p.VideoType == VideoType.Mp4 && p.Resolution == Convert.ToInt32(comboBox1.Text));

                if (video.RequiresDecryption)
                    DownloadUrlResolver.DecryptDownloadUrl(video);
                VideoDownloader downloader = new VideoDownloader(video, Path.Combine(@"C:\Users\Waldes\Desktop\", video.Title + video.VideoExtension));
            }
            catch (Exception)
            {

                MessageBox.Show("Отказано в доступе");
            }
        }
    }
}
