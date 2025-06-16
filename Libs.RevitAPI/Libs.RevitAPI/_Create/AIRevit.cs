using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Libs.RevitAPI._Common;
using Libs.RevitAPI._Distince;
using Libs.RevitAPI._Selection;
using Libs.RevitAPI._SelectionFilters;
using Grid = Autodesk.Revit.DB.Grid;

namespace Libs.RevitAPI._Create
{
    public class AIRevit
    {
        //private void Create(Document doc, XYZ basePoint, List<double> spaces, string startName, bool isHorizontal)
        //{
        //    double gridLength = 30, extLength = 3; // Unit: feet
        //    using Transaction t = new(doc, "CreateGrids Grids");
        //    t.Start();
        //    XYZ offsetDir = isHorizontal ? XYZ.BasisY.Multiply(-1) : XYZ.BasisX;
        //    XYZ startPoint = isHorizontal ? basePoint.Add(XYZ.BasisX.Multiply(-extLength)) : basePoint.Add(XYZ.BasisY.Multiply(extLength));
        //    XYZ endPoint = isHorizontal ? startPoint.Add(XYZ.BasisX.Multiply(gridLength)) : startPoint.Add(XYZ.BasisY.Multiply(-gridLength));
        //    Line geoLine = Line.CreateBound(startPoint, endPoint);
        //    Grid grid = Grid.Create(doc, geoLine);
        //    grid.Name = startName;
        //    foreach (double space in spaces)
        //    {
        //        startPoint = startPoint.Add(offsetDir.Multiply(space));
        //        endPoint = endPoint.Add(offsetDir.Multiply(space));
        //        geoLine = Line.CreateBound(startPoint, endPoint);
        //        Grid.Create(doc, geoLine);
        //    }
        //    t.Commit();
        //}
        //public static void CreateGrids(Document doc, List<double> spaces = null, string startName = "A", bool isHorizontal = false)
        //{
        //    spaces ??= new List<double> { 10, 20, 20 };
        //    XYZ basePoint = new();
        //    double gridLength = 30, extLength = 3; // Unit: feet
        //    using Transaction t = new(doc, "CreateGrids");
        //    t.Start();
        //    XYZ offsetDir = isHorizontal ? XYZ.BasisY.Multiply(-1) : XYZ.BasisX;
        //    XYZ startPoint = isHorizontal ? basePoint.Add(XYZ.BasisX.Multiply(-extLength)) : basePoint.Add(XYZ.BasisY.Multiply(extLength));
        //    XYZ endPoint = isHorizontal ? startPoint.Add(XYZ.BasisX.Multiply(gridLength)) : startPoint.Add(XYZ.BasisY.Multiply(-gridLength));
        //    Line geoLine = Line.CreateBound(startPoint, endPoint);
        //    Grid grid = Grid.Create(doc, geoLine);
        //    grid.Name = startName;
        //    foreach (double space in spaces)
        //    {
        //        startPoint = startPoint.Add(offsetDir.Multiply(space));
        //        endPoint = endPoint.Add(offsetDir.Multiply(space));
        //        geoLine = Line.CreateBound(startPoint, endPoint);
        //        Grid.Create(doc, geoLine);
        //    }
        //    t.Commit();
        //}
        //public static void CreateGrids(Document doc, int n, double kcTruc, string startName = "A", bool isHorizontal = false)
        //{
        //    XYZ basePoint = new(0, 0, 0);
        //    double gridLength = 30, extLength = 3; // Unit: feet
        //    using Transaction t = new(doc, "CreateGrids");
        //    t.Start();
        //    XYZ offsetDir = isHorizontal ? XYZ.BasisY.Multiply(-1) : XYZ.BasisX;
        //    XYZ startPoint = isHorizontal ? basePoint.Add(XYZ.BasisX.Multiply(-extLength)) : basePoint.Add(XYZ.BasisY.Multiply(extLength));
        //    XYZ endPoint = isHorizontal ? startPoint.Add(XYZ.BasisX.Multiply(gridLength)) : startPoint.Add(XYZ.BasisY.Multiply(-gridLength));
        //    Line geoLine = Line.CreateBound(startPoint, endPoint);
        //    Grid grid = Grid.Create(doc, geoLine);
        //    grid.Name = startName;
        //    for (int i = 0; i < n; i++)
        //    {
        //        startPoint = startPoint.Add(offsetDir.Multiply(i * kcTruc));
        //        endPoint = endPoint.Add(offsetDir.Multiply(i * kcTruc));
        //        geoLine = Line.CreateBound(startPoint, endPoint);
        //        Grid.Create(doc, geoLine);
        //    }
        //    t.Commit();
        //}
        //public static void CreateLevels(Document doc, int numberOfLevels = 5, double levelHeight = 3000)
        //{
        //    using Transaction transaction = new(doc, "Create Levels");
        //    transaction.Start();
        //    double initialHeight = 0;
        //    for (int i = 0; i < numberOfLevels; i++)
        //    {
        //        double levelElevation = initialHeight + i * UnitConverter.MMToInternal(levelHeight);
        //        Level level = Level.Create(doc, levelElevation);
        //        level.Name = $"Lv {i + 1}";
        //        level.ShowBubbleInView(DatumEnds.End1, doc.ActiveView);
        //        level.HideBubbleInView(DatumEnds.End0, doc.ActiveView);
        //    }
        //    transaction.Commit();
        //}
        //public static void CreateColumns(Document doc, XYZ basePoint, FamilySymbol familySymbol)
        //{
        //    List<Level> allLevel = SelectByFilter.Levels(doc);
        //    allLevel = allLevel.OrderBy(x => x.Elevation).ToList();
        //    Level botLevel = allLevel.FirstOrDefault();
        //    Level topLevel = allLevel.Count > 1 ? allLevel[1] : allLevel[0];
        //    FamilyInstance familyInstance = null;
        //    using TransactionGroup txG = new(doc, "TransGroup");
        //    txG.Start("Group");
        //    using (Transaction tx = new(doc, "Trans"))
        //    {
        //        tx.Start("Insert Column");
        //        familyInstance = doc.Create.NewFamilyInstance(basePoint, familySymbol, botLevel, StructuralType.Column);
        //        familyInstance.get_Parameter(BuiltInParameter.FAMILY_BASE_LEVEL_PARAM).Set(botLevel.Id);
        //        familyInstance.get_Parameter(BuiltInParameter.FAMILY_TOP_LEVEL_PARAM).Set(topLevel.Id);
        //        familyInstance.get_Parameter(BuiltInParameter.FAMILY_BASE_LEVEL_OFFSET_PARAM).Set(UnitConverter.MMToInternal(0));
        //        familyInstance.get_Parameter(BuiltInParameter.FAMILY_TOP_LEVEL_OFFSET_PARAM).Set(UnitConverter.MMToInternal(0));
        //        tx.Commit();
        //    }
        //    txG.Assimilate();
        //}
        //public static void EditColumns(Document doc, double b, double h)
        //{
        //    // từ active => lấy các cột 
        //    FilteredElementCollector collector = new(doc, doc.ActiveView.Id);
        //    List<Element> elements = collector.WherePasses(new ElementCategoryFilter(BuiltInCategory.OST_StructuralColumns)).ToElements().ToList();
        //    foreach (Element element in elements)
        //    {
        //        Parameter _b = element.LookupParameter("b");
        //        Parameter _h = element.LookupParameter("h");
        //    }

        //    // chọn 1 cột => lấy ra các kích thước b, h
        //    // set lại b x h khi nhập lại

        //}
        //public static FamilySymbol SetFamilySymbolColumn(Family family, Document doc, double b, double h)
        //{
        //    List<FamilySymbol> allFamilySymbol = GetAllFamilySymbol(family);
        //    //Msg.Show(string.Join("\n", allFamilySymbol.Select(x => $"Name: {x.Name}")));
        //    try
        //    {
        //        using Transaction tr = new(doc, "Column");
        //        tr.Start("Create new Column Type");
        //        ElementType s1 = allFamilySymbol[0].Duplicate(allFamilySymbol[0].Name);
        //        s1.LookupParameter("b").Set(UnitConverter.MMToInternal(b));
        //        s1.LookupParameter("h").Set(UnitConverter.MMToInternal(h));
        //        s1.Name = $"{b}x{h}";
        //        FamilySymbol fs = s1 as FamilySymbol;
        //        tr.Commit();
        //        return fs;
        //    }
        //    catch { }
        //    return null;
        //}
        //public static List<FamilySymbol> GetAllFamilySymbol(Family family)
        //{
        //    List<FamilySymbol> familySymbols = new();
        //    foreach (ElementId familySymbolId in family.GetFamilySymbolIds())
        //    {
        //        FamilySymbol familySymbol = family.Document.GetElement(familySymbolId) as FamilySymbol;
        //        if (familySymbol != null) familySymbols.Add(familySymbol);
        //    }
        //    return familySymbols;
        //}
        //public static List<XYZ> GetIntersectGrids(Document doc)
        //{
        //    List<XYZ> listPoint = new();
        //    List<Grid> allGrid = new FilteredElementCollector(doc)
        //                        .WhereElementIsNotElementType()
        //                        .OfClass(typeof(Grid))
        //                        .Cast<Grid>()
        //                        .Where(x => (x.Curve as Line) != null)
        //                        .ToList();

        //    for (int i = 0; i < allGrid.Count - 1; i++)
        //    {
        //        for (int j = 1; j < allGrid.Count; j++)
        //        {
        //            Line gridLine1 = allGrid[i].Curve as Line;
        //            Line gridLine2 = allGrid[j].Curve as Line;
        //            IntersectionResultArray intersectionResultArray;
        //            SetComparisonResult comparisonResult = gridLine1.Intersect(gridLine2, out intersectionResultArray);
        //            // Nếu có điểm giao nhau, thêm vào danh sách
        //            if (comparisonResult == SetComparisonResult.Overlap)
        //            {
        //                foreach (IntersectionResult intersectionResult in intersectionResultArray)
        //                {
        //                    XYZ intersectionPoint = intersectionResult.XYZPoint;
        //                    listPoint.Add(intersectionPoint);
        //                }
        //            }
        //        }
        //    }
        //    listPoint = listPoint.Distinct(new DistincePoint()).ToList();
        //    return listPoint;
        //}
    }
}
