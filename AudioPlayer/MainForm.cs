using AxWMPLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace AudioPlayer
{
    public partial class MainForm : Form
    {
        List<MediaFile> mediaFiles = new List<MediaFile>();
        public MainForm()
        {
            InitializeComponent();
            var myPlayList = axWindowsMediaPlayer1.playlistCollection.newPlaylist("MyPlayList");
            axWindowsMediaPlayer1.currentPlaylist = myPlayList;

            
            this.Text = "Audio Player";
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            listBox1.ValueMember = "Path";
            listBox1.DisplayMember = "FileName";
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //
            //  Поиск нужной песни
            //
            int index = 0;
            for (int i = 0; i < axWindowsMediaPlayer1.currentPlaylist.count; i++)
            {
                if (axWindowsMediaPlayer1.currentPlaylist.Item[i].name.Equals(listBox1.SelectedItem.ToString()))
                {
                    index = i;
                    break;       
                }
            }

            //
            //  Начать проигрывать песню
            //
            axWindowsMediaPlayer1.Ctlcontrols.playItem(axWindowsMediaPlayer1.currentPlaylist.Item[index]);

        }

        private void button1_Click(object sender, EventArgs e)
        {
            using(OpenFileDialog openFileDialog = new OpenFileDialog() { Multiselect = true,ValidateNames= true,Filter = "MP3|*.mp3|MP4|*mp4|WAV|*wav"})
            {
                if(openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    if(!mediaFiles.Any((x) => x.FileName.Equals(Path.GetFileNameWithoutExtension(openFileDialog.FileName))))
                    {
                        //
                        //  Добавить в список и проиграть
                        //
                        listBox1.Items.Add(Path.GetFileNameWithoutExtension(openFileDialog.FileName));
                        mediaFiles.Add(new MediaFile(Path.GetFileNameWithoutExtension(openFileDialog.FileName), openFileDialog.FileName));
                        var mediaItem = axWindowsMediaPlayer1.newMedia(openFileDialog.FileName);
                        axWindowsMediaPlayer1.currentPlaylist.appendItem(mediaItem);
                        axWindowsMediaPlayer1.Ctlcontrols.play();
                    }
                } 
            }
        }


        //
        //  Очистка песен
        //
        private void button2_Click(object sender, EventArgs e)
        {
            this.listBox1.Items.Clear();
            this.mediaFiles.Clear();
            axWindowsMediaPlayer1.Ctlcontrols.stop();
        }

        private void axWindowsMediaPlayer1_PlayStateChange(object sender, AxWMPLib._WMPOCXEvents_PlayStateChangeEvent e)
        {
            if (axWindowsMediaPlayer1.playState == WMPLib.WMPPlayState.wmppsMediaEnded)
            {
                axWindowsMediaPlayer1.Ctlcontrols.next();
                axWindowsMediaPlayer1.Ctlcontrols.play();
            }
        }


        //
        //  Выделение в списке
        //
        private void axWindowsMediaPlayer1_CurrentItemChange(object sender, AxWMPLib._WMPOCXEvents_CurrentItemChangeEvent e)
        {
            if (listBox1 == null || listBox1.Items.Count == 0)
            {
                return;
            }

            for (int i = 0; i < axWindowsMediaPlayer1.currentPlaylist.count; i++)
            {
                if (axWindowsMediaPlayer1.currentPlaylist.Item[i].name.Equals(axWindowsMediaPlayer1.Ctlcontrols.currentItem.name))
                {
                    listBox1.SelectedIndex = i;
                    break;
                }
            }
        }
    }
}
