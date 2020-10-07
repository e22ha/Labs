using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp5
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        System.Windows.Threading.DispatcherTimer dispatcherTimer;
        DateTime date;
        bool ch = false;
        
        DateTime outWatch = new DateTime();

        public MainWindow()
        {
            InitializeComponent();

            //  DispatcherTimer setup
        }
        private void Timer_Tick(object sender, EventArgs e)
        {
            long tick = DateTime.Now.Ticks - date.Ticks;
           
            outWatch = outWatch.AddTicks(tick);
            
            output.Content = String.Format("{0:HH:mm:ss:ff}", outWatch); 
        }


        private void Start_Click(object sender, RoutedEventArgs e)
        {
            outWatch = new DateTime();
            date = DateTime.Now;
            dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(Timer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0,10);
            dispatcherTimer.Start();

        }
        private void Stop_Click(object sender, RoutedEventArgs e)
        {
            dispatcherTimer.Stop();
        }

        private void Remember_Click(object sender, RoutedEventArgs e)
        {
            int data = (int)outWatch.Second;
            data = data + (int)(outWatch.Minute * 60);
            data = data + (int)(outWatch.Hour * 60*60);
            if (ch == false)
            {
            
                memory.Items.Add(output.Content.ToString());

            }
            if (ch == true)
            {
                memory.Items.Add(data.ToString() + " сек");
            }
        }


        private void check_Checked(object sender, RoutedEventArgs e)
        {
            if (ch == false)
            {
                ch = true;
            }
            else
            {
                ch = false;
            }
        }
    }
}
