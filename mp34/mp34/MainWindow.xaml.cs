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
using System.Windows.Controls.Primitives;

namespace mp34
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Random rnd = new Random();
        MediaPlayer mp = new MediaPlayer();
        Dictionary<string, string> p_list = new Dictionary<string, string>();
        List<string> now_p_list = new List<string>();
        DirectoryInfo info = new DirectoryInfo(@"D:\Music\Пушка");
        DispatcherTimer timer = new DispatcherTimer();
        string nowplaying;

        bool isDragged = false;
        bool random = false;
        int reapet = 0;
        bool playy = false;


        public MainWindow()
        {
            InitializeComponent();
            load();
            music_to_list();
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.Tick += Timer_Tick;

            mp.MediaOpened += Mp_MediaOpened;
            mp.MediaEnded += Mp_MediaEnded;
        }

        private void music_to_list()
        {
            now_p_list.Clear();
            playList.Items.Clear();
            if (!random)
            {
                foreach (string mus in p_list.Keys)
                {
                    now_p_list.Add(mus);
                    playList.Items.Add(mus);
                }
            }
            else
            {
                int count = p_list.Count;
                int[] mas_index = new int[count];
                //for (int i = 0; i < count; i++)
                //{
                //    mas_index[i] = -1;
                //}
                mas_index = createRandomMas(count, mas_index);
                for (int i = 0; i < count; i++)
                {
                    //int k = rnd.Next(count);
                    //while (numExist(mas_index, k))
                    //{
                    //    k = rnd.Next(count);
                    //}
                    //mas_index[i] = k;

                    now_p_list.Add(p_list.Keys.ElementAt(mas_index[i]));
                }
                mas_index = null;
                foreach (string mus in now_p_list)
                {
                    playList.Items.Add(mus);
                }
            }
        }

        private int[] createRandomMas(int count, int[] mas)
        {
            List<int> listOfNum = new List<int>();
            for (int i = 0; i < count; i++)
            {
                listOfNum.Add(i);
            }
            for (int i = 0; i < count; i++)
            {
                int k = rnd.Next(listOfNum.Count);
                mas[i] = listOfNum.ElementAt(k);
                listOfNum.RemoveAt(k);
            }
            return mas;
        }

        private bool numExist(int[] mas, int k)
        {
            foreach (int item in mas)
            {
                if (item == k) return true;
            }
            return false;
        }

        private void Mp_MediaEnded(object sender, EventArgs e)
        {
            timer.Stop();
            int ind = playList.SelectedIndex;

            if (playList.SelectedIndex + 1 >= playList.Items.Count) return;

            if (reapet == 2)
            {
                mp.Stop();
                timer.Stop();

                duration.Value = 0;
                mp.Play(); timer.Start();
            }
            else playList.SelectedIndex++;
        }

        private void Mp_MediaOpened(object sender, EventArgs e)
        {
            Duration dur = mp.NaturalDuration;
            TimeEnd.Content = dur.TimeSpan.Minutes + ":" + dur.TimeSpan.Seconds;
            duration.Maximum = dur.TimeSpan.TotalSeconds;
            duration.Value = 0;
            mp.Play();
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (!isDragged)
            {
                mp.Play();
                duration.Value++;
            }
            else
            {
                mp.Stop();
            }
        }

        private void playList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (nowplaying != playList.SelectedItem.ToString())
            {
                play_btn.Source = new BitmapImage(new Uri(@"Source/pause_off.png", UriKind.Relative));
                play();
                playy = true;
            }
            else if (nowplaying == playList.SelectedItem.ToString())
            {
                play_btn.Source = new BitmapImage(new Uri(@"Source/play_btn_off.png", UriKind.Relative));
                mp.Pause();
                timer.Stop();
                playy = false;
            }
        }

        private void play_btn_Click(object sender, MouseButtonEventArgs e)
        {
            if (playy == false)
            {
                play_btn.Source = new BitmapImage(new Uri(@"Source/pause_off.png", UriKind.Relative));
                play();
                playy = true;
            }
            else if (playy == true & nowplaying != null)
            {
                play_btn.Source = new BitmapImage(new Uri(@"Source/play_btn_off.png", UriKind.Relative));
                mp.Pause();
                timer.Stop();
                playy = false;
            }
        }

        void play()
        {
            if (playList.SelectedIndex < 0) playList.SelectedIndex = 0; // на случай пустого выбора
            if (playList.SelectedItem.ToString() == nowplaying) //на случай повторного нажатия
            {
                timer.Start();
                mp.Play();
                return;
            }

            if (playList.SelectedIndex > -1)
            {
                nowplaying = playList.SelectedItem.ToString();
                p_list.TryGetValue(nowplaying, out string fname);
                mp.Open(new Uri(fname, UriKind.Relative));

                mp.Volume = Volume.Value;
                PlayNow.Content = nowplaying;
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
                        now_p_list.Add(name);
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

                mp.Volume = vol * 0.1;
            }
        }

        private void duration_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
        }

        private void duration_DragStarted(object sender, System.Windows.Controls.Primitives.DragStartedEventArgs e)
        {
            isDragged = true;
        }

        private void duration_DragCompleted(object sender, System.Windows.Controls.Primitives.DragCompletedEventArgs e)
        {
            isDragged = false;
            if (nowplaying != null)
            {
                mp.Position = new TimeSpan(0, 0, (int)duration.Value);
                TimeStart.Content = Math.Floor(duration.Value / 60) + ":" + Math.Floor(duration.Value % 60);
            }
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
            if (reapet == 0)
            {
                reapet = 1;
                rpt_btn.Source = new BitmapImage(new Uri(@"Source/rep.png", UriKind.Relative));
            }
            else if (reapet == 1)
            {
                rpt_btn.Source = new BitmapImage(new Uri(@"Source/rpt_on_off.png", UriKind.Relative));
                reapet = 2;
            }
            else if (reapet == 2)
            {
                rpt_btn.Source = new BitmapImage(new Uri(@"Source/rpt_on_on.png", UriKind.Relative));
                reapet = 0;
            }

        }

        private void rnd_btn_Click(object sender, MouseButtonEventArgs e)
        {
            if (nowplaying != null)
            {
                mp.Stop();
                timer.Stop();
            }


            if (random == true)
            {
                random = false;

                rnd_btn.Source = new BitmapImage(new Uri(@"Source/rnd.png", UriKind.Relative));
            }
            else if (random == false)
            {
                random = true;
                rnd_btn.Source = new BitmapImage(new Uri(@"Source/rnd_on.png", UriKind.Relative));
            }
            if (playy) mp.Stop();
            music_to_list();
        }

        private void ExitButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (nowplaying != null) mp.Stop();
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

        private void vp_btn_Click(object sender, RoutedEventArgs e)
        {
            if (playy == true & nowplaying != null)
            {
                play_btn.Source = new BitmapImage(new Uri(@"Source/play_btn_off.png", UriKind.Relative));
                mp.Pause();
                timer.Stop();
                playy = false;
            }

            VideoPlayer vp = new VideoPlayer();
            vp.ShowDialog();
        }
    }
}
