using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;

namespace ImportantClasses.Math
{
    /// <summary>
    /// a 3-dimensional vector
    /// </summary>
    public class Vector3 : Vector
    {
        #region variables
        /// <summary>
        /// The number of dimensions of the vector
        /// </summary>
        public override int Dimensions => 3;

        /// <summary>
        /// the geometric length of the vector
        /// </summary>
        public double Magnitude
        {
            get { return Norm(2); }
            set
            {
                Vector v = Normalize(2)*value;
                this[0] = v[0];
                this[1] = v[1];
                this[2] = v[2];
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

        /// <summary>
        /// the z-coordinate of the vector
        /// </summary>
        public double Z { get { return this[2]; } set { this[2] = value; } }
        #endregion

        #region functions
        /// <summary>
        /// a 2-dimensional vector
        /// </summary>
        public Vector3() : base (3) { }

        /// <summary>
        /// a 2-dimensional vector
        /// </summary>
        /// <param name="x">the x-coordinate of the vector</param>
        /// <param name="y">the y-coordinate of the vector</param>
        /// <param name="z">the z-coordinate of the vector</param>
        public Vector3(double x, double y, double z) :base (new[] { x, y, z }) { }

        /// <summary>
        /// calculates the cross product of this x other
        /// </summary>
        /// <param name="other">the other Vector of the cross product</param>
        /// <returns>the resulting vector</returns>
        public Vector3 CrossProduct(Vector3 other)
        {
            if (other == null)
                throw new NullReferenceException("the other vector of the cross product cannot be null");
            Vector3 buffer = new Vector3();
            buffer[0] = this[1]*other[2] - this[2]*other[1];
            buffer[1] = this[2]*other[0] - this[0]*other[2];
            buffer[2] = this[0]*other[1] - this[1]*other[0];
            return buffer;
        }

        /// <summary>
        /// calculates the spat product of (this x b) * c
        /// </summary>
        /// <param name="b">the second vector the the spat product</param>
        /// <param name="c">the third vector the the spat product</param>
        /// <returns>the result of the spat product</returns>
        public double SpatProduct(Vector3 b, Vector3 c)
        {
            if(b==null||c==null)
                throw new NullReferenceException("the vector b or c was null");
            return CrossProduct(b)*c;
        }
        
        /// <summary>
        /// calculates the angel between 2 vectors
        /// </summary>
        /// <param name="other">the other vector</param>
        /// <returns>the angle between the 2 vectors in radiant</returns>
        public double GetEnclosedAngle(Vector3 other)
        {
            if (Magnitude.Equals(0) || other.Magnitude.Equals(0))
                return 0;
            double buffer = (this*other)/(Magnitude*other.Magnitude);
            //the arccos is defined between -1 and 1
            if (buffer > 1)
                buffer = buffer - ((int)buffer) * 2;
            if (buffer < -1)
                buffer = buffer + ((int)buffer) * 2;
            return System.Math.Acos(buffer);
        }
        #endregion
    }
}
