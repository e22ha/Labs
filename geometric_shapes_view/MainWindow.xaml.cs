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

        Random rnd = new Random();
        private void btn_point_Click(object sender, RoutedEventArgs e)
        {
            Point2D p = InitShape.create2DPoint();
            SolidColorBrush rndbrash = new SolidColorBrush(Color.FromRgb((byte)rnd.Next(256), (byte)rnd.Next(256), (byte)rnd.Next(256)));
            Ellipse ellipse = new Ellipse
            {
                Fill = rndbrash,
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
            //Point2D p = Canvas.Children[i];
            //p.shiftY(int.Parse(varY.Text));
        }

        private void shiftX_point_Click(object sender, RoutedEventArgs e)
        {
            //p.shiftX(int.Parse(varX.Text));
        }

        private void btn_tri_Click(object sender, RoutedEventArgs e)
        {
            Triangle tri = InitShape.createRndTriangle();

            Polygon poly = new Polygon();
            Point pA = new Point(tri.getA().getX(), tri.getA().getY());
            poly.Points.Add(pA);
            Point pB = new Point(tri.getB().getX(), tri.getB().getY());
            poly.Points.Add(pB);
            Point pC = new Point(tri.getC().getX(), tri.getC().getY());
            poly.Points.Add(pC);

            SolidColorBrush rndbrash = new SolidColorBrush(Color.FromRgb((byte)rnd.Next(256), (byte)rnd.Next(256), (byte)rnd.Next(256)));
            poly.Fill = rndbrash;
            poly.Stroke = Brushes.Black;
            poly.Opacity = 0.5;
            poly.StrokeThickness = 3;

            Canvas.Children.Add(poly);

            log.Text = null;
            log.Text += "PointA: " + tri.getA().getX() + " " + tri.getA().getY() + "\n";
            log.Text += "PointB: " + tri.getB().getX() + " " + tri.getB().getY() + "\n";
            log.Text += "PointC: " + tri.getC().getX() + " " + tri.getC().getY() + "\n";
            log.Text += "Area: " + tri.getArea() + "\n";
            log.Text += "Perimeter: " + tri.getPerimeter();


        }

        private void btn_clean_Click(object sender, RoutedEventArgs e)
        {
            Canvas.Children.Clear();
            log.Text = null;
        }

        private void btn_recRnd_Click(object sender, RoutedEventArgs e)
        {
            Rectangle rec = InitShape.createRndRectangel();

            Polygon poly = new Polygon();
            Point pA = new Point(rec.getA().getX(), rec.getA().getY());
            poly.Points.Add(pA);
            Point pB = new Point(rec.getB().getX(), rec.getB().getY());
            poly.Points.Add(pB);
            Point pC = new Point(rec.getC().getX(), rec.getC().getY());
            poly.Points.Add(pC);
            Point pD = new Point(rec.getD().getX(), rec.getD().getY());
            poly.Points.Add(pD);



            SolidColorBrush rndbrash = new SolidColorBrush(Color.FromRgb((byte)rnd.Next(256), (byte)rnd.Next(256), (byte)rnd.Next(256)));
            poly.Fill = rndbrash;
            poly.Stroke = Brushes.Black;
            poly.Opacity = 0.5;
            poly.StrokeThickness = 5;

            Canvas.Children.Add(poly);
            log.Text = null;
            log.Text += "PointA: " + rec.getA().getX() + " " + rec.getA().getY() + "\n";
            log.Text += "PointB: " + rec.getB().getX() + " " + rec.getB().getY() + "\n";
            log.Text += "PointC: " + rec.getC().getX() + " " + rec.getC().getY() + "\n";
            log.Text += "PointD: " + rec.getD().getX() + " " + rec.getD().getY() + "\n";
            log.Text += "Area: " + rec.getArea();

        }


    }
}
