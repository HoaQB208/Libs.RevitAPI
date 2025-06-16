using Autodesk.Revit.DB;
using Libs.RevitAPI._Selection;
using System.Linq;

namespace Libs.RevitAPI._Create
{
    public class CreateText
    {
        public static void Create(Document doc, XYZ position, string textContent, TextNoteType textNoteType)
        {
            using (Transaction tr = new Transaction(doc))
            {
                tr.Start("CreateGrids Text");
                if (textNoteType == null) textNoteType = SelectByFilter.TextNoteTypes(doc).First();
                TextNote.Create(doc, doc.ActiveView.Id, position, textContent, textNoteType.Id);
                tr.Commit();
            }
        }
    }
}