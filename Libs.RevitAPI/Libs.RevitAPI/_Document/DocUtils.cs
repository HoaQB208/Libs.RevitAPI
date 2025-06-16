using Autodesk.Revit.DB;
using System.Linq;

namespace Libs.RevitAPI._Document
{
    public class DocUtils
    {
        public static string GetPathName(Document doc)
        {
            return doc.PathName;
        }

        public static ElementId GetFirstFillPattern(Document doc)
        {
            FilteredElementCollector elements = new FilteredElementCollector(doc);
            return elements.OfClass(typeof(FillPatternElement)).Cast<FillPatternElement>().First(a => a.GetFillPattern().IsSolidFill).Id;
        }
    }
}