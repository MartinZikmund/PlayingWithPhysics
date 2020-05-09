using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Linq;
using System.Net.Mime;
using System.Numerics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;

namespace Physics.Shared.Logic.Geometry
{
    public struct Point2d : IEquatable<Point2d>, IFormattable
    {
        /// <summary>
        /// The x coordinate
        /// </summary>
        public readonly double X;

        /// <summary>
        /// The y coordinate
        /// </summary>
        public readonly double Y;

        /// <summary>
        /// Initializes a new instance of the <see cref="Point2d"/> struct.
        /// Creates a point for given coordinates (x, y)
        /// </summary>
        /// <param name="x">The x coordinate</param>
        /// <param name="y">The y coordinate</param>
        public Point2d(double x, double y)
        {
            this.X = x;
            this.Y = y;
        }

        /// <summary>
        /// Gets a point at the origin (0,0)
        /// </summary>
        public static Point2d Origin => new Point2d(0, 0);

        /// <summary>
        /// Adds a point and a vector together
        /// </summary>
        /// <param name="point">A point</param>
        /// <param name="vector">A vector</param>
        /// <returns>A new point at the summed location</returns>
        public static Point2d operator +(Point2d point, Vector2d vector)
        {
            return new Point2d(point.X + vector.X, point.Y + vector.Y);
        }

        /// <summary>
        /// Subtracts a vector from a point
        /// </summary>
        /// <param name="left">A point</param>
        /// <param name="right">A vector</param>
        /// <returns>A new point at the difference</returns>
        public static Point2d operator -(Point2d left, Vector2d right)
        {
            return new Point2d(left.X - right.X, left.Y - right.Y);
        }

        /// <summary>
        /// Subtracts the first point from the second point
        /// </summary>
        /// <param name="left">The first point</param>
        /// <param name="right">The second point</param>
        /// <returns>A vector pointing to the difference</returns>
        public static Vector2d operator -(Point2d left, Point2d right)
        {
            return new Vector2d(left.X - right.X, left.Y - right.Y);
        }

        /// <summary>
        /// Returns a value that indicates whether each pair of elements in two specified points is equal.
        /// </summary>
        /// <param name="left">The first point to compare</param>
        /// <param name="right">The second point to compare</param>
        /// <returns>True if the points are the same; otherwise false.</returns>
        public static bool operator ==(Point2d left, Point2d right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Returns a value that indicates whether any pair of elements in two specified points is not equal.
        /// </summary>
        /// <param name="left">The first point to compare</param>
        /// <param name="right">The second point to compare</param>
        /// <returns>True if the points are different; otherwise false.</returns>
        public static bool operator !=(Point2d left, Point2d right)
        {
            return !left.Equals(right);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Point2d"/> struct.
        /// Creates a point r from origin rotated a counterclockwise from X-Axis
        /// </summary>
        /// <param name="radius">distance from origin</param>
        /// <param name="angle">the angle</param>
        /// <returns>The <see cref="Point2d"/></returns>
        public static Point2d FromPolar(double radius, Angle angle)
        {
            if (radius < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(radius), radius, "Expected a radius greater than or equal to zero.");
            }

            return new Point2d(
                radius * Math.Cos(angle.Radians),
                radius * Math.Sin(angle.Radians));
        }

        /// <summary>
        /// Returns the centeroid or center of mass of any set of points
        /// </summary>
        /// <param name="points">a list of points</param>
        /// <returns>the centeroid point</returns>
        public static Point2d Centroid(IEnumerable<Point2d> points)
        {
            return Centroid(points.ToArray());
        }

        /// <summary>
        /// Returns the centeroid or center of mass of any set of points
        /// </summary>
        /// <param name="points">a list of points</param>
        /// <returns>the centeroid point</returns>
        public static Point2d Centroid(params Point2d[] points)
        {
            return new Point2d(
                points.Average(point => point.X),
                points.Average(point => point.Y));
        }

        /// <summary>
        /// Returns a point midway between the provided points <paramref name="point1"/> and <paramref name="point2"/>
        /// </summary>
        /// <param name="point1">point A</param>
        /// <param name="point2">point B</param>
        /// <returns>a new point midway between the provided points</returns>
        public static Point2d MidPoint(Point2d point1, Point2d point2)
        {
            return Centroid(point1, point2);
        }

        ///// <summary>
        ///// Create a new Point2d from a Math.NET Numerics vector of length 2.
        ///// </summary>
        ///// <param name="vector"> A vector with length 2 to populate the created instance with.</param>
        ///// <returns> A <see cref="Point2d"/></returns>
        //public static Point2d OfVector(Vector<double> vector)
        //{
        //    if (vector.Count != 2)
        //    {
        //        throw new ArgumentException("The vector length must be 2 in order to convert it to a Point2d");
        //    }

        //    return new Point2d(vector.At(0), vector.At(1));
        //}

        ///// <summary>
        ///// Applies a transform matrix to the point
        ///// </summary>
        ///// <param name="m">A transform matrix</param>
        ///// <returns>A new point</returns>
        //public Point2d TransformBy(Matrix<double> m)
        //{
        //    return OfVector(m.Multiply(this.ToVector()));
        //}

        /// <summary>
        /// Gets a vector from this point to another point
        /// </summary>
        /// <param name="otherPoint">The point to which the vector should go</param>
        /// <returns>A vector pointing to the other point.</returns>
        public Vector2d VectorTo(Point2d otherPoint)
        {
            return otherPoint - this;
        }

        /// <summary>
        /// Finds the straight line distance to another point
        /// </summary>
        /// <param name="otherPoint">The other point</param>
        /// <returns>a distance measure</returns>
        public double DistanceTo(Point2d otherPoint)
        {
            var vector = this.VectorTo(otherPoint);
            return vector.Length;
        }

        /// <summary>
        /// Converts this point into a vector from the origin
        /// </summary>
        /// <returns>A vector equivalent to this point</returns>
        [Pure]
        public Vector2d ToVector2d()
        {
            return new Vector2d(this.X, this.Y);
        }

        ///// <summary>
        ///// Convert to a Math.NET Numerics dense vector of length 2.
        ///// </summary>
        ///// <returns> A <see cref="Vector{Double}"/> with the x and y values from this instance.</returns>
        //public Vector<double> ToVector()
        //{
        //    return Vector<double>.Build.Dense(new[] { this.X, this.Y });
        //}

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
        [Pure]
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

        /// <summary>
        /// Returns a value to indicate if a pair of points are equal
        /// </summary>
        /// <param name="other">The point to compare against.</param>
        /// <param name="tolerance">A tolerance (epsilon) to adjust for floating point error</param>
        /// <returns>true if the points are equal; otherwise false</returns>
        public bool Equals(Point2d other, double tolerance)
        {
            if (tolerance < 0)
            {
                throw new ArgumentException("epsilon < 0");
            }

            return Math.Abs(other.X - this.X) < tolerance &&
                   Math.Abs(other.Y - this.Y) < tolerance;
        }

        /// <inheritdoc />
        public bool Equals(Point2d other) => this.X.Equals(other.X) && this.Y.Equals(other.Y);

        /// <inheritdoc />
        public override bool Equals(object obj) => obj is Point2d p && this.Equals(p);

        /// <inheritdoc />
        public override int GetHashCode() => this.X.GetHashCode() * 13 + this.Y.GetHashCode();
    }
}
