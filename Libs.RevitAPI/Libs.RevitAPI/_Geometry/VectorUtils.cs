using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Libs.RevitAPI._Geometry
{
    public static class VectorUtils
    {
        /// <summary>
        /// The boolean value that indicates whether the 2 vectors are parallel to each other.
        /// </summary>
        /// <param name="firstVector">The first vector to compare.</param>
        /// <param name="secondVector">The second vector to compare.</param>
        /// <returns></returns>
        public static bool IsParallel(this XYZ firstVector, XYZ secondVector)
        {
            var first = firstVector.Normalize();
            var second = secondVector.Normalize();
            double dot = first.DotProduct(second);
            return Math.Abs(dot).Equals(1);
        }
        /// <summary>
        /// The boolean value that indicates whether the 2 vectors are same direction.
        /// </summary>
        /// <param name="firstVector"></param>
        /// <param name="secondVector"></param>
        /// <returns></returns>
        public static bool IsSameDirection(this XYZ firstVector, XYZ secondVector)
        {
            var first = firstVector.Normalize();
            var second = secondVector.Normalize();
            double dot = first.DotProduct(second);
            return dot.Equals(1);
        }
        /// <summary>
        /// The boolean value that indicates whether the 2 vectors are opposite direction.
        /// </summary>
        /// <param name="firstVector"></param>
        /// <param name="secondVector"></param>
        /// <returns></returns>
        public static bool IsOppositeDirection(this XYZ firstVector, XYZ secondVector)
        {
            var first = firstVector.Normalize();
            var second = secondVector.Normalize();
            double dot = first.DotProduct(second);
            return dot.Equals(-1);
        }

        /// <summary>
        /// The boolean value that indicates whether the 2 vectors are perpendicular.
        /// </summary>
        /// <param name="firstVector"></param>
        /// <param name="secondVector"></param>
        /// <returns></returns>
        public static bool IsPerpendicular(this XYZ firstVector, XYZ secondVector)
        {
            double dot = firstVector.DotProduct(secondVector);
            return dot.Equals(0);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="firstVector"></param>
        /// <param name="secondVector"></param>
        /// <returns></returns>
        public static XYZ GetNormal(this XYZ firstVector, XYZ secondVector)
        {
            var normal1 = firstVector.Normalize();
            var normal2 = secondVector.Normalize();
            return normal1.CrossProduct(normal2);
        }
    }
}
