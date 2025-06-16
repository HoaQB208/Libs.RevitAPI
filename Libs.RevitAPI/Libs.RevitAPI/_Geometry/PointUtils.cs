using Autodesk.Revit.DB;
using System.Collections.Generic;
using System;
using System.Linq;

namespace Libs.RevitAPI._Geometry
{
    public class PointUtils
    {
        public static XYZ GetMiddlePoint(Line line)
        {
            return line.GetEndPoint(0).Add(line.GetEndPoint(1)).Divide(2.0);
        }

        public static List<XYZ> GetPointsOfCurves(List<Curve> curves)
        {
            List<XYZ> list = new List<XYZ>();
            foreach (Curve curve in curves)
            {
                list.Add(curve.GetEndPoint(0));
                list.Add(curve.GetEndPoint(1));
            }
            return list.Distinct(new XYZEqualityComparer()).ToList<XYZ>();
        }

        public static XYZ GetMidPoints(XYZ pt1, XYZ pt2)
        {
            return new XYZ((pt1.X + pt2.X) / 2, (pt1.Y + pt2.Y) / 2, (pt1.Z + pt2.Z) / 2);
        }

        public static List<XYZ> GetIntersectionsWithPlane(Curve curve, Plane plane, double tolerance)
        {
            List<XYZ> intersectionPoints = new List<XYZ>();
            if (curve is Line line)
            {
                XYZ intersectionPoint;
                double parameter;
                Plane_Line result = Intersect(plane, line, tolerance, out intersectionPoint, out parameter);
                if (result == Plane_Line.Intersecting && intersectionPoint != null)
                {
                    intersectionPoints.Add(intersectionPoint);
                }
            }
            return intersectionPoints;
        }
        public static FamilyInstance FindFamilyInstanceAtPoint(Document doc, XYZ point, ElementId hostId)
        {
            return new FilteredElementCollector(doc)
                .OfClass(typeof(FamilyInstance))
                .Cast<FamilyInstance>()
                .FirstOrDefault(fi => fi.Location is LocationPoint locationPoint &&
                                      locationPoint.Point.IsAlmostEqualTo(point) &&
                                      fi.Host.Id == hostId);
        }
        public static XYZ GetIntersectionWithPlane(Curve curve, Plane plane, double tolerance)
        {
            XYZ intersectionPoint = new XYZ();
            if (curve is Line line)
            {
                double parameter;
                Plane_Line result = Intersect(plane, line, tolerance, out intersectionPoint, out parameter);
                if (result == Plane_Line.Intersecting && intersectionPoint != null)
                {
                    intersectionPoint.Add(intersectionPoint);
                }
            }
            return intersectionPoint;
        }
        public static XYZ FindIntersectionPoint(Curve curvePipe, Face floorFace)
        {

            if (floorFace.Intersect(curvePipe, out IntersectionResultArray intersectionR) != SetComparisonResult.Disjoint && intersectionR != null && !intersectionR.IsEmpty)
            {
                return intersectionR.get_Item(0).XYZPoint;
            }
            return null;
        }
        //public static XYZ FindIntersectionSolidAndCurve(Solid solid , Curve curve)
        //{
        //    SolidCurveIntersectionOptions options = new() { ResultType = SolidCurveIntersectionMode.CurveSegmentsInside };
        //    SolidCurveIntersection intersectionResult = solid.IntersectWithCurve(curve, options);
        //    if (intersectionResult == null || intersectionResult.SegmentCount == 0) continue;
        //}

        public static XYZ FindIntersectionPoint(Curve curvePipe, Face floorFace, double extensionLength)
        {
            // Mở rộng Curve trước khi tìm giao điểm
            Curve extendedCurve = ExtendCurve(curvePipe, extensionLength);

            // Tìm giao điểm giữa mặt và Curve đã được mở rộng
            if (floorFace.Intersect(extendedCurve, out IntersectionResultArray intersectionR) != SetComparisonResult.Disjoint && intersectionR != null && !intersectionR.IsEmpty)
            {
                return intersectionR.get_Item(0).XYZPoint;
            }
            return null;
        }
        public static Curve ExtendCurve(Curve curve, double extensionLength)
        {
            // Lấy điểm đầu và điểm cuối của Curve
            XYZ start = curve.GetEndPoint(0);
            XYZ end = curve.GetEndPoint(1);

            // Tính hướng của Curve (từ điểm đầu đến điểm cuối)
            XYZ direction = (end - start).Normalize();

            // Tính toán các điểm mở rộng mới
            XYZ newStart = start - direction * extensionLength; // Mở rộng điểm đầu
            XYZ newEnd = end + direction * extensionLength;     // Mở rộng điểm cuối

            // Tạo lại đường cong mới dựa trên loại Curve gốc
            if (curve is Line)
            {
                // Nếu Curve là một đường thẳng, tạo lại đường thẳng mới
                return Line.CreateBound(newStart, newEnd);
            }
            else if (curve is Arc)
            {
                // Nếu Curve là một cung tròn, bạn sẽ cần thêm logic để mở rộng cung tròn
                // Đây là ví dụ cơ bản, có thể cần tùy chỉnh thêm tùy thuộc vào cung tròn cụ thể
                Arc arc = curve as Arc;
                return Arc.Create(newStart, newEnd, arc.Center);
            }

            throw new InvalidOperationException("Chỉ hỗ trợ đường thẳng và cung tròn.");
        }
        private static Plane_Line Intersect(Plane p, Line l, double tolerance, out XYZ intersectionPoint, out double parameter)
        {
            //compute the dot prodcut and signed distance 
            double denominator = l.Direction.DotProduct(p.Normal);
            double numerator = (p.Origin - l.GetEndPoint(0)).DotProduct(p.Normal);
            //check if the dot product is almost zero 
            if (Math.Abs(denominator) < tolerance)
            {
                // line is parallel to plane (could be inside or outside the plane)
                if (Math.Abs(numerator) < tolerance)
                {
                    //line is inside the plane
                    intersectionPoint = null;
                    parameter = double.NaN;
                    return Plane_Line.Subset;
                }
                else
                {
                    // line is outside the plane                    
                    intersectionPoint = null;
                    parameter = double.NaN;
                    return Plane_Line.Disjoint;
                }
            }
            else
            {
                // line is intersecting wih plane
                // compute the line paramemer 
                parameter = numerator / denominator;
                intersectionPoint = l.GetEndPoint(0) + parameter * l.Direction;
                return Plane_Line.Intersecting;
            }

        }

        public enum Plane_Line
        {
            Subset,
            Disjoint,
            Intersecting
        }
    }


    class XYZEqualityComparer : IEqualityComparer<XYZ>, IEqualityComparer<UV>
    {
        public bool Equals(UV first, UV second)
        {
            return first != null && second != null && Math.Abs(first.U - second.U) < 0.0001 && Math.Abs(first.V - second.V) < 0.0001;
        }
        public bool Equals(XYZ first, XYZ second)
        {
            return first != null && second != null && (Math.Abs(first.Z - second.Z) < 0.0001 && Math.Abs(first.X - second.X) < 0.0001) && Math.Abs(first.Y - second.Y) < 0.0001;
        }
        public int GetHashCode(UV obj)
        {
            return 0;
        }
        public int GetHashCode(XYZ obj)
        {
            return 0;
        }
    }
}
