using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using GMap.NET;
using GMap.NET.WindowsPresentation;

namespace _33.Пшы
{
    class Location : MapObject
    {
        PointLatLng point;

        public Location(string title, PointLatLng point) : base(title)
        {
            this.point = point;
        }

        public override double getDistance(PointLatLng point)
        {
            double dist;

            double lat = (double)Math.Abs(point.Lat - this.point.Lat);
            double lng = (double)Math.Abs(point.Lng - this.point.Lng);

            dist = Math.Sqrt(Math.Pow(lat, 2) + Math.Pow(lng, 2));

            return dist;
        }

        public override PointLatLng getFocus()
        {
            return point;
        }

        public override GMapMarker getMarker()
        {
            GMapMarker MarkLoc = new GMapMarker(point)
            {
                Shape = new Image
                {
                    Width = 32, // ширина маркера
                    Height = 32, // высота маркера
                    ToolTip = "Your location", // всплывающая подсказка
                    Source = new BitmapImage(new Uri("pack://application:,,,/Resources/location.png")) // картинка
                }
            };

            return MarkLoc;
        }
    }
}
