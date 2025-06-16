using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Libs.RevitAPI._Distince
{
    public class DistincePoint : IEqualityComparer<XYZ>
    {
        public bool Equals(XYZ x, XYZ y)
        {
            return x.IsAlmostEqualTo(y);
        }

        public int GetHashCode(XYZ obj)
        {
            return 1;
        }
    }


    public class XYZEqualityComparer : IEqualityComparer<XYZ>
    {
        public bool Equals(XYZ p1, XYZ p2)
        {
            // So sánh các điểm XYZ với độ chính xác cao
            return p1.IsAlmostEqualTo(p2);
        }

        public int GetHashCode(XYZ point)
        {
            // Tạo hash code từ XYZ
            return point.X.GetHashCode() ^ point.Y.GetHashCode() ^ point.Z.GetHashCode();
        }
    }

}
