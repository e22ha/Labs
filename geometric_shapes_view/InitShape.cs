using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace geometric_shapes_view
{
    class InitShape
    {
        public static Random rnd = new Random();

        public static Point2D create2DPoint()
        {


            Point2D p = new Point2D();

            p.maxX = 350;

            while (true)
            {
                double x = rnd.NextDouble() * 350;

                if (p.setX(x)) break;
            }

            p.maxY = 350;

            while (true)
            {
                double y = rnd.NextDouble() * 350;

                if (p.setY(y)) break;
            }

            return p;
        }

        public static Triangle createRndTriangle()
        {
            Point2D a = create2DPoint();
            Point2D b = create2DPoint();
            Point2D c = create2DPoint();
            Triangle tri = new Triangle(a, b, c);
            return tri;
        }

        public static Rectangle createRndRectangel()
        {
            Point2D a = create2DPoint();
            Point2D c = create2DPoint();

            Point2D b = new Point2D(a.getX(),c.getY());
            Point2D d = new Point2D(c.getX(),a.getY());

            Rectangle rec = new Rectangle(a,b,c,d);
            return rec;
        }

        public static void createRectangel()
        {

        }

    }
}
