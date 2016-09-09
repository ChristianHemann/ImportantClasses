using System.Collections.Generic;
using System.Linq;
using ImportantClasses.Math;

namespace ImportantClasses
{
    public static class Extensions
    {
        /// <summary>
        /// checks if the elements of an IEnumarable has the same values than the elements of the other IEnumerable
        /// </summary>
        /// <param name="value1">The first IEnumerable</param>
        /// <param name="value2">the second IEnumerable</param>
        /// <returns>true if each element of the first IEnumerable equals the element of the second</returns>
        public static bool ElementWiseEqual(this IEnumerable<object> value1, IEnumerable<object> value2)
        {
            if (value1.Count() != value2.Count())
                return false;

            IEnumerator<object> enumerator1 = value1.GetEnumerator();
            IEnumerator<object> enumerator2 = value2.GetEnumerator();
            do
            {
                if (!enumerator1.Current.Equals(enumerator2.Current))
                    return false;
            }
            while (enumerator1.MoveNext() && enumerator2.MoveNext());
            return true;
        }

        /// <summary>
        /// creates a new vector based on the values of an array
        /// </summary>
        /// <param name="values">the values of the vector</param>
        /// <returns>the new vector</returns>
        public static Vector ToVector(this double[] values)
        {
            return new Vector(values);
        }

        /// <summary>
        /// creates a new vector based on the values of an array
        /// </summary>
        /// <param name="values">the values of the vector</param>
        /// <returns>the new vector</returns>
        public static Vector ToVector(this float[] values)
        {
            return new Vector(values.ToDoubleArray());
        }

        /// <summary>
        /// converts a float-array into a double-array
        /// </summary>
        /// <param name="arr">the array to convert</param>
        /// <returns>the generated array</returns>
        public static double[] ToDoubleArray(this float[] arr)
        {
            double[] buffer = new double[arr.Length];
            for (int i = 0; i < arr.Length; i++)
            {
                buffer[i] = arr[i];
            }
            return buffer;
        }
    }
}
