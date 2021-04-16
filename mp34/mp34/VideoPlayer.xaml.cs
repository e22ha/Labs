using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using Microsoft.Win32;

namespace mp34
{
    /// <summary>
    /// Логика взаимодействия для VideoPlayer.xaml
    /// </summary>
    public partial class VideoPlayer : Window
    {
        MediaPlayer player = new MediaPlayer();
        DispatcherTimer timer = new DispatcherTimer();

        bool isDragged = false;
        public VideoPlayer()
        {
            InitializeComponent();
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.Tick += Timer_Tick;

            play_win.MediaOpened += p_MediaOpened;
            play_win.MediaEnded += p_MediaEnded;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            position_sl.Value = play_win.Position.Seconds;
        }

        private void p_MediaEnded(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void p_MediaOpened(object sender, RoutedEventArgs e)
        {
            position_sl.Maximum = play_win.NaturalDuration.TimeSpan.TotalSeconds;
            Duration dur = play_win.NaturalDuration;
            alltime.Content = dur.TimeSpan.Hours + ":" + dur.TimeSpan.Minutes + ":" + dur.TimeSpan.Seconds;
            play_win.MaxHeight = play_win.NaturalVideoHeight;
            play_win.MaxWidth = play_win.NaturalVideoWidth;
            play_btn.Content = "||";
            //play_win.Play();
        }

        private void play_btn_Click(object sender, RoutedEventArgs e)
        {
            if (play_btn.Content.ToString() == "▶")
            {
                play_btn.Content = "||";
                play_win.Play();
                timer.Start();
            }
            else if (play_btn.Content.ToString() == "||")
            {
                play_btn.Content = "▶";
                play_win.Pause();
                timer.Stop();
            }
        }

        private void load_v_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.ShowDialog();

            //загрузка видео файла
            //player.Open(new Uri(dlg.FileName, UriKind.Relative));
            play_win.Source = new Uri(dlg.FileName, UriKind.Relative);

            play_win.Volume = 50.0 / 100.0;

        }

        private void load_vp_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Volume_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            double vol = volume_sl.Value;
            if (play_win != null)
            {
                play_win.Volume = vol * 0.1;
            }
        }

        private void position_sl_ValueChanged(object sender, DragCompletedEventArgs e)
        {
            int SliderValue = (int)position_sl.Value;
            TimeSpan ts = new TimeSpan( 0, 0, SliderValue);
            play_win.Position = ts;

            timenow.Content = ts.Hours + ":" + ts.Minutes + ":" + ts.Seconds;
        }

        private void position_sl_start_change(object sender, DragStartedEventArgs e)
        {
            //isDragged = true;
        }

    }
}
