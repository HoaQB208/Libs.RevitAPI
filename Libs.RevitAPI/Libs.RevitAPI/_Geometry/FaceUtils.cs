using Autodesk.Revit.DB;
using System.Collections.Generic;
using System.Linq;
using Libs.RevitAPI._Common;

namespace Libs.RevitAPI._Geometry
{
    public class FaceUtils
    {

        public static Plane CreatePlaneFromPlanarFace(PlanarFace planarFace)
        {
            XYZ origin = planarFace.Origin;
            XYZ xVector = planarFace.XVector;
            XYZ yVector = planarFace.YVector;
            Plane plane = Plane.CreateByOriginAndBasis(origin, xVector, yVector);
            return plane;
        }
        public static Plane GetPlaneFromLevel(Level level)
        {
            XYZ origin = new XYZ(0, 0, level.Elevation);  // Lấy độ cao Z của Level
            XYZ normal = XYZ.BasisZ;  // Mặt phẳng ngang có vector pháp tuyến là trục Z

            return Plane.CreateByNormalAndOrigin(normal, origin);
        }
        public static List<Face> Get(Element el)
        {
            List<Face> faces = new List<Face>();
            if (el != null)
            {
                List<Solid> solids = TBFSolidUtils.Get(el);
                foreach (Solid solid in solids) foreach (Face f in solid.Faces) faces.Add(f);
            }
            return faces;
        }

        public static PlanarFace GetMatchingPlanarFace(PlanarFace planarFace, double extrusionLength)
        {
            // Lấy CurveLoop đầu tiên từ PlanarFace
            CurveLoop curveLoop = planarFace.GetEdgesAsCurveLoops().FirstOrDefault();
            if (curveLoop == null)
            {
                return null; // Trả về null nếu không tìm thấy CurveLoop
            }
            // Đặt vector đùn là vector pháp tuyến của PlanarFace
            XYZ extrusionDirection = planarFace.FaceNormal;
            // Tạo Solid bằng cách đùn CurveLoop
            Solid solid = GeometryCreationUtilities.CreateExtrusionGeometry(new List<CurveLoop> { curveLoop }, extrusionDirection, extrusionLength);
            // Duyệt qua các mặt của Solid để tìm PlanarFace tương ứng
            foreach (Face face in solid.Faces)
            {
                PlanarFace solidFace = face as PlanarFace;
                if (solidFace != null && solidFace.FaceNormal.IsAlmostEqualTo(planarFace.FaceNormal.Negate()))
                {
                    // So sánh vị trí của solidFace với PlanarFace ban đầu
                    return solidFace;
                }
            }
            return null; // Trả về null nếu không tìm thấy mặt tương ứng
        }

        public static List<XYZ> GetPoints(Face f)
        {
            List<XYZ> points = new List<XYZ>();
            if (f != null)
            {
                foreach (EdgeArray edgeArray in f.EdgeLoops)
                {
                    foreach (Edge edge in edgeArray)
                    {
                        XYZ startPoint = edge.Evaluate(0);
                        XYZ endPoint = edge.Evaluate(1);
                        if (!points.Any(x => x.DistanceTo(startPoint) < 1e-8)) points.Add(startPoint);
                        if (!points.Any(x => x.DistanceTo(endPoint) < 1e-8)) points.Add(endPoint);
                    }
                }
            }
            return points;
        }


        public static Transform PlanarFaceTransform(PlanarFace face)
        {
            return TransformByVectors(
                XYZ.BasisX, XYZ.BasisY, XYZ.BasisZ, XYZ.Zero,
                 face.XVector, face.YVector, face.FaceNormal, face.Origin);
        }


        /// <summary>
        /// recheck and create documention 
        /// </summary>
        /// <param name="oldX"></param>
        /// <param name="oldY"></param>
        /// <param name="oldZ"></param>
        /// <param name="oldOrigin"></param>
        /// <param name="newX"></param>
        /// <param name="newY"></param>
        /// <param name="newZ"></param>
        /// <param name="newOrigin"></param>
        /// <returns></returns>
        public static Transform TransformByVectors(
        XYZ oldX, XYZ oldY, XYZ oldZ, XYZ oldOrigin,
        XYZ newX, XYZ newY, XYZ newZ, XYZ newOrigin)
        {
            // [new vector] = [transform]*[old vector]
            // [3x1] = [3x4] * [4x1]
            //
            // [v'x]   [ i*i'  j*i'  k*i'  translationX' ]   [vx]
            // [v'y] = [ i*j'  j*j'  k*j'  translationY' ] * [vy]
            // [v'z]   [ i*k'  j*k'  k*k'  translationZ' ]   [vz]
            //                                               [1 ]
            Transform t = Transform.Identity;

            double xx = oldX.DotProduct(newX);
            double xy = oldX.DotProduct(newY);
            double xz = oldX.DotProduct(newZ);

            double yx = oldY.DotProduct(newX);
            double yy = oldY.DotProduct(newY);
            double yz = oldY.DotProduct(newZ);

            double zx = oldZ.DotProduct(newX);
            double zy = oldZ.DotProduct(newY);
            double zz = oldZ.DotProduct(newZ);

            t.BasisX = new XYZ(xx, xy, xz);
            t.BasisY = new XYZ(yx, yy, yz);
            t.BasisZ = new XYZ(zx, zy, zz);

            // The movement of the origin point 
            // in the old coordinate system
            XYZ translation = newOrigin - oldOrigin;

            // Convert the translation into coordinates 
            // in the new coordinate system
            double translationNewX = xx * translation.X + yx * translation.Y + zx * translation.Z;
            double translationNewY = xy * translation.X + yy * translation.Y + zy * translation.Z;
            double translationNewZ = xz * translation.X + yz * translation.Y + zz * translation.Z;

            t.Origin = new XYZ(-translationNewX, -translationNewY, -translationNewZ);

            return t;
        }


        public static bool IsInsideFace(Face bigFace, Face smallFace)
        {
            bool isInside = false;


            return isInside;
        }

        public static PlanarFace GetTopFaceFromSolid(Solid solid)
        {
            // Lặp qua các mặt của Solid để lấy PlanarFace có FaceNormal.Z > 0
            foreach (Face face in solid.Faces)
            {
                if (face is PlanarFace planarFace && planarFace.FaceNormal.Z > 0)
                {
                    return planarFace;
                }
            }
            return null;
        }
        public static Solid CreateSolidFromCurveLoop(CurveLoop curveLoop, double height, XYZ vector = null)
        {
            // Tạo danh sách chứa CurveLoop
            List<CurveLoop> curveLoops = new List<CurveLoop>() { curveLoop };
            // Tạo Solid bằng cách đùn CurveLoop theo hướng Z lên với chiều cao nhất định
            Solid solid = GeometryCreationUtilities.CreateExtrusionGeometry(curveLoops, vector, height);
            return solid;
        }
        public static PlanarFace CreateFaceFromCurveLoop(CurveLoop curveLoop, double height, XYZ vector =null)
        {
            // Nếu vector không được cung cấp, sử dụng giá trị mặc định là XYZ.BasisZ.Negate()
            vector = vector ?? XYZ.BasisZ.Negate();
            // Tạo Solid từ CurveLoop với chiều cao đùn (extrude height)
            Solid solid = CreateSolidFromCurveLoop(curveLoop, height,vector);
            // Trích xuất mặt trên cùng từ Solid
            return GetTopFaceFromSolid(solid);
        }
    }
}