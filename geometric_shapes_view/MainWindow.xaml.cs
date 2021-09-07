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


namespace geometric_shapes_view
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

        private void btn_point_Click(object sender, RoutedEventArgs e)
        {
            Point2D p = InitShape.create2DPoint();
            Ellipse ellipse = new Ellipse
            {
                Fill = Brushes.Black,
                Stroke = Brushes.Black,
                StrokeThickness = 5,
                Margin = new Thickness(p.getX(), p.getY(), 0, 0)
            };
            Canvas.Children.Add(ellipse);

            log.Text = p.getX() + "x;\n " + p.getY() + "y;\n";


        }

        private void shiftY_point_Click(object sender, RoutedEventArgs e)
        {
            int i = Canvas.Children.Count;
            Point2D p = Canvas.Children[i];
            p.shiftY(int.Parse(varY.Text));
        }

        private void shiftX_point_Click(object sender, RoutedEventArgs e)
        {
            //p.shiftX(int.Parse(varX.Text));
        }
    }
}
