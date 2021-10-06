using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Shapes;
using GMap.NET;
using GMap.NET.WindowsPresentation;

namespace _33.Пшы
{
    class Route : MapObject
    {

        private List<PointLatLng> points;

        public Route(string title, List<PointLatLng> points) : base(title)
        {
            this.points = points;
        }

        public override double getDistance(PointLatLng point)
        {
            double dist;

            double lat = (double)Math.Abs(point.Lat - getFocus().Lat);
            double lng = (double)Math.Abs(point.Lng - getFocus().Lng);

            dist = Math.Sqrt(Math.Pow(lat, 2) + Math.Pow(lng, 2));

            return dist;
        }

        public override PointLatLng getFocus()
        {
            PointLatLng max = new PointLatLng();

            foreach (var p in points)
            {
                if (max.Lat+max.Lng <p.Lat+p.Lng)
                {
                    max = p;
                }
            }

            PointLatLng min = new PointLatLng(180,180);

            foreach (var p in points)
            {
                if (min.Lat + min.Lng > p.Lat + p.Lng)
                {
                    min = p;
                }
            }

            return new PointLatLng((max.Lat + min.Lat)/2, (max.Lng + min.Lng)/2);
        }


        public override GMapMarker getMarker()
        {
            GMapMarker markPath = new GMapRoute(this.points)
            {
                Shape = new Path()
                {
                    Stroke = Brushes.DarkBlue, // цвет обводки
                    Fill = Brushes.DarkBlue, // цвет заливки
                    ToolTip = this.getTitle(),
                    StrokeThickness = 4 // толщина обводки
                }
            };

            return markPath;

        }
    }
}
