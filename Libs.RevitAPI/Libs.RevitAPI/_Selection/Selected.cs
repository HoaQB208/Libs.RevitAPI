using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System.Collections.Generic;
using System.Linq;

namespace Libs.RevitAPI._Selection
{
    public class Selected
    {
        public static List<ElementId> SelectedIds(UIDocument uidoc)
        {
            List<ElementId> ids = new List<ElementId>();
            try { ids = uidoc.Selection.GetElementIds().ToList(); } catch { }
            return ids;
        }


        public static List<Element> SelectedEles(UIDocument uidoc)
        {
            List<Element> eles = new List<Element>();
            try
            {
                ICollection<ElementId> ids = uidoc.Selection.GetElementIds();
                Document doc = uidoc.Document;
                foreach (ElementId id in ids)
                {
                    Element el = doc.GetElement(id);
                    if (el != null) eles.Add(el);
                }
            }
            catch { }
            return eles;
        }
    }
}