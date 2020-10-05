using System;
using System.Collections.Generic;
using System.Globalization;
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
using Microsoft.SqlServer.Server;

namespace WpfApp4
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public int da;
        public int mo;
        public int ya;


        public MainWindow()
        {
            InitializeComponent();
            for (int i = 0; i<=70;i++) {
                int a = 1950;
                Year_choice.Items.Add(a+i);
            }
            Month_choice.IsEnabled = false;
            Day_Choice.IsEnabled = false;
            
            
        }

        private void Year_choice_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
            Month_choice.Items.Clear();
            string[] m = new string[12] { "Jan", "Feb", "Mar", "Apr", "Может", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };
            for (int i = 0; i <= 11; i++)
            {
                Month_choice.Items.Add(m[i]);
            }
            ya = int.Parse(Year_choice.SelectedItem.ToString());
            Month_choice.IsEnabled = true;
        }

        private void Month_choice_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            
            if ((Month_choice.SelectedIndex == 1) &  (int.Parse(Year_choice.Text) % 4 == 0))
            {
                Day_Choice.Items.Clear();
                for (int i = 1; i <= 29; i++)
                {
                    Day_Choice.Items.Add(i);
                }
            }
            else if ((Month_choice.SelectedIndex == 1) & (int.Parse(Year_choice.Text) % 4 != 0))
            {
                Day_Choice.Items.Clear();
                for (int i = 1; i <= 28; i++)
                {
                    Day_Choice.Items.Add(i);
                }
            }
            else if ((Month_choice.SelectedIndex == 0) | (Month_choice.SelectedIndex ==2) | (Month_choice.SelectedIndex == 4) | (Month_choice.SelectedIndex == 6) | (Month_choice.SelectedIndex == 7) | (Month_choice.SelectedIndex == 9) | (Month_choice.SelectedIndex == 11))
            {
                Day_Choice.Items.Clear();
                for (int i = 1; i <= 31; i++)
                {
                    Day_Choice.Items.Add(i);
                }
            }
            else 
            {
                Day_Choice.Items.Clear();
                for (int i = 1; i <= 30; i++)
                {
                    Day_Choice.Items.Add(i);
                }
            }
            mo = Month_choice.SelectedIndex + 1;
            Day_Choice.IsEnabled = true;

        }

        private void Day_Choice_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            da = int.Parse(Day_Choice.SelectedItem.ToString());
        }


        private void go_Click(object sender, RoutedEventArgs e)
        {
            output.Text = null;
            DateTime datanow = DateTime.Today.Date;
            int nYear = datanow.Year;
            int nMo = datanow.Month;
            int nDay = datanow.Day;

            DateTime choosedata = new DateTime(ya, mo, da);
            
            string dayOf = datanow.Subtract(choosedata).Days.ToString();
            var outYear = nYear - choosedata.Year;
            var outMonth = nMo - choosedata.Month;
            var outDay = nDay - choosedata.Day;
           
            if (outMonth <0)
            {
                outYear --;
                outMonth = 12 + outMonth;
            }
            
            if (outDay <0)
            {
                outMonth--;
                if ((outMonth == 1) | (outMonth == 3) | (outMonth == 5) | (outMonth == 7) | (outMonth == 8) | (outMonth == 10) | (outMonth == 12))
                {
                    outDay = 31 + outDay;
                }
                else if ((outMonth == 2)&(outYear % 4 == 0))
                {
                    outDay = 29 + outDay;
                }
                else if ((outMonth == 2) & (outYear % 4 != 0))
                {
                    outDay = 28 + outDay;
                }
                else
                {
                    outDay = 30 + outDay;
                }
            }

            output.Text = "Years: "+(outYear)+"; Months: "+(outMonth)+"; Days: "+(outDay);
        }
    } 
}
