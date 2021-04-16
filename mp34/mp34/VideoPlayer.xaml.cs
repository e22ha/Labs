using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Microsoft.Win32;

namespace mp34
{
    /// <summary>
    /// Логика взаимодействия для VideoPlayer.xaml
    /// </summary>
    public partial class VideoPlayer : Window
    {
        MediaPlayer player = new MediaPlayer();
        public VideoPlayer()
        {
            InitializeComponent();

            play_win.MediaOpened += p_MediaOpened;
            play_win.MediaEnded += p_MediaEnded;
        }

        private void p_MediaEnded(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void p_MediaOpened(object sender, RoutedEventArgs e)
        {
            position_sl.Maximum = play_win.NaturalDuration.TimeSpan.TotalMilliseconds;

            play_btn.Content = "||";
            play_win.Play();
        }

        private void play_btn_Click(object sender, RoutedEventArgs e)
        {
            if(play_btn.Content.ToString() == "▶")
            {
                play_btn.Content = "||";
                play_win.Play();
            }
            else if (play_btn.Content.ToString() == "||")
            {
                play_btn.Content = "▶";
                play_win.Pause();
            }
        }

        private void load_v_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.ShowDialog();

            //загрузка видео файла
            //player.Open(new Uri(dlg.FileName, UriKind.Relative));
            play_win.Source = new Uri(dlg.FileName, UriKind.Relative);
            //play_win.Height = player.NaturalVideoHeight;
            //play_win.Width = player.NaturalVideoWidth;
            play_win.Volume = 50.0 / 100.0;

        }

        private void load_vp_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
