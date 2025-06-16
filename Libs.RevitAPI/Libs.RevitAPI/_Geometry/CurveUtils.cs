using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Libs.RevitAPI._Geometry
{
    public class CurveUtils
    {
        public static Line GetLineOfGrid(Document doc, Grid grid)
        {
            Line result = null;

            Options opt = new Options
            {
                ComputeReferences = true,
                IncludeNonVisibleObjects = false,
                View = doc.ActiveView
            };
            GeometryElement geos = grid.get_Geometry(opt);

            foreach (GeometryObject geo in geos)
            {
                if (geo is Line ln)
                {
                    result = ln; break;
                }
            }

            return result;
        }

        public static List<Curve> GetCurves(Face face)
        {
            IList<CurveLoop> curveLoops = face.GetEdgesAsCurveLoops();
            return GetCurves(curveLoops.ToList());
        }

        public static List<Curve> GetCurves(List<CurveLoop> curveLoops)
        {
            List<Curve> curves = new List<Curve>();
            foreach (CurveLoop cvl in curveLoops)
            {
                foreach (Curve cv in cvl) curves.Add(cv);
            }
            return curves;
        }

        public static CurveLoop GetCurveLoop(List<Curve> curves)
        {
            CurveLoop curveLoop = new CurveLoop();
            foreach (Curve cv in curves) curveLoop.Append(cv);
            return curveLoop;
        }

        public static CurveLoop GetCurveLoop(List<XYZ> points, bool makeClosed = true)
        {
            return GetCurveLoop(GetCurves(points, makeClosed));
        }

        public static List<Curve> GetCurves(List<XYZ> points, bool makeClosed = true)
        {
            List<Curve> curves = new List<Curve>();

            for (int i = 1; i < points.Count; i++)
            {
                var start = points[i - 1];
                var end = points[i];
                var curve = Line.CreateBound(start, end);
                curves.Add(curve);
            }
            if (makeClosed)
            {
                var start = points.Last();
                var end = points[0];
                var curve = Line.CreateBound(start, end);
                curves.Add(curve);
            }
            return curves;
        }



        public static bool IsInsideEntire(Curve curveCheck, List<Curve> curvesForChecking)
        {
            foreach (Curve curveTarget in curvesForChecking)
            {
                if (IsInsideEntire(curveCheck, curveTarget))
                {
                    return true;
                }
            }
            return false;
        }
        public static bool IsInsideEntire( Curve curveCheck, Curve curveTarget)
        {
            bool result;
            try
            {
                XYZ endPoint = curveCheck.GetEndPoint(0);
                XYZ endPoint2 = curveCheck.GetEndPoint(1);
                result = (IsContains(curveTarget,endPoint, 0.0001) && IsContains(curveTarget, endPoint2, 0.0001));
            }
            catch
            {
                result = false;
            }
            return result;
        }
        public static bool IsContains( Curve c, XYZ p, double tolerance = 0.0001)
        {
            if (c.IsBound)
            {
                try
                {
                    XYZ endPoint = c.GetEndPoint(0);
                    XYZ endPoint2 = c.GetEndPoint(1);
                    double num = endPoint.DistanceTo(endPoint2);
                    double num2 = endPoint.DistanceTo(p);
                    double num3 = p.DistanceTo(endPoint2);
                    return Math.Abs(num2 + num3 - num) < tolerance;
                }
                catch (Exception)
                {
                    return false;
                }
            }
            return false;
        }
        public static bool IsIntersection(Curve c1, Curve c2)
        {
            IntersectionResultArray intersectionResultArray;
            SetComparisonResult setComparisonResult = c1.Intersect(c2, out intersectionResultArray);
            return setComparisonResult == (SetComparisonResult)(8) && intersectionResultArray != null && intersectionResultArray.Size == 1;
        }
        public static XYZ Intersection(Curve c1, Curve c2)
        {
            IntersectionResultArray intersectionResultArray;
            c1.Intersect(c2, out intersectionResultArray);
            if (intersectionResultArray != null)
            {
                if (intersectionResultArray.Size > 0) return intersectionResultArray.get_Item(0).XYZPoint;
                else return null;
            }
            else return null;
        }
    }
}