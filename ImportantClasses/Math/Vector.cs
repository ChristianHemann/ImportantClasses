using System;
using System.Collections;
using System.Text;

namespace ImportantClasses.Math
{
    /// <summary>
    /// a n-dimensional vector
    /// </summary>
    public class Vector : IEnumerable
    {
        #region variables
        /// <summary>
        /// The number of dimensions of the vector
        /// </summary>
        public virtual int Dimensions
        {
            get { return Values.Length;}
            set
            {
                if (value < 1)
                    throw new IndexOutOfRangeException("A Vector cannot have less than one dimension");
                double[] buffer = new double[value];
                for (int i = 0; i < value && i < Values.Length; i++)
                {
                    buffer[i] = Values[i];
                }
                _values = buffer;
            }
        }

        /// <summary>
        /// the raw values of the vector
        /// </summary>
        public double[] Values => _values;

        private double[] _values;

        #endregion

        #region functions
        /// <summary>
        /// a n-dimensional vector
        /// </summary>
        /// <param name="dimesions">the number of dimensions</param>
        public Vector(int dimesions = 1)
        {
            if (dimesions < 1)
                throw new IndexOutOfRangeException("A Vector cannot have less than one dimension");
            _values = new double[dimesions];
        }

        /// <summary>
        /// a n-dimensional vector
        /// </summary>
        /// <param name="values">the values of the vector</param>
        public Vector(double[] values)
        {
            _values = new double[values.Length];
            for (int i = 0; i < values.Length; i++) //clone the array
            {
                Values[i] = values[i];
            }
        }

        /// <summary>
        /// creates a clone of this vector
        /// </summary>
        /// <returns>the cloned vector</returns>
        public Vector Clone()
        {
            Vector buffer = new Vector(Dimensions);
            for (int i = 0; i < Dimensions; i++)
            {
                buffer[i] = this[i];
            }
            return buffer;
        }

        /// <summary>
        /// calculates the scalar product of this vector with another vector
        /// </summary>
        /// <param name="other">the other vector of the product</param>
        /// <returns>the calculated scalar</returns>
        public double ScalarProduct(Vector other)
        {
            if (Dimensions != other.Dimensions)
                throw new DimensionMismatchException(
                    "The scalar product can only be calculated for vectors of the same length");
            double result = 0;
            for (int i = 0; i < Dimensions; i++)
            {
                result += this[i]*other[i];
            }
            return result;
        }

        /// <summary>
        /// returns the norm of this vector according to the given p-norm
        /// </summary>
        /// <param name="pNorm">the p-norm to calculate</param>
        /// <returns>the norm of the vector</returns>
        public double Norm(double pNorm)
        {
            double buffer = 0;
            for (int i = 0; i < Dimensions; i++)
            {
                buffer += System.Math.Pow(this[i], pNorm);
            }
            return System.Math.Pow(buffer, 1/pNorm);
        }

        /// <summary>
        /// normalize the vector to a lenght of 1 according to the given p-norm
        /// </summary>
        /// <param name="pNorm">the p-normal to normalite the vector</param>
        /// <returns>the normalized vector</returns>
        public Vector Normalize(double pNorm)
        {
            return this/Norm(pNorm);
        }

        /// <summary>
        /// implementation of the IEnumerable interface
        /// </summary>
        /// <returns>the Enumerator of the vector</returns>
        public IEnumerator GetEnumerator()
        {
            return Values.GetEnumerator();
        }

        /// <summary>
        /// converts all informations into a string
        /// </summary>
        /// <returns>a string containing the number of dimensions and all values of the vector</returns>
        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(Dimensions);
            builder.Append("-dimensional Vector");
            for (int i = 0; i < Dimensions; i++)
            {
                builder.Append(" [");
                builder.Append(i);
                builder.Append("] = ");
                builder.Append(this[i]);
            }
            return builder.ToString();
        }

        /// <summary>
        /// checks if this vector is equal to another object
        /// </summary>
        /// <param name="obj">the other object to compare</param>
        /// <param name="maxDifference">the maximum reasonable difference between each value of the 2 vectors. Often used to compensate loss of precision.</param>
        /// <returns>true if the vectors have the same values within the given difference</returns>
        public bool Equals(object obj, double maxDifference)
        {
            if (obj == null)
                return false;
            if (!(obj is Vector))
                return false;
            Vector v = (Vector) obj;
            if (v.Dimensions != Dimensions)
                return false;
            for (int i = 0; i < Dimensions; i++)
            {
                if ((this[i] > v[i] + maxDifference) ||
                    (this[i]+maxDifference < v[i]))
                    return false;
            }
            return true;
        }

        /// <summary>
        /// checks if this vector is equal to another vector
        /// </summary>
        /// <param name="obj">the other object to compare</param>
        /// <returns>true if the vectors have the same values</returns>
        public override bool Equals(object obj)
        {
            return Equals(obj, 0);
        }

        /// <summary>
        /// converts this vector into a 3-dimensional vector.\n
        /// If this vector has more dimensions they will be lost
        /// </summary>
        /// <returns>the first 3 dimensions of this vector</returns>
        public Vector3 ToVector3()
        {
            Vector3 buffer = new Vector3();
            buffer[0] = this[0];
            if (Dimensions > 1)
                buffer[1] = this[1];
            if (Dimensions > 2)
                buffer[2] = this[2];
            return buffer;
        }

        /// <summary>
        /// converts this vector into a 2-dimensional vector.\n
        /// If this vector has more dimensions they will be lost
        /// </summary>
        /// <returns>the first 2 dimensions of this vector</returns>
        public Vector2 ToVector2()
        {
            Vector2 buffer = new Vector2();
            buffer[0] = this[0];
            if (Dimensions > 1)
                buffer[1] = this[1];
            return buffer;
        }

        /// <summary>
        /// converts this vector into an array which contains its values
        /// </summary>
        /// <returns>the values of this vector</returns>
        public double[] ToArray()
        {
            return Clone()._values;
        }

        #endregion

        #region operators
        /// <summary>
        /// returns the value number i of the vector started with zero (0)
        /// </summary>
        /// <param name="i">the index to get the value from started with 0</param>
        /// <returns>the value at position i</returns>
        public double this[int i]
        {
            get
            {
                if (!(i < Dimensions))
                    throw new IndexOutOfRangeException("You tried to access the element number " + i + " in a " + Dimensions + "-diemnsional vector");
                return Values[i];
            }
            set { Values[i] = value; }
        }

        public static Vector operator +(Vector v1, Vector v2)
        {
            Vector buffer;
            if (v1.Dimensions > v2.Dimensions)
            {
                buffer = v1.Clone();
                for (int i = 0; i < v2.Dimensions; i++)
                {
                    buffer[i] += v2[i];
                }
            }
            else
            {
                buffer = v2.Clone();
                for (int i = 0; i < v1.Dimensions; i++)
                {
                    buffer[i] += v1[i];
                }
            }
            return buffer;
        }

        public static Vector operator -(Vector v)
        {
            Vector buffer = new Vector(v.Dimensions);
            for (int i = 0; i < v.Dimensions; i++)
            {
                buffer[i] = -v[i];
            }
            return buffer;
        }

        public static Vector operator -(Vector v1, Vector v2)
        {
            return v1 + -v2;
        }

        /// <summary>
        /// calculates the scalar product of two vectors
        /// </summary>
        /// <param name="v1">the first vector</param>
        /// <param name="v2">the second vector</param>
        /// <returns>the calculated scalar product</returns>
        public static double operator *(Vector v1, Vector v2)
        {
            return v1.ScalarProduct(v2);
        }

        public static Vector operator *(Vector v, double factor)
        {
            Vector buffer = v.Clone();
            for (int i = 0; i < buffer.Dimensions; i++)
            {
                buffer[i] *= factor;
            }
            return buffer;
        }

        public static Vector operator *(double factor, Vector v)
        {
            return v*factor;
        }

        public static Vector operator /(Vector v, double divisor)
        {
            Vector buffer = v.Clone();
            for (int i = 0; i < buffer.Dimensions; i++)
            {
                buffer[i] /= divisor;
            }
            return buffer;
        }

        #endregion
    }
}
