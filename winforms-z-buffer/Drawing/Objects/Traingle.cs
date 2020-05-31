using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Media.Media3D;
using winforms_z_buffer.Utils;

namespace winforms_z_buffer
{
    public class Triangle
    {
        public Point3D[] Vertices;
        public Color EdgeColor;
        public Color FaceColor;

        public Triangle(Point3D v1, Point3D v2, Point3D v3, Color eColor, Color fColor)
        {
            Vertices = new Point3D[3] { v1, v2, v3 };
            EdgeColor = eColor;
            FaceColor = fColor;
        }

        public void Translate(Vector3D displacement)
        {
            Vertices[0] += displacement;
            Vertices[1] += displacement;
            Vertices[2] += displacement;
        }

        public Point3D[][] GetVertexPairs()
        {
            return createVertexPairs<Point3D>(Vertices);
        }

        T[][] createVertexPairs<T>(Point3D[] set)
        {
            return new T[][]
            {
                new T[] { (T)(object)set[0], (T)(object)set[1] },
                new T[] { (T)(object)set[1], (T)(object)set[2] },
                new T[] { (T)(object)set[2], (T)(object)set[0] },
            };
        }
        
        Point3D[] getPerspective(Point3D[] set, int len = 3)
        {
            var points = new Point3D[len];

            foreach (var (v, i) in set.WithIndex())
            {
                Point4D point = (Point4D)v;

                point.Z += 100;

                Camera.Instance.GetProjectionMatrix().MultiplyAndNormalizePoint(ref point);

                points[i] = (Point3D)point;
            }

            return points;
        }

        public Point[][] TriangleEdgeScreenPointsAsPoint()
        {
            return createVertexPairs<Point>(getPerspective(Vertices));
        }

        public Point3D[][] TriangleEdgeScreenPointsAsPoint3D()
        {
            return createVertexPairs<Point3D>(getPerspective(Vertices));
        }

        public Point3D[] TriangleVertexScreenPointsAsPoint3D()
        {
            return getPerspective(Vertices);
        }
    }
}