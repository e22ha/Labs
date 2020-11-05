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
        DateTime d1;
        TimeSpan ts;
        private DispatcherTimer dispatcherTimer;

        public MainWindow()
        {
            InitializeComponent();
            //устанавливается дата - 28.02.2020 21:30:10

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
                //поскольку переменная current, окна add_timer была объявлена как public
                //можно обратиться к ней напрямую и получить необходимые данные
                d1 = add_timer.current;
            }
            else //если окно отработало с результатом false
            {
                //либо вывести сообщение, что данные не были получены
                //либо ничего не делать
            }

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
                    //вызов сообщения об окаончании даты
                }
            }
        }



        private void tlist_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Del_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void Change_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }
    }
}
