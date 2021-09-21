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

        List<Point2D> listPoint = new List<Point2D>();
        List<Triangle> listTri = new List<Triangle>();
        List<Rectangle> listRec = new List<Rectangle>();
        bool dotlast;
        bool trilast;
        bool reclast;

        public MainWindow()
        {
            InitializeComponent();
        }

        Random rnd = new Random();
        private void btn_point_Click(object sender, RoutedEventArgs e)
        {
            Point2D p = InitShape.create2DPoint();
            Ellipse ellipse = predrawpoint(p);
            listPoint.Add(p);
            x_slider.Value = p.getX();
            y_slider.Value = p.getY();
            //Canvas.Children.Add(ellipse);
        }

        private Ellipse predrawpoint(Point2D p)
        {
            SolidColorBrush rndbrash = new SolidColorBrush(Color.FromRgb((byte)rnd.Next(256), (byte)rnd.Next(256), (byte)rnd.Next(256)));
            Ellipse ellipse = new Ellipse
            {
                Fill = rndbrash,
                Stroke = rndbrash,
                StrokeThickness = 5,
                Margin = new Thickness(p.getX(), p.getY(), 0, 0)
            };

            log.Text = "Point \n" + "X = " + p.getX() + "\n" + "Y = " + p.getY() + "\n";
            dotlast = true;
            trilast = false;
            reclast = false;
            return ellipse;
        }

        private void btn_tri_Click(object sender, RoutedEventArgs e)
        {
            Triangle tri = InitShape.createRndTriangle();
            listTri.Add(tri);
            Polygon poly = predrawtri(tri);
            x_slider.Value = tri.getA().getX();
            y_slider.Value = tri.getA().getY();
            //Canvas.Children.Add(poly);
        }

        private Polygon predrawtri(Triangle tri)
        {
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

            log.Text = null;
            log.Text += "Triangle \n";
            log.Text += "Point A: \n" + "X = " + tri.getA().getX() + "\nY = " + tri.getA().getY() + "\n\r";
            log.Text += "Point B: \n" + "X = " + tri.getB().getX() + "\nY = " + tri.getB().getY() + "\n\r";
            log.Text += "Point C: \n" + "X = " + tri.getC().getX() + "\nY = " + tri.getC().getY() + "\n\r";
            log.Text += "Area: " + tri.getArea() + "\n";
            log.Text += "Perimeter: " + tri.getPerimeter();

            dotlast = false;
            trilast = true;
            reclast = false;
            return poly;
        }


        private void btn_recRnd_Click(object sender, RoutedEventArgs e)
        {
            Rectangle rec = InitShape.createRndRectangel();
            listRec.Add(rec);
            Polygon poly = predrawrec(rec);
            //Canvas.Children.Add(poly);
            x_slider.Value = (rec.getB().getX() - rec.getA().getX()) / 2 + rec.getA().getX();
            y_slider.Value = (rec.getD().getY() - rec.getA().getY()) / 2 + rec.getA().getY();
        }

        private Polygon predrawrec(Rectangle rec)
        {
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

            log.Text = null;
            log.Text += "Point A: \n" + "X = " + rec.getA().getX() + "\nY = " + rec.getA().getY() + "\n\r";
            log.Text += "Point B: \n" + "X = " + rec.getB().getX() + "\nY = " + rec.getB().getY() + "\n\r";
            log.Text += "Point C: \n" + "X = " + rec.getC().getX() + "\nY = " + rec.getC().getY() + "\n\r";
            log.Text += "Point D: \n" + "X = " + rec.getD().getX() + "\nY = " + rec.getD().getY() + "\n\r";
            log.Text += "Area: " + rec.getArea() + "\n";
            log.Text += "Perimeter: " + rec.getPerimeter();
            dotlast = false;
            trilast = false;
            reclast = true;
            return poly;
        }

        private void btn_rec_Click(object sender, RoutedEventArgs e)
        {
            Rectangle rec = InitShape.createRectangel(double.Parse(h_vaule.Text), double.Parse(w_vaule.Text));

            listRec.Add(rec);
            Polygon poly = predrawrec(rec);
            x_slider.Value = (rec.getB().getX() - rec.getA().getX()) / 2 + rec.getA().getX();
            y_slider.Value = (rec.getD().getY() - rec.getA().getY()) / 2 + rec.getA().getY();
        }
        private void btn_clean_Click(object sender, RoutedEventArgs e)
        {
            Canvas.Children.Clear();
            log.Text = null;
            listRec.Clear();
            listTri.Clear();
            listPoint.Clear();
            dotlast = false;
            trilast = false;
            reclast = false;
        }

        void shiftX(double value)
        {
            if (dotlast)
            {
                listPoint.Last().shiftX(value);

                drawAllRec();
                drawAllTri();
                drawAllPoint();
            }
            else if (trilast)
            {
                listTri.Last().shiftX(value);

                drawAllRec();
                drawAllPoint();
                drawAllTri();
            }
            else if (reclast)
            {
                listRec.Last().shiftX(value);

                drawAllPoint();
                drawAllTri();
                drawAllRec();
            }
        }
        void shiftY(double value)
        {
            if (dotlast)
            {
                listPoint.Last().shiftY(value);

                drawAllRec();
                drawAllTri();
                drawAllPoint();
            }
            else if (trilast)
            {
                listTri.Last().shiftY(value);

                drawAllRec();
                drawAllPoint();
                drawAllTri();
            }
            else if (reclast)
            {
                listRec.Last().shiftY(value);

                drawAllPoint();
                drawAllTri();
                drawAllRec();
            }

        }


        void drawAllTri()
        {
            foreach (Triangle tri in listTri)
            {
                Canvas.Children.Add(predrawtri(tri));
            }
        }

        void drawAllRec()
        {
            foreach (Rectangle rec in listRec)
            {
                Canvas.Children.Add(predrawrec(rec));
            }
        }

        void drawAllPoint()
        {
            foreach (Point2D point in listPoint)
            {
                Canvas.Children.Add(predrawpoint(point));
            }
        }

        private void y_slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            double shift = e.NewValue;
            Canvas.Children.Clear();
            shiftY(shift);

        }

        private void x_slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            double shift = e.NewValue;
            Canvas.Children.Clear();
            shiftX(shift);
        }
    }
}
