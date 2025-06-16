using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace Libs.RevitAPI._Selection
{
    public class SelectPoint
    {
        public static XYZ Click(UIDocument uidoc, string msg = "")
        {
            XYZ result = null;
            try { result = uidoc.Selection.PickPoint(msg); } catch { }
            return result;
        }
    }
}