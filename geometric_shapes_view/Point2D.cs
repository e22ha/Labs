using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace geometric_shapes_view
{
    class Point2D : Shape
    {
        private double x;

        private double y;

        public double maxX;
        public double maxY;

        public Point2D(Point2D other)
        {
            x = other.getX();
            y = other.getY();
        }

        public Point2D()
        {
            x = 0;
            y = 0;
        }

        public Point2D(double x, double y)
        {
            this.x = x;
            this.y = y;
        }

        public bool setX(double x)
        {
            maxX = 400;
            if (x < maxX)
                this.x = x;
            else
            {
                this.x = maxX;
                return false;
            }
            return true;
        }

        public bool setY(double y)
        {
            maxY = 400;
            if (y < maxY)
                this.y = y;
            else
            {
                this.y = maxY;
                return false;
            }
            return true;
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
            //if ((x + value) < 0)
            //    x = 0;
            //else
            //if ((x + value) > 400)
            //    x = 400;
            //else 
                x += value;
        }
        public void shiftY(double value)
        {
            //if((y + value) < 0)
            //    y = 0;
            //else
            //if ((y + value) > 400)
            //    y = 400;
            //else 
                y += value;
        }

        public double getDistance(Point2D otherPoint)
        {
            double value;
            value = Math.Sqrt(Math.Pow(otherPoint.x - this.x, 2) + Math.Pow(otherPoint.y - this.y, 2));

            Math.Abs(value);

            return value;
        }
    }
}
