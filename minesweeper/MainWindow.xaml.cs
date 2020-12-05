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

namespace minesweeper
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        BitmapImage plate = new BitmapImage(new Uri(@"pack://application:,,,/Images/Default_but.png", UriKind.Absolute));
        BitmapImage mine = new BitmapImage(new Uri(@"pack://application:,,,/Images/Corona_plate.png", UriKind.Absolute));
        BitmapImage marker = new BitmapImage(new Uri(@"pack://application:,,,/Images/Marker_but.png", UriKind.Absolute));
        BitmapImage oplate = new BitmapImage(new Uri(@"pack://application:,,,/Images/Open_but.png", UriKind.Absolute));
        BitmapImage one = new BitmapImage(new Uri(@"pack://application:,,,/Images/One_plate.png", UriKind.Absolute));
        BitmapImage two = new BitmapImage(new Uri(@"pack://application:,,,/Images/Two_plate.png", UriKind.Absolute));
        BitmapImage three = new BitmapImage(new Uri(@"pack://application:,,,/Images/Three_plate.png", UriKind.Absolute));
        BitmapImage four = new BitmapImage(new Uri(@"pack://application:,,,/Images/4_plate.png", UriKind.Absolute));
        BitmapImage five = new BitmapImage(new Uri(@"pack://application:,,,/Images/5_plate.png", UriKind.Absolute));
        BitmapImage six = new BitmapImage(new Uri(@"pack://application:,,,/Images/6_plate.png", UriKind.Absolute));
        BitmapImage seven = new BitmapImage(new Uri(@"pack://application:,,,/Images/7_plate.png", UriKind.Absolute));
        BitmapImage eight = new BitmapImage(new Uri(@"pack://application:,,,/Images/8_plate.png", UriKind.Absolute));


        int n = 10;
        int _mine = 10;

        Button[,] btns;
        int[,] f;

        public MainWindow()
        {
            InitializeComponent();


        }

        private void Btn_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.RightButton == MouseButtonState.Pressed)
            {
                Image op = new Image();//ячейка картинки
                StackPanel stackPnl1 = new StackPanel();//привязка иконки к кнопке
                int i = (int)(((Button)sender).Tag) / n;
                int j = (int)((Button)sender).Tag % n;
                if (f[i, j] == 0)
                {
                    op.Source = oplate;//пркрепление картинки
                    stackPnl1.Children.Add(op);//добавить на кнопку
                }
                if (f[i, j] == 9)
                {
                    op.Source = mine;//пркрепление картинки
                    stackPnl1.Children.Add(op);//добавить на кнопку
                }
                if (f[i, j] == 1)
                {
                    op.Source = one;//пркрепление картинки
                    stackPnl1.Children.Add(op);//добавить на кнопку
                }
                if (f[i, j] == 2)
                {
                    op.Source = two;//пркрепление картинки
                    stackPnl1.Children.Add(op);//добавить на кнопку
                }
                if (f[i, j] == 3)
                {
                    op.Source = three;//пркрепление картинки
                    stackPnl1.Children.Add(op);//добавить на кнопку
                }
                if (f[i, j] == 4)
                {
                    op.Source = four;//пркрепление картинки
                    stackPnl1.Children.Add(op);//добавить на кнопку
                }
                if (f[i, j] == 5)
                {
                    op.Source = five;//пркрепление картинки
                    stackPnl1.Children.Add(op);//добавить на кнопку
                }
                if (f[i, j] == 6)
                {
                    op.Source = six;//пркрепление картинки
                    stackPnl1.Children.Add(op);//добавить на кнопку
                }
                if (f[i, j] == 7)
                {
                    op.Source = seven;//пркрепление картинки
                    stackPnl1.Children.Add(op);//добавить на кнопку
                }
                if (f[i, j] == 8)
                {
                    op.Source = eight;//пркрепление картинки
                    stackPnl1.Children.Add(op);//добавить на кнопку
                }

                ((Button)sender).Content = stackPnl1;
                ((Button)sender).IsEnabled = false;
            }

            if (e.MiddleButton == MouseButtonState.Pressed)
            {
                Image mrk = new Image();
                mrk.Source = marker;
                StackPanel stackPnl2 = new StackPanel();
                stackPnl2.Children.Add(mrk);
                ((Button)sender).Content = stackPnl2;
            }
        }


        private void NewGame_Click(object sender, RoutedEventArgs e)
        {
            f = logicField.generateField(n, _mine);

            btns = new Button[n, n];

            ugr.Rows = n;
            ugr.Columns = n;
            ugr.Width = n * (24);
            ugr.Height = n * (24);

            for (int x = 0; x < n; x++)
            {
                for (int y = 0; y < n; y++)
                {
                    btns[x, y] = new Button();
                    btns[x, y].Tag = x+y*n;
                    btns[x, y].Width = btns[x, y].Height = 24;

                    Image img = new Image();
                    img.Source = plate;
                    StackPanel stackPnl = new StackPanel();
                    stackPnl.Children.Add(img);

                    btns[x, y].Content = stackPnl;

                    btns[x, y].MouseDown += Btn_MouseDown;
                    ugr.Children.Add(btns[x, y]);
                }
            }

        }
    }
}

