using Autodesk.Revit.DB;

namespace Libs.RevitAPI._Join
{
    public class Join
    {
        public static string JoinObject(Document doc, Element el1, Element el2)
        {
            WarningDiscard.HasWarning = false;
            try
            {
                if (JoinGeometryUtils.AreElementsJoined(doc, el1, el2))
                {
                    if (JoinGeometryUtils.IsCuttingElementInJoin(doc, el2, el1))
                    {
                        JoinGeometryUtils.SwitchJoinOrder(doc, el1, el2);
                    }
                }
                else
                {
                    JoinGeometryUtils.JoinGeometry(doc, el1, el2);
                }
            }
            catch { }
            return WarningDiscard.HasWarning ? $"{el1.Id}\t{el1.Name}\tjoin with\t{el2.Id}\t{el2.Name}" : "";
        }
    }
}
