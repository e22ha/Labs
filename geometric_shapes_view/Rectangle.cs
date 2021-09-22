using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace geometric_shapes_view
{
    class Rectangle : Shape
    {

        private Point2D pointA;
        private Point2D pointB;
        private Point2D pointC;
        private Point2D pointD;


        private double h { get; set; }
        private double w { get; set; }

        public Rectangle()
        {

        }

        public Rectangle(Point2D ltc, double w, double h)
        {
            pointA = new Point2D(ltc);
            pointB = new Point2D(ltc);
            pointC = new Point2D(ltc);
            pointD = new Point2D(ltc);

            this.h = h;
            this.w = w;


            pointB.shiftX(w);
            pointC.shiftX(w);
            pointC.shiftY(h);
            pointD.shiftY(h);
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

        public double getPerimeter()
        {
            double perimeter;
            double a = this.getA().getDistance(this.getB());
            double b = this.getA().getDistance(this.getD());
            perimeter = 2 * (a + b);

            return perimeter;
        }

        public void shiftX(double value)
        {
            this.pointA.shiftX(value);
            this.pointB.shiftX(value);
            this.pointC.shiftX(value);
            this.pointD.shiftX(value);

        }

        public void shiftY(double value)
        {
            this.pointA.shiftY(value);
            this.pointB.shiftY(value);
            this.pointC.shiftY(value);
            this.pointD.shiftY(value);

        }
    }
}
