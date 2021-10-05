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
using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.WindowsPresentation;
using System.Device.Location;
using System.Windows.Forms;

namespace _33.Пшы
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        List<PointLatLng> pointsArea = new List<PointLatLng>();
        List<PointLatLng> pointsRoute = new List<PointLatLng>();
        Dictionary<MapObject, double> DictObjDist = new Dictionary<MapObject, double>();
        List<MapObject> ListOfAll = new List<MapObject>();
        int setTool; // 0 - 
        public MainWindow()
        {
            InitializeComponent();

            PointLatLng point = new PointLatLng(55.016511, 82.946152);


        }

        private void Map_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            PointLatLng point = Map.FromLocalToLatLng((int)e.GetPosition(Map).X, (int)e.GetPosition(Map).Y);
            xy_log.Content = "x: " + (int)e.GetPosition(Map).X + "; y:" + (int)e.GetPosition(Map).Y;

            switch (setTool)
            {
                case 0:

                    break;
                case 1:
                    int lastCar = ListOfAll.FindAll(FindCar).Count();
                    Car car = new Car($"Car {lastCar + 1}", point);
                    Map.Markers.Add(car.getMarker());
                    ListOfAll.Add(car);
                    ListofObj.Items.Add(car.getTitle());
                    break;
                case 2:
                    int lastHu = ListOfAll.FindAll(FindHu).Count();
                    Human human = new Human($"Human {lastHu + 1}", point);
                    Map.Markers.Add(human.getMarker());
                    ListOfAll.Add(human);
                    ListofObj.Items.Add(human.getTitle());
                    break;
                case 3:
                    pointsRoute.Add(point);
                    if (pointsRoute.Count < 2) break;
                    DialogResult result = System.Windows.Forms.MessageBox.Show(
                         "Добавить ещё?",
                         "Сообщение",
                         MessageBoxButtons.YesNo,
                         MessageBoxIcon.Information);
                    if (result == System.Windows.Forms.DialogResult.No)
                    {
                        int lastRoute = ListOfAll.FindAll(FindRo).Count();
                        Route route = new Route($"Route  {lastRoute + 1}", pointsRoute);
                        Map.Markers.Add(route.getMarker());
                        ListOfAll.Add(route);
                        ListofObj.Items.Add(route.getTitle());
                        setTool = 0;

                    }
                    else { setTool = 3; }

                    break;

                case 4:
                    pointsArea.Add(point);
                    if (pointsArea.Count < 3) break;
                    DialogResult result1 = System.Windows.Forms.MessageBox.Show(
                         "Добавить ещё?",
                         "Сообщение",
                         MessageBoxButtons.YesNo,
                         MessageBoxIcon.Information);
                    if (result1 == System.Windows.Forms.DialogResult.No)
                    {
                        int lastArea = ListOfAll.FindAll(FindArea).Count();
                        Area area = new Area($"Area  {lastArea + 1}", pointsArea);
                        Map.Markers.Add(area.getMarker());
                        ListOfAll.Add(area);
                        ListofObj.Items.Add(area.getTitle());
                        setTool = 0;
                    }
                    else { setTool = 4; }
                    break;

                case 5:

                    DictObjDist = new Dictionary<MapObject, double>();

                    foreach (MapObject obj in ListOfAll)
                    {
                        DictObjDist.Add(
                        obj,
                        obj.getDistance(point));

                    }

                    var items = from pair in DictObjDist
                                orderby pair.Value ascending,
                                        pair.Key
                                select pair;

                    foreach (MapObject obj in ListOfAll)
                    {
                        ListSearch.Items.Add(obj.getTitle());
                    }

                    break;

                default:
                    break;
            }


        }

        private bool FindRo(MapObject obj)
        {
            return obj.getTitle().StartsWith("R");
        }
        private bool FindArea(MapObject obj)
        {
            return obj.getTitle().StartsWith("A");
        }

        private bool FindHu(MapObject obj)
        {
            return obj.getTitle().StartsWith("H");
        }

        private bool FindCar(MapObject obj)
        {
            return obj.getTitle().StartsWith("C");
        }

        private void MapLoaded(object sender, RoutedEventArgs e)
        {
            // настройка доступа к данным
            GMaps.Instance.Mode = AccessMode.ServerAndCache;

            // установка провайдера карт
            Map.MapProvider = GoogleMapProvider.Instance;

            // установка зума карты
            Map.MinZoom = 2;
            Map.MaxZoom = 17;
            Map.Zoom = 15;
            // установка фокуса карты
            Map.Position = new PointLatLng(55.012823, 82.950359);
            // настройка взаимодействия с картой
            Map.MouseWheelZoomType = MouseWheelZoomType.MousePositionAndCenter;
            Map.CanDragMap = true;
            Map.DragButton = MouseButton.Middle;
        }

        private void btn_loc_Click(object sender, RoutedEventArgs e)
        {
            Map.Position = new PointLatLng(55.012823, 82.950359);
        }

        private void btn_arrow_Click(object sender, RoutedEventArgs e)
        {
            setTool = 0;
        }

        private void btn_car_Click(object sender, RoutedEventArgs e)
        {
            setTool = 1;
        }

        private void btn_human_Click(object sender, RoutedEventArgs e)
        {
            setTool = 2;
        }

        private void btn_route_Click(object sender, RoutedEventArgs e)
        {
            pointsRoute = new List<PointLatLng>();
            setTool = 3;
        }

        private void btn_area_Click(object sender, RoutedEventArgs e)
        {
            pointsArea = new List<PointLatLng>();
            setTool = 4;
        }
        private void btn_search_Click(object sender, RoutedEventArgs e)
        {
            setTool = 5;
        }

        private void btn_back_Click(object sender, RoutedEventArgs e)
        {
            Map.Markers.RemoveAt(Map.Markers.Count() - 1);
            ListOfAll.RemoveAt(ListOfAll.Count() - 1);
            ListofObj.Items.RemoveAt(ListofObj.Items.Count - 1);

        }

        private void ListofObj_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ListofObj.SelectedIndex == -1) return;
            Map.Position = ListOfAll.ElementAt(ListofObj.SelectedIndex).getFocus();
            ListofObj.SelectedIndex = -1;
        }

        private void ListSearch_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ListSearch.SelectedIndex == -1) return;

            Map.Position = DictObjDist.ElementAt(ListSearch.SelectedIndex).Key.getFocus();
            

            
        }
    }
}
