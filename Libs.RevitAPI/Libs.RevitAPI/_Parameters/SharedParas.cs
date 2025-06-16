using Autodesk.Revit.DB;
using Libs.RevitAPI._Selection;
using System.Collections.Generic;

namespace Libs.RevitAPI._Parameters
{
    public class SharedParas
    {
        public static List<string> GetParas(Document doc)
        {
            List<string> paras = new List<string>();

            List<SharedParameterElement> sharedPara = SelectByFilter.SharedParameterElements(doc);
            foreach (SharedParameterElement param in sharedPara)
            {
                if (param != null)
                {
                    Definition def = param.GetDefinition();
                    paras.Add(def.Name);
                }
            }

            return paras;
        }
    }
}