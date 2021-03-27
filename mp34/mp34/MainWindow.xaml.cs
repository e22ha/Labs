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

namespace mp34
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Dictionary<String, MediaPlayer> p_list = new Dictionary<String, MediaPlayer>();
        public MainWindow()
        {
            InitializeComponent();
        }

        bool isDragged = false;

        private void stop_btn_Click(object sender, RoutedEventArgs e)
        {
            if (playList.SelectedIndex > -1)
            {
                MediaPlayer mp;
                p_list.TryGetValue(playList.SelectedItem.ToString(), out mp);
                mp.Stop();

            }
        }

        private void play_btn_Click(object sender, MouseButtonEventArgs e)
        {
            if (playList.SelectedIndex > -1)
            {
                MediaPlayer mp;
                p_list.TryGetValue(playList.SelectedItem.ToString(), out mp);
                mp.Play();
                mp.Volume = Volume.Value;
            }
        }

        private void pause_btn_Click(object sender, MouseButtonEventArgs e)
        {
            if (playList.SelectedIndex > -1)
            {
                MediaPlayer mp;
                p_list.TryGetValue(playList.SelectedItem.ToString(), out mp);
                mp.Pause();
            }
        }

        private void load_btn_Click(object sender, MouseButtonEventArgs e)
        {
            // создание объекта, обычно глобального
            MediaPlayer player = new MediaPlayer();

            //выбор медиа файла, например, в формате .mp3
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.ShowDialog();
            //загрузка выбранного файла
            player.Open(new Uri(dlg.FileName, UriKind.Relative));

            //команда на асинхронную загрузку и назначение обработчика события на завершение загрузки

            //функция, вызываемая при завершении загрузки
            p_list.Add(dlg.FileName.ToString(), player);
            playList.Items.Add(dlg.FileName.ToString());
        }
        private void Sp_LoadCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            MessageBox.Show("Звук загружен!");
        }

        private void Volume_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            double vol = Volume.Value;
            if (playList.SelectedIndex > -1)
            {
                MediaPlayer mp;
                p_list.TryGetValue(playList.SelectedItem.ToString(), out mp);
                mp.Volume = vol;

            }
        }

        private void duration_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (playList.SelectedIndex > -1)
            {
                //получение длительности медиа файла (свойство доступно только после события открытия)
                //установка проигрывания со середины медиа файла
                
                MediaPlayer mp;
                p_list.TryGetValue(playList.SelectedItem.ToString(), out mp);
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

        private void next_btn_Click(object sender, MouseButtonEventArgs e)
        {

        }

        private void prv_btn_Click(object sender, MouseButtonEventArgs e)
        {

        }

        private void rpt_btn_Click(object sender, MouseButtonEventArgs e)
        {

        }

        private void rnd_btn_Click(object sender, MouseButtonEventArgs e)
        {

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
