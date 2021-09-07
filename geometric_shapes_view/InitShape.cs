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
            double x = rnd.NextDouble() * 350;
            double y = rnd.NextDouble() * 350;
            Point2D p = new Point2D(x, y);

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

        public static void createRndRectangel()
        {

        }

        public static void createRectangel()
        {

        }

    }
}
