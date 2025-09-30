using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Selection;
using Autodesk.Revit.UI;
using System.Collections.Generic;
using System.Linq;
using ViewRevit = Autodesk.Revit.DB.View;

namespace Libs.RevitAPI._View
{
    public class ViewUtils
    {
        public static View3D Find3DView(Document targetFamilyDocument, string nameView, bool createIfNotFound = true)
        {
            Element view3D = new FilteredElementCollector(targetFamilyDocument)
                                                .OfCategory(BuiltInCategory.OST_Views)
                                                .OfClass(typeof(View3D))
                                                .FirstOrDefault(v => v.Name.Equals(nameView));
            if (view3D != null) return view3D as View3D;

            if (createIfNotFound)
            {
                ViewFamilyType viewFamilyType = new FilteredElementCollector(targetFamilyDocument)
                                                    .OfClass(typeof(ViewFamilyType))
                                                    .Where(e => e is ViewFamilyType)
                                                    .Cast<ViewFamilyType>()
                                                    .FirstOrDefault(vf => vf.ViewFamily == ViewFamily.ThreeDimensional);
                if (viewFamilyType == null) return null;
                var view = View3D.CreateIsometric(targetFamilyDocument, viewFamilyType.Id);
                view.Name = nameView;
                return view;
            }
            return null;
        }

        public static ViewRevit Find2DView(Document familyDoc, string nameView)
        {
            //Get Default Sheet
            Element view2D = new FilteredElementCollector(familyDoc).OfClass(typeof(ViewSheet)).FirstOrDefault();
            if (view2D != null) return view2D as ViewRevit;
            ViewRevit view = view2D as ViewRevit;
            view.Name = nameView;
            return view;
        }

        public static List<Curve> GetSelectionBox(UIDocument uidoc)
        {
            PickedBox pickedBox;
            try { pickedBox = uidoc.Selection.PickBox(PickBoxStyle.Crossing, "Crop View..."); } catch { return null; }

            Transform viewTransform = uidoc.ActiveView.CropBox.Transform;
            XYZ min = viewTransform.Inverse.OfPoint(pickedBox.Min);
            XYZ max = viewTransform.Inverse.OfPoint(pickedBox.Max);

            // Tạo CurveLoop (chu vi vùng crop)
            XYZ pt1 = new XYZ(min.X, min.Y, 0);
            XYZ pt2 = new XYZ(max.X, min.Y, 0);
            XYZ pt3 = new XYZ(max.X, max.Y, 0);
            XYZ pt4 = new XYZ(min.X, max.Y, 0);
            List<Curve> profile = new List<Curve>
                        {
                            Line.CreateBound(pt1, pt2),
                            Line.CreateBound(pt2, pt3),
                            Line.CreateBound(pt3, pt4),
                            Line.CreateBound(pt4, pt1)
                        };
            return profile.Select(c => c.CreateTransformed(viewTransform)).ToList();
        }

        public static void Cropping_View2D(Document doc, List<Curve> lsCurve)
        {
            CurveLoop curveLoop = CurveLoop.Create(lsCurve);
            using (Transaction tr = new Transaction(doc, "CropView"))
            {
                tr.Start();
                doc.ActiveView.CropBoxActive = true;
                doc.ActiveView.CropBoxVisible = true;
                ViewCropRegionShapeManager cropRegionShapeManager = doc.ActiveView.GetCropRegionShapeManager();
                cropRegionShapeManager.SetCropShape(curveLoop);
                tr.Commit();
            }
        }

        public static HashSet<string> GetViewSheetNames(Document doc)
        {
            return new FilteredElementCollector(doc).OfClass(typeof(ViewSheet)).WhereElementIsNotElementType().Cast<ViewSheet>().Where(x => x != null).Select(x => x.SheetNumber).ToHashSet();
        }

        public static List<ViewRevit> GetAllViews(Document doc)
        {
            var excludedTypes = new HashSet<ViewType>
            {
                ViewType.DrawingSheet,
                ViewType.ProjectBrowser,
                ViewType.Undefined,
                ViewType.Report,
                ViewType.SystemBrowser
            };
            return new FilteredElementCollector(doc)
                .OfCategory(BuiltInCategory.OST_Views)
                .WhereElementIsNotElementType()
                .Cast<ViewRevit>()
                .Where(view => !view.IsTemplate && !excludedTypes.Contains(view.ViewType))
                .OrderBy(view => view.Name)
                .ToList();
        }

        public static List<Viewport> GetAllViewport(ViewSheet viewSheet)
        {
            ICollection<ElementId> allViewports = viewSheet.GetAllViewports();
            var doc = viewSheet.Document;
            List<Viewport> viewports = new List<Viewport>();
            foreach (var elementId in allViewports)
            {
                var el = doc.GetElement(elementId);
                if(el is Viewport vp) viewports.Add(vp);  
            }    
            return viewports;
        }
    }
}
