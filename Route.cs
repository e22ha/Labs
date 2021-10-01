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

        List<PointLatLng> points;


        Route(string title, List<PointLatLng> points): base(title){ 
        }

        public override double getDistance(PointLatLng point)
        {
            throw new NotImplementedException();
        }

        public override PointLatLng getFocus()
        {
            throw new NotImplementedException();
        }

        public override GMapMarker getMarker()
        {
            // координаты точек маршрута
            List<PointLatLng> points1 = new PointLatLng[] {
                    new PointLatLng(55.010637, 82.938550),
                    new PointLatLng(55.012421, 82.940781),
                    new PointLatLng(55.014613, 82.943497),
                    new PointLatLng(55.016214, 82.945469) }.ToList();
            GMapMarker markPath = new GMapRoute(points1)
            {
                Shape = new Path()
                {
                    Stroke = Brushes.DarkBlue, // цвет обводки
                    Fill = Brushes.DarkBlue, // цвет заливки
                    StrokeThickness = 4 // толщина обводки
                }
            };

            return markPath;

        }
    }
}
