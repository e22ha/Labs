using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GMap.NET;

namespace _33.Пшы
{
    class DistanceCalculator
    {
        private const double EARTH_RADIUS = 6378137;

        /// <summary>
        /// Нахождение минимального расстояния от точки до сегмента
        /// </summary>
        /// <param name="start">координаты начала сегмента</param>
        /// <param name="end">координаты конца сегмента</param>
        /// <param name="point">координаты точки</param>
        /// <returns>Минимальное расстояние от точки до сегмента в метрах</returns>
        public double GetMinDistance(PointLatLng start, PointLatLng end, PointLatLng point)
        {
            double[] a = new double[] { start.Lat, start.Lng };
            double[] b = new double[] { end.Lat, end.Lng };
            double[] c = new double[] { point.Lat, point.Lng };
            double[] nearestPoint = nearestPointGreatCircle(a, b, c);
            return haversine(c[0], c[1], nearestPoint[0], nearestPoint[1]);
        }

        private double haversine(double lat1, double lng1, double lat2, double lng2)
        {
            double dlong = degreesToRadians(lng2 - lng1);
            double dlat = degreesToRadians(lat2 - lat1);
            double a = Math.Pow(Math.Sin(dlat / 2), 2) + Math.Cos(degreesToRadians(lat1)) * Math.Cos(degreesToRadians(lat2)) * Math.Pow(Math.Sin(dlong / 2), 2);
            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            double d = EARTH_RADIUS * c;
            return d;
        }

        private double[] nearestPointGreatCircle(double[] a, double[] b, double[] c)
        {
            double[] a_ = toCartsian(a);
            double[] b_ = toCartsian(b);
            double[] c_ = toCartsian(c);
            double[] G = vectorProduct(a_, b_);
            double[] F = vectorProduct(c_, G);
            double[] t = vectorProduct(G, F);
            return fromCartsian(multiplyByScalar(normalize(t), EARTH_RADIUS));
        }

        private double[] toCartsian(double[] coord)
        {
            double[] result = new double[3];
            result[0] = EARTH_RADIUS * Math.Cos(degreesToRadians(coord[0])) * Math.Cos(degreesToRadians(coord[1]));
            result[1] = EARTH_RADIUS * Math.Cos(degreesToRadians(coord[0])) * Math.Sin(degreesToRadians(coord[1]));
            result[2] = EARTH_RADIUS * Math.Sin(degreesToRadians(coord[0]));
            return result;
        }

        private double[] fromCartsian(double[] coord)
        {
            double[] result = new double[2];
            result[0] = radiansToDegrees(Math.Asin(coord[2] / EARTH_RADIUS));
            result[1] = radiansToDegrees(Math.Atan2(coord[1], coord[0]));
            return result;
        }

        private double[] vectorProduct(double[] a, double[] b)
        {
            double[] result = new double[3];
            result[0] = a[1] * b[2] - a[2] * b[1];
            result[1] = a[2] * b[0] - a[0] * b[2];
            result[2] = a[0] * b[1] - a[1] * b[0];
            return result;
        }

        private double[] normalize(double[] t)
        {
            double length = Math.Sqrt((t[0] * t[0]) + (t[1] * t[1]) + (t[2] * t[2]));
            double[] result = new double[3];
            result[0] = t[0] / length;
            result[1] = t[1] / length;
            result[2] = t[2] / length;
            return result;
        }

        private double[] multiplyByScalar(double[] normalize, double k)
        {
            double[] result = new double[3];
            result[0] = normalize[0] * k;
            result[1] = normalize[1] * k;
            result[2] = normalize[2] * k;
            return result;
        }

        private double degreesToRadians(double degrees)
        {
            return degrees * (Math.PI / 180);
        }

        private double radiansToDegrees(double radians)
        {
            return radians / (Math.PI / 180);
        }
    }
}
