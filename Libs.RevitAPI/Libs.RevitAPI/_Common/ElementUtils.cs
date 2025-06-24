using Autodesk.Revit.DB;
using Libs.RevitAPI._Document;

namespace Libs.RevitAPI._Common
{
    public class ElementUtils
    {
        /// <summary>
        /// Hàm này phải chạy trong transaction
        /// </summary>
        /// <param name="document"></param>
        /// <param name="elementId"></param>
        /// <param name="color"></param>
        public static void SetOverrideGraphicSettings(Document document, ElementId elementId, Autodesk.Revit.DB.Color color = null)
        {
            if (color != null)
            {
                OverrideGraphicSettings ogs = new OverrideGraphicSettings();
                var solidFillPatternId = DocUtils.GetFirstFillPattern(document);

                ogs.SetProjectionLineColor(color);
                ogs.SetCutLineColor(color);

                //ogs.SetProjectionFillColor(color);
                //ogs.SetCutFillColor(color);

                ogs.SetSurfaceForegroundPatternId(solidFillPatternId);
                ogs.SetCutForegroundPatternId(solidFillPatternId);
                ogs.SetCutForegroundPatternColor(color);
                ogs.SetSurfaceForegroundPatternColor(color);
                document.ActiveView.SetElementOverrides(elementId, ogs);
            }
        }

        /// <summary>
        /// Hàm này đã có transaction
        /// </summary>
        /// <param name="document"></param>
        /// <param name="elementId"></param>
        /// <param name="color"></param>
        public static void SetOverrideGraphicSettingsWithTransaction(Document document, ElementId elementId, Autodesk.Revit.DB.Color color = null)
        {
            if (color != null)
            {
                using (Transaction tr = new Transaction(document))
                {
                    tr.Start("CreateGrids DirectShape");

                    OverrideGraphicSettings ogs = new OverrideGraphicSettings();

                    var solidFillPatternId = DocUtils.GetFirstFillPattern(document);

                    ogs.SetProjectionLineColor(color);
                    ogs.SetCutLineColor(color);

                    //ogs.SetProjectionFillColor(color);
                    //ogs.SetCutFillColor(color);

                    ogs.SetSurfaceForegroundPatternId(solidFillPatternId);
                    ogs.SetCutForegroundPatternId(solidFillPatternId);
                    ogs.SetCutForegroundPatternColor(color);
                    ogs.SetSurfaceForegroundPatternColor(color);
                    document.ActiveView.SetElementOverrides(elementId, ogs);
                    tr.Commit();
                }
            }
        }
    }
}
