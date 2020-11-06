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
using System.Windows.Shapes;

namespace lab_timer
{
    /// <summary>
    /// Логика взаимодействия для AddTimer.xaml
    /// </summary>
    public partial class AddTimer : Window
    {
        public DateTime current;
        public AddTimer()
        {
            InitializeComponent();

        }

        private void Done_MouseDown(object sender, RoutedEventArgs e)
        {
            if ((Hour.Text.Length != 0) & (Min.Text.Length != 0) & (Sec.Text.Length != 0))
            {

                int H = int.Parse(Hour.Text);
                int M = int.Parse(Min.Text);
                int S = int.Parse(Sec.Text);
                current = new DateTime(Calendar.DisplayDate.Year, Calendar.DisplayDate.Month, Calendar.DisplayDate.Day,H,M,S);
            }
            //при нажатии кнопки “Закрыть” происходит
            //закрытие окна с отметкой об не успешном завершении работы
            this.DialogResult = false;
        }

        private void Cancel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }
        
        private void ToolBar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }

        private void ExitButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }
    }
}
