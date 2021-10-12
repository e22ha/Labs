using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GMap.NET;
using GMap.NET.WindowsPresentation;

namespace _33.Пшы
{

    abstract class MapObject
    {
        private string title;

        private DateTime creationDate;

        protected MapObject(string title)
        {
            this.title = title;
            this.creationDate = DateTime.Now;
        }

        public string getTitle() { return this.title; }

        public DateTime getCreationDate() { return this.creationDate; }

        public abstract double getDistance(PointLatLng point);

        public abstract PointLatLng getFocus();

        public abstract GMapMarker getMarker();
    }
}
