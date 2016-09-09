using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ImportantClasses.Math
{
    /// <summary>
    /// a 2-dimensional vector
    /// </summary>
    public class Vector2 : Vector
    {
        #region variables

        /// <summary>
        /// The number of dimensions of the vector
        /// </summary>
        public override int Dimensions => 2;

        /// <summary>
        /// the geometric length of the vector
        /// </summary>
        public double Magnitude
        {
            get { return Norm(2); }
            set
            {
                Vector v = Normalize(2) * value;
                this[0] = v[0];
                this[1] = v[1];
            }
        }

        /// <summary>
        /// the x-coordinate of the vector
        /// </summary>
        public double X { get { return this[0]; } set { this[0] = value; } }

        /// <summary>
        /// the y-coordinate of the vector
        /// </summary>
        public double Y { get { return this[1]; } set { this[1] = value; } }

        #endregion
        #region functions

        /// <summary>
        /// a 2-dimensional vector
        /// </summary>
        public Vector2() : base(2) { }

        /// <summary>
        /// a 2-dimensional vector
        /// </summary>
        /// <param name="x">the x-coordinate of the vector</param>
        /// <param name="y">the y-coordinate of the vector</param>
        public Vector2(double x, double y) : base(new[] { x, y }) { }

        /// <summary>
        /// calculates the angel between 2 vectors
        /// </summary>
        /// <param name="other">the other vector</param>
        /// <returns>the angle between the 2 vectors in radiant</returns>
        public double GetEnclosedAngle(Vector2 other)
        {
            if (Magnitude.Equals(0) || other.Magnitude.Equals(0))
                return 0;
            double buffer = (this * other) / (Magnitude * other.Magnitude);
            //the arccos is defined between -1 and 1
            if (buffer > 1)
                buffer = buffer - ((int)buffer) * 2;
            if (buffer < -1)
                buffer = buffer + ((int)buffer) * 2;
            return System.Math.Acos(buffer);
        }

        #endregion

        /// <summary>
        /// converts a 2-dimensional vector into a 3-dimensional vector
        /// </summary>
        /// <param name="v"></param>
        public static implicit operator Vector3(Vector2 v)
        {
            return new Vector3(v.X, v.Y, 0);
        }
    }
}
