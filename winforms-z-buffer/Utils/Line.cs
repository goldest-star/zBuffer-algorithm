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
                while (Math.Abs(Ut.F(x1) - Ut.F(p2.X)) > dE)
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
                while (Math.Abs(Ut.F(y1) - Ut.F(p2.Y)) > dE)
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
                while (Math.Abs(Ut.F(z1) - Ut.F(p2.Z)) > dE)
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

        public static List<Point3D> OutlineTriangle(List<Point3D> vertices)
        {
            var points = new List<Point3D>();

            var p1 = vertices[0];
            var p2 = vertices[1];
            var p3 = vertices[2];

            // Sorting the points in order to always have this order on screen p1, p2 & p3
            // with p1 always up (thus having the Y the lowest possible to be near the top screen)
            // then p2 between p1 & p3
            if (p1.Y > p2.Y)
            {
                var temp = p2;
                p2 = p1;
                p1 = temp;
            }

            if (p2.Y > p3.Y)
            {
                var temp = p2;
                p2 = p3;
                p3 = temp;
            }

            if (p1.Y > p2.Y)
            {
                var temp = p2;
                p2 = p1;
                p1 = temp;
            }

            // inverse slopes
            double dP1P2, dP1P3;

            // http://en.wikipedia.org/wiki/Slope
            // Computing inverse slopes
            if (p2.Y - p1.Y > 0)
                dP1P2 = (p2.X - p1.X) / (p2.Y - p1.Y);
            else
                dP1P2 = 0;

            if (p3.Y - p1.Y > 0)
                dP1P3 = (p3.X - p1.X) / (p3.Y - p1.Y);
            else
                dP1P3 = 0;

            // First case where triangles are like that:
            // P1
            // -
            // -- 
            // - -
            // -  -
            // -   - P2
            // -  -
            // - -
            // -
            // P3
            if (dP1P2 > dP1P3)
            {
                for (var y = (int)p1.Y; y <= (int)p3.Y; y++)
                {
                    if (y < p2.Y)
                    {
                        points.AddRange(ProcessScanLine(y, p1, p3, p1, p2));
                    }
                    else
                    {
                        points.AddRange(ProcessScanLine(y, p1, p3, p2, p3));
                    }
                }
            }
            // First case where triangles are like that:
            //       P1
            //        -
            //       -- 
            //      - -
            //     -  -
            // P2 -   - 
            //     -  -
            //      - -
            //        -
            //       P3
            else
            {
                for (var y = (int)p1.Y; y <= (int)p3.Y; y++)
                {
                    if (y < p2.Y)
                    {
                        points.AddRange(ProcessScanLine(y, p1, p2, p1, p3));
                    }
                    else
                    {
                        points.AddRange(ProcessScanLine(y, p2, p3, p1, p3));
                    }
                }
            }

            return points;
        }

        static double Clamp(double value, double min = 0, double max = 1)
        {
            return Math.Max(min, Math.Min(value, max));
        }

        static double Interpolate(double min, double max, double gradient)
        {
            return min + (max - min) * Clamp(gradient);
        }

        static List<Point3D> ProcessScanLine(int y, Point3D pa, Point3D pb, Point3D pc, Point3D pd)
        {
            var points = new List<Point3D>();

            var gradient1 = pa.Y != pb.Y ? (y - pa.Y) / (pb.Y - pa.Y) : 1;
            var gradient2 = pc.Y != pd.Y ? (y - pc.Y) / (pd.Y - pc.Y) : 1;

            int sx = (int)Interpolate(pa.X, pb.X, gradient1);
            int ex = (int)Interpolate(pc.X, pd.X, gradient2);

            double z1 = Interpolate(pa.Z, pb.Z, gradient1);
            double z2 = Interpolate(pc.Z, pd.Z, gradient2);

            var x = sx;

            float gradient = (x - sx) / (float)(ex - sx);

            var z = Interpolate(z1, z2, gradient);
            points.Add(new Point3D(x, y, z));

            for (x = sx + 1; x < ex; x++)
            {
                gradient = (x - sx) / (float)(ex - sx);

                z = Interpolate(z1, z2, gradient);
            }
            
            points.Add(new Point3D(x, y, z));

            return points;
        }
    }
}
