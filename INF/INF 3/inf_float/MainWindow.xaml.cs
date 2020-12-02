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

namespace inf_float
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

        private void but_decToBin_Click(object sender, RoutedEventArgs e)
        {
            if (dec_in_out.Text != "")
            {
                string res = "";
                float n = float.Parse(dec_in_out.Text);


                byte[] tmp = BitConverter.GetBytes(n);

                for (int j = 3; j >= 0; j--)
                {
                    res += BinToStr(DecToBin(tmp[j]));
                }

                string output = "";
                int i = 0;
                for (; i < 1; i++)
                {
                    output += res[i].ToString();
                }
                output += " ";
                for (; i < 9; i++)
                {
                    output += res[i].ToString();
                }
                output += " ";
                for (; i < 32; i++)
                {
                    output += res[i].ToString();
                }

                bin_in_out.Text = output;

            }
        }


        private void but_BinToDec_Click(object sender, RoutedEventArgs e)
        {
            if (bin_in_out.Text != "")
            {
                byte[] m = strToBin(bin_in_out.Text);

                byte[] m1 = new byte[8];
                byte[] m2 = new byte[8];
                byte[] m3 = new byte[8];
                byte[] m4 = new byte[8];



                for (int i = 0; i < 8; i++)
                {
                    m1[i] = (byte)(m[i] & '1'); ;
                    m2[i] = (byte)(m[i + 8] & '1');
                    m3[i] = (byte)(m[i + 16] & '1');
                    m4[i] = (byte)(m[i + 24] & '1');

                }


                byte[] mas = new byte[4] { (byte)BinToDec(m4), (byte)BinToDec(m3), (byte)BinToDec(m2), (byte)BinToDec(m1) };

                float f = BitConverter.ToSingle(mas, 0);

                dec_in_out.Text = f.ToString();
            }
        }

        byte[] strToBin(string n)
        {
            byte[] m = new byte[n.Length];
            for (int i = n.Length - 1; i >= 0; i--)
            {
                m[i] = byte.Parse(n[i].ToString());

            }

            return m;
        }
        int BinToDec(byte[] m)
        {
            int res = 0;

            for (int i = 7; i >= 0; i--)
            {
                res += (int)(m[i] * Math.Pow(2, 7 - i));
            }

            return res;
        }

        string BinToStr(int[] m)
        {
            string res = "";

            for (int i = 0; i < m.Length; i++)
            {
                res += m[i] & '1';
            }

            return res;
        }

        int[] DecToBin(int n)
        {
            int[] res = new int[8];

            for (int i = 7; i >= 0; i--)
            {
                res[i] = n % 2;
                n /= 2;
            }

            return res;
        }
    }
}
