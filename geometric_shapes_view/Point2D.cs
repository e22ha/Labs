using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace geometric_shapes_view
{
    class Point2D
    {
        private double x;
        private double y;

        public Point Init(double a, double b)
        {
            x = a;
            y = b;

            Point p = new Point(x, y);

            return p;
        }

        public double GetX()
        {
            return x;
        }

        public double GetY()
        {
            return y;
        }

        public void shiftX(double value)
        {
            x += value;
        }
        public void shiftY(double vaule)
        {
            y += vaule;
        }

        public double getDistance(Point2D otherPoint, Point2D point)
        {
            double value;
            value = Math.Sqrt(Math.Pow(otherPoint.x - point.x, 2) + Math.Pow(otherPoint.y - point.y, 2));
            return value;
        }



    }
}
