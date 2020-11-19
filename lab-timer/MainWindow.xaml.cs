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

        private void Time_(object sender, EventArgs e)
        {
            timenow.Content = DateTime.Now.ToString("HH:mm:ss");
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
                if (dicDate.TryGetValue(add_timer.name.Text, out dateTime)==false)
                {
                    dicDate.Add(add_timer.name.Text.ToString(), new DateTime(add_timer.Calendar.SelectedDate.Value.Year, add_timer.Calendar.SelectedDate.Value.Month, add_timer.Calendar.SelectedDate.Value.Day, H, M, S));
                }
            }
            else //если окно отработало с результатом false
            {
                //либо вывести сообщение, что данные не были получены
                //либо ничего не делать
            }
            tlist.Items.Add(add_timer.name.Text);
        }

        private bool diffDateTime(DateTime d1, DateTime d2)
        {
            if (d1 == d2)
            {
                return true;
            }return false;
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



        private void tlist_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (tlist.SelectedIndex> -1)
            {
                dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
                dispatcherTimer.Tick += new EventHandler(Time_dif);
                dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
                dispatcherTimer.Start();
                TimeSpan dif = dicDate[tlist.SelectedValue.ToString()] - DateTime.Now;
                timecd.Content = $"{dif.Hours}:{dif.Minutes}:{dif.Seconds}";
                dayscd.Content = $"{dif.Days} days";
            }
        }

        private void Time_dif(object sender, EventArgs e)
        {
            TimeSpan dif = dicDate[tlist.SelectedValue.ToString()] - DateTime.Now;
            timecd.Content = $"{dif.Hours}:{dif.Minutes}:{dif.Seconds}";
            dayscd.Content = $"{dif.Days} days";
            if (dif.TotalMilliseconds == 0)
            {
                MessageBox.Show("Всё!");
            }
        }

        private void Del_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (tlist.SelectedIndex > -1)
            {
                dicDate.Remove(tlist.SelectedValue.ToString());
                tlist.Items[tlist.SelectedIndex]=null;
            }
        }

        private void Change_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }
    }
}
