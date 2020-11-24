using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Threading;


namespace lab_timer
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Dictionary<string, DateTime> dicDate = new Dictionary<string, DateTime>();

        DateTime d1;
        TimeSpan ts;
        private DispatcherTimer dispatcherTimer;

        System.Windows.Forms.NotifyIcon ni = new System.Windows.Forms.NotifyIcon();

        public MainWindow()
        {
            InitializeComponent();

            ni.Icon = new System.Drawing.Icon(@"Timer.ico");
            ni.Visible = true;

            ni.DoubleClick +=
                delegate (object sender, EventArgs args)
                {
                    this.Show();
                    this.WindowState = WindowState.Normal;
                };

            string line;
            string fullPath = System.IO.Path.GetFullPath(@"data.txt");
            System.IO.StreamReader file = new System.IO.StreamReader(fullPath);
            
            while ((line = file.ReadLine()) != null)
            {
                string name = line;    
                if ((line = file.ReadLine()) == null) break;
                DateTime dt = DateTime.Parse(line);

                dicDate.Add(name, dt);
                tlist.Items.Add(name);
            }
 
            file.Close();

            timenow.Content = DateTime.Now.ToString("HH:mm:ss");
            dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(Time_);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            dispatcherTimer.Start();
        }

        protected override void OnStateChanged(EventArgs e)
        {
            if (WindowState == System.Windows.WindowState.Minimized)
                this.Hide();

            base.OnStateChanged(e);
        }

        private void Time_(object sender, EventArgs e)
        {
            timenow.Content = DateTime.Now.ToString("HH:mm:ss");
            foreach (KeyValuePair<string, DateTime> kvp in dicDate)
            {
                TimeSpan diff = kvp.Value - DateTime.Now;
                if ((diff.TotalSeconds < 0) && (diff.TotalSeconds > -1)) MessageBox.Show($"Timer {kvp.Key} is over!");
            }
        }

        private void ExitButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            System.IO.StreamWriter outputFile = new System.IO.StreamWriter(@"data.txt");

            using (outputFile) ///создание файла с таймерами
            {
                foreach (KeyValuePair<string, DateTime> kvp in dicDate)
                {
                    outputFile.WriteLine(kvp.Key);
                    outputFile.WriteLine(kvp.Value.ToString());
                }
            }
            outputFile.Close();
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

        private void Add_MouseDown(object sender, RoutedEventArgs e)
        {
            AddTimer add_timer = new AddTimer();
            add_timer.Hour.Text = DateTime.Now.Hour.ToString();
            add_timer.Min.Text = DateTime.Now.Minute.ToString();
            add_timer.Sec.Text = DateTime.Now.Second.ToString();

            if (add_timer.ShowDialog() == true)
            {
                int H = int.Parse(add_timer.Hour.Text);
                int M = int.Parse(add_timer.Min.Text);
                int S = int.Parse(add_timer.Sec.Text);

                DateTime dateTime = new DateTime(add_timer.Calendar.SelectedDate.Value.Year, add_timer.Calendar.SelectedDate.Value.Month, add_timer.Calendar.SelectedDate.Value.Day, H, M, S);

                if (dicDate.TryGetValue(add_timer.name.Text, out dateTime) == false)
                {
                    dicDate.Add(add_timer.name.Text.ToString(), new DateTime(add_timer.Calendar.SelectedDate.Value.Year, add_timer.Calendar.SelectedDate.Value.Month, add_timer.Calendar.SelectedDate.Value.Day, H, M, S));
                    tlist.Items.Add(add_timer.name.Text);
                }

            }
            else
            {

            }
        }

        private bool diffDateTime(DateTime d1, DateTime d2)
        {
            if (d1 == d2)
            {
                return true;
            }
            return false;
        }

        private void countdown(DateTime dnow, DateTime dfinish)
        {
            DateTime zero = new DateTime(0, 0, 0, 0, 0, 0);
            TimeSpan dnowTime = dnow.Subtract(zero);
            DateTime intervalDate = dfinish.Subtract(dnowTime);

            dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(Timer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            dispatcherTimer.Start();

            void Timer_Tick(object sender, EventArgs e)
            {
                while (intervalDate.Year != 0)
                {
                    intervalDate.AddSeconds(-1);
                } while (intervalDate.Month != 0)
                {
                    intervalDate.AddSeconds(-1);
                } while (intervalDate.Day != 0)
                {
                    intervalDate.AddSeconds(-1);
                } while (intervalDate.Hour != 0)
                {
                    intervalDate.AddSeconds(-1);
                } while (intervalDate.Minute != 0)
                {
                    intervalDate.AddSeconds(-1);
                } while (intervalDate.Second != 0)
                {
                    intervalDate.AddSeconds(-1);
                }
                if (intervalDate == zero)
                {
                    MessageBox.Show("Всё!");
                }
            }
        }

        private DispatcherTimer timeOut = new System.Windows.Threading.DispatcherTimer();

        public void tlist_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (tlist.SelectedIndex > -1)
            {
                timeOut.Tick += new EventHandler(Time_dif);
                timeOut.Interval = new TimeSpan(0, 0, 1);
                timeOut.Start();
                TimeSpan dif = dicDate[tlist.SelectedValue.ToString()] - DateTime.Now;

                if (dif.TotalSeconds > -1)
                {
                    status.Foreground = new SolidColorBrush(Colors.Green);
                    status.Content = "Осталось:";
                    timecd.Foreground = new SolidColorBrush(Colors.Green);
                    timecd.Content = $"{dif.Hours}:{dif.Minutes}:{dif.Seconds}";
                    dayscd.Foreground = new SolidColorBrush(Colors.Green);
                    dayscd.Content = $"{dif.Days} days";
                }
                else
                {
                    status.Foreground = new SolidColorBrush(Colors.Red);
                    status.Content = "Таймер прошёл:";
                    timecd.Foreground = new SolidColorBrush(Colors.Red);
                    timecd.Content = $"{Math.Abs(dif.Hours)}:{Math.Abs(dif.Minutes)}:{Math.Abs(dif.Seconds)}";
                    dayscd.Foreground = new SolidColorBrush(Colors.Red);
                    dayscd.Content = $"{Math.Abs(dif.Days)} days";
                }

            }
        }

        private void Time_dif(object sender, EventArgs e)
        {
            TimeSpan dif = dicDate[tlist.SelectedValue.ToString()] - DateTime.Now;
            if (dif.TotalSeconds > -1)
            {
                status.Foreground = new SolidColorBrush(Colors.Green);
                status.Content = "Осталось:";
                timecd.Foreground = new SolidColorBrush(Colors.Green);
                timecd.Content = $"{dif.Hours}:{dif.Minutes}:{dif.Seconds}";
                dayscd.Foreground = new SolidColorBrush(Colors.Green);
                dayscd.Content = $"{dif.Days} days";
            }
            else
            {
                status.Foreground = new SolidColorBrush(Colors.Red);
                status.Content = "Таймер прошёл:";
                timecd.Foreground = new SolidColorBrush(Colors.Red);
                timecd.Content = $"{Math.Abs(dif.Hours)}:{Math.Abs(dif.Minutes)}:{Math.Abs(dif.Seconds)}";
                dayscd.Foreground = new SolidColorBrush(Colors.Red);
                dayscd.Content = $"{Math.Abs(dif.Days)} days";
            }
        }

        private void Del_MouseDown(object sender, MouseButtonEventArgs e)
        { 
            if (tlist.SelectedIndex > -1)
            {
                timeOut.Stop();
                dicDate.Remove(tlist.SelectedValue.ToString());
                tlist.Items.RemoveAt(tlist.SelectedIndex);
                timecd.Content = "00:00:00";
                dayscd.Content = "0 days";
            }
        }

        private void Change_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (tlist.SelectedIndex > -1)
            {
                AddTimer add_timer = new AddTimer();
                add_timer.name.Text = tlist.SelectedValue.ToString();
                add_timer.Hour.Text = dicDate[tlist.SelectedValue.ToString()].Hour.ToString();
                add_timer.Min.Text = dicDate[tlist.SelectedValue.ToString()].Minute.ToString();
                add_timer.Sec.Text = dicDate[tlist.SelectedValue.ToString()].Second.ToString();
                add_timer.Calendar.SelectedDate = dicDate[tlist.SelectedValue.ToString()];
                
                timeOut.Stop();
                dicDate.Remove(tlist.SelectedValue.ToString());
                tlist.Items.RemoveAt(tlist.SelectedIndex);
                timecd.Content = "00:00:00";
                dayscd.Content = "0 days";

                if (add_timer.ShowDialog() == true)
                {
                    int H = int.Parse(add_timer.Hour.Text);
                    int M = int.Parse(add_timer.Min.Text);
                    int S = int.Parse(add_timer.Sec.Text);

                    if (dicDate.TryGetValue(add_timer.name.Text, out DateTime dateTime) == false)
                    {
                        dicDate.Add(add_timer.name.Text.ToString(), new DateTime(add_timer.Calendar.SelectedDate.Value.Year, add_timer.Calendar.SelectedDate.Value.Month, add_timer.Calendar.SelectedDate.Value.Day, H, M, S));
                        tlist.Items.Add(add_timer.name.Text);
                    }

                }
                else
                {

                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (Box.Visibility == Visibility.Hidden)
            {
                Box.Visibility = Visibility.Visible;
            }
            else
                Box.Visibility = Visibility.Hidden;
        }

        private void _day_Click(object sender, RoutedEventArgs e)
        {
            TimeSpan dif = dicDate[tlist.SelectedValue.ToString()] - DateTime.Now;
            timecd.Content = $"{Math.Abs(dif.Hours)}:{Math.Abs(dif.Minutes)}:{Math.Abs(dif.Seconds)}";
            dayscd.Content = $"{Math.Abs(dif.Days)} days";
        }

        private void _hour_Click(object sender, RoutedEventArgs e)
        {
            TimeSpan dif = dicDate[tlist.SelectedValue.ToString()] - DateTime.Now;
            timecd.Content = $"{Math.Abs(dif.Days * 24 + dif.Hours)}:{Math.Abs(dif.Minutes)}:{Math.Abs(dif.Seconds)}";
            dayscd.Content = "";
        }

        private void _min_Click(object sender, RoutedEventArgs e)
        {
            TimeSpan dif = dicDate[tlist.SelectedValue.ToString()] - DateTime.Now;
            timecd.Content = $"{Math.Abs((dif.Days * 24 + dif.Hours) * 60 + dif.Minutes)}:{Math.Abs(dif.Seconds)}";
            dayscd.Content = "";
        }

        private void _sec_Click(object sender, RoutedEventArgs e)
        {
            TimeSpan dif = dicDate[tlist.SelectedValue.ToString()] - DateTime.Now;
            timecd.Content = $"{Math.Abs(((dif.Days * 24 + dif.Hours) * 60 + dif.Minutes) * 60 + dif.Seconds)}";
            dayscd.Content = "seconds";
        }

        private void ShowSec(object sender, EventArgs e)
        {
            TimeSpan dif = dicDate[tlist.SelectedValue.ToString()] - DateTime.Now;
            
            if (dif.TotalSeconds > -1)
            {
                status.Content = "Осталось:";
                timecd.Content = $"{Math.Abs(((dif.Days * 24 + dif.Hours) * 60 + dif.Minutes) * 60 + dif.Seconds)}";
                dayscd.Content = "0 days";
                status.Foreground = new SolidColorBrush(Colors.Green);
                timecd.Foreground = new SolidColorBrush(Colors.Green);
                dayscd.Foreground = new SolidColorBrush(Colors.Green);
            }
            else
            {
                status.Content = "Таймер прошёл:";
                timecd.Content = $"{Math.Abs(((dif.Days * 24 + dif.Hours) * 60 + dif.Minutes) * 60 + dif.Seconds)}";
                dayscd.Content = "0 days";
                status.Foreground = new SolidColorBrush(Colors.Red);
                timecd.Foreground = new SolidColorBrush(Colors.Red);
                dayscd.Foreground = new SolidColorBrush(Colors.Red);
            }
        }
    }
}
