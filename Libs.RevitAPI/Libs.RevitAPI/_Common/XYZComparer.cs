using Autodesk.Revit.DB;
using System.Collections.Generic;
using System;

namespace Libs.RevitAPI._Common
{
    public class XYZComparer : IEqualityComparer<XYZ>
    {
        private readonly int _precision;

        public XYZComparer(int precision = 6)
        {
            _precision = precision;
        }

        public bool Equals(XYZ p1, XYZ p2)
        {
            return Math.Round(p1.X, _precision) == Math.Round(p2.X, _precision) &&
                   Math.Round(p1.Y, _precision) == Math.Round(p2.Y, _precision) &&
                   Math.Round(p1.Z, _precision) == Math.Round(p2.Z, _precision);
        }

        public int GetHashCode(XYZ p)
        {
            return (Math.Round(p.X, _precision), Math.Round(p.Y, _precision), Math.Round(p.Z, _precision)).GetHashCode();
        }
    }
}
