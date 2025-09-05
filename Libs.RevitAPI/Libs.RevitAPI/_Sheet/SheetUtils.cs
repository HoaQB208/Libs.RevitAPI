using Autodesk.Revit.DB;
using System.Collections.Generic;
using System.Linq;

namespace Libs.RevitAPI._Sheet
{
    public class SheetUtils
    {
        public static HashSet<string> GetSheetNumbers(Document doc)
        {
            return new FilteredElementCollector(doc).OfClass(typeof(ViewSheet)).WhereElementIsNotElementType().Cast<ViewSheet>().Where(x => x != null).Select(x => x.SheetNumber).ToHashSet();
        }
    }
}
