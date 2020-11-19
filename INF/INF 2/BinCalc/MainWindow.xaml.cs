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

namespace BinCalc
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        int[] A = new int[8];
        int[] B = new int[8];

        public MainWindow()
        {
            InitializeComponent();
            for (int i = 0; i < 8; i++)
            {
                Button btn = new Button();
                btn.Tag = i;
                btn.Content = "0";
                btn.Width = btn.Height = 20;

                btn.Click += Btn_Click_A;
                ugrA.Children.Add(btn);
            }

            for (int i = 0; i < 8; i++)
            {
                Button btn = new Button();
                btn.Tag = i;

                btn.Width = btn.Height = 20;
                btn.Content = "0";
                btn.Click += Btn_Click_B;
                ugrB.Children.Add(btn);
            }
        }

        sbyte binToDec(int[] mas)
        {
            sbyte s = 0;
            for (int i = 0; i < 8; i++)
            {
                s += (sbyte)((1<<i)*mas[7-i]);
            }
            return s;
        }

        private int[] sbyteToBin(sbyte n)
        {
            int[] mas = new int[8];
            for (int i = 0; i < 8; i++)
            {
                mas[i] = (n >> (7-i)) & 1;
            }
            return mas;
        }

        private void Btn_Click_A(object sender, RoutedEventArgs e)
        {
            if (A[(int)((Button)sender).Tag]== 0)
            {
                A[(int)((Button)sender).Tag] = 1;
                ((Button)sender).Content = "1";
                
            }
            else
            {
                A[(int)((Button)sender).Tag] = 0;
                ((Button)sender).Content = "0";
                
            }
            txA.Text = binToDec(A).ToString();
            
        }
        
        private void Btn_Click_B(object sender, RoutedEventArgs e)
        {
            if (B[(int)((Button)sender).Tag]== 0)
            {
                B[(int)((Button)sender).Tag] = 1;
                ((Button)sender).Content = "1";
                
            }
            else
            {
                B[(int)((Button)sender).Tag] = 0;
                ((Button)sender).Content = "0";
                
            }
            txB.Text = binToDec(B).ToString();
            
        }

        private void txA_TextChanged(object sender, TextChangedEventArgs e)
        {
            sbyte n = 0;
            if (sbyte.TryParse(txA.Text, out n) == true)
            {
                A = sbyteToBin(n);
                for (int i = 0; i < ugrA.Children.Count; i++)
                {
                    ((Button)ugrA.Children[i]).Content = A[i];
                }
            }
        }

        private void txB_TextChanged(object sender, TextChangedEventArgs e)
        {
            sbyte n = 0;
            if (sbyte.TryParse(txB.Text, out n) == true)
            {
                B = sbyteToBin(n);
                for (int i = 0; i < ugrB.Children.Count; i++)
                {
                    ((Button)ugrB.Children[i]).Content = B[i];
                }
            }
        }

        private void plus_Click(object sender, RoutedEventArgs e)
        {
            int[] copyA = new int[8];
            int[] copyB = new int[8];
            A.CopyTo(copyA, 0);
            B.CopyTo(copyB, 0);
            
            int[] c = sum(copyA, copyB);
            string s = "";
            for (int i = 0; i < 8; i++)
            {
                s += c[i].ToString();
            }
            result.Content = s;
        }

        private static int[] addOne(int[] n, int start)
        {
            int k = 1;

            for (int i = start; i >= 0; i--)
            {
                n[i] += k;
                k = 0;
                if (n[i] > 1)
                {
                    n[i] = 0;
                    k = 1;
                }

                if (k == 0) break;
            }
            return n;
        }

        private static int[] sum(int[] a, int[] b)
        {
            for (int i = 7; i >= 0; i--)
            {
                if (b[i] == 1)
                {
                    a = addOne(a, i);
                }
            }

            return a;
        }

        private void minus_Click(object sender, RoutedEventArgs e)
        {
            int[] copyA = new int[8];
            int[] copyB = new int[8];
            A.CopyTo(copyA, 0);
            B.CopyTo(copyB, 0);
            copyB = invert(copyB);
            addOne(copyB,7);
            int[] c = sum(copyA, copyB);
            string s = "";
            for (int i = 0; i < 8; i++)
            {
                s += c[i].ToString();
            }
            result.Content = s;
        }

        private int[] invert(int[] b)
        {
            for (int i = 0; i < b.Length; i++)
            {
                if (b[i] == 0)
                {
                    b[i] = 1;
                }
                else
                {
                    b[i] = 0;
                }
            }
            return b;
        }

        private void mul_Click(object sender, RoutedEventArgs e)
        {
            int[] copyA = new int[8];
            int[] copyB = new int[8];

            A.CopyTo(copyA, 0);
            B.CopyTo(copyB, 0);

            int[] res = new int[8]; ; //sbyteToBin((sbyte)(binToDec(A)*binToDec(B)));
            
            for (int i = 7; i >= 0; i--)
            {
                if (copyB[i] == 1)
                {
                    res = sum(res,copyA);
                }
                copyA = sdvig(copyA);
            }

            

            string s = "";
            for (int i = 0; i < 8; i++)
            {
                s += res[i].ToString();
            }
            result.Content = s;
            
        }

        private int[] sdvig(int[] a)
        {
            for (int i = 0; i < 7; i++)
            {
                a[i] = a[i + 1];
            }
            a[7] = 0;
            return a;
        }

        private void div_Click(object sender, RoutedEventArgs e)
        {
            int[] res = new int[8];//sbyteToBin((sbyte)(binToDec(A) / binToDec(B)));
            int[] copyA = new int[8];
            int[] copyB = new int[8];

            A.CopyTo(copyA, 0);
            B.CopyTo(copyB, 0);
            int indA = 0;
            
            for (int i = 0; i < 8; i++)
            {
                if(copyA[i] == 1)
                {
                    indA = i;
                    break;
                }
            }
            
            int indB = 0;
            
            for (int i = 0; i < 8; i++)
            {
                if(copyB[i] == 1)
                {
                    indB = i;
                    break;
                }
            }


            res = copyA;
            int[]invB = invert(copyB);
            addOne(invB, 7);
            int[] O = new int[8];
            for (int i = 7; i >=0; i++)
            {
                copyA = unsdvig(copyA, 1);

                res = sdvig(res);

                res[7] = copyA[7];


            ///if ((res>=B)==true)//написать фнукцию сравнения массивов
            ///{
            ///    res = sum(res ,copyB);
            ///    O[7 - i] = 1; 
            ///}
            ///else
            ///{
            ///    O[7 - i] = 0; 
            ///}

            }


            string s = "";
            for (int i = 0; i < 8; i++)
            {
                s += res[i].ToString();
            }
            result.Content = s;
        }

        private int[] unsdvig(int[] n, int k)
        {
            for (int i = 7-k; i >= 0; i--)
            {
                n[i+k] = n[i];
                n[i] = 0;
            }

            return n;
        }
    }
}
