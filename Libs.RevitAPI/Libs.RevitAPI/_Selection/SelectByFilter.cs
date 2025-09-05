using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Architecture;
using Autodesk.Revit.DB.Electrical;
using Autodesk.Revit.DB.Mechanical;
using Autodesk.Revit.DB.Plumbing;
using Autodesk.Revit.DB.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using View = Autodesk.Revit.DB.View;

namespace Libs.RevitAPI._Selection
{
    public class SelectByFilter
    {
        public static List<Element> GetAllElements(Document document)
        {
            return new FilteredElementCollector(document).WhereElementIsNotElementType().ToElements().Where(x => x != null).ToList();
        }

        public static List<Element> GetActiveViewElements(Document document)
        {
            return new FilteredElementCollector(document, document.ActiveView.Id).WhereElementIsNotElementType().ToElements().Where(x => x != null).ToList();
        }

        public static List<ElementId> GetActiveViewElementIs(Document document)
        {
            return new FilteredElementCollector(document, document.ActiveView.Id).WhereElementIsNotElementType().ToElementIds().Where(x => x != null).ToList();
        }

        public static List<Element> GetAllDimStyles(Document document)
        {
            ElementClassFilter elementClassFilter = new ElementClassFilter(typeof(DimensionType));
            FilteredElementCollector filteredElementCollector = new FilteredElementCollector(document).WherePasses(elementClassFilter);
            List<Element> ListEle = filteredElementCollector.ToElements().ToList();
            for (int i = 0; i < ListEle.Count; i++)
            {
                DimensionType DType = ListEle[i] as DimensionType;
                if (DType.FamilyName != "Linear Dimension Style" || DType.Name == "Linear Dimension Style")
                {
                    ListEle.Remove(ListEle[i]);
                }
            }
            return ListEle;
        }

        /// <summary>
        /// Get all Line Styles in project.
        /// </summary>
        /// <param name="document"></param>
        /// <returns></returns>
        public static List<Category> GetAllLineStyles(Document document)
        {
            List<Category> categories = new List<Category>();

            Category linesCategory = Category.GetCategory(document, BuiltInCategory.OST_Lines);

            if (linesCategory != null)
            {
                List<Category> subCategories = linesCategory.SubCategories.Cast<Category>().ToList();

                foreach (Category subCategory in subCategories)
                {
                    if (subCategory.GetLineWeight(GraphicsStyleType.Projection) != null)
                    {
                        categories.Add(subCategory);
                    }
                }
            }
            return categories;
        }

        public static Level GetLevelFromHostElement(Element hostElement, Document doc)
        {
            if (hostElement is Autodesk.Revit.DB.Floor floor)
            {
                Level level = doc.GetElement(floor.LevelId) as Level;
                return level;
            }
            // Kiểm tra nếu hostElement là dầm (Structural Framing)
            else if (hostElement is FamilyInstance familyInstance
                     && familyInstance.StructuralType == StructuralType.Beam)
            {
                // Lấy LevelId từ tham số INSTANCE_REFERENCE_LEVEL_PARAM
                ElementId levelId = familyInstance.get_Parameter(BuiltInParameter.INSTANCE_REFERENCE_LEVEL_PARAM).AsElementId();
                Level level = doc.GetElement(levelId) as Level;
                return level;
            }
            return null;
        }

        public static List<DetailLine> GetDetailLines(Document document)
        {
            return new FilteredElementCollector(document).OfClass(typeof(DetailLine)).WhereElementIsNotElementType().Cast<DetailLine>().ToList();
        }

        /// <summary>
        /// Get All Line Patterns in project.
        /// </summary>
        /// <param name="document"></param>
        /// <returns></returns>
        public static List<LinePatternElement> GetAllLinePatterns(Document document)
        {
            return new FilteredElementCollector(document).OfClass(typeof(LinePatternElement)).Cast<LinePatternElement>().ToList();
        }

        /// <summary>
        /// Get All Fill Patterns in project.
        /// </summary>
        /// <param name="document"></param>
        /// <returns></returns>
        public static List<FillPatternElement> GetAllFillPatterns(Document document)
        {
            return new FilteredElementCollector(document).OfClass(typeof(FillPatternElement)).Cast<FillPatternElement>().ToList();
        }

        public static List<Solid> GetAllSolids(Element element)
        {
            List<Solid> elementSolids = new List<Solid>();
            Options options = new Options() { ComputeReferences = true, View = element.Document.ActiveView };
            GeometryElement geometryElement = element.get_Geometry(options);

            foreach (GeometryObject geometryObject in geometryElement)
            {
                Solid solid = geometryObject as Solid;

                if (solid != null)
                {
                    if (solid.Volume > 1e-9)
                    {
                        elementSolids.Add(solid);
                    }
                }
                else
                {
                    GeometryInstance geometryInstance = geometryObject as GeometryInstance;

                    if (geometryInstance != null)
                    {
                        GeometryElement geometryElement1 = geometryInstance.GetInstanceGeometry();
                        foreach (GeometryObject geometryObject1 in geometryElement1)
                        {
                            Solid solid1 = geometryObject1 as Solid;

                            if (solid1 != null)
                            {
                                if (solid1.Volume > 1e-9)
                                {
                                    elementSolids.Add(solid1);
                                }
                            }
                        }
                    }

                }
            }
            return elementSolids;
        }

        public static List<Curve> GetCurvesOfFace(Face face)
        {
            IList<Curve> list = new List<Curve>();
            EdgeArrayArray edgeLoops = face.EdgeLoops;
            List<Curve> result;
            if (edgeLoops.IsEmpty)
            {
                result = null;
            }
            else
            {
                foreach (object obj in edgeLoops)
                {
                    EdgeArray edgeArray = (EdgeArray)obj;
                    if (!edgeArray.IsEmpty)
                    {
                        foreach (object obj2 in edgeArray)
                        {
                            Edge edge = (Edge)obj2;
                            list.Add(edge.AsCurve());
                        }
                    }
                }
                result = list.ToList<Curve>();
            }
            return result;
        }

        public static List<object> GetLevelNear(Document document, double cdZ)
        {
            List<object> lvResult = new List<object>();
            FilteredElementCollector collector = new FilteredElementCollector(document);
            IList<Element> ls = collector.OfClass(typeof(Level)).ToElements();
            double DentaMinZ = 0;
            foreach (Element item in ls)
            {
                Level lv = item as Level;
                if (lvResult.Count == 0)
                {
                    lvResult.Add(lv);
                    lvResult.Add(cdZ - lv.Elevation);
                    DentaMinZ = Math.Abs(cdZ - lv.Elevation);
                }
                else
                {
                    if (Math.Abs(cdZ - lv.Elevation) < DentaMinZ)
                    {
                        DentaMinZ = Math.Abs(cdZ - lv.Elevation);
                        lvResult.Clear();
                        lvResult.Add(lv);
                        lvResult.Add(cdZ - lv.Elevation);
                    }
                }
            }
            return lvResult;
        }

        public static List<string> GetListLevelsName(Document document)
        {
            List<Level> AllLevel = new FilteredElementCollector(document).OfClass(typeof(Level)).Cast<Level>().ToList<Level>();
            AllLevel = (from level in AllLevel orderby level.Elevation select level).ToList<Level>();
            List<string> Ls = new List<string>();
            foreach (Level item in AllLevel) Ls.Add(item.Name);
            return Ls;
        }

        public static Level GetLevelByName(Document document, string levelName)
        {
            List<Level> AllLevel = new FilteredElementCollector(document).OfClass(typeof(Level)).Cast<Level>().ToList<Level>();
            foreach (Level level in AllLevel)
            {
                if (level.Name == levelName) return level;
            }
            return null;
        }

        public static List<string> GetLsParametersName(ViewSheet viewSheet)
        {
            List<string> lsParaAvailable = new List<string>();
            foreach (Autodesk.Revit.DB.Parameter p in viewSheet.Parameters)
            {
                if (!p.IsReadOnly) lsParaAvailable.Add(p.Definition.Name);
            }
            return lsParaAvailable;
        }

        public static IEnumerable<Family> GetAllFamilies(Document document)
        {
            return new FilteredElementCollector(document).WhereElementIsNotElementType().OfClass(typeof(Family)).Cast<Family>();
        }

        public static IEnumerable<Family> GetAllInPlaceFamilies(Document document)
        {
            return new FilteredElementCollector(document).WhereElementIsNotElementType().OfClass(typeof(Family)).OfType<Family>().Where(x => x.IsInPlace);
        }

        public static FilteredElementCollector GetGroups(Document document, BuiltInCategory category)
        {
            return new FilteredElementCollector(document).WherePasses(new ElementCategoryFilter(category)).WhereElementIsNotElementType();
        }

        /// <summary>
        /// Get all Type which is shown in Project Browser
        /// </summary>
        /// <param name="document"></param>
        /// <returns></returns>
        public static IEnumerable<ElementType> GetAllFamiliesTypes(Document document)
        {
            var allFamiliesTypes = new List<ElementType>();

            var allTypes = new FilteredElementCollector(document).WhereElementIsElementType().Cast<ElementType>()
                .Where(x => x.CanBeRenamed == true).ToList();

            Type[] typesToCheck = { typeof(FamilySymbol), typeof(AnalyticalLinkType), typeof(StairsPathType), typeof(AnnotationSymbolType), typeof(RoofType), typeof(FloorType), typeof(WallType),
                                    typeof(CeilingType), typeof(GutterType),typeof(FasciaType),typeof(SlabEdgeType),typeof(CurtainSystemType),typeof(BeamSystemType),typeof(WallFoundationType),
                                    typeof(MechanicalSystemType), typeof(StairsType),typeof(StairsLandingType),typeof(StairsRunType),typeof(CutMarkType),typeof(TopRailType),typeof(HandRailType),
                                    typeof(PipingSystemType), typeof(ConduitType),typeof(CableTrayType),typeof(FlexPipeType),typeof(PipeType),typeof(FlexDuctType),typeof(RailingType), typeof(FilledRegionType),};

            foreach (var item in allTypes)
            {
                if (typesToCheck.Contains(item.GetType()))
                {
                    allFamiliesTypes.Add(item);
                }
            }

            return allFamiliesTypes;
        }

        /// <summary>
        /// Get all Type which is shown in Project Browser
        /// </summary>
        /// <param name="document"></param>
        /// <returns></returns>
        public static IEnumerable<ElementType> GetAllFamiliesTypesForFamilyCleanup(Document document)
        {
            var allFamiliesTypes = new List<ElementType>();

            var allTypes = new FilteredElementCollector(document).WhereElementIsElementType().Cast<ElementType>()
                .Where(x => x.CanBeRenamed == true).ToList();

            Type[] typesToCheck = { typeof(FamilySymbol), typeof(AnalyticalLinkType), typeof(StairsPathType), typeof(AnnotationSymbolType), typeof(RoofType), typeof(FloorType), typeof(WallType),
                                    typeof(CeilingType), typeof(GutterType),typeof(FasciaType),typeof(SlabEdgeType),typeof(CurtainSystemType),typeof(BeamSystemType),typeof(WallFoundationType),
                                    typeof(MechanicalSystemType), typeof(StairsType),typeof(StairsLandingType),typeof(StairsRunType),typeof(CutMarkType),typeof(TopRailType),typeof(HandRailType),
                                    typeof(RailingType), typeof(FilledRegionType),};

            foreach (var item in allTypes)
            {
                if (typesToCheck.Contains(item.GetType()))
                {
                    allFamiliesTypes.Add(item);
                }
            }

            return allFamiliesTypes;
        }

        public static IEnumerable<Material> GetAllMaterials(Document document)
        {
            return new FilteredElementCollector(document).WhereElementIsNotElementType().OfClass(typeof(Material)).Cast<Material>();
        }

        public static IEnumerable<FilledRegionType> GetAllFillRegions(Document document)
        {
            return new FilteredElementCollector(document).OfClass(typeof(FilledRegionType)).Cast<FilledRegionType>();
        }

        public static IList<Workset> GetAllWorksets(Document document)
        {
            return new FilteredWorksetCollector(document).OfKind(WorksetKind.UserWorkset).ToWorksets();
        }

        public static IEnumerable<ViewSheet> GetAllSheets(Document document)
        {
            return new FilteredElementCollector(document).OfClass(typeof(ViewSheet)).WhereElementIsNotElementType().Cast<ViewSheet>();
        }

        public static IEnumerable<RevitLinkType> GetAllRevitLinks(Document document)
        {
            return new FilteredElementCollector(document).OfClass(typeof(RevitLinkType)).OfType<RevitLinkType>();
        }

        public static IEnumerable<CADLinkType> GetAllCADLinks(Document document)
        {
            return new FilteredElementCollector(document).OfClass(typeof(CADLinkType)).OfType<CADLinkType>().Where(x => x.IsExternalFileReference());
        }

        public static IEnumerable<CADLinkType> GetAllCADImports(Document document)
        {
            return new FilteredElementCollector(document).OfClass(typeof(CADLinkType)).OfType<CADLinkType>().Where(x => !x.IsExternalFileReference());
        }

        public static IEnumerable<ImportInstance> GetImportInstance(Document document)
        {
            return new FilteredElementCollector(document).OfClass(typeof(ImportInstance)).OfType<ImportInstance>().Where(x => !x.IsExternalFileReference());
        }

        public static List<Material> GetUnusedMaterials(Document document)
        {
            List<Material> unusedMaterialsList = new List<Material>();
            MethodInfo meThodGetUnusedMaterials = document.GetType().GetMethod("GetUnusedMaterials", BindingFlags.Instance | BindingFlags.NonPublic);

            ICollection<ElementId> unusedMaterials = ((meThodGetUnusedMaterials != null) ? meThodGetUnusedMaterials.Invoke(document, null) : null) as ICollection<ElementId>;

            foreach (var item in unusedMaterials)
            {
                Element element = document.GetElement(item);

                if (element is Material material)
                {
                    unusedMaterialsList.Add(material);
                }
            }
            return unusedMaterialsList;
        }

        public static IEnumerable<View> GetAllLegends(Document document)
        {
            return new FilteredElementCollector(document).OfClass(typeof(View)).Cast<View>().Where(x => x != null && x.ViewType == ViewType.Legend);
        }

        public static IEnumerable<ViewSchedule> GetAllSchedules(Document document)
        {
            return new FilteredElementCollector(document).OfClass(typeof(ViewSchedule)).Cast<ViewSchedule>().Where(x => x != null && !x.IsTitleblockRevisionSchedule);
        }

        public static IEnumerable<View> GetAllViewTemplates(Document document)
        {
            return new FilteredElementCollector(document).OfClass(typeof(View)).Cast<View>().Where(x => x != null && x.IsTemplate);
        }

        public static IEnumerable<ParameterFilterElement> GetAllFilters(Document document)
        {
            return new FilteredElementCollector(document).OfClass(typeof(ParameterFilterElement)).Cast<ParameterFilterElement>().Where(x => x != null);
        }

        public static IEnumerable<View> GetAllViews(Document document)
        {
            return new FilteredElementCollector(document).WhereElementIsNotElementType().OfClass(typeof(View)).Cast<View>()
                .Where(x => x.ViewType == ViewType.FloorPlan ||
                x.ViewType == ViewType.CeilingPlan ||
                x.ViewType == ViewType.Elevation ||
                x.ViewType == ViewType.ThreeD ||
                x.ViewType == ViewType.DraftingView ||
                x.ViewType == ViewType.EngineeringPlan ||
                x.ViewType == ViewType.AreaPlan ||
                x.ViewType == ViewType.Section ||
                x.ViewType == ViewType.Detail ||
                x.ViewType == ViewType.Walkthrough ||
                x.ViewType == ViewType.Rendering ||
                x.ViewType == ViewType.Legend).Where(y => y.IsTemplate == false);
        }


        /// <summary>
        /// Ví dụ sử dụng: GetAllElementsOfType(doc, typeof(Level));
        /// </summary>
        public static List<Element> GetAllElementsOfType(Document document, Type type)
        {
            return new FilteredElementCollector(document).OfClass(type).ToElements().ToList();
        }

        /// <summary>
        /// Get All Elements of a specific Type (Where Element Is Not Element Type)  
        /// </summary>
        /// <param name="document"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static List<Element> GetAllElementsIsNotElementType(Document document, Type type)
        {
            return new FilteredElementCollector(document).OfClass(type).WhereElementIsNotElementType().ToElements().ToList();
        }

        public static List<Element> GetAllFamilyInstanceByName(Document document, string familyName)
        {
            return new FilteredElementCollector(document)
           .OfClass(typeof(FamilyInstance))
           .Cast<FamilyInstance>()
           .Where(x => x.Symbol.FamilyName.Equals(familyName)).ToList<Element>();
        }

        public static List<Level> Levels(Document document)
        {
            return new FilteredElementCollector(document).OfClass(typeof(Level)).Cast<Level>().ToList();
        }

        public static List<DimensionType> DimensionTypes(Document document)
        {
            return new FilteredElementCollector(document).OfClass(typeof(DimensionType)).Cast<DimensionType>().ToList();
        }

        public static List<FamilySymbol> FamilySymbols(Document document)
        {
            return new FilteredElementCollector(document).OfClass(typeof(FamilySymbol)).Cast<FamilySymbol>().ToList();
        }

        public static List<TextNoteType> TextNoteTypes(Document document)
        {
            return new FilteredElementCollector(document).OfClass(typeof(TextNoteType)).Cast<TextNoteType>().ToList();
        }

        public static List<TextElement> TextElements(Document document)
        {
            return new FilteredElementCollector(document).WhereElementIsNotElementType().OfClass(typeof(TextElement)).Cast<TextElement>().ToList();
        }

        public static List<Dimension> Dimensions(Document document)
        {
            return new FilteredElementCollector(document).WhereElementIsNotElementType().OfClass(typeof(Dimension)).Cast<Dimension>().ToList();
        }

        public static List<SharedParameterElement> SharedParameterElements(Document document)
        {
            return new FilteredElementCollector(document).WhereElementIsNotElementType().OfClass(typeof(SharedParameterElement)).Cast<SharedParameterElement>().ToList();
        }


    }
}