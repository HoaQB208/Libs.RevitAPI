using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Libs.RevitAPI._LinkImport
{
    public class RevitLink
    {
        public static List<RevitLinkInstance> GetRevitLinkInstances(Document doc)
        {
            List<RevitLinkInstance> linkInstances = new();
            FilteredElementCollector collector = new(doc);
            ICollection<Element> revitLinkInstances = collector.OfClass(typeof(RevitLinkInstance)).ToElements();
            foreach (Element element in revitLinkInstances)
            {
                RevitLinkInstance linkInstance = element as RevitLinkInstance;
                if (linkInstance != null) linkInstances.Add(linkInstance);
            }
            return linkInstances;
        }
    }
}
