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


namespace _3d
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        double angle = 0.0;
        PerspectiveCamera camera;
        public MainWindow()
        {
            InitializeComponent();
            grd.Background = Brushes.LightGray;
            SetCamera();
            addTerrain();
        }
        public void SetCamera()
        {
            // создание объекта “камера”
            camera = new PerspectiveCamera();
            //установка позиции камеры
            camera.Position = new Point3D(0, 2, 0.1);
            
            //точка, на которую камера будет смотреть
            Vector3D lookAt = new Vector3D(0, 0, 0);
            //вычисление направления вектора камеры (можно задавать как вектор)
            camera.LookDirection = Vector3D.Subtract(lookAt, new Vector3D(0, 2, 0.1));
            
            camera.UpDirection = new Vector3D(0, 1, 0);

            //установка дальней и ближней плоскостей отсечения и вектора определяющего где верх
            camera.FarPlaneDistance = 1000;
            camera.NearPlaneDistance = 1;
            //угол обзора камеры
            camera.FieldOfView = 75;
            //установка камеры в сцену
            scene.Camera = camera;


        }

        void addTerrain()
        {
            //создание гкометрии
            MeshGeometry3D geometry = new MeshGeometry3D();
            //добавление координат вершин треугольника
            geometry.Positions.Add(new Point3D(-0.5, 0, -0.5));
            geometry.Positions.Add(new Point3D(-0.5, 0, 0.5));
            geometry.Positions.Add(new Point3D(0.5, 0, 0.5));
            //перечисление индексов вершин в порядке их соединения (против часовой стрелки)
            geometry.TriangleIndices.Add(0);
            geometry.TriangleIndices.Add(1);
            geometry.TriangleIndices.Add(2);
            //создание материала (тёмно синего цвета)
            DiffuseMaterial mat = new DiffuseMaterial(new SolidColorBrush(Colors.DarkBlue));
            //создание модели
            GeometryModel3D model = new GeometryModel3D(geometry, mat);
            //создание визуальной модели
            ModelVisual3D triangle = new ModelVisual3D();
            triangle.Content = model;
            //добавление модели в сцену
            scene.Children.Add(triangle);
        }

        private void scene_KeyDown(object sender, KeyEventArgs e)
        {
            ////если нажата стрелка влево
            //if (e.Key == Key.Left)
            //{
            //    angle--;
            //}
            ////если нажата стрелка вправо
            //if (e.Key == Key.Right)
            //{
            //    angle++;
            //}
            ////создание поворота вокруг оси Y на угол angle
            //AxisAngleRotation3D ax3d = new AxisAngleRotation3D(new Vector3D(0, 1, 0), angle);
            //RotateTransform3D rt = new RotateTransform3D(ax3d);
            ////создание переносов центра ландшафта в центр системы координат и обратно
            //TranslateTransform3D tr1 = new TranslateTransform3D(-N / 2, 0, -N / 2);
            //TranslateTransform3D tr2 = new TranslateTransform3D(N / 2, 0, N / 2);

            //Transform3DGroup tg = new Transform3DGroup();
            ////комбинирование преобразований
            //tg.Children.Add(tr1);
            //tg.Children.Add(rt);
            //tg.Children.Add(tr2);
            //terrain.Transform = tg;

        }
    }
}
