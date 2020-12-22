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

namespace minesweeper
{
    /// <summary>
    /// Логика взаимодействия для Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        List<string> records = new List<string>();

        public Window1()
        {
            InitializeComponent();

            string line;
            string fullPath = System.IO.Path.GetFullPath(@"data.txt");
            System.IO.StreamReader file = new System.IO.StreamReader(fullPath);
            while ((line = file.ReadLine()) != null)
            {
                string name = line;
                if ((line = file.ReadLine()) == null) break;
                DateTime dt = DateTime.Parse(line);

                records.Add(str);
                tlist.Items.Add(name);
            }

            file.Close();
        }
    }
}
