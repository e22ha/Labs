﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace geometric_shapes_view
{
    class Triangle : Shape
    {
        private Point2D pointA { get; set; }
        private Point2D pointB { get; set; }
        private Point2D pointC { get; set; }

        private Double h1 { get; set; }
        private Double h2 { get; set; }
        private Double w1 { get; set; }
        private Double w2 { get; set; }


        public Triangle(Point2D pointA, Point2D pointB, Point2D pointC)
        {
            this.pointA = pointA;
            this.pointB = pointB;
            this.pointC = pointC;
        }

        public Triangle(Point2D pointA, double w1, double h1, double w2, double h2)
        {
            this.pointA = new Point2D(pointA);
            pointB = new Point2D(pointA);
            pointC = new Point2D(pointA);

            this.w1 = w1;
            this.w2 = w2;
            this.h1 = h1;
            this.h2 = h2;

            pointB.shiftX(pointA.getX() + w1);
            pointB.shiftY(pointA.getY() + h1);
            pointC.shiftX(pointA.getX() + w2);
            pointC.shiftY(pointA.getY() + h2);
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




        public double getPerimeter()
        {
            double perimeter;


            perimeter = this.pointA.getDistance(this.pointB) + this.pointB.getDistance(this.pointC) + this.pointC.getDistance(this.pointA);

            /*perimeter = Math.Sqrt(
                                    Math.Pow(this.pointB.getX(), 2)
                                   - Math.Pow(this.pointA.getX(), 2)
                                   + (Math.Pow(this.pointB.getY(), 2)
                                   - Math.Pow(this.pointA.getY(), 2))
                                   )
                + Math.Sqrt(
                                    Math.Pow(this.pointC.getX(), 2)
                                   - Math.Pow(this.pointB.getX(), 2)
                                   + (Math.Pow(this.pointC.getY(), 2)
                                   - Math.Pow(this.pointB.getY(), 2))
                                   )
                + Math.Sqrt(
                                    Math.Pow(this.pointA.getX(), 2)
                                   - Math.Pow(this.pointC.getX(), 2)
                                   + (Math.Pow(this.pointA.getY(), 2)
                                   - Math.Pow(this.pointC.getY(), 2))
                                   )
                ;
            */
            return perimeter;
        }

        public double getArea()
        {
            double area;

            area = 0.5 * (pointA.getX() * (pointB.getY() - pointC.getY()) + pointB.getX() * (pointC.getY() - pointA.getY()) + pointC.getX() * (pointA.getY() - pointB.getY()));

            return area;
        }


        public void shiftX(double value)
        {

            this.pointA.shiftX(value);
            this.pointB.shiftX(value);
            this.pointC.shiftX(value);
        }

        public void shiftY(double value)
        {
            this.pointA.shiftY(value);
            this.pointB.shiftY(value);
            this.pointC.shiftY(value);

        }


    }
}
