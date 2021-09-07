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
        private double x { get; set; }

        private double y { get; set; }

        public Point2D(double x, double y)
        {
            this.x = x;
            this.y = y;
        }

        public double getX()
        {
            return this.x;
        }

        public double getY()
        {
            return this.y;
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
