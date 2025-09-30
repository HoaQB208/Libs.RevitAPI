using Autodesk.Revit.DB;

namespace Libs.RevitAPI._FailuresPreprocessors
{
    public class DuplicateTypePreprocessor : IDuplicateTypeNamesHandler
    {
        public DuplicateTypeAction OnDuplicateTypeNamesFound(DuplicateTypeNamesHandlerArgs args)
        {
            return DuplicateTypeAction.UseDestinationTypes;
        }
    }
}
