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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Media;
using Microsoft.Win32;
using System.IO;
using Microsoft.VisualBasic;
using Microsoft.SqlServer.Server;
using System.Windows.Threading;

namespace mp34
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        MediaPlayer mp = new MediaPlayer();
        Dictionary<string, string> p_list = new Dictionary<string, string>();
        DirectoryInfo info = new DirectoryInfo(@"E:\Music\Пушка");
        DispatcherTimer timer = new DispatcherTimer();
        string nowplaying;

        bool isDragged = false;
        bool random = false;
        bool reapet = false;
        bool playy = false;


        public MainWindow()
        {
            InitializeComponent();
            load();
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.Tick += Timer_Tick;

            mp.MediaOpened += Mp_MediaOpened;
            mp.MediaEnded += Mp_MediaEnded;
        }

        private void Mp_MediaEnded(object sender, EventArgs e)
        {
            if (playList.SelectedIndex + 1 >= playList.Items.Count) return;
            playList.SelectedIndex++;
        }

        private void Mp_MediaOpened(object sender, EventArgs e)
        {
            mp.Play();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {

        }

        private void playList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            play();
        }

        private void play_btn_Click(object sender, MouseButtonEventArgs e)
        {
            if(playy == false)
            {
                play_btn.Source = new BitmapImage(new Uri(@"Source/pause_off.png", UriKind.Relative));
                play();
                playy = true;
            }
            else if (playy == true & nowplaying != null)
            {
                play_btn.Source = new BitmapImage(new Uri(@"Source/play_btn_off.png", UriKind.Relative));
                p_list.TryGetValue(nowplaying, out string fname);
                mp.Pause();
                playy = false;
            } 
        }

        void play()
        {
            if (playList.SelectedIndex < 0) playList.SelectedIndex = 0;
            if (playList.SelectedItem.ToString() == nowplaying) 
            { 
                mp.Play();
                return;
            }

            if (playList.SelectedIndex > -1)
            {
                string fname;
                if ((playList.SelectedItem.ToString() != nowplaying) & (nowplaying != null))
                {
                    p_list.TryGetValue(nowplaying, out fname);

                    mp.Open(new Uri(fname, UriKind.Relative));

                    return;
                }

                nowplaying = playList.SelectedItem.ToString();

                p_list.TryGetValue(nowplaying, out fname);


                mp.Open(new Uri(fname, UriKind.Relative));

                mp.Volume = Volume.Value;
                PlayNow.Content = nowplaying;
                Duration dur = mp.NaturalDuration;
                TimeEnd.Content = dur.ToString();
            }
        }

        private void load_btn_Click(object sender, RoutedEventArgs e)
        {
            load();
        }

        void load()
        {
            if (info != null)
            {
                foreach (var fname in info.GetFiles())
                {
                    if (fname.Extension == ".mp3")
                    {
                        string name = System.IO.Path.GetFileNameWithoutExtension(fname.FullName);
                        p_list.Add(name, fname.FullName);
                        playList.Items.Add(name);
                    }
                }
            }
        }

        private void Volume_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            double vol = Volume.Value;
            if (nowplaying != null)
            {
                //p_list.TryGetValue(nowplaying, out string fname);

                //mp.Open(new Uri(fname, UriKind.Relative));

                mp.Volume = vol*0.1;

            }
        }

        private void duration_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

            if (nowplaying != null)
            {
                //p_list.TryGetValue(nowplaying, out string fname);

                //mp.Open(new Uri(fname, UriKind.Relative));

                mp.Position = new TimeSpan(0, 0, (int)duration.Value);

            }

        }
        private void duration_DragStarted(object sender, System.Windows.Controls.Primitives.DragStartedEventArgs e)
        {
            isDragged = true;
        }

        private void duration_DragCompleted(object sender, System.Windows.Controls.Primitives.DragCompletedEventArgs e)
        {
            isDragged = false;
        }

        private void pref_click(object sender, RoutedEventArgs e)
        {
            AskPath askP = new AskPath();
            askP.ShowDialog();

            info = new DirectoryInfo(askP.path.Text.ToString());

            load();
        }

        private void next_btn_Click(object sender, MouseButtonEventArgs e)
        {
            if (playList.SelectedIndex + 1 <= playList.Items.Count) playList.SelectedIndex++;
        }

        private void prv_btn_Click(object sender, MouseButtonEventArgs e)
        {
            if (playList.SelectedIndex - 1 > -1) playList.SelectedIndex--;
        }

        private void rpt_btn_Click(object sender, MouseButtonEventArgs e)
        {
            if (reapet == true) reapet = false;
            else reapet = true;
        }

        private void rnd_btn_Click(object sender, MouseButtonEventArgs e)
        {
            if (random == true)
            {
                random = false;
 
                rnd_btn.Source = new BitmapImage(new Uri(@"Source/rnd_off.png", UriKind.Relative));
            }
            else
            {
                random = true;
                rnd_btn.Source = new BitmapImage(new Uri(@"Source/rnd_on.png", UriKind.Relative));
            }
        }

        private void ExitButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        private void MinButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void ToolBar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }

    }
}
