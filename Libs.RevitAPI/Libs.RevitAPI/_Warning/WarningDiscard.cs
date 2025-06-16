using Autodesk.Revit.DB;
using System.Collections.Generic;

public class WarningDiscard : IFailuresPreprocessor
{
    public static bool HasWarning { get; set; } = false;

    FailureProcessingResult IFailuresPreprocessor.PreprocessFailures(FailuresAccessor failuresAccessor)
    {
        IList<FailureMessageAccessor> fmas = failuresAccessor.GetFailureMessages();

        if (fmas.Count == 0)
        {
            return FailureProcessingResult.Continue;
        }

        bool isResolved = false;

        foreach (FailureMessageAccessor fma in fmas)
        {
            if (fma.HasResolutions())
            {
                failuresAccessor.ResolveFailure(fma);
                isResolved = true;
            }
        }

        if (isResolved)
        {
            return FailureProcessingResult.ProceedWithCommit;
        }

        HasWarning = true;

        return FailureProcessingResult.Continue;
    }

}

