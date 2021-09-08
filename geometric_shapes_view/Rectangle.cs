using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace geometric_shapes_view
{
    class Rectangle
    {

        private Point2D pointA;
        private Point2D pointB;
        private Point2D pointC;
        private Point2D pointD;


        public Rectangle()
        {

        }

        public Rectangle(Point2D pointA, Point2D pointB, Point2D pointC, Point2D pointD)
        {
            this.pointA = pointA;
            this.pointB = pointB;
            this.pointC = pointC;
            this.pointD = pointD;
        }




        public Point2D getA()
        {
            return this.pointA;
        }

        public Point2D getB()
        {
            return this.pointB;
        }
        public Point2D getC()
        {
            return this.pointC;
        }
        public Point2D getD()
        {
            return this.pointD;
        }

        public double getArea()
        {
            double area;

            double a = this.getA().getDistance(this.getB());
            double b = this.getA().getDistance(this.getD());

            area = a * b;
            return area;
        }

    }
}
