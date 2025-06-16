using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Libs.RevitAPI._Geometry
{
    public class TBFSolidUtils
    {
        public static List<Solid> Get(Element el)
        {
            List<Solid> solids = new List<Solid>();
            Options options = new Options() { ComputeReferences = true, IncludeNonVisibleObjects = false, DetailLevel = ViewDetailLevel.Fine };
            GeometryElement geos = el.get_Geometry(options);

            solids = geos.OfType<Solid>().Where(x => x != null & !x.Edges.IsEmpty & !x.Volume.Equals(0)).ToList();

            IEnumerable<GeometryInstance> geoIns = geos.OfType<GeometryInstance>();
            foreach (GeometryInstance geo in geoIns)
            {
                if (geo != null)
                {
                    solids.AddRange(geo.GetInstanceGeometry().Cast<Solid>().Where(x => null != x & !x.Edges.IsEmpty).ToList());
                }
            }

            return solids;
        }

        public static Solid GetSolid(Element el)
        {

            Options geomOptions = new Options
            {
                ComputeReferences = true, // Đảm bảo tham chiếu được tính toán
                DetailLevel = ViewDetailLevel.Fine
            };
            GeometryElement geoElement = el.get_Geometry(new Options(geomOptions));
            foreach (GeometryObject geoObject in geoElement)
            {
                if (geoObject is Solid)
                {
                    Solid solid = geoObject as Solid;
                    if (solid.Volume > 0) return solid;
                }
                else if (geoObject is GeometryInstance)
                {
                    GeometryInstance geoInstance = geoObject as GeometryInstance;
                    GeometryElement geoElement2 = geoInstance.GetInstanceGeometry();
                    foreach (GeometryObject geoObject2 in geoElement2)
                    {
                        if (geoObject2 is Solid)
                        {
                            Solid solid = geoObject2 as Solid;
                            if (solid.Volume > 0) return solid;
                        }
                    }
                }
            }
            return null;
        }
        public static IList<PlanarFace> GetAllPlanarFaces(Solid solid)
        {
            IList<PlanarFace> planarFaces = new List<PlanarFace>();
            foreach (Face face in solid.Faces)
            {
                if (face is PlanarFace pf) planarFaces.Add(pf);
            }
            return planarFaces;
        }
        public static IList<PlanarFace> GetAllPlanarFacesWithUniqueAreas(Solid solid)
        {
            IList<PlanarFace> planarFaces = new List<PlanarFace>();
            HashSet<double> uniqueAreas = new HashSet<double>();

            foreach (Face face in solid.Faces)
            {
                if (face is PlanarFace pf)
                {
                    double area = Math.Round(pf.Area, 6);
                    if (uniqueAreas.Add(area)) 
                    {
                        planarFaces.Add(pf);
                    }
                }
            }

            return planarFaces;
        }

    }
}