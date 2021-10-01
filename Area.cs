using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;
using GMap.NET;
using GMap.NET.WindowsPresentation;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace _33.Пшы
{
    class Area : MapObject
    {

        private List<PointLatLng> points;

        public Area(string title, List<PointLatLng> points) : base(title)
        {
            this.points = points;
        }

        public override double getDistance(PointLatLng point)
        {
            throw new NotImplementedException();
        }

        public override PointLatLng getFocus()
        {
            PointLatLng center = new PointLatLng();

            center.Lat = 0;
            center.Lng = 0;

            //need to code for search center of Area

            return center;
        }

        public override GMapMarker getMarker()
        {
            GMapMarker markZone = new GMapPolygon(this.points)
            {
                Shape = new Path
                {
                    Stroke = Brushes.Black, // стиль обводки
                    Fill = Brushes.Violet, // стиль заливки
                    Opacity = 0.7 // прозрачность
                }
            };

            return markZone;
        }
    }
}
