using System;
using System.Collections.Generic;
using System.IO;
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
using static System.Net.WebRequestMethods;

namespace WpfApp6
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        List<string> da;
        public MainWindow()
        {
            InitializeComponent();
        }
        public class MyClass
        {
            public static List<string> Open(StreamReader f)
            {
                List<string> myList = new List<string>();
                string line;

                while ((line = f.ReadLine()) != null)
                {
                    myList.Add(line);
                }

                f.Close();
                return myList;
            }
        }

        private void Load_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            //настройка параметров диалога
            dlg.DefaultExt = ".txt"; // Default file extension
            dlg.Filter = "Text documents (.txt)|*.txt"; // Filter files by extension
                                                        //вызов диалога
            dlg.ShowDialog();
            StreamReader file = new StreamReader(dlg.OpenFile());
            da = MyClass.Open(file);
            
        }
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            //настройка параметров диалога
            dlg.FileName = "Document"; // Default file name
            dlg.DefaultExt = ".txt"; // Default file extension
            dlg.Filter = "Text documents (.txt)|*.txt"; // Filter files by extension
                                                        //вызов диалога
            dlg.ShowDialog();
            using (StreamWriter outputFile = new StreamWriter(dlg.FileName))
            {
                foreach (string line in da) outputFile.WriteLine(line);
            }
        }

        //получение выбранного имени файла
        ///lb1.Content = dlg.FileName;

    }       
}
