using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Libs.RevitAPI._Parameters
{
    public class ElementParas
    {

        public static string GetStructuralMaterialOfElement(Document doc, Element element)
        {
            string strucMaterialName = "";

            ElementId typeId = element.GetTypeId();
            if (typeId != null && typeId != ElementId.InvalidElementId)
            {
                // Get the ElementType using the ElementId
                ElementType elementType = doc.GetElement(typeId) as ElementType;
                if (elementType != null)
                {
                    var paStructuralMaterial = elementType.LookupParameter("Structural Material");
                    if (paStructuralMaterial != null)
                    {
                        strucMaterialName = paStructuralMaterial.AsValueString();
                    }
                }
            }

            return strucMaterialName;
        }

        public static string GetWorkSetOfElement(Document doc, Element element)
        {
            string workSetName = "";

            // the table of worksets
            WorksetTable worksetTable = doc.GetWorksetTable();

            // access the workset from element ID
            WorksetId worksetIdByElement = doc.GetWorksetId(element.Id);

            if (worksetIdByElement != WorksetId.InvalidWorksetId)
            {
                // now the workset
                Workset worksetByElement = worksetTable.GetWorkset(worksetIdByElement);
                workSetName = worksetByElement.Name;
            }
            return workSetName;
        }


        public static string GetTypeNameOfElement(Document doc, Element element)
        {
            ElementId typeId = element.GetTypeId();
            if (typeId != null && typeId != ElementId.InvalidElementId)
            {
                // Get the ElementType using the ElementId
                ElementType elementType = doc.GetElement(typeId) as ElementType;
                if (elementType != null)
                {
                    return elementType.Name;
                }
            }
            return "";
        }

        public static string GetFamilyNameOfElement(Document doc, Element element)
        {
            string familyName = "";
            ElementId eId = element.GetTypeId();

            ElementType elementType = (ElementType)doc.GetElement(eId);

            if (elementType != null)
            {
                familyName = elementType.FamilyName;
            }
            return familyName;
        }

        public static string GetFunctionOfWall(Document doc, Element element)
        {
            ElementId typeId = element.GetTypeId();
            if (typeId != null && typeId != ElementId.InvalidElementId)
            {
                // Get the ElementType using the ElementId
                ElementType elementType = doc.GetElement(typeId) as ElementType;
                if (elementType != null)
                {
                    if (elementType is WallType)
                    {
                        string functionName = elementType.get_Parameter(BuiltInParameter.FUNCTION_PARAM).AsValueString();
                        return functionName;
                    }
                }
            }
            return "";
        }

        public static string GetLevelNameOfElement(Element ele)
        {
            string levelName = "null";

            if (ele.LevelId != ElementId.InvalidElementId)
            {
                Level level = ele.Document.GetElement(ele.LevelId) as Level;

                if (level != null)
                {
                    levelName = level.Name;
                }
            }
            return levelName;
        }
    }
}
