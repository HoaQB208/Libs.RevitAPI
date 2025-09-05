using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Selection;

namespace Libs.RevitAPI._SelectionFilters
{
    public class LevelFilter : ISelectionFilter
    {
        public bool AllowElement(Element elem)
        {
            return elem.Category.Name == "Levels";
        }

        public bool AllowReference(Reference reference, XYZ position)
        {
            return false;
        }
    }
}