using System;
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
            IEnumerator<object> enumerator1 = value1.GetEnumerator();
            IEnumerator<object> enumerator2 = value2.GetEnumerator();
            bool enum1HasNext, enum2HasNext;
            do
            {
                if(enumerator1.Current == null && enumerator2.Current == null) { }
                else if (!enumerator1.Current?.Equals(enumerator2.Current)??false)
                    return false;
                enum1HasNext = enumerator1.MoveNext();
                enum2HasNext = enumerator2.MoveNext();
            }
            while (enum1HasNext&&enum2HasNext);
            return !(enum1HasNext || enum2HasNext);
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

        /// <summary>
        /// Clones a List of items which implements the IClonable-interface
        /// found this code at http://stackoverflow.com/questions/222598/how-do-i-clone-a-generic-list-in-c
        /// </summary>
        /// <typeparam name="T">the type of the List</typeparam>
        /// <param name="listToClone">the list which ought to be cloned</param>
        /// <returns>the cloned list</returns>
        public static List<T> Clone<T>(this List<T> listToClone) where T : ICloneable
        {
            return listToClone.Select(item => (T)item.Clone()).ToList();
        }
    }
}
