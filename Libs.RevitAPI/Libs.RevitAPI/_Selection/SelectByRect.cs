using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Selection;
using Autodesk.Revit.UI;
using System.Collections.Generic;
using System.Linq;

namespace Libs.RevitAPI._Selection
{
    public class SelectByRect
    {
        public static List<Element> Get(UIDocument uidoc, ISelectionFilter filter, string msg)
        {
            List<Element> eles = new List<Element>();
            try { uidoc.Selection.PickElementsByRectangle(filter, msg).ToList(); } catch { }
            return eles;
        }
    }
}