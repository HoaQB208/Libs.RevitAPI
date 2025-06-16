using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Libs.RevitAPI._Parameters
{
    public class TBFParameterUtils
    {
        public static bool IsUsedParameter(Parameter parameter)
        {
            if (!parameter.IsShared)
                return false;

            switch (parameter.StorageType)
            {
                case StorageType.Integer:
                    return (parameter.AsInteger() != 0);
                case StorageType.Double:
                    return (parameter.AsDouble() != 0);
                case StorageType.String:
                    return !string.IsNullOrEmpty(parameter.AsString()); // return (parameter.AsString() != null);
                case StorageType.ElementId:
                    return (parameter.AsElementId() != ElementId.InvalidElementId);
                default:
                    return false;
            }
        }

        public static string GetParameterValue(Parameter parameter)
        {
            switch (parameter.StorageType)
            {
                case StorageType.Integer:
                    return parameter.AsInteger().ToString();
                case StorageType.Double:
                    return parameter.AsDouble().ToString();
                case StorageType.String:
                    return parameter.AsString();
                case StorageType.ElementId:
                    return parameter.AsElementId().ToString();
                default:
                    return "Unknown";
            }
        }

    }
}
