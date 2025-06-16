using Autodesk.Revit.DB;
using System.Collections.Generic;

namespace Libs.RevitAPI._Create
{
    public class CreateSolid
    {
        public static Solid Create(Face face, XYZ vector, double distance)
        {
            return GeometryCreationUtilities.CreateExtrusionGeometry(face.GetEdgesAsCurveLoops(), vector, distance);
        }

        public static Solid Create(List<CurveLoop> curveLoops, XYZ vector, double distance)
        {
            return GeometryCreationUtilities.CreateExtrusionGeometry(curveLoops, vector, distance);
        }
    }
}