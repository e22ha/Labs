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

namespace FileCoder
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

        private void Load_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            //настройка параметров диалога
            dlg.DefaultExt = ".txt"; // Default file extension
            dlg.Filter = "Text documents (.txt)|*.txt"; // Filter files by extension
                                                        //вызов диалога
            dlg.ShowDialog();
            FileStream fs = File.OpenRead(dlg.FileName); //открытие файла на чтение
            byte[] array = new byte[fs.Length]; //создание массива байт
            fs.Read(array, 0, array.Length); //запись содержимого файла в массив байт
            string textFromFile = System.Text.Encoding.UTF32.GetString(array); //преобразование массива в строку
            textIn.Text = textFromFile;
        }

        private void Encode_Click(object sender, RoutedEventArgs e)
        {
            textOut.Text = null;
            string str = textIn.Text;
            try
            {
                byte key = Byte.Parse(keyCipher.Text);
                for (int i = 0; i < str.Length; i++) textOut.Text += char.ConvertFromUtf32((byte)str[i] ^ key);
            }
            catch (Exception)
            {

                MessageBox.Show("key invaild");
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            //настройка параметров диалога
            dlg.DefaultExt = ".txt"; // Default file extension
            dlg.Filter = "Text documents (.txt)|*.txt"; // Filter files by extension
                                                        //вызов диалога
            dlg.ShowDialog();
            using (FileStream fstream = new FileStream(dlg.FileName, FileMode.OpenOrCreate))
            {
                byte[] array = System.Text.Encoding.UTF32.GetBytes(textOut.Text); //получение строки в виде массива байт
                fstream.Write(array, 0, array.Length); //запись массива байт в файл
            }
        }

        private void Decode_Click(object sender, RoutedEventArgs e)
        {
            textOut.Text = null;
            string str = textIn.Text;
            try
            {
                byte key = Byte.Parse(keyCipher.Text);
                for (int i = 0; i < str.Length; i++) textOut.Text += char.ConvertFromUtf32((byte)str[i] ^ key);
            }
            catch (Exception)
            {

                MessageBox.Show("key invaild");
            }
        }

        private void swap_Click(object sender, RoutedEventArgs e)
        {
            string mass = textIn.Text;
            textIn.Text = textOut.Text;
            textOut.Text = mass;
        }
    }
}
