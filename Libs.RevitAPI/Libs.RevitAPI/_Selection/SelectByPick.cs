using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Selection;
using Autodesk.Revit.UI;
using Libs.RevitAPI._SelectionFilters;
using System;
using Libs.RevitAPI._Common;

namespace Libs.RevitAPI._Selection
{
    public class SelectByPick
    {
        public static Element Element(UIDocument uidoc, string msg = "Select Element")
        {
            Element el = null;
            try
            {
                Reference re = uidoc.Selection.PickObject(ObjectType.Element, msg);
                el = uidoc.Document.GetElement(re.ElementId);
            }
            catch { }
            return el;
        }

        public static XYZ Point(UIDocument uidoc, out string errror, string msg = "Pick point")
        {
            errror = "";
            XYZ point = null;
            try
            {
                 point = uidoc.Selection.PickPoint(msg);
            }
            catch (Exception ex) { errror = ex.Message; }
            return point;
        }


        public static ImportInstance AutoCADImport(UIDocument uidoc, string msg = "Select file AutoCAD import")
        {
            ImportInstance import = null;
            try
            {
                Reference re = uidoc.Selection.PickObject(ObjectType.Element, new FilterByTypes(typeof(ImportInstance)), msg);
                import = uidoc.Document.GetElement(re.ElementId) as ImportInstance;
            }
            catch { }
            return import;
        }

        public static RevitLinkInstance RevitLink(UIDocument uidoc, string msg = "Select file Revit link")
        {
            RevitLinkInstance result = null;
            try
            {
                Reference re = uidoc.Selection.PickObject(ObjectType.Element, new FilterByTypes(typeof(RevitLinkInstance)), msg);
                result = uidoc.Document.GetElement(re.ElementId) as RevitLinkInstance;
            }
            catch { }
            return result;
        }
    }
}