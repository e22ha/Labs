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

namespace CustomUserCard
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btn_accept_Click(object sender, RoutedEventArgs e)
        {
            if (tb_name.Text != "") UC.lb_name1.Content = tb_name.Text;
            if (tb_name2.Text != "") UC.lb_name2.Content = tb_name2.Text;
            if (tb_pos.Text != "") UC.lb_position.Content = tb_pos.Text;
            if (tb_company.Text != "") UC.lb_company.Content = tb_company.Text;
        }
    }
}
