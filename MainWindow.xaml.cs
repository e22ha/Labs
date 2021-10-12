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
        Dictionary<MapObject, double> distDict = new Dictionary<MapObject, double>();
        Dictionary<MapObject, double> sortDistDict = new Dictionary<MapObject, double>();
        List<MapObject> listOfAllObj = new List<MapObject>();
        int setTool; // 0 - arrow; 1 - car; 2 - human; 3 - route; 4 - area; 5 - search obj&dist; 
        string searchName = "";
        
        public MainWindow()
        {
            InitializeComponent();
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
                    int lastCar = listOfAllObj.FindAll(FindCar).Count();
                    Car car = new Car($"Car {lastCar + 1}", point);
                    Map.Markers.Add(car.getMarker());
                    listOfAllObj.Add(car);
                    lb_objectOnMap.Items.Add(car.getTitle());
                    break;
                case 2:
                    int lastHu = listOfAllObj.FindAll(FindHu).Count();
                    Human human = new Human($"Human {lastHu + 1}", point);
                    Map.Markers.Add(human.getMarker());
                    listOfAllObj.Add(human);
                    lb_objectOnMap.Items.Add(human.getTitle());
                    break;
                case 3:
                    pointsRoute.Add(point);
                    if (pointsRoute.Count < 2) break;
                    DialogResult result = System.Windows.Forms.MessageBox.Show(
                         "Добавить ещё?",
                         "Сообщение",
                         MessageBoxButtons.YesNo,
                         MessageBoxIcon.Information,
                         MessageBoxDefaultButton.Button2);
                    if (result == System.Windows.Forms.DialogResult.No)
                    {
                        int lastRoute = listOfAllObj.FindAll(FindRo).Count();
                        Route route = new Route($"Route  {lastRoute + 1}", pointsRoute);
                        Map.Markers.Add(route.getMarker());
                        listOfAllObj.Add(route);
                        lb_objectOnMap.Items.Add(route.getTitle());
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
                         MessageBoxIcon.Information,
                         MessageBoxDefaultButton.Button2);
                    if (result1 == System.Windows.Forms.DialogResult.No)
                    {
                        int lastArea = listOfAllObj.FindAll(FindArea).Count();
                        Area area = new Area($"Area  {lastArea + 1}", pointsArea);
                        Map.Markers.Add(area.getMarker());
                        listOfAllObj.Add(area);
                        lb_objectOnMap.Items.Add(area.getTitle());
                        setTool = 0;
                    }
                    else { setTool = 4; }
                    break;
                case 5:
                    lb_searchItmesByDist.Items.Clear();
                    foreach (MapObject obj in listOfAllObj)
                    {
                        distDict.Add(obj, obj.getDistance(point));
                    }
                    sortDistDict = distDict.OrderBy(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
                    foreach (MapObject obj in sortDistDict.Keys)
                    {
                        lb_searchItmesByDist.Items.Add(obj.getTitle() + " " + obj.getDistance(point));
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
            distDict = new Dictionary<MapObject, double>();
            setTool = 5;
        }

        private void btn_back_Click(object sender, RoutedEventArgs e)
        {
            lb_searchItmesByDist.Items.Clear();
            Map.Markers.RemoveAt(Map.Markers.Count() - 1);
            listOfAllObj.RemoveAt(listOfAllObj.Count() - 1);
            lb_objectOnMap.Items.RemoveAt(lb_objectOnMap.Items.Count - 1);
        }

        private void ListofObj_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lb_objectOnMap.SelectedIndex == -1) return;
            searchName = lb_objectOnMap.SelectedItem.ToString();
            Map.Position = listOfAllObj.Find(FindByName).getFocus();
            lb_objectOnMap.SelectedIndex = -1;
        }

        private bool FindByName(MapObject obj)
        {
            if (obj.getTitle().StartsWith(searchName))
            {
                return true;
            }
            return false;
        }

        private void ListSearch_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lb_searchItmesByDist.SelectedIndex == -1) return;

            Map.Position = sortDistDict.ElementAt(lb_searchItmesByDist.SelectedIndex).Key.getFocus();
        }

        private void btn_srchByName_Click(object sender, RoutedEventArgs e)
        {
            lb_searchItemsByName.Items.Clear();
            string nameSearch = tb_Search.Text;
            foreach (MapObject obj in listOfAllObj)
            {
                if (obj.getTitle().StartsWith(nameSearch))
                {
                    lb_searchItemsByName.Items.Add(obj.getTitle());
                }
            }
        }

        private void ListSearchByName_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lb_searchItemsByName.SelectedIndex == -1) return;
            searchName = lb_searchItemsByName.SelectedItem.ToString();
            Map.Position = listOfAllObj.Find(FindByName).getFocus();
            lb_objectOnMap.SelectedIndex = -1;
        }
    }
}
