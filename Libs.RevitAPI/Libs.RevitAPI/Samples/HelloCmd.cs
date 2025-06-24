using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace Libs.RevitAPI.Samples
{
    /// <summary>
    /// Lệnh này sẽ hiển thị thông báo 'Hello' lên màn hình
    /// </summary>
    [Transaction(TransactionMode.Manual)]
    public class HelloCmd : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            //UIDocument uidoc = commandData.Application.ActiveUIDocument;
            //Document doc = uidoc.Document;

            // Do something here...

            return Result.Succeeded;
        }
    }
}