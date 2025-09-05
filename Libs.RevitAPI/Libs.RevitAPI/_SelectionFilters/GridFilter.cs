using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Selection;

namespace Libs.RevitAPI._SelectionFilters
{
    public class GridFilter : ISelectionFilter
    {
        public bool AllowElement(Element elem)
        {
            return elem.Category.Name == "Grids";
        }

        public bool AllowReference(Reference reference, XYZ position)
        {
            return false;
        }
    }
}