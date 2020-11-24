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
        public AddTimer()
        {
            InitializeComponent();
        }

        private void Done_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (int.Parse(Hour.Text) > 24)
            {
                Hour.Text = "";
                Window error = new War();
                Hour.BorderBrush = new SolidColorBrush(Colors.Red);
                error.Show();
            }
            else if (int.Parse(Min.Text) > 60)
            {
                Min.Text = "";
                Window error = new War();
                Min.BorderBrush = new SolidColorBrush(Colors.Red);
                error.Show();
            }
            else if (int.Parse(Sec.Text) > 60)
            {
                Sec.Text = "";
                Window error = new War();
                Sec.BorderBrush = new SolidColorBrush(Colors.Red);
                error.Show();
            }
            else
            {
                this.DialogResult = true;
            }
        }

        private void Cancel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.DialogResult = false;
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
            this.DialogResult = false;
            this.Close();
        }
    }
}
