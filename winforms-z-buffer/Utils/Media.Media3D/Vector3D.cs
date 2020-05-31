using MS.Internal;
using System;
using System.Windows;
using System.Windows.Media.Media3D;

namespace System.Windows.Media.Media3D
{
    /// <summary>
    /// Vector3D - 3D vector representation.
    /// </summary>
    public partial struct Vector3D
    {
        //------------------------------------------------------
        //
        //  Constructors
        //
        //------------------------------------------------------

        #region Constructors

        /// <summary>
        /// Constructor that sets vector's initial values.
        /// </summary>
        /// <param name="x">Value of the X coordinate of the new vector.</param>
        /// <param name="y">Value of the Y coordinate of the new vector.</param>
        /// <param name="z">Value of the Z coordinate of the new vector.</param>
        /// 
        public double X;
        public double Y;
        public double Z;
        public Vector3D(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        #endregion Constructors


        //------------------------------------------------------
        //
        //  Public Methods
        //
        //------------------------------------------------------

        #region Public Methods

        /// <summary>
        /// Length of the vector.
        /// </summary>
        public double Length
        {
            get
            {
                return Math.Sqrt(X * X + Y * Y + Z * Z);
            }
        }

        /// <summary>
        /// Length of the vector squared.
        /// </summary>
        public double LengthSquared
        {
            get
            {
                return X * X + Y * Y + Z * Z;
            }
        }

        /// <summary>
        /// Updates the vector to maintain its direction, but to have a length
        /// of 1. Equivalent to dividing the vector by its Length.
        /// Returns NaN if length is zero.
        /// </summary>
        public void Normalize()
        {
            // Computation of length can overflow easily because it
            // first computes squared length, so we first divide by
            // the largest coefficient.
            double m = Math.Abs(X);
            double absy = Math.Abs(Y);
            double absz = Math.Abs(Z);
            if (absy > m)
            {
                m = absy;
            }
            if (absz > m)
            {
                m = absz;
            }

            X /= m;
            Y /= m;
            Z /= m;

            double length = Math.Sqrt(X * X + Y * Y + Z * Z);
            this /= length;
        }

        /// <summary>
        /// Computes the angle between two vectors.
        /// </summary>
        /// <param name="vector1">First vector.</param>
        /// <param name="vector2">Second vector.</param>
        /// <returns>
        /// Returns the angle required to rotate vector1 into vector2 in degrees.
        /// This will return a value between [0, 180] degrees.
        /// (Note that this is slightly different from the Vector member
        /// function of the same name.  Signed angles do not extend to 3D.)
        /// </returns>
       

        /// <summary>
        /// Operator -Vector (unary negation).
        /// </summary>
        /// <param name="vector">Vector being negated.</param>
        /// <returns>Negation of the given vector.</returns>
        public static Vector3D operator -(Vector3D vector)
        {
            return new Vector3D(-vector.X, -vector.Y, -vector.Z);
        }

        /// <summary>
        /// Negates the values of X, Y, and Z on this Vector3D
        /// </summary>
        public void Negate()
        {
            X = -X;
            Y = -Y;
            Z = -Z;
        }

        /// <summary>
        /// Vector addition.
        /// </summary>
        /// <param name="vector1">First vector being added.</param>
        /// <param name="vector2">Second vector being added.</param>
        /// <returns>Result of addition.</returns>
        public static Vector3D operator +(Vector3D vector1, Vector3D vector2)
        {
            return new Vector3D(vector1.X + vector2.X,
                                vector1.Y + vector2.Y,
                                vector1.Z + vector2.Z);
        }

        /// <summary>
        /// Vector addition.
        /// </summary>
        /// <param name="vector1">First vector being added.</param>
        /// <param name="vector2">Second vector being added.</param>
        /// <returns>Result of addition.</returns>
        public static Vector3D Add(Vector3D vector1, Vector3D vector2)
        {
            return new Vector3D(vector1.X + vector2.X,
                                vector1.Y + vector2.Y,
                                vector1.Z + vector2.Z);
        }

        /// <summary>
        /// Vector subtraction.
        /// </summary>
        /// <param name="vector1">Vector that is subtracted from.</param>
        /// <param name="vector2">Vector being subtracted.</param>
        /// <returns>Result of subtraction.</returns>
        public static Vector3D operator -(Vector3D vector1, Vector3D vector2)
        {
            return new Vector3D(vector1.X - vector2.X,
                                vector1.Y - vector2.Y,
                                vector1.Z - vector2.Z);
        }

        /// <summary>
        /// Vector subtraction.
        /// </summary>
        /// <param name="vector1">Vector that is subtracted from.</param>
        /// <param name="vector2">Vector being subtracted.</param>
        /// <returns>Result of subtraction.</returns>
        public static Vector3D Subtract(Vector3D vector1, Vector3D vector2)
        {
            return new Vector3D(vector1.X - vector2.X,
                                vector1.Y - vector2.Y,
                                vector1.Z - vector2.Z);
        }

        /// <summary>
        /// Vector3D + Point3D addition.
        /// </summary>
        /// <param name="vector">Vector by which we offset the point.</param>
        /// <param name="point">Point being offset by the given vector.</param>
        /// <returns>Result of addition.</returns>
        public static Point3D operator +(Vector3D vector, Point3D point)
        {
            return new Point3D(vector.X + point.X,
                               vector.Y + point.Y,
                               vector.Z + point.Z);
        }

        /// <summary>
        /// Vector3D + Point3D addition.
        /// </summary>
        /// <param name="vector">Vector by which we offset the point.</param>
        /// <param name="point">Point being offset by the given vector.</param>
        /// <returns>Result of addition.</returns>
        public static Point3D Add(Vector3D vector, Point3D point)
        {
            return new Point3D(vector.X + point.X,
                               vector.Y + point.Y,
                               vector.Z + point.Z);
        }

        /// <summary>
        /// Vector3D - Point3D subtraction.
        /// </summary>
        /// <param name="vector">Vector by which we offset the point.</param>
        /// <param name="point">Point being offset by the given vector.</param>
        /// <returns>Result of subtraction.</returns>
        public static Point3D operator -(Vector3D vector, Point3D point)
        {
            return new Point3D(vector.X - point.X,
                               vector.Y - point.Y,
                               vector.Z - point.Z);
        }

        /// <summary>
        /// Vector3D - Point3D subtraction.
        /// </summary>
        /// <param name="vector">Vector by which we offset the point.</param>
        /// <param name="point">Point being offset by the given vector.</param>
        /// <returns>Result of subtraction.</returns>
        public static Point3D Subtract(Vector3D vector, Point3D point)
        {
            return new Point3D(vector.X - point.X,
                               vector.Y - point.Y,
                               vector.Z - point.Z);
        }

        /// <summary>
        /// Scalar multiplication.
        /// </summary>
        /// <param name="vector">Vector being multiplied.</param>
        /// <param name="scalar">Scalar value by which the vector is multiplied.</param>
        /// <returns>Result of multiplication.</returns>
        public static Vector3D operator *(Vector3D vector, double scalar)
        {
            return new Vector3D(vector.X * scalar,
                                vector.Y * scalar,
                                vector.Z * scalar);
        }

        /// <summary>
        /// Scalar multiplication.
        /// </summary>
        /// <param name="vector">Vector being multiplied.</param>
        /// <param name="scalar">Scalar value by which the vector is multiplied.</param>
        /// <returns>Result of multiplication.</returns>
        public static Vector3D Multiply(Vector3D vector, double scalar)
        {
            return new Vector3D(vector.X * scalar,
                                vector.Y * scalar,
                                vector.Z * scalar);
        }

        /// <summary>
        /// Scalar multiplication.
        /// </summary>
        /// <param name="scalar">Scalar value by which the vector is multiplied</param>
        /// <param name="vector">Vector being multiplied.</param>
        /// <returns>Result of multiplication.</returns>
        public static Vector3D operator *(double scalar, Vector3D vector)
        {
            return new Vector3D(vector.X * scalar,
                                vector.Y * scalar,
                                vector.Z * scalar);
        }

        /// <summary>
        /// Scalar multiplication.
        /// </summary>
        /// <param name="scalar">Scalar value by which the vector is multiplied</param>
        /// <param name="vector">Vector being multiplied.</param>
        /// <returns>Result of multiplication.</returns>
        public static Vector3D Multiply(double scalar, Vector3D vector)
        {
            return new Vector3D(vector.X * scalar,
                                vector.Y * scalar,
                                vector.Z * scalar);
        }

        /// <summary>
        /// Scalar division.
        /// </summary>
        /// <param name="vector">Vector being divided.</param>
        /// <param name="scalar">Scalar value by which we divide the vector.</param>
        /// <returns>Result of division.</returns>
        public static Vector3D operator /(Vector3D vector, double scalar)
        {
            return vector * (1.0 / scalar);
        }

        /// <summary>
        /// Scalar division.
        /// </summary>
        /// <param name="vector">Vector being divided.</param>
        /// <param name="scalar">Scalar value by which we divide the vector.</param>
        /// <returns>Result of division.</returns>
        public static Vector3D Divide(Vector3D vector, double scalar)
        {
            return vector * (1.0 / scalar);
        }

        /// <summary>
        /// Vector3D * Matrix3D multiplication
        /// </summary>
        /// <param name="vector">Vector being tranformed.</param>
        /// <param name="matrix">Transformation matrix applied to the vector.</param>
        /// <returns>Result of multiplication.</returns>
        public static Vector3D operator *(Vector3D vector, Matrix3D matrix)
        {
            return matrix.Transform(vector);
        }

        /// <summary>
        /// Vector3D * Matrix3D multiplication
        /// </summary>
        /// <param name="vector">Vector being tranformed.</param>
        /// <param name="matrix">Transformation matrix applied to the vector.</param>
        /// <returns>Result of multiplication.</returns>
        public static Vector3D Multiply(Vector3D vector, Matrix3D matrix)
        {
            return matrix.Transform(vector);
        }

        /// <summary>
        /// Vector dot product.
        /// </summary>
        /// <param name="vector1">First vector.</param>
        /// <param name="vector2">Second vector.</param>
        /// <returns>Dot product of two vectors.</returns>
        public static double DotProduct(Vector3D vector1, Vector3D vector2)
        {
            return DotProduct(ref vector1, ref vector2);
        }

        /// <summary>
        /// Faster internal version of DotProduct that avoids copies
        ///
        /// vector1 and vector2 to a passed by ref for perf and ARE NOT MODIFIED
        /// </summary>
        internal static double DotProduct(ref Vector3D vector1, ref Vector3D vector2)
        {
            return vector1.X * vector2.X +
                   vector1.Y * vector2.Y +
                   vector1.Z * vector2.Z;
        }

        /// <summary>
        /// Vector cross product.
        /// </summary>
        /// <param name="vector1">First vector.</param>
        /// <param name="vector2">Second vector.</param>
        /// <returns>Cross product of two vectors.</returns>
        public static Vector3D CrossProduct(Vector3D vector1, Vector3D vector2)
        {
            Vector3D result;
            CrossProduct(ref vector1, ref vector2, out result);
            return result;
        }

        /// <summary>
        /// Faster internal version of CrossProduct that avoids copies
        ///
        /// vector1 and vector2 to a passed by ref for perf and ARE NOT MODIFIED
        /// </summary>
        internal static void CrossProduct(ref Vector3D vector1, ref Vector3D vector2, out Vector3D result)
        {
            result.X = vector1.Y * vector2.Z - vector1.Z * vector2.Y;
            result.Y = vector1.Z * vector2.X - vector1.X * vector2.Z;
            result.Z = vector1.X * vector2.Y - vector1.Y * vector2.X;
        }

        /// <summary>
        /// Vector3D to Point3D conversion.
        /// </summary>
        /// <param name="vector">Vector being converted.</param>
        /// <returns>Point representing the given vector.</returns>
        public static explicit operator Point3D(Vector3D vector)
        {
            return new Point3D(vector.X, vector.Y, vector.Z);
        }

        /// <summary>
        /// Explicit conversion to Size3D.  Note that since Size3D cannot contain negative values,
        /// the resulting size will contains the absolute values of X, Y, and Z.
        /// </summary>
        /// <param name="vector">The vector to convert to a size.</param>
        /// <returns>A size equal to this vector.</returns>


        #endregion Public Methods
    }
}