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

        int n = 16;
        int _mine = 18;

        Button[,] btns;

        public MainWindow()
        {
            InitializeComponent();

            btns = new Button[n, n];

            ugr.Rows = n;
            ugr.Columns = n;
            ugr.Width = n * (24);
            ugr.Height = n * (24);

            for (int x = 0; x < n; x++)
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

        private void Btn_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.RightButton == MouseButtonState.Pressed)
            {
                Image op = new Image();
                op.Source = oplate;
                StackPanel stackPnl1 = new StackPanel();
                stackPnl1.Children.Add(op);
                ((Button)sender).Content = stackPnl1;
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

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
