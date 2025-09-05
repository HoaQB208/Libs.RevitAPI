using Autodesk.Revit.DB;
using System.Collections.Generic;

namespace Libs.RevitAPI._Parameters
{
    public class ParaUtils
    {
        public static void CopyParaValues(Element source, Element target, HashSet<string> skipParaNames)
        {
            foreach (Parameter sourcePara in source.Parameters)
            {
                try
                {
                    if (sourcePara is null) continue;
                    if (sourcePara.IsReadOnly) continue;
                    if (skipParaNames.Contains(sourcePara.Definition.Name)) continue;
                    foreach (Parameter targetPara in target.Parameters)
                    {
                        if (sourcePara.Definition.Name == targetPara.Definition.Name)
                        {
                            try
                            {
                                targetPara.Set(targetPara.StorageType == StorageType.String ? sourcePara.AsString() : sourcePara.AsValueString());
                            }
                            catch  { }
                            break;
                        }
                    }
                }
                catch { }
            }
        }
    }
}
