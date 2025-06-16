using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
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
            ViewRevit view= view2D as ViewRevit;
            view.Name = nameView;
            return view;
        }

    }
}
