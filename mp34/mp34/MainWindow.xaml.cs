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

namespace mp34
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Dictionary<string, MediaPlayer> p_list = new Dictionary<string, MediaPlayer>();
        DirectoryInfo info = new DirectoryInfo(@"D:\Music\Пушка");
        string nowplaying;
        public MainWindow()
        {
            InitializeComponent();
            load();
            
        }

        bool isDragged = false;

        private void stop_btn_Click(object sender, RoutedEventArgs e)
        {
            if (nowplaying != null)
            {
                MediaPlayer mp;
                p_list.TryGetValue(nowplaying, out mp);
                mp.Stop();

            }
        }

        private void play_btn_Click(object sender, MouseButtonEventArgs e)
        {
            if (playList.SelectedIndex > -1)
            {
                MediaPlayer mp;
                if ((playList.SelectedItem.ToString() != nowplaying) & (nowplaying != null))
                {
                    p_list.TryGetValue(nowplaying, out mp);
                    mp.Stop();
                }


                p_list.TryGetValue(playList.SelectedItem.ToString(), out mp);
                mp.Play();
                mp.Volume = Volume.Value;
                nowplaying = playList.SelectedItem.ToString();


            }
        }

        private void pause_btn_Click(object sender, MouseButtonEventArgs e)
        {
            if (nowplaying != null)
            {
                MediaPlayer mp;
                p_list.TryGetValue(nowplaying, out mp);
                mp.Pause();
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
                        MediaPlayer player = new MediaPlayer();
                        player.Open(new Uri(fname.FullName, UriKind.Relative));
                        p_list.Add(name, player);
                        playList.Items.Add(name);
                        player = null;
                    }
                }
            }
            if (playList.Items.Count > -1) playList.SelectedIndex = 0;
        }
        private void Sp_LoadCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            MessageBox.Show("Звук загружен!");
        }

        private void Volume_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            double vol = Volume.Value;
            if (nowplaying != null)
            {
                MediaPlayer mp;
                p_list.TryGetValue(nowplaying, out mp);
                mp.Volume = vol;

            }
        }

        private void duration_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

            if (nowplaying != null)
            {
                MediaPlayer mp;
                p_list.TryGetValue(nowplaying, out mp);
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
    }
}
