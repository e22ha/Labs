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

namespace _33.Пшы
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        GeoCoordinate geo;
        public MainWindow()
        {
            InitializeComponent();

            PointLatLng point = new PointLatLng(55.016511, 82.946152);






        }

        private void Map_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            PointLatLng point = Map.FromLocalToLatLng((int)e.GetPosition(Map).X, (int)e.GetPosition(Map).Y);
            xy_log.Content = "x: " + (int)e.GetPosition(Map).X + "; y:" + (int)e.GetPosition(Map).Y;

            MapObject obj = null;
            if ((bool)Car_ChB.IsChecked)
            {
                string title = "car";
                obj = new Car(title, point);
            }
            else if ((bool)Human_ChB.IsChecked) {
                string title = "Human";
                obj = new Human(title, point);
            }

            Map.Markers.Add(obj.getMarker());

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
            Map.DragButton = MouseButton.Left;
        }

        private void btn_loc_Click(object sender, RoutedEventArgs e)
        {
            GeoCoordinate geo = new GeoCoordinate();
            PointLatLng point = new PointLatLng(geo.Latitude, geo.Longitude);
            Map.Position = point;

        }
    }
}
