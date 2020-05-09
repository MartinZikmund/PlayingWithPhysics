using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Physics.Shared.Logic.Geometry
{
    /// <summary>
    /// A struct representing a vector in 2D space
    /// </summary>
    [Serializable]
    public struct Vector2d : IEquatable<Vector2d>, IFormattable
    {
        /// <summary>
        /// The x component.
        /// </summary>
        public readonly double X;

        /// <summary>
        /// The y component.
        /// </summary>
        public readonly double Y;

        /// <summary>
        /// Initializes a new instance of the <see cref="Vector2d"/> struct.
        /// </summary>
        /// <param name="x">The x component.</param>
        /// <param name="y">The y component.</param>
        public Vector2d(double x, double y)
        {
            this.X = x;
            this.Y = y;
        }

        /// <summary>
        /// Gets a vector representing the X Axis
        /// </summary>
        public static Vector2d XAxis { get; } = new Vector2d(1, 0);

        /// <summary>
        /// Gets a vector representing the Y Axis
        /// </summary>
        public static Vector2d YAxis { get; } = new Vector2d(0, 1);

        /// <summary>
        /// Gets the length of the vector
        /// </summary>
        public double Length => Math.Sqrt((this.X * this.X) + (this.Y * this.Y));

        /// <summary>
        /// Returns a value that indicates whether each pair of elements in two specified vectors is equal.
        /// </summary>
        /// <param name="left">The first vector to compare.</param>
        /// <param name="right">The second vector to compare.</param>
        /// <returns>True if the vectors are the same; otherwise false.</returns>
        public static bool operator ==(Vector2d left, Vector2d right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Returns a value that indicates whether any pair of elements in two specified vectors is not equal.
        /// </summary>
        /// <param name="left">The first vector to compare.</param>
        /// <param name="right">The second vector to compare.</param>
        /// <returns>True if the vectors are different; otherwise false.</returns>
        public static bool operator !=(Vector2d left, Vector2d right)
        {
            return !left.Equals(right);
        }

        /// <summary>
        /// Adds two vectors
        /// </summary>
        /// <param name="left">The first vector</param>
        /// <param name="right">The second vector</param>
        /// <returns>A new summed vector</returns>
        public static Vector2d operator +(Vector2d left, Vector2d right)
        {
            return left.Add(right);
        }

        /// <summary>
        /// Subtracts two vectors
        /// </summary>
        /// <param name="left">The first vector</param>
        /// <param name="right">The second vector</param>
        /// <returns>A new difference vector</returns>
        public static Vector2d operator -(Vector2d left, Vector2d right)
        {
            return left.Subtract(right);
        }

        /// <summary>
        /// Negates the vector
        /// </summary>
        /// <param name="v">A vector to negate</param>
        /// <returns>A new negated vector</returns>
        public static Vector2d operator -(Vector2d v)
        {
            return v.Negate();
        }

        /// <summary>
        /// Multiplies a vector by a scalar
        /// </summary>
        /// <param name="d">A scalar</param>
        /// <param name="v">A vector</param>
        /// <returns>A scaled vector</returns>
        public static Vector2d operator *(double d, Vector2d v)
        {
            return new Vector2d(d * v.X, d * v.Y);
        }

        /// <summary>
        /// Multiplies a vector by a scalar
        /// </summary>
        /// <param name="v">A vector</param>
        /// <param name="d">A scalar</param>
        /// <returns>A scaled vector</returns>
        public static Vector2d operator *(Vector2d v, double d)
        {
            return d * v;
        }

        /// <summary>
        /// Divides a vector by a scalar
        /// </summary>
        /// <param name="v">A vector</param>
        /// <param name="d">A scalar</param>
        /// <returns>A scaled vector</returns>
        public static Vector2d operator /(Vector2d v, double d)
        {
            return new Vector2d(v.X / d, v.Y / d);
        }

        /// <summary>
        /// Creates a Vector from Polar coordinates
        /// </summary>
        /// <param name="radius">The distance of the point from the origin</param>
        /// <param name="angle">The angle of the point as measured from the X Axis</param>
        /// <returns>A vector.</returns>
        public static Vector2d FromPolar(double radius, Angle angle)
        {
            if (radius < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(radius), radius, "Expected a radius greater than or equal to zero.");
            }

            return new Vector2d(
                radius * Math.Cos(angle.Radians),
                radius * Math.Sin(angle.Radians));
        }

        /// <summary>
        /// Computes whether or not this vector is perpendicular to <paramref name="other"/> vector by:
        /// 1. Normalizing both
        /// 2. Computing the dot product.
        /// 3. Comparing 1- Math.Abs(dot product) to <paramref name="tolerance"/>
        /// </summary>
        /// <param name="other">The other <see cref="Vector2d"/></param>
        /// <param name="tolerance">The tolerance for when vectors are said to be parallel</param>
        /// <returns>True if the vector dot product is within the given double tolerance of unity, false if not</returns>
        public bool IsParallelTo(Vector2d other, double tolerance = 1e-10)
        {
            var dp = Math.Abs(this.Normalize().DotProduct(other.Normalize()));
            return Math.Abs(1 - dp) <= tolerance;
        }

        /// <summary>
        /// Computes whether or not this vector is parallel to another vector within a given angle tolerance.
        /// </summary>
        /// <param name="other">The other <see cref="Vector2d"/></param>
        /// <param name="tolerance">The tolerance for when vectors are said to be parallel</param>
        /// <returns>True if the vectors are parallel within the angle tolerance, false if they are not</returns>
        public bool IsParallelTo(Vector2d other, Angle tolerance)
        {
            // Compute the angle between these vectors
            var angle = this.AngleTo(other);
            if (angle < tolerance)
            {
                return true;
            }

            // Compute the 180° opposite of the angle
            return Angle.FromRadians(Math.PI) - angle < tolerance;
        }

        /// <summary>
        /// Computes whether or not this vector is perpendicular to <paramref name="other"/> vector by:
        /// 1. Normalizing both
        /// 2. Computing the dot product.
        /// 3. Comparing Math.Abs(dot product) to <paramref name="tolerance"/>
        /// </summary>
        /// <param name="other">The other <see cref="Vector2d"/></param>
        /// <param name="tolerance">The tolerance for when vectors are said to be parallel</param>
        /// <returns>True if the vector dot product is within the given double tolerance of unity, false if not</returns>
        public bool IsPerpendicularTo(Vector2d other, double tolerance = 1e-10)
        {
            return Math.Abs(this.Normalize().DotProduct(other.Normalize())) < tolerance;
        }

        /// <summary>
        /// Computes whether or not this vector is parallel to another vector within a given angle tolerance.
        /// </summary>
        /// <param name="other">The other <see cref="Vector2d"/></param>
        /// <param name="tolerance">The tolerance for when vectors are said to be parallel</param>
        /// <returns>True if the vectors are parallel within the angle tolerance, false if they are not</returns>
        public bool IsPerpendicularTo(Vector2d other, Angle tolerance)
        {
            var angle = this.AngleTo(other);
            const double Perpendicular = Math.PI / 2;
            return Math.Abs(angle.Radians - Perpendicular) < tolerance.Radians;
        }

        /// <summary>
        /// Compute the signed angle to another vector.
        /// </summary>
        /// <param name="other">The other <see cref="Vector2d"/></param>
        /// <param name="clockWise">Positive in clockwise direction</param>
        /// <param name="returnNegative">When true and the result is > 180° a negative value is returned</param>
        /// <returns>The angle between the vectors.</returns>
        public Angle SignedAngleTo(Vector2d other, bool clockWise = false, bool returnNegative = false)
        {
            var sign = clockWise ? -1 : 1;
            var a1 = Math.Atan2(this.Y, this.X);
            if (a1 < 0)
            {
                a1 += 2 * Math.PI;
            }

            var a2 = Math.Atan2(other.Y, other.X);
            if (a2 < 0)
            {
                a2 += 2 * Math.PI;
            }

            var a = sign * (a2 - a1);
            if (a < 0 && !returnNegative)
            {
                a += 2 * Math.PI;
            }

            if (a > Math.PI && returnNegative)
            {
                a -= 2 * Math.PI;
            }

            return Angle.FromRadians(a);
        }

        /// <summary>
        /// Compute the angle between this vector and another using the arccosine of the dot product.
        /// </summary>
        /// <param name="other">The other <see cref="Vector2d"/></param>
        /// <returns>The angle between vectors, with a range between 0° and 180°</returns>
        public Angle AngleTo(Vector2d other)
        {
            return Angle.FromRadians(
                Math.Abs(
                    Math.Atan2(
                        (this.X * other.Y) - (other.X * this.Y),
                        (this.X * other.X) + (this.Y * other.Y))));
        }

        /// <summary>
        /// Rotates a Vector by an angle
        /// </summary>
        /// <param name="angle">The angle.</param>
        /// <returns>A new rotated vector.</returns>
        public Vector2d Rotate(Angle angle)
        {
            var cs = Math.Cos(angle.Radians);
            var sn = Math.Sin(angle.Radians);
            var x = (this.X * cs) - (this.Y * sn);
            var y = (this.X * sn) + (this.Y * cs);
            return new Vector2d(x, y);
        }

        /// <summary>
        /// Perform the dot product on a pair of vectors
        /// </summary>
        /// <param name="other">The second vector</param>
        /// <returns>The result of the dot product.</returns>
        public double DotProduct(Vector2d other)
        {
            return (this.X * other.X) + (this.Y * other.Y);
        }

        /// <summary>
        /// Performs the 2D 'cross product' as if the 2D vectors were really 3D vectors in the z=0 plane, returning
        /// the scalar magnitude and direction of the resulting z value.
        /// Formula: (this.X * other.Y) - (this.Y * other.X)
        /// </summary>
        /// <param name="other">The other <see cref="Vector2d"/></param>
        /// <returns>(this.X * other.Y) - (this.Y * other.X)</returns>
        public double CrossProduct(Vector2d other)
        {
            // Though the cross product is undefined in 2D space, this is a useful mathematical operation to
            // determine angular direction and to compute the area of 2D shapes
            return (this.X * other.Y) - (this.Y * other.X);
        }

        /// <summary>
        /// Projects this vector onto another vector
        /// </summary>
        /// <param name="other">The other <see cref="Vector2d"/></param>
        /// <returns>A <see cref="Vector2d"/> representing this vector projected on <paramref name="other"/></returns>
        public Vector2d ProjectOn(Vector2d other)
        {
            return other * (this.DotProduct(other) / other.DotProduct(other));
        }

        /// <summary>
        /// Creates a new unit vector from the existing vector.
        /// </summary>
        /// <returns>A new unit vector in the same direction as the original vector</returns>
        public Vector2d Normalize()
        {
            var l = this.Length;
            return new Vector2d(this.X / l, this.Y / l);
        }

        /// <summary>
        /// Scales the vector by the provided value
        /// </summary>
        /// <param name="d">a scaling factor</param>
        /// <returns>A new scale adjusted vector</returns>
        public Vector2d ScaleBy(double d)
        {
            return new Vector2d(d * this.X, d * this.Y);
        }

        /// <summary>
        /// Returns the negative of the vector
        /// </summary>
        /// <returns>A new negated vector.</returns>
        public Vector2d Negate()
        {
            return new Vector2d(-1 * this.X, -1 * this.Y);
        }

        /// <summary>
        /// Subtracts a vector from this vector.
        /// </summary>
        /// <param name="v">A vector to subtract</param>
        /// <returns>A new vector which is the difference of the current vector and the provided vector</returns>
        public Vector2d Subtract(Vector2d v)
        {
            return new Vector2d(this.X - v.X, this.Y - v.Y);
        }

        /// <summary>
        /// Adds a vector to this vector
        /// </summary>
        /// <param name="v">A vector to add</param>
        /// <returns>A new vector which is the sum of the existing vector and the provided vector</returns>
        public Vector2d Add(Vector2d v)
        {
            return new Vector2d(this.X + v.X, this.Y + v.Y);
        }

        ///// <summary>
        ///// Transforms a vector by multiplying it against a provided matrix
        ///// </summary>
        ///// <param name="m">The matrix to multiply</param>
        ///// <returns>A new transformed vector</returns>
        //public Vector2d TransformBy(Matrix<double> m)
        //{
        //    var transformed = m.Multiply(this.ToVector());
        //    return new Vector2d(transformed.At(0), transformed.At(1));
        //}

        ///// <summary>
        ///// Convert to a Math.NET Numerics dense vector of length 2.
        ///// </summary>
        ///// <returns> A <see cref="Vector"/> with the x and y values from this instance.</returns>
        //public Vector<double> ToVector()
        //{
        //    return Vector<double>.Build.Dense(new[] { this.X, this.Y });
        //}

        /// <summary>
        /// Compare this instance with <paramref name="other"/>
        /// </summary>
        /// <param name="other">The other <see cref="Vector2d"/></param>
        /// <param name="tolerance">The tolerance when comparing the x and y components</param>
        /// <returns>True if found to be equal.</returns>
        public bool Equals(Vector2d other, double tolerance)
        {
            if (tolerance < 0)
            {
                throw new ArgumentException("epsilon < 0");
            }

            return Math.Abs(other.X - this.X) < tolerance &&
                   Math.Abs(other.Y - this.Y) < tolerance;
        }

        /// <inheritdoc />
        public bool Equals(Vector2d other) => this.X.Equals(other.X) && this.Y.Equals(other.Y);

        /// <inheritdoc />
        public override bool Equals(object obj) => obj is Vector2d v && this.Equals(v);

        /// <inheritdoc />
        public override int GetHashCode() => this.X.GetHashCode() * 13 + this.Y.GetHashCode();

        /// <inheritdoc />
        public override string ToString()
        {
            return this.ToString(null, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Returns a string representation of this instance using the provided <see cref="IFormatProvider"/>
        /// </summary>
        /// <param name="provider">A <see cref="IFormatProvider"/></param>
        /// <returns>The string representation of this instance.</returns>
        public string ToString(IFormatProvider provider)
        {
            return this.ToString(null, provider);
        }

        /// <inheritdoc />
        public string ToString(string format, IFormatProvider provider = null)
        {
            var numberFormatInfo = provider != null ? NumberFormatInfo.GetInstance(provider) : CultureInfo.InvariantCulture.NumberFormat;
            var separator = numberFormatInfo.NumberDecimalSeparator == "," ? ";" : ",";
            return $"({this.X.ToString(format, numberFormatInfo)}{separator}\u00A0{this.Y.ToString(format, numberFormatInfo)})";
        }
    }
}
