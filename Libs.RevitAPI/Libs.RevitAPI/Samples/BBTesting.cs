using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Libs.RevitAPI._Create;
using Libs.RevitAPI._Selection;

namespace Libs.RevitAPI.Samples
{
    /// <summary>
    /// Hàm này dùng để kiểm tra BoundingBox của 1 Element
    /// </summary>
    [Transaction(TransactionMode.Manual)]
    public class BBTesting : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uidoc = commandData.Application.ActiveUIDocument;
            Document doc = uidoc.Document;
            Element el = SelectByPick.Element(uidoc);
            if (el != null) CreateDirectShape.Create(doc, el.get_BoundingBox(null));
            return Result.Succeeded;
        }
    }
}