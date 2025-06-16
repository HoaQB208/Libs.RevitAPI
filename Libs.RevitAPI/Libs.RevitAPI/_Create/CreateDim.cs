using Autodesk.Revit.DB;

namespace Libs.RevitAPI._Create
{
    public class CreateDim
    {
        public static void Create(Document doc, Line line, ReferenceArray refs, DimensionType dimStyle)
        {
            using (Transaction tr = new Transaction(doc))
            {
                tr.Start("CreateGrids Dim");
                doc.Create.NewDimension(doc.ActiveView, line, refs, dimStyle);
                tr.Commit();
            }
        }
    }
}