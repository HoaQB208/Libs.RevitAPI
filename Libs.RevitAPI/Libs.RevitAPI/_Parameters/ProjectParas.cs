using Autodesk.Revit.DB;
using System.Collections.Generic;

namespace Libs.RevitAPI._Parameters
{
    public class ProjectParas
    {
        public static List<string> GetParaNames(Document doc)
        {
            List<string> paras = new List<string>();

            DefinitionBindingMapIterator it = doc.ParameterBindings.ForwardIterator();
            it.Reset();
            while (it.MoveNext())
            {
                paras.Add(it.Key.Name);
            }

            return paras;
        }
    }
}