﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.VisualStyles;
using System.Windows.Media.Media3D;

namespace winforms_z_buffer
{
    public class Cube
    {

        public Color EdgeColor;
        public Color FaceColor;

        public Point3D[] Vertices;
        Point3D center;

        public Triangle[][] Sides
        {
            get
            {
                var sides = new Triangle[][]
                {
                    new Triangle[]
                    {
                        new Triangle(Vertices[0], Vertices[1], Vertices[2], EdgeColor, FaceColor),
                        new Triangle(Vertices[1], Vertices[2], Vertices[3], EdgeColor, FaceColor)
                    },
                    new Triangle[]
                    {
                        new Triangle(Vertices[4], Vertices[5], Vertices[6], EdgeColor, FaceColor),
                        new Triangle(Vertices[5], Vertices[6], Vertices[7], EdgeColor, FaceColor)
                    },
                    new Triangle[]
                    {
                        new Triangle(Vertices[0], Vertices[4], Vertices[1], EdgeColor, FaceColor),
                        new Triangle(Vertices[5], Vertices[4], Vertices[1], EdgeColor, FaceColor)
                    },
                    new Triangle[]
                    {
                        new Triangle(Vertices[3], Vertices[5], Vertices[7], EdgeColor, FaceColor),
                        new Triangle(Vertices[3], Vertices[5], Vertices[1], EdgeColor, FaceColor)
                    },
                    new Triangle[]
                    {
                        new Triangle(Vertices[2], Vertices[6], Vertices[3], EdgeColor, FaceColor),
                        new Triangle(Vertices[7], Vertices[6], Vertices[3], EdgeColor, FaceColor)
                    },
                    new Triangle[]
                    {
                        new Triangle(Vertices[2], Vertices[6], Vertices[4], EdgeColor, FaceColor),
                        new Triangle(Vertices[2], Vertices[0], Vertices[4], EdgeColor, FaceColor)
                    },
                };

                return sides;
            }
        }

        public Cube(Point3D[] vertices, Color eColor, Color fColor)
        {
            Vertices = vertices;
            EdgeColor = eColor;
            FaceColor = fColor;
            center = findCenter(vertices);
        }

        #region statics

        public static Cube UnitCube(Color eColor, Color fColor)
        {
            Point3D[] vertices = {
                new Point3D(-1, -1, -1), new Point3D(-1, -1, 1), new Point3D(-1, 1, -1),
                new Point3D(-1, 1, 1), new Point3D(1, -1, -1), new Point3D(1, -1, 1),
                new Point3D(1, 1, -1), new Point3D(1, 1, 1)
            };

            return new Cube(vertices, eColor, fColor);
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
            for (int i = 0; i < 8; i++)
                Vertices[i] = Vertices[i] + displacement;

            center += displacement;
        }

        public void RotateAroundOrigin(double angleX, double angleY, double angleZ)
        {
            var pitch = angleX;
            var roll = angleY;
            var yaw = angleZ;

            var cosa = Math.Cos(yaw);
            var sina = Math.Sin(yaw);

            var cosb = Math.Cos(pitch);
            var sinb = Math.Sin(pitch);

            var cosc = Math.Cos(roll);
            var sinc = Math.Sin(roll);

            var Axx = cosa * cosb;
            var Axy = cosa * sinb * sinc - sina * cosc;
            var Axz = cosa * sinb * cosc + sina * sinc;

            var Ayx = sina * cosb;
            var Ayy = sina * sinb * sinc + cosa * cosc;
            var Ayz = sina * sinb * cosc - cosa * sinc;

            var Azx = -sinb;
            var Azy = cosb * sinc;
            var Azz = cosb * cosc;

            for (var i = 0; i < Vertices.Length; i++)
            {
                var px = Vertices[i].X;
                var py = Vertices[i].Y;
                var pz = Vertices[i].Z;

                Vertices[i].X = Axx * px + Axy * py + Axz * pz;
                Vertices[i].Y = Ayx * px + Ayy * py + Ayz * pz;
                Vertices[i].Z = Azx * px + Azy * py + Azz * pz;
            }
        }

        public void RotateAroundLocalAxis(double angleX, double angleY, double angleZ)
        {
            relativeToOrigin(
                () => { RotateAroundOrigin(angleX, angleY, angleZ); }
                );
        }

        public void Rescale(double factorX, double factorY, double factorZ)
        {
            relativeToOrigin(
                () =>
                {
                    for (int i = 0; i < 8; i++)
                    {
                        Vertices[i].X *= factorX;
                        Vertices[i].Y *= factorY;
                        Vertices[i].Z *= factorZ;
                    }
                }
                );
        }

        Point3D findCenter(Point3D[] points)
        {
            double x = 0, y = 0, z = 0;

            foreach (var point in points)
            {
                x += point.X;
                y += point.Y;
                z += point.Z;
            }

            return new Point3D(x / points.Length, y / points.Length, z / points.Length);
        }

        void relativeToOrigin(Action operation)
        {
            for (int i = 0; i < 8; i++)
                Vertices[i] = Vertices[i] - (Vector3D)center;

            operation();

            for (int i = 0; i < 8; i++)
                Vertices[i] = Vertices[i] + (Vector3D)center;
        }

        #endregion
    }
}
