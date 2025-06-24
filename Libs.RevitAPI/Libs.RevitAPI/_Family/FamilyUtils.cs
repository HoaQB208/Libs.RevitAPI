using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Libs.RevitAPI._Family
{
    public class FamilyUtils
    {
        public static Family GetFamilyColumnByType(Document doc, string columnType)
        {
            Category category = Category.GetCategory(doc, BuiltInCategory.OST_StructuralColumns);
            List<Family> families = new FilteredElementCollector(doc)
                                            .OfClass(typeof(Family))
                                            .Cast<Family>()
                                            .Where(e => e.FamilyCategory.Id.Equals(category.Id))
                                            .ToList();
            //Msg.Show(string.Join("\n", families.Select(x => $"Name :{x.Name}")));
            foreach (var family in families)
            {
                string nameFamily = family.Name;
                if (nameFamily.Contains(columnType.ToString()))
                {
                    return family;
                }
            }
            return null;
        }

        public static List<Family> GetFamiliesByType(Document doc, BuiltInCategory builtInCategory)
        {
            Category category = Category.GetCategory(doc, builtInCategory);
            List<Family> families = new FilteredElementCollector(doc)
                                            .OfClass(typeof(Family))
                                            .Cast<Family>()
                                            .Where(e => e.FamilyCategory.Id.Equals(category.Id))
                                            .ToList();

            return families;
        }

        public static List<FamilySymbol> GetAllFamilySymbol(Family family)
        {
            List<FamilySymbol> familySymbols = new List<FamilySymbol>();
            foreach (ElementId familySymbolId in family.GetFamilySymbolIds())
            {
                FamilySymbol familySymbol = family.Document.GetElement(familySymbolId) as FamilySymbol;
                if (familySymbol != null) familySymbols.Add(familySymbol);
            }
            return familySymbols;
        }
        public static void LoadFamilies(Document doc, string pathFolder)
        {
            string[] familyFiles = Directory.GetFiles(pathFolder, "*.rfa");
            if (familyFiles.Length == 0)
            {
                //Msg.Show($"No family files found in the {pathFolder} ");
                return;
            };
            foreach (string familyFile in familyFiles)
            {
                doc.LoadFamily(familyFile);
            }
        }

        public static FamilyInstance PlaceFamilyInstance(Document doc, FamilySymbol familySymbol, XYZ location, Element host, Level level, StructuralType structuralType = StructuralType.NonStructural)
        {
            double heightOffset = 50 / 304.8;
            FamilyInstance familyInstance = null;
            using (Transaction trans = new Transaction(doc, "Place Family Instance"))
            {
                trans.Start();
                FailureHandlingOptions option = trans.GetFailureHandlingOptions();
                option.SetFailuresPreprocessor(new _Warning.DeleteWarningSuper());
                trans.SetFailureHandlingOptions(option);
                if (!familySymbol.IsActive) familySymbol.Activate();
                familyInstance = doc.Create.NewFamilyInstance(location, familySymbol, host, level, structuralType);
                var para = familyInstance.get_Parameter(BuiltInParameter.INSTANCE_ELEVATION_PARAM);
                para.Set(location.Z - heightOffset);
                trans.Commit();
            }
            return familyInstance;
        }


        public static FamilyInstance PlaceFamilyInstanceWithBeam(Document doc, PlanarFace pf, FamilySymbol familySymbol, XYZ location, bool isVertical)
        {
            FamilyInstance familyInstance = null;
            BoundingBoxUV bboxUV = pf.GetBoundingBox();
            UV center = (bboxUV.Max + bboxUV.Min) / 2.0;
            XYZ normal = pf.ComputeNormal(center);
            XYZ refDir;
            //XYZ refDir = normal.CrossProduct(XYZ.BasisX);
            if (!isVertical) refDir = normal.CrossProduct(XYZ.BasisZ);
            else refDir = normal.CrossProduct(XYZ.BasisX);
            using (Transaction trans = new Transaction(doc, "Place Family"))
            {
                trans.Start("Place Family");
                FailureHandlingOptions option = trans.GetFailureHandlingOptions();
                option.SetFailuresPreprocessor(new _Warning.DeleteWarningSuper());
                trans.SetFailureHandlingOptions(option);
                try
                {
                    if (!familySymbol.IsActive) familySymbol.Activate();
                    familyInstance = doc.Create.NewFamilyInstance(pf, location, refDir, familySymbol);
                    trans.Commit();
                }
                catch
                {
                    trans.RollBack();
                    //Msg.Show(ex.Message);
                }
            }
            return familyInstance;
        }



        public static FamilySymbol GetFamilySymbol(Document doc, string familySymbolName)
        {
            FilteredElementCollector collector = new FilteredElementCollector(doc).OfClass(typeof(FamilySymbol));
            foreach (FamilySymbol symbol in collector)
            {
                if (symbol.Name.Equals(familySymbolName))
                {
                    return symbol;
                }
            }
            //Msg.Show("FamilySymbol not found: " + familySymbolName);
            return null;
        }

        public static List<FamilySymbol> GetFamilySymbols(Document doc)
        {
            FilteredElementCollector collector = new FilteredElementCollector(doc);
            ICollection<Element> familySymbols = collector.OfClass(typeof(FamilySymbol)).ToElements();
            List<FamilySymbol> symbols = new List<FamilySymbol>();
            foreach (Element element in familySymbols)
            {
                if (element is FamilySymbol familySymbol)
                {
                    symbols.Add(familySymbol);
                }
            }
            return symbols;
        }



    }




}
