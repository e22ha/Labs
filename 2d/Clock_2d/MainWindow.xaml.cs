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
using System.Windows.Threading;

namespace Clock_2d
{
    public partial class MainWindow : Window
    {
        DispatcherTimer timer = new DispatcherTimer();
        public MainWindow()
        {
            InitializeComponent();
            timer.Tick += Timer_Tick;
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.Start();
        }



        private void drawLine()
        {
            //создание объекта линия
            Line myLine = new Line();
            //установка цвета линии
            myLine.Stroke = System.Windows.Media.Brushes.Black;
            //координаты начала линии
            myLine.X1 = 1;
            myLine.Y1 = 1;
            //координаты конца линии
            myLine.X2 = 50;
            myLine.Y2 = 50;
            //параметры выравнивания в сцене
            myLine.HorizontalAlignment = HorizontalAlignment.Left;
            myLine.VerticalAlignment = VerticalAlignment.Center;
            //толщина линии
            myLine.StrokeThickness = 2;
            //добавление линии в сцену
            scene.Children.Add(myLine);
        }

        private void drawEllipse()
        {
            // создание объекта овал
            Ellipse myEllipse = new Ellipse();
            //создание объекта кисть
            SolidColorBrush mySolidColorBrush = new SolidColorBrush();
            //установка цвета в виде сочетания компонент ARGB (alpha, red, green, blue)
            mySolidColorBrush.Color = Color.FromArgb(255, 255, 255, 0);
            //установка объекта кисти в параметр заливки объекта овал
            myEllipse.Fill = mySolidColorBrush;
            //толщина и цвет обводки
            myEllipse.StrokeThickness = 2;
            myEllipse.Stroke = Brushes.Black;
            //размеры овала
            myEllipse.Width = 100;
            myEllipse.Height = 100;
            //позиция овала
            myEllipse.Margin = new Thickness(50, 50, 0, 0);
            //добавление овала в сцену
            scene.Children.Add(myEllipse);

        }

        private void drawRect()
        {
            //создание объекта прямоугольник
            Rectangle myRect = new Rectangle();
            //установка цвета линии обводки и цвета заливки при помощи коллекции кистей
            myRect.Stroke = Brushes.Black;
            myRect.Fill = Brushes.SkyBlue;
            //параметры выравнивания
            myRect.HorizontalAlignment = HorizontalAlignment.Left;
            myRect.VerticalAlignment = VerticalAlignment.Center;
            //размеры прямоугольника
            myRect.Height = 50;
            myRect.Width = 50;
            //добавление объекта в сцену
            scene.Children.Add(myRect);

        }

        private void drawPoly()
        {
            //создание объекта многоугольник
            Polygon myPolygon = new Polygon();
            //установка цвета обводки, цвета заливки и толщины обводки
            myPolygon.Stroke = Brushes.Black;
            myPolygon.Fill = Brushes.LightSeaGreen;
            myPolygon.StrokeThickness = 2;
            //позиционирование объекта
            myPolygon.HorizontalAlignment = HorizontalAlignment.Left;
            myPolygon.VerticalAlignment = VerticalAlignment.Center;
            //создание точек многоугольника
            Point Point1 = new Point(0, 0);
            Point Point2 = new Point(100, 0);
            Point Point3 = new Point(100, 50);
            Point Point4 = new Point(50, 100);
            Point Point5 = new Point(0, 50);
            //создание и заполнение коллекции точек
            PointCollection myPointCollection = new PointCollection();
            myPointCollection.Add(Point1);
            myPointCollection.Add(Point2);
            myPointCollection.Add(Point3);
            myPointCollection.Add(Point4);
            myPointCollection.Add(Point5);
            //установка коллекции точек в объект многоугольник
            myPolygon.Points = myPointCollection;
            //добавление многоугольника в сцену
            scene.Children.Add(myPolygon);

        }

        private void drawPath()
        {
            //создание объекта путь и установка параметров его отрисовки
            Path path = new Path();
            path.Stroke = Brushes.Black;
            path.StrokeThickness = 1;
            //создание двух сегментов пути при помощи кривых Безье
            //параметры - (первая контрольная точка, вторая контрольная точка, конец кривой)
            BezierSegment bezierCurve1 = new BezierSegment(new Point(0, 0), new Point(0, 50), new Point(50, 90),
            true);
            BezierSegment bezierCurve2 = new BezierSegment(new Point(100, 50), new Point(100, 0), new Point(50,
            30), true);
            //создание коллекции сегментов и добавление к ней кривых
            PathSegmentCollection psc = new PathSegmentCollection();
            psc.Add(bezierCurve1);
            psc.Add(bezierCurve2);
            //создание объекта фигуры и установка начальной точки пути
            PathFigure pf = new PathFigure();
            pf.Segments = psc;
            pf.StartPoint = new Point(50, 30);
            //создание коллекции фигур
            PathFigureCollection pfc = new PathFigureCollection();
            pfc.Add(pf);
            //создание геометрии пути
            PathGeometry pg = new PathGeometry();
            pg.Figures = pfc;
            //создание набора геометрий
            GeometryGroup pathGeometryGroup = new GeometryGroup();
            pathGeometryGroup.Children.Add(pg);
            //
            path.Data = pathGeometryGroup;
            //добавление объекта путь в сцену
            scene.Children.Add(path);

        }

        private void drLi_Click(object sender, RoutedEventArgs e)
        {
            scene.Children.Clear();
            drawLine();
        }

        private void drEll_Click(object sender, RoutedEventArgs e)
        {
            scene.Children.Clear();
            drawEllipse();
        }

        private void drRec_Click(object sender, RoutedEventArgs e)
        {
            scene.Children.Clear();
            drawRect();
        }

        private void drPoly_Click(object sender, RoutedEventArgs e)
        {
            scene.Children.Clear();
            drawPoly();
        }

        private void drPath_Click(object sender, RoutedEventArgs e)
        {
            scene.Children.Clear();
            drawPath();
        }

        private void drClock()
        {
            // создание объекта овал
            Ellipse myEllipse = new Ellipse();
            //кисть для заполнения прямоугольника изображением
            ImageBrush ib = new ImageBrush();
            //позиция изображения будет указана как координаты левого верхнего угла
            //изображение будет растянуто по размерам прямоугольника, описанного вокруг фигуры
            ib.AlignmentX = AlignmentX.Left;
            ib.AlignmentY = AlignmentY.Top;
            //загрузка изображения и назначение кисти
            ib.ImageSource = new BitmapImage(new Uri(@"pack://application:,,,/Pic/clock_form.jpg", UriKind.Absolute));
            myEllipse.Fill = ib;
            //толщина и цвет обводки
            myEllipse.StrokeThickness = 2;
            myEllipse.Stroke = Brushes.Black;
            //размеры овала
            myEllipse.Width = 200;
            myEllipse.Height = 200;
            //позиция овала
            myEllipse.Margin = new Thickness(100, 50, 0, 0);
            //добавление овала в сцену
            scene.Children.Add(myEllipse);

            //создание объекта линия
            Line lineOfHour = new Line();
            //установка цвета линии
            lineOfHour.Stroke = System.Windows.Media.Brushes.DimGray;
            //координаты начала линии
            lineOfHour.X1 = 200;
            lineOfHour.Y1 = 90;
            //координаты конца линии
            lineOfHour.X2 = 200;
            lineOfHour.Y2 = 150;
            //параметры выравнивания в сцене
            //myLine.HorizontalAlignment = HorizontalAlignment.Center;
            //myLine.VerticalAlignment = VerticalAlignment.Center;
            //толщина линии
            lineOfHour.StrokeThickness = 10;
            //добавление линии в сцену
            scene.Children.Add(lineOfHour);

            //создание объекта линия
            Line lineOfMin = new Line();
            //установка цвета линии
            lineOfMin.Stroke = System.Windows.Media.Brushes.Blue;
            //координаты начала линии
            lineOfMin.X1 = 200;
            lineOfMin.Y1 = 62;
            //координаты конца линии
            lineOfMin.X2 = 200;
            lineOfMin.Y2 = 150;
            //параметры выравнивания в сцене
            //myLine.HorizontalAlignment = HorizontalAlignment.Center;
            //myLine.VerticalAlignment = VerticalAlignment.Center;
            //толщина линии
            lineOfMin.StrokeThickness = 6;
            //добавление линии в сцену
            scene.Children.Add(lineOfMin);

            //создание объекта линия
            Line lineOfSec = new Line
            {
                //установка цвета линии
                Stroke = System.Windows.Media.Brushes.Red,
                //координаты начала линии
                X1 = 200,
                Y1 = 52,
                //координаты конца линии
                X2 = 200,
                Y2 = 150,
                //параметры выравнивания в сцене
                //myLine.HorizontalAlignment = HorizontalAlignment.Center;
                //myLine.VerticalAlignment = VerticalAlignment.Center;
                //толщина линии
                StrokeThickness = 3
            };
            //добавление линии в сцену
            scene.Children.Add(lineOfSec);
        }

        private void Clock_Click(object sender, RoutedEventArgs e)
        {
            scene.Children.Clear();
            drClock();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            lineOfSec.
        }

    }
}
