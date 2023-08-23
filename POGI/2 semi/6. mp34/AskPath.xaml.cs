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

namespace mp34
{
    /// <summary>
    /// Логика взаимодействия для AskPath.xaml
    /// </summary>
    public partial class AskPath : Window
    {
        public string p;
        public AskPath()
        {
            InitializeComponent();
        }

        private void done_Click(object sender, RoutedEventArgs e)
        {
            p = path.Text.ToString();
            this.DialogResult = true;
            this.Close();
        }
    }
}
