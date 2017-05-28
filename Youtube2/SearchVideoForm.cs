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
    public partial class SeachVideoFrom : Form
    {
        public SeachVideoFrom()
        {
            InitializeComponent();
            dataGridView.RowTemplate.Height = 110;
            dataGridView.Visible = false;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            VideoSearch items = new VideoSearch();
            List<Video> list = new List<Video>();
            foreach (var item in items.SearchQuery(searchTextBox.Text, 1))
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
            dataGridView.Visible = true;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                Data.Url = dataGridView.Rows[e.RowIndex].Cells[3].Value.ToString();
                DownloadForm download = new DownloadForm();
                download.Show();
            }
        }

        private void SeachVideoFrom_Load(object sender, EventArgs e)
        {
                Data.Path = "D:\\Downloads";
        }

        private void linkButton_Click(object sender, EventArgs e)
        {
            DownloadForm download = new DownloadForm();
            download.Show();
        }

        private void changeDirectoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                Data.Path = folderBrowserDialog1.SelectedPath.ToString();
            }
        }

        private void audioButton_Click(object sender, EventArgs e)
        {
            MusicForm music = new MusicForm();
            music.Show();
        }
    }
}
