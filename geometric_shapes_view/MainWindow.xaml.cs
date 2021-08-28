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
            Point p = InitShape.create2DPoint();
            Ellipse ellipse = new Ellipse();
            ellipse.Fill = System.Windows.Media.Brushes.Black;
            ellipse.Stroke = System.Windows.Media.Brushes.Black;
            ellipse.StrokeThickness = 5;
            ellipse.Margin = new Thickness(p.X, p.Y, 0, 0);
            Canvas.Children.Add(ellipse);

            log.Text = p.X + "x;\n " + p.Y + "y;\n";


        }
    }
}
