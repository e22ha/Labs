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
//пространство имён для работы с 3D
using System.Windows.Media.Media3D;
using Microsoft.Win32;
using System.IO;
using System.Windows.Threading;
using System.Media;

namespace _3d
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        double angle = 0.0;
        bool partychesker = false;
        GeometryModel3D model;
        ModelVisual3D terrain;
        DispatcherTimer timerDisco = new DispatcherTimer();
        //размер ландшафта (256х256 пикселей, как у карты высот)
        const int N = 256;
        ModelVisual3D light = new ModelVisual3D();
        Random rnd = new Random();
        public MainWindow()
        {
            InitializeComponent();
            grd.Background = Brushes.LightGray;
            SetCamera();
            addTerrain();
            addLight();
            timerDisco.Tick += TimerDisco_Tick;
            timerDisco.Interval = new TimeSpan(0, 0, 0, 0,100);
        }

        private void TimerDisco_Tick(object sender, EventArgs e)
        {
            scene.Children.Remove(light);

            PointLight pl1 = new PointLight();
            byte r = (byte)rnd.Next(155, 255);
            byte g = (byte)rnd.Next(50, 200);
            byte b = (byte)rnd.Next(50, 200);
            pl1.Color = Color.FromRgb(r, g, b);

            r_vaule.Content = "r=" + r.ToString();
            g_vaule.Content = "g=" + g.ToString();
            b_vaule.Content = "b=" + b.ToString();

            pl1.Position = new Point3D(N, N, N / 2);
            light.Content = null;
            light.Content = pl1;

            scene.Children.Add(light);


            angle++;

            //создание поворота вокруг оси Y на угол angle
            AxisAngleRotation3D ax3d = new AxisAngleRotation3D(new Vector3D(0, 1, 0), angle);
            RotateTransform3D rt = new RotateTransform3D(ax3d);
            //создание переносов центра ландшафта в центр системы координат и обратно
            TranslateTransform3D tr1 = new TranslateTransform3D(-N / 2, 0, -N / 2);
            TranslateTransform3D tr2 = new TranslateTransform3D(N / 2, 0, N / 2);

            Transform3DGroup tg = new Transform3DGroup();
            //комбинирование преобразований
            tg.Children.Add(tr1);
            tg.Children.Add(rt);
            tg.Children.Add(tr2);
            terrain.Transform = tg;
        }

        public void SetCamera()
        {
            PerspectiveCamera camera = new PerspectiveCamera();
            //расположение камеры
            camera.Position = new Point3D(N / 2, N / 2, N * 1.5);

            //точка, на которую направлена камера (центр ландшафта)
            Vector3D lookAt = new Vector3D(N / 2, 0, N / 2);
            camera.LookDirection = Vector3D.Subtract(lookAt, new Vector3D(N / 2, N / 2, N * 2));

            camera.FarPlaneDistance = 1000;
            camera.NearPlaneDistance = 1;
            camera.UpDirection = new Vector3D(0, 1, 0);
            camera.FieldOfView = 90;

            scene.Camera = camera;

            //// создание объекта “камера”
            //camera = new PerspectiveCamera();
            ////установка позиции камеры
            //camera.Position = new Point3D(0, 2, 0.1);

            ////точка, на которую камера будет смотреть
            //Vector3D lookAt = new Vector3D(0, 0, 0);
            ////вычисление направления вектора камеры (можно задавать как вектор)
            //camera.LookDirection = Vector3D.Subtract(lookAt, new Vector3D(0, 2, 0.1));

            //camera.UpDirection = new Vector3D(0, 1, 0);

            ////установка дальней и ближней плоскостей отсечения и вектора определяющего где верх
            //camera.FarPlaneDistance = 1000;
            //camera.NearPlaneDistance = 1;
            ////угол обзора камеры
            //camera.FieldOfView = 75;
            ////установка камеры в сцену
            //scene.Camera = camera;
        }

        void addLight()
        {

            PointLight pl = new PointLight();
            pl.Color = Colors.LightYellow;

            pl.Position = new Point3D(N, N, N / 2);

            light.Content = pl;

            scene.Children.Add(light);

            ////создание точечного источника освещения
            //PointLight pl = new PointLight();
            ////установка цвета света
            //pl.Color = Colors.LightYellow;
            ////установка позиция источника
            //pl.Position = new Point3D(0, 5, -5);
            ////создание модели описывающей источник в сцене
            //ModelVisual3D light = new ModelVisual3D();
            //light.Content = pl;
            ////добавление источника в сцену
            //scene.Children.Add(light);
        }

        void addTerrain()
        {
            System.Drawing.Bitmap hMap;

            hMap = new System.Drawing.Bitmap("D:\\Code\\LABs_2\\3d\\pic\\plateau.jpg");

            //модель для отображения ландшафта
            terrain = new ModelVisual3D();
            MeshGeometry3D geometry = new MeshGeometry3D();

            for (int i = 0; i < N; i++)
                for (int j = 0; j < N; j++)
                {
                    //расстановка точек ландшафта
                    double y = hMap.GetPixel(i, j).R / 3.0;
                    geometry.Positions.Add(new Point3D(i, y, j));
                    //вычисление текстурных координат для точек ландшафта
                    double tu = i / Convert.ToDouble(N);
                    double tv = j / Convert.ToDouble(N);
                    geometry.TextureCoordinates.Add(new Point(tu, tv));
                }

            for (int i = 0; i < N - 1; i++)
                for (int j = 0; j < N - 1; j++)
                {
                    //вычисление индексов 4х точек, находящихся рядом
                    int ind0 = i + j * N;
                    int ind1 = (i + 1) + j * N;
                    int ind2 = i + (j + 1) * N;
                    int ind3 = (i + 1) + (j + 1) * N;
                    //описание первого треугольника
                    geometry.TriangleIndices.Add(ind0);
                    geometry.TriangleIndices.Add(ind1);
                    geometry.TriangleIndices.Add(ind3);
                    //описание второго треугольника
                    geometry.TriangleIndices.Add(ind0);
                    geometry.TriangleIndices.Add(ind3);
                    geometry.TriangleIndices.Add(ind2);
                }

            ImageBrush ib = new ImageBrush();
            //загрузка изображения и назначение кисти
            ib.ImageSource = new BitmapImage(new Uri(@"pack://application:,,,/pic/grasstile.jpg",
            UriKind.Absolute));
            //масштабирование кисти, текстура будет повторена 4 раза по поверхности ландшафта
            ib.Transform = new ScaleTransform(0.5, 0.5);
            ib.TileMode = TileMode.Tile;
            ib.Stretch = Stretch.Fill;
            //создание материала
            DiffuseMaterial mat = new DiffuseMaterial(ib);
            //создание модели
            GeometryModel3D model = new GeometryModel3D(geometry, mat);
            //установка модели в визуальный объект
            terrain.Content = model;
            //добавление визуального объекта в сцену
            scene.Children.Add(terrain);

            ////создание гкометрии
            //MeshGeometry3D geometry = new MeshGeometry3D();
            ////добавление координат вершин треугольника
            //geometry.Positions.Add(new Point3D(-0.5, 0, -0.5));
            //geometry.Positions.Add(new Point3D(-0.5, 0, 0.5));
            //geometry.Positions.Add(new Point3D(0.5, 0, 0.5));
            ////перечисление индексов вершин в порядке их соединения (против часовой стрелки)
            //geometry.TriangleIndices.Add(0);
            //geometry.TriangleIndices.Add(1);
            //geometry.TriangleIndices.Add(2);

            ////добавление текстурных координат
            //geometry.TextureCoordinates.Add(new Point(1, 0));
            //geometry.TextureCoordinates.Add(new Point(1, 1));
            //geometry.TextureCoordinates.Add(new Point(0, 1));

            ////создание кисти
            //ImageBrush ib = new ImageBrush();
            ////загрузка изображения и назначение кисти
            //ib.ImageSource = new BitmapImage(new Uri(@"pack://application:,,,/pic/yachik3.jpg", UriKind.Absolute));
            ////создание материала
            //DiffuseMaterial mat = new DiffuseMaterial(ib);

            ////создание материала (тёмно синего цвета)
            ////DiffuseMaterial mat = new DiffuseMaterial(new SolidColorBrush(Colors.DarkBlue));

            ////создание модели
            //model = new GeometryModel3D(geometry, mat);
            ////создание визуальной модели
            //ModelVisual3D triangle = new ModelVisual3D();
            //triangle.Content = model;
            ////добавление модели в сцену
            //scene.Children.Add(triangle);
        }

        private void scene_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            //если нажата стрелка влево
            if (e.Key == Key.Left)
            {
                angle--;
            }
            //если нажата стрелка вправо
            if (e.Key == Key.Right)
            {
                angle++;
            }
            //создание поворота вокруг оси Y на угол angle
            AxisAngleRotation3D ax3d = new AxisAngleRotation3D(new Vector3D(0, 1, 0), angle);
            RotateTransform3D rt = new RotateTransform3D(ax3d);
            //создание переносов центра ландшафта в центр системы координат и обратно
            TranslateTransform3D tr1 = new TranslateTransform3D(-N / 2, 0, -N / 2);
            TranslateTransform3D tr2 = new TranslateTransform3D(N / 2, 0, N / 2);

            Transform3DGroup tg = new Transform3DGroup();
            //комбинирование преобразований
            tg.Children.Add(tr1);
            tg.Children.Add(rt);
            tg.Children.Add(tr2);
            terrain.Transform = tg;

            ////преобразование переноса модели в точку с координатами 10:0:10
            //TranslateTransform3D tr = new TranslateTransform3D(10, 0, 10);
            ////преобразование масштабирования модели (уменьшение в 2 раза по всем осям)
            //ScaleTransform3D sc = new ScaleTransform3D(0.5, 0.5, 0.5);
            ////преобразование вращения модели вокруг оси Y на 30 градусов
            //AxisAngleRotation3D ax3d = new AxisAngleRotation3D(new Vector3D(0, 1, 0), 30);
            //RotateTransform3D rt = new RotateTransform3D(ax3d);
            ////создание группы преобразований
            //Transform3DGroup tg = new Transform3DGroup();

            ////последовательное применение преобразований (масштабирование, вращение, перенос)
            //tg.Children.Add(sc);
            //tg.Children.Add(rt);
            //tg.Children.Add(tr);

            ////назначение преобразований модели
            //model.Transform = rt;
        }

        private void disco_Click(object sender, RoutedEventArgs e)
        {
            if (!partychesker)
            {
                timerDisco.Start();
                partychesker = true;
                load();
                sp.PlayLooping();
            }
            else if (partychesker)
            {
                timerDisco.Stop();
                partychesker = false;
                sp.Stop();
            }
        }

            SoundPlayer sp = new SoundPlayer();
        void load()
        {
            sp = new SoundPlayer(@"D:\\Code\\LABs_2\\3d\\pic\\Rick Astley - Never Gonna Give You Up (Video)_1.wav");
            //загрузка файла
            sp.Load();
            
        }
    }
}
