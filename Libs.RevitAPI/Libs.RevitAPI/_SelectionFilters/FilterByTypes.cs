using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Libs.RevitAPI._SelectionFilters
{
    /// <summary>
    /// Ví dụ sử dụng:  Reference re = uidoc.Selection.PickObject(ObjectType.Element, new FilterByTypes(typeof(ImportInstance)), msg);
    /// </summary>
    public class FilterByTypes : ISelectionFilter
    {
        private List<Guid> _TypeGuids = new List<Guid>();

        public FilterByTypes(Type type)
        {
            this._TypeGuids.Add(type.GUID);
        }
        public FilterByTypes(List<Type> types)
        {
            this._TypeGuids = (from category in types select category.GUID).ToList();
        }

        public bool AllowElement(Element elem)
        {
            return this._TypeGuids.Contains(elem.GetType().GUID);
        }
        public bool AllowReference(Reference reference, XYZ position)
        {
            return false;
        }
    }
}