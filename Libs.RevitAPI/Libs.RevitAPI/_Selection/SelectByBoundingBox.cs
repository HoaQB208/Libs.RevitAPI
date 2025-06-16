using Autodesk.Revit.DB;
using System.Collections.Generic;
using System.Linq;
using Libs.RevitAPI._Common;

namespace Libs.RevitAPI._Selection
{
    public class SelectByBoundingBox
    {
        public static List<Element> GetElements(Document doc, BoundingBoxXYZ bb)
        {
            LogicalAndFilter filter = new LogicalAndFilter(new List<ElementFilter> { new BoundingBoxIntersectsFilter(new Outline(bb.Min, bb.Max), false) });
            return new FilteredElementCollector(doc, doc.ActiveView.Id).WherePasses(filter).ToList();
        }

        public static List<Element> GetElements(Document doc, XYZ min, XYZ max)
        {
            LogicalAndFilter filter = new LogicalAndFilter(new List<ElementFilter> { new BoundingBoxIntersectsFilter(new Outline(min, max), false) });
            return new FilteredElementCollector(doc, doc.ActiveView.Id).WherePasses(filter).ToList();
        }

        public static List<Element> GetElementsTouchBoundingBox(Document doc, Element el, double scaleBoudingBox = 1.05)
        {
            FilteredElementCollector filteredElementCollector = new FilteredElementCollector(doc);
            BoundingBoxXYZ boundingBoxXYZ = el.get_BoundingBox(doc.ActiveView);
            Outline outline = new Outline(boundingBoxXYZ.Min, boundingBoxXYZ.Max);
            outline.Scale(scaleBoudingBox);
            BoundingBoxIntersectsFilter boundingBoxIntersectsFilter = new BoundingBoxIntersectsFilter(outline, false);
            List<ElementFilter> listElementFilter = new List<ElementFilter>() { boundingBoxIntersectsFilter };
            LogicalAndFilter logicalAndFilter = new LogicalAndFilter(listElementFilter);
            List<Element> temp = filteredElementCollector.WherePasses(logicalAndFilter).Excluding(new List<ElementId> { el.Id }).WhereElementIsNotElementType().ToList();

            List<Element> result = new List<Element>();
            foreach (Element ele in temp) result.Add(ele);
            result.RemoveAll(x => x == null);
            return result;
        }
    }
}