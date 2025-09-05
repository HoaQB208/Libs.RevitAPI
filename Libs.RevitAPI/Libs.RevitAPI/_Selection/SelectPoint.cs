using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;

namespace Libs.RevitAPI._Selection
{
    public class SelectPoint
    {
        public static XYZ Click(UIDocument uidoc, out string error, string msg = "")
        {
            error = "";
            try
            {
                return uidoc.Selection.PickPoint(msg);
            }
            catch (Exception ex)
            {
                error = ex.Message;
            }
            return null;
        }
    }
}