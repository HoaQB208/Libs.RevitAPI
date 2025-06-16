using Autodesk.Revit.DB;

namespace Libs.RevitAPI._Create
{
    public class CreateWall
    {
        public static void Create(Document doc, Curve curve, ElementId levelId, bool isStruct = false, bool isDisallowWallJoinAtEnd = false)
        {
            using Transaction tr = new (doc);
            tr.Start("CreateGrids Wall");
            Wall wa = Wall.Create(doc, curve, levelId, isStruct);
            if (isDisallowWallJoinAtEnd)
            {
                WallUtils.DisallowWallJoinAtEnd(wa, 0);
                WallUtils.DisallowWallJoinAtEnd(wa, 1);
            }
            tr.Commit();
        }
    }
}