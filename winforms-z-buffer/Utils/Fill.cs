using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Media.Media3D;
using winforms_z_buffer.Utils;

namespace winforms_z_buffer
{

    public struct ActiveEdgeTableEntry
    {
        public double yMax;
        public double yMin;
        public double mInv;
        public double xOfMin;
        public double xOfMax;

        public ActiveEdgeTableEntry(Point start, Point end)
        {
            Point lower = start.Y > end.Y ? end : start;
            Point higher = start.Y > end.Y ? start : end;

            yMax = higher.Y;
            yMin = lower.Y;
            xOfMax = higher.X;
            xOfMin = lower.X;
            mInv = (xOfMax - xOfMin) / (yMax - yMin);
        }

        public ActiveEdgeTableEntry(ActiveEdgeTableEntry aete)
        {
            yMax = aete.yMax;
            yMin = aete.yMin;
            xOfMax = aete.xOfMax;
            xOfMin = aete.xOfMin + aete.mInv;
            mInv = aete.mInv;
        }

    }
    public static class Fill
    {
        public static List<Point> GetPoints(List<Point> pVertices)
        {
            List<Point> vertices = new List<Point>();
            List<KeyValuePair<int, int>> indicies = new List<KeyValuePair<int, int>>();
            List<ActiveEdgeTableEntry> AET = new List<ActiveEdgeTableEntry>();

            var dict = new Dictionary<int, int>();

            for (int l = 0; l < pVertices.Count; l++)
            {
                vertices.Add(pVertices[l]);
                dict.Add(l, pVertices[l].Y);
            }

            indicies = dict.OrderBy(x => x.Value).ToList();

            var points = new List<Point>();

            int k = 0;
            int i = indicies[k].Key;
            int y, ymin;
            y = ymin = vertices[indicies[0].Key].Y;
            int ymax = vertices[indicies[indicies.Count - 1].Key].Y;

            int len = vertices.Count;

            while (y < ymax)
            {
                while (vertices[i].Y == y)
                {
                    if (vertices[(i - 1 + len) % len].Y > vertices[i].Y)
                        AET.Add(new ActiveEdgeTableEntry(vertices[i], vertices[(i - 1 + len) % len]));

                    if (vertices[(i + 1) % len].Y > vertices[i].Y)
                        AET.Add(new ActiveEdgeTableEntry(vertices[i], vertices[(i + 1) % len]));

                    i = indicies[++k].Key;
                }

                AET.Sort(delegate (ActiveEdgeTableEntry e1, ActiveEdgeTableEntry e2)
                {
                    return e1.xOfMin.CompareTo(e2.xOfMin);
                });

                for (int t = 0; t < AET.Count; t += 2)
                    for (int x1 = (int)AET[t].xOfMin; x1 <= AET[(t + 1) % AET.Count].xOfMin; x1++)
                    {
                        points.Add(new Point(x1, y));
                    }

                ++y;
                for (int t = 0; t < AET.Count; t++)
                {
                    AET[t] = new ActiveEdgeTableEntry(AET[t]);
                    if (AET[t].yMax == y)
                        AET.RemoveAt(t--);
                }

            }

            return points;
        }

        public static List<Point3D> FillTriange(List<Point3D> vertices)
        {
            var points = new List<Point3D>();

            vertices = vertices.OrderByDescending(x => x.Y).ToList();

            /* here we know that v1.y <= v2.y <= v3.y */
            /* check for trivial case of bottom-flat triangle */
            if (vertices[1].Y == vertices[2].Y)
                points.AddRange(fillBottomFlatTriangle(vertices[0], vertices[1], vertices[2]));

            /* check for trivial case of top-flat triangle */
            else if (vertices[0].Y == vertices[1].Y)
                points.AddRange(fillTopFlatTriangle(vertices[0], vertices[1], vertices[2]));

            else
            {
                double x = (int)(vertices[0].X + ((int)(vertices[1].Y - vertices[0].Y) / (float)(vertices[2].Y - vertices[0].Y)) * (vertices[2].X - vertices[0].X));
                double z = vertices[0].Z * (1 - (x - vertices[0].X) / (vertices[2].X - vertices[1].X)) + vertices[2].Z * (x - vertices[0].X) / (vertices[2].X - vertices[1].X);
                Point3D v4 = new Point3D(
                    x,
                    vertices[1].Y,
                    z);
                points.AddRange(fillBottomFlatTriangle(vertices[0], vertices[1], v4));
                points.AddRange(fillTopFlatTriangle(vertices[1], v4, vertices[2]));
            }

            return points;
        }

        static List<Point3D> fillTopFlatTriangle(Point3D v1, Point3D v2, Point3D v3)
        {
            System.Console.WriteLine("Entered Top");
            List<Point3D> points = new List<Point3D>();

            double invslope1 = (v3.X - v1.X) / (v3.Y - v1.Y);
            double invslope2 = (v3.X - v2.X) / (v3.Y - v2.Y);

            double curx1 = v3.X;
            double curx2 = v3.X;

            for (int scanlineY = (int)v3.Y; scanlineY > v1.Y; scanlineY--)
            {
                points = Line.GetPoints(new Point3D((int)curx1, scanlineY, 0), new Point3D((int)curx2, scanlineY, 0));
                curx1 -= invslope1;
                curx2 -= invslope2;
            }

            System.Console.WriteLine(points.Count);
            return points;
        }

        static List<Point3D> fillBottomFlatTriangle(Point3D v1, Point3D v2, Point3D v3)
        {
            System.Console.WriteLine("Entered Bot");
            var points = new List<Point3D>();

            double invslope1 = (v2.X - v1.X) / (v2.Y - v1.Y);
            double invslope2 = (v3.X - v1.X) / (v3.Y - v1.Y);

            double curx1 = v1.X;
            double curx2 = v1.X;

            for (int scanlineY = (int)v1.Y; scanlineY <= v2.Y; scanlineY++)
            {
                points = Line.GetPoints(new Point3D((int)curx1, scanlineY, 0), new Point3D((int)curx2, scanlineY, 0));
                curx1 += invslope1;
                curx2 += invslope2;
            }

            System.Console.WriteLine(points.Count);
            return points;
        }
    }
}