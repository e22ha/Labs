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

namespace Float
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        int[] bin = new int[32];

        public MainWindow()
        {
            InitializeComponent();
        }

        int[] DecToBin(int n)
        {
            int[] res = new int[32];

            for (int i = 31; i >= 0; i--)
            {
                res[i] = n % 2;
                n /= 2;
            }

            return res;
        }

        int[] remainsToBin(float remains)
        {
            int[] res = new int[32];

            for (int i = 0; i < 32; i++)
            {
                remains *= 2;
                res[i] = (int)remains;
                remains -= (int)remains;
                if (remains == 0) break;
            }

            return res;
        }

        private void toBin_Click(object sender, RoutedEventArgs e)
        {
            float f = float.Parse(tb_Dec.Text);
            if (f < 0)
            {
                bin[0] = 1;
                tb_sign.Text = "1";
                f *= -1;
            }
            else
            {
                bin[0] = 0;
                tb_sign.Text = "0";
            }
            int whole = (int)f;
            float remains = f - whole;


            if (whole > 0)
            {
                int[] W = DecToBin(whole);
                int[] R = remainsToBin(remains);

                int i = 0;
                int j = 9;
                for (; i < 32; i++)
                {
                    if (W[i] == 1) break;
                }
                i++;
                int pow = 32 - i;
                pow += 127;
                for (; i < 32; i++, j++)
                {
                    bin[j] = W[i];
                }
                for (i = 0; j < 32; i++, j++)
                {
                    bin[j] = R[i];
                }

                int[] Ex = DecToBin(pow);

                for (i = 24, j = 1; j < 9; i++, j++)
                {
                    bin[j] = Ex[i];
                }
            }
            else
            {
                int[] R = remainsToBin(remains);

                int i = 0;
                int j = 9;
                for (; i < 32; i++)
                {
                    if (R[i] == 1) break;
                }
                i++;
                int pow = -(i);
                pow += 127;

                for (; j < 32; i++, j++)
                {
                    bin[j] = R[i];
                }

                int[] Ex = DecToBin(pow);

                for (i = 24, j = 1; j < 9; i++, j++)
                {
                    bin[j] = Ex[i];
                }
            }


            tb_mant.Text = binToStr(bin, 9, 32);
            tb_exp.Text = binToStr(bin, 1, 9);
        }



        string binToStr(int[] mas, int start, int end)
        {
            string str = "";

            for (int i = start; i < end; i++)
            {
                str += mas[i].ToString();
            }

            return str;
        }

        private void toDec_Click(object sender, RoutedEventArgs e)
        {
            string res = "";
            int Exp = 0;
            float Res = 0;


            if (tb_sign.Text != "")
            {
                if (tb_sign.Text == "1")//Определение знака
                {
                    res += "-";//Добавление знака в результат
                }
            }


            //Определение степпени
            if (tb_exp.Text != "")
            {
                string expStr = tb_exp.Text.ToString();
                int[] expMas = new int[8];
                expMas = strToBin(expStr);
                Exp = binToDec(expMas) - 127;

                //Определение мантисы
                if (tb_mant.Text != "")
                {
                    string manStr = tb_mant.Text.ToString();
                    int[] manMas = new int[23];
                    manMas = strToBin(manStr);
                    if (Exp >= 0)
                    {

                        int[] resR = new int[24 - Exp];

                        int i = Exp-1;
                        int j = 0;
                        Res += (int)Math.Pow(2, Exp);
                        for (; i > 0; i--, j++)
                        {
                            Res += (int)(manMas[j] * Math.Pow(2, i));
                        }
                        j++;

                        for (i = 0; i < 23 - Exp; i++, j++)
                        {
                            resR[i] = manMas[j];
                        }
                        float remains = binToRemains(resR);
                        Res += remains;

                    }
                    else
                    {
                        int[] resR = new int[24 - Exp];
                        Exp = Math.Abs(Exp);
                        int i = Exp - 1;
                        int j = 0;
                        resR[i] = 1;
                        i++;
                        for (; i < 23; i++, j++)
                        {
                            resR[i] = manMas[j];
                        }
                        float remains = binToRemains(resR);
                        Res += remains;
                    }

                }
            }
            res += Res;

            tb_Dec.Text = res;
        }

        private float binToRemains(int[] resR)
        {
            float res = 0;

            for (int i = 0; i < resR.Length; i++)
            {
                if (resR[i] == 1)
                {
                    res += 1 / (float)Math.Pow(2, i + 1);
                }
            }

            return res;
        }

        int[] strToBin(string n)
        {
            int[] m = new int[n.Length];
            for (int i = n.Length - 1; i >= 0; i--)
            {
                m[i] = int.Parse(n[i].ToString());

            }

            return m;
        }
        private int binToDec(int[] mas)
        {
            int res = 0;

            for (int i = 7; i >= 0; i--)
            {

                res += mas[i] * (int)Math.Pow(2, 7 - i);

            }

            return res;
        }
    }
}
