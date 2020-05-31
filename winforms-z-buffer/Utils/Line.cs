using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace winforms_z_buffer.Utils
{
    public static class Line
    {
        // Bresenham's Line in 3D
        // Source https://www.geeksforgeeks.org/bresenhams-algorithm-for-3-d-line-drawing/
        public static List<Point3D> GetPoints(Point3D p1, Point3D p2)
        {
            double dE = 1; 

            List<Point3D> points = new List<Point3D>() { p1 };

            double dx = Math.Abs(p2.X - p1.X);
            double dy = Math.Abs(p2.Y - p1.Y);
            double dz = Math.Abs(p2.Z - p1.Z);

            double xs = p2.X > p1.X ? 1 : -1;
            double ys = p2.Y > p1.Y ? 1 : -1;
            double zs = p2.Z > p1.Z ? 1 : -1;

            double
                d1,
                d2;
            double
                x1 = p1.X,
                y1 = p1.Y,
                z1 = p1.Z;

            if (dx >= dy && dx >= dz)
            {
                d1 = 2 * dy - dx;
                d2 = 2 * dz - dx;
                while (Math.Abs(x1 - p2.X) > dE)
                {
                    x1 += xs;
                    if (d1 >= 0)
                    {
                        y1 += ys;
                        d1 -= 2 * dx;
                    }
                    if (d2 >= 0)
                    {
                        z1 += zs;
                        d2 -= 2 * dx;
                    }
                    d1 += 2 * dy;
                    d2 += 2 * dz;
                    points.Add(new Point3D(x1, y1, z1));
                }
            }

            else if (dy >= dx && dy >= dz)
            {
                d1 = 2 * dx - dy;
                d2 = 2 * dz - dy;
                while (Math.Abs(y1 - p2.Y) > dE)
                {
                    y1 += ys;
                    if (d1 >= 0)
                    {
                        x1 += xs;
                        d1 -= 2 * dy;
                    }
                    if (d2 >= 0)
                    {
                        z1 += zs;
                        d2 -= 2 * dy;
                    }
                    d1 += 2 * dx;
                    d2 += 2 * dz;
                    points.Add(new Point3D(x1, y1, z1));
                }
            }

            else
            {
                d1 = 2 * dy - dz;
                d2 = 2 * dz - dz;
                while (Math.Abs(z1 - p2.Z) > dE)
                {
                    z1 += zs;
                    if (d1 >= 0)
                    {
                        y1 += ys;
                        d1 -= 2 * dz;
                    }
                    if (d2 >= 0)
                    {
                        x1 += xs;
                        d2 -= 2 * dz;
                    }
                    d1 += 2 * dy;
                    d2 += 2 * dx;
                    points.Add(new Point3D(x1, y1, z1));
                }
            }

            points.Add(p2);

            return points;
        }

    }
}
