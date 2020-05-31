using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Media.Media3D;


namespace winforms_z_buffer
{
    public class Triangle
    {
        public Point3D[] Vertices;
        public Color Color;

        public Triangle(Point3D v1, Point3D v2, Point3D v3, Color color)
        {
            Vertices = new Point3D[3] { v1, v2, v3 };
            Color = color;
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
        
        Matrix3D getProjectionMatrix()
        {
            double near = 0.001;
            double far = 1000;
            double fn = far - near;
            double fov = Math.PI / 2;
            double fovRad = 1 / Math.Tan(fov * 0.5);

            Matrix3D matrix = new Matrix3D(
                    fovRad, 0,      0,      0,
                    0,      fovRad, 0,      0,
                    0,      0,      far/fn, -far * near / fn,
                    0,      0,      1,      0
                );

            return matrix;
        }

        Matrix3D getRotationXMatrix(double angle)
        {
            double cos = Math.Cos(angle * 0.5);
            double sin = Math.Sin(angle * 0.5);

            Matrix3D matrix = new Matrix3D(
                    1, 0,   0,      0,
                    0, cos, -sin,   0,
                    0, sin, cos,    0,
                    0, 0,   0,      1
                );

            return matrix;
        }

        Matrix3D getRotationZMatrix(double angle)
        {
            double cos = Math.Cos(angle);
            double sin = Math.Sin(angle);

            Matrix3D matrix = new Matrix3D(
                    cos,    -sin,   0, 0,
                    sin,    cos,    0, 0,
                    0,      0,      1, 0,
                    0,      0,      0, 1
                );

            return matrix;
        }

        void multiplyAndNormalize(ref Point4D point, Matrix3D matrix)
        {
            matrix.MultiplyPoint(ref point);
            
            if (point.W != 0)
            {
                point.X /= point.W;
                point.Y /= point.W;
                point.Z /= point.W;
            }
        }

        Point3D[] getPerspective()
        {
            var points = new Point3D[3];

            foreach (var (v, i) in Vertices.WithIndex())
            {
                Point4D point = (Point4D)v;

                multiplyAndNormalize(ref point, getRotationZMatrix(0));
                multiplyAndNormalize(ref point, getRotationXMatrix(0));

                point.Z += 100;

                multiplyAndNormalize(ref point, getProjectionMatrix());

                points[i] = (Point3D)point;
            }

            return points;
        }

        public Point[][] TriangleEdgeScreenPointsAsPoint(double angle = 0)
        {
            return createVertexPairs<Point>(getPerspective());
        }

        public Point3D[][] TriangleEdgeScreenPointsAsPoint3D()
        {
            return createVertexPairs<Point3D>(getPerspective());
        }

        public Point3D[] TriangleVertexScreenPointsAsPoint3D()
        {
            return getPerspective();
        }
    }
}