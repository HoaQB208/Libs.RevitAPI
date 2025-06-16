using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using System;
using System.Collections.Generic;

namespace Libs.RevitAPI._Family
{
    public class PlaceFamily
    {
        public static void Place(Document doc, FamilySymbol symbol, XYZ location, Element host, double rotate = 0, StructuralType structuralType = StructuralType.NonStructural, Dictionary<string, object> parameters = null)
        {
            using (Transaction tr = new Transaction(doc))
            {
                tr.Start("Place Family");
                var fa = doc.Create.NewFamilyInstance(location, symbol, host, structuralType);
                if (rotate != 0)
                {
                    double rRadian = rotate * (Math.PI / 180.0);
                    Line rotationLineZ = Line.CreateBound(location, new XYZ(location.X, location.Y, location.Z + 1000));
                    ElementTransformUtils.RotateElement(doc, fa.Id, rotationLineZ, rRadian);
                }
                if (parameters != null)
                {
                    foreach (var para in parameters)
                    {
                        var pa = fa.LookupParameter(para.Key);
                        if (pa != null)
                        {
                            switch (pa.StorageType)
                            {
                                case StorageType.Integer:
                                    pa.Set((int)para.Value); break;
                                case StorageType.Double:
                                    pa.Set((double)para.Value); break;
                                case StorageType.String:
                                    pa.Set((string)para.Value); break;
                                case StorageType.ElementId:
                                    pa.Set((ElementId)para.Value); break;
                                default:
                                    pa.Set((string)para.Value); break;
                            }
                        }
                    }
                }
                tr.Commit();
            }
        }
    }
}