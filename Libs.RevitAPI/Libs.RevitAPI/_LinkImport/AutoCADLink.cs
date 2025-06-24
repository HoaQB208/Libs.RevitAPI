using Autodesk.Revit.DB;
using System.Collections.Generic;

namespace Libs.RevitAPI._LinkImport
{
    public class AutoCADLink
    {
        public static List<string> GetBlockNames(ImportInstance cadInstance)
        {
            List<string> blockNames = new List<string>();
            foreach (GeometryObject geo in cadInstance.get_Geometry(new Options()))
            {
                if (geo is GeometryInstance geoIns)
                {
                    foreach (GeometryObject obj in geoIns.SymbolGeometry)
                    {
                        if (obj is GeometryInstance blockIns)
                        {
                            if (blockIns != null)
                            {
#if R2024 || R2025
                                //need to fix for Revit 24
#else
                                if (blockIns.Symbol != null)
                                {
                                    string name = blockIns.Symbol.Name;
                                    if (!blockNames.Contains(name)) blockNames.Add(name);
                                }
#endif

                            }
                        }
                    }
                }
            }
            return blockNames;
        }

        public static List<GeometryInstance> GetBlocksByName(ImportInstance cadInstance, string blockName)
        {
            List<GeometryInstance> blocks = new List<GeometryInstance>();
            foreach (GeometryObject geo in cadInstance.get_Geometry(new Options()))
            {
                if (geo is GeometryInstance geoIns)
                {
                    foreach (GeometryObject obj in geoIns.SymbolGeometry)
                    {
                        if (obj is GeometryInstance blockIns)
                        {
                            if (blockIns != null)
                            {
#if R2024 || R2025
                                //need to fix for Revit 24
#else
                                if (blockIns.Symbol != null)
                                {
                                    if (blockIns.Symbol.Name == blockName) blocks.Add(blockIns);
                                }
#endif


                            }
                        }
                    }
                }
            }
            return blocks;
        }


        public static List<string> GetListNameBlock(ImportInstance cadInstance, string TrimName)
        {
            List<string> list = new List<string>();
            foreach (GeometryObject geometryObject in cadInstance.get_Geometry(new Options()))
            {
                if (geometryObject is GeometryInstance)
                {
                    foreach (GeometryObject blockObject in (geometryObject as GeometryInstance).SymbolGeometry)
                    {
                        if (blockObject is GeometryInstance) // This could be a block
                        {
#if R2024 || R2025
                            // need to fix in revit 2024
#else
                            GeometryInstance blockInstance = blockObject as GeometryInstance;
                            string nameBlock = blockInstance.Symbol.Name.Replace(TrimName, "");
                            if (!list.Contains(nameBlock))
                            {
                                list.Add(nameBlock);
                            }
#endif


                        }
                    }
                }
            }
            return list;
        }
    }
}