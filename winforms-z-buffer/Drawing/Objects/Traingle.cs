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

        Point3D[] getPerspective(Point3D[] set)
        {
            var points = new Point3D[set.Length];

            foreach (var (v, i) in set.WithIndex())
            {
                Point4D point = (Point4D)v;

                point.Z += 100;

                Camera.Instance.GetProjectionMatrix().MultiplyAndNormalizePoint(ref point); 

                points[i] = (Point3D)point;
            }

            return points;
        }

        public Point3D[] TriangleVertexScreenPointsAsPoint3D()
        {
            return getPerspective(Vertices);
        }
    }
}