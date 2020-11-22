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

        public MainWindow()
        {
            InitializeComponent();

            timenow.Content = DateTime.Now.ToString("HH:mm:ss");
            dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(Time_);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            dispatcherTimer.Start();

        }


        protected override void OnStateChanged(EventArgs e)
        {
            if (WindowState == WindowState.Minimized)
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
            foreach (KeyValuePair<string, DateTime> kvp in dicDate)
            {
                ///создание файла с таймерами
            }

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
            //создание нового окна (название класса – то, что было указано при добавлении окна)
            AddTimer add_timer = new AddTimer();
            //вызов окна + проверка, отработало ли окно корректно
            if (add_timer.ShowDialog() == true)
            {
                int H = int.Parse(add_timer.Hour.Text);
                int M = int.Parse(add_timer.Min.Text);
                int S = int.Parse(add_timer.Sec.Text);

                DateTime dateTime = new DateTime(add_timer.Calendar.SelectedDate.Value.Year, add_timer.Calendar.SelectedDate.Value.Month, add_timer.Calendar.SelectedDate.Value.Day, H, M, S);

                //при нажатии кнопки “Закрыть” происходит
                //закрытие окна с отметкой об не успешном завершении работы
                if (dicDate.TryGetValue(add_timer.name.Text, out dateTime) == false)
                {
                    dicDate.Add(add_timer.name.Text.ToString(), new DateTime(add_timer.Calendar.SelectedDate.Value.Year, add_timer.Calendar.SelectedDate.Value.Month, add_timer.Calendar.SelectedDate.Value.Day, H, M, S));
                    tlist.Items.Add(add_timer.name.Text);
                }

            }
            else //если окно отработало с результатом false
            {
                //либо вывести сообщение, что данные не были получены
                //либо ничего не делать
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
                dayscd.Foreground = new SolidColorBrush(Colors.White);
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
                tlist.Items[tlist.SelectedIndex] = null;
                timecd.Content = "00:00:00";
                dayscd.Content = "0 days";
            }
        }

        private void Change_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }
    }
}
