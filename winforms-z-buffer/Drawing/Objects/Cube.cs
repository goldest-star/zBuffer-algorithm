using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.VisualStyles;
using System.Windows.Media.Media3D;
using System.Xml.Linq;

namespace winforms_z_buffer
{
    public class Cube
    {
        public Color[] Colors = new Color[6];

        public Color EdgeColor;
        public Color FaceColor;

        public Point3D[] Vertices; // Model Vertices

        #region model to world operations

        Vector3D translationVector = new Vector3D(0, 0, 0);
        Matrix3D translation
        {
            get
            {
                Matrix3D m = new Matrix3D(
                    1, 0, 0, 0,
                    0, 1, 0, 0,
                    0, 0, 1, 0,
                    translationVector.X, translationVector.Y, translationVector.Z, 1
                );
                return m;
            }
        }

        Vector3D rotationOVector = new Vector3D(0, 0, 0);
        Matrix3D rotationO
        {
            get
            {
                return getRotationMatrix(rotationOVector);
            }
        }

        Vector3D rotationLVector = new Vector3D(0, 0, 0);
        Matrix3D rotationL
        {
            get
            {
                return getRotationMatrix(rotationLVector);
            }
        }

        Vector3D scaleVector = new Vector3D(1, 1, 1);
        Matrix3D scale
        {
            get
            {
                return new Matrix3D(
                    scaleVector.X, 0, 0, 0,
                    0, scaleVector.Y, 0, 0,
                    0, 0, scaleVector.Z, 0,
                    0, 0, 0, 1
                );
            }
        }

        public Matrix3D Model
        {
            get
            {
                return scale * rotationL * translation * rotationO;
            }
        }

        Matrix3D getRotationMatrix(Vector3D v)
        {
            Matrix3D rX = new Matrix3D(
                1, 0, 0, 0,
                0, Math.Cos(v.X), -Math.Sin(v.X), 0,
                0, Math.Sin(v.X), Math.Cos(v.X), 0,
                0, 0, 0, 1
            );
            Matrix3D rY = new Matrix3D(
                Math.Cos(v.Y), 0, Math.Sin(v.Y), 0,
                0, 1, 0, 0,
                -Math.Sin(v.Y), 0, Math.Cos(v.Y), 0,
                0, 0, 0, 1
            );
            Matrix3D rZ = new Matrix3D(
                Math.Cos(v.Z), -Math.Sin(v.Z), 0, 0,
                Math.Sin(v.Z), Math.Cos(v.Z), 0, 0,
                0, 0, 1, 0,
                0, 0, 0, 1
            );

            return rX * rY * rZ;
        }

        Point3D[] modelToWorld()
        {
            Point3D[] Vertices = new Point3D[this.Vertices.Length];
            for (int i = 0; i < this.Vertices.Length; i++)
            {
                Vertices[i] = this.Vertices[i];
                Model.MultiplyPoint(ref Vertices[i]);
            }
            return Vertices;
        }

        #endregion

        #region triangles

        public Triangle[][] Sides
        {
            get
            {
                Point3D[] Vertices = modelToWorld();

                var sides = new Triangle[][]
                {
                    new Triangle[]
                    {
                        new Triangle(Vertices[0], Vertices[1], Vertices[2], EdgeColor, Colors[0]),
                        new Triangle(Vertices[1], Vertices[2], Vertices[3], EdgeColor, Colors[0])
                    },
                    new Triangle[]
                    {
                        new Triangle(Vertices[4], Vertices[5], Vertices[6], EdgeColor, Colors[1]),
                        new Triangle(Vertices[5], Vertices[6], Vertices[7], EdgeColor, Colors[1])
                    },
                    new Triangle[]
                    {
                        new Triangle(Vertices[0], Vertices[4], Vertices[1], EdgeColor, Colors[2]),
                        new Triangle(Vertices[5], Vertices[4], Vertices[1], EdgeColor, Colors[2])
                    },
                    new Triangle[]
                    {
                        new Triangle(Vertices[3], Vertices[5], Vertices[7], EdgeColor, Colors[3]),
                        new Triangle(Vertices[3], Vertices[5], Vertices[1], EdgeColor, Colors[3])
                    },
                    new Triangle[]
                    {
                        new Triangle(Vertices[2], Vertices[6], Vertices[3], EdgeColor, Colors[4]),
                        new Triangle(Vertices[7], Vertices[6], Vertices[3], EdgeColor, Colors[4])
                    },
                    new Triangle[]
                    {
                        new Triangle(Vertices[2], Vertices[6], Vertices[4], EdgeColor, Colors[5]),
                        new Triangle(Vertices[2], Vertices[0], Vertices[4], EdgeColor, Colors[5])
                    },
                };
                return sides;
            }
        }

        #endregion

        #region statics & contructor

        public Cube(Point3D[] vertices, int colorSeed)
        {
            Vertices = vertices;

            Random rnd = new Random(colorSeed);

            for (int i = 0; i < 6; i++)
                Colors[i] = Color.FromArgb(rnd.Next(256), rnd.Next(256), rnd.Next(256));
        }

        public static Cube UnitCube(int colorSeed)
        {
            Point3D[] vertices = {
                new Point3D(-1, -1, -1), new Point3D(-1, -1, 1), new Point3D(-1, 1, -1),
                new Point3D(-1, 1, 1), new Point3D(1, -1, -1), new Point3D(1, -1, 1),
                new Point3D(1, 1, -1), new Point3D(1, 1, 1)
            };

            return new Cube(vertices, colorSeed);
        }

        public static int[][] UnitCubeEdgePairs()
        {
            return new int[][]
            {
                new int[] {0, 1}, new int[] {1, 3}, new int[] {3, 2}, new int[] {2, 0},
                new int[] {4, 5}, new int[] {5, 7}, new int[] {7, 6}, new int[] {6, 4},
                new int[] {0, 4}, new int[] {1, 5},
                new int[] {2, 6}, new int[] {3, 7}
            };
        }

        #endregion

        #region methods-properties

        public void Translate(Vector3D displacement)
        {
            translationVector += displacement;
        }

        public void RotateAroundOrigin(double angleX, double angleY, double angleZ)
        {
            rotationOVector.X += angleX;
            rotationOVector.Y += angleY;
            rotationOVector.Z += angleZ;
        }

        public void RotateAroundLocalAxis(double angleX, double angleY, double angleZ)
        {
            rotationLVector.X += angleX;
            rotationLVector.Y += angleY;
            rotationLVector.Z += angleZ;
        }

        public void Rescale(double factorX, double factorY, double factorZ)
        {
            scaleVector = new Vector3D(factorX * scaleVector.X, factorY * scaleVector.Y, factorZ * scaleVector.Z);
        }

        #endregion
    }
}
