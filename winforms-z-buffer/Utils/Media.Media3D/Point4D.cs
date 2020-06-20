
using System.Windows;
using System.Windows.Media.Media3D;
using System;
using System.Text;

namespace System.Windows.Media.Media3D
{
    public partial struct Point4D
    {
        public override string ToString()
        {
            var sb = new StringBuilder("[ ");
            sb.Append(X);
            sb.Append(" ");
            sb.Append(Y);
            sb.Append(" ");
            sb.Append(Z);
            sb.Append(" ");
            sb.Append(W);
            sb.Append(" ]");

            return sb.ToString();
        }

        //------------------------------------------------------
        //
        //  Constructors
        //
        //------------------------------------------------------

        #region Constructors
        public double X;
        public double Y;
        public double Z;
        public double W;
        public Point4D(double x, double y, double z, double w)
        {
            X = x;
            Y = y;
            Z = z;
            W = w;
        }

        #endregion Constructors


        //------------------------------------------------------
        //
        //  Public Methods
        //
        //------------------------------------------------------

        #region Public Methods

        public void Offset(double deltaX, double deltaY, double deltaZ, double deltaW)
        {
            X += deltaX;
            Y += deltaY;
            Z += deltaZ;
            W += deltaW;
        }

        public static Point4D operator +(Point4D point1, Point4D point2)
        {
            return new Point4D(point1.X + point2.X,
                               point1.Y + point2.Y,
                               point1.Z + point2.Z,
                               point1.W + point2.W);
        }

        public static Point4D Add(Point4D point1, Point4D point2)
        {
            return new Point4D(point1.X + point2.X,
                               point1.Y + point2.Y,
                               point1.Z + point2.Z,
                               point1.W + point2.W);
        }

        public static Point4D operator -(Point4D point1, Point4D point2)
        {
            return new Point4D(point1.X - point2.X,
                               point1.Y - point2.Y,
                               point1.Z - point2.Z,
                               point1.W - point2.W);
        }

        public static Point4D Subtract(Point4D point1, Point4D point2)
        {
            return new Point4D(point1.X - point2.X,
                               point1.Y - point2.Y,
                               point1.Z - point2.Z,
                               point1.W - point2.W);
        }

        public static Point4D operator *(Point4D point, Matrix3D matrix)
        {
            return matrix.Transform(point);
        }

        public static Point4D operator *(double factor, Point4D point)
        {
            return new Point4D(factor * point.X,
                               factor * point.Y,
                               factor * point.Z,
                               factor * point.W);
        }

        public static Point4D Multiply(Point4D point, Matrix3D matrix)
        {
            return matrix.Transform(point);
        }

        public static explicit operator Point3D(Point4D point)
        {
            return new Point3D(point.X, point.Y, point.Z);
        }

        #endregion Public Methods
    }
}