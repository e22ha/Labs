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

namespace PassGen
{
    /// <summary>
    /// Генератор случайных паролей
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }


        //Генератор случайных чисел
        long X = DateTime.Now.Ticks;
        int a = 13;
        int c = 26;
        int N = int.MaxValue;
        int getRandom(int min, int max)
        {
            int rand = 0;

            X = (int)((a * X + c) % N);

            rand = (int)((X % (max - min)) + min);

            return rand;
        }


        //Процедура вывода символов Алфавита
        void symbolOut()
        {
            int startSymbol = int.Parse(start.Text, System.Globalization.NumberStyles.HexNumber);
            int j = 1;

            for (int i = startSymbol; i <= int.Parse(end.Text, System.Globalization.NumberStyles.HexNumber); i++, j++)
            {
                string str = char.ConvertFromUtf32(i).ToString();
                outAlphabet.Text += (str + " = " + i.ToString("X4") + ";  ");
                if (j % 7 == 0) outAlphabet.Text += "\n";
            }
        }


        //Фнукция генерации пароля
        private void Generate_Click(object sender, RoutedEventArgs e)
        {
            symbolOut();

            string pass = "";

            for (int i = 0; i < int.Parse(length.Text); i++)
            {
                int a = getRandom(int.Parse(start.Text, System.Globalization.NumberStyles.HexNumber), int.Parse(end.Text, System.Globalization.NumberStyles.HexNumber));
                pass += char.ConvertFromUtf32(a).ToString();
            }
            outPass.Text = pass.ToString();

        }
    }
}
