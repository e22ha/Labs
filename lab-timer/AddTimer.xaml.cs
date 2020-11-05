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

        

        private void exit_Click(object sender, RoutedEventArgs e)
        {
            if ((Hour.Text.Length != 0) & (Min.Text.Length != 0) & (Sec.Text.Length != 0))
            {

                int H = int.Parse(Hour.Text);
                int M = int.Parse(Min.Text);
                int S = int.Parse(Sec.Text);
                current = new DateTime(1, 1, 1, H, M, S);
            }
            //при нажатии кнопки “Закрыть” происходит
            //закрытие окна с отметкой об не успешном завершении работы
            this.DialogResult = false;
        }
    }
}
