using Autodesk.Revit.DB;
using Libs.RevitAPI._Geometry;
using System.Collections.Generic;
using System.Linq;
using Libs.RevitAPI._Common;
using System;

namespace Libs.RevitAPI._Create
{
    public class CreateDirectShape
    {
        public static void Create(Document doc, List<Solid> solids, Color color = null, BuiltInCategory builtInCategory = BuiltInCategory.OST_GenericModel)
        {
            using (Transaction transaction = new Transaction(doc, "CreateGrids DirectShape"))
            {
                transaction.Start();

                DirectShape ds = DirectShape.CreateElement(doc, new ElementId(builtInCategory));
                ds.SetShape(solids.Cast<GeometryObject>().ToList(), DirectShapeTargetViewType.Default);
                ElementUtils.SetOverrideGraphicSettings(doc, ds.Id, color);
                transaction.Commit();
            }
        }
        public static void CreateDirectShapeFromPlanarFace1(PlanarFace planarFace, Document doc)
        {
            // Bắt đầu giao dịch
            using (Transaction trans = new Transaction(doc, "Create DirectShape from PlanarFace"))
            {
                try
                {
                    trans.Start();
                    IList<CurveLoop> curveLoops = planarFace.GetEdgesAsCurveLoops();
                    List<CurveLoop> validCurveLoops = new List<CurveLoop>();
                    foreach (CurveLoop curveLoop in curveLoops)
                        if (!curveLoop.IsOpen()) validCurveLoops.Add(curveLoop);
                    Material material = GetOrCreateMaterial(doc, "CustomMaterial", new Color(255, 0, 0)); // Đỏ
                    // Tạo một Solid từ các CurveLoop hợp lệ (tạo một khối rắn từ bề mặt)
                    SolidOptions solidOptions = new SolidOptions(material.Id, ElementId.InvalidElementId);
                    Solid planarFaceSolid = GeometryCreationUtilities.CreateExtrusionGeometry(validCurveLoops, planarFace.FaceNormal, 1, solidOptions); // Chiều cao đùn ra
                    // Tạo DirectShape
                    DirectShape ds = DirectShape.CreateElement(doc, new ElementId(BuiltInCategory.OST_GenericModel));
                    // Gán thông tin metadata
                    ds.ApplicationId = "PlanarFaceToDirectShape"; // ID ứng dụng tạo ra đối tượng
                    ds.ApplicationDataId = "PlanarFaceData"; // ID dữ liệu liên quan
                    // Gán hình học cho DirectShape
                    ds.SetShape(new List<GeometryObject> { planarFaceSolid });
                    // Đặt tên hoặc các thuộc tính khác cho DirectShape nếu cần
                    ds.Name = "PlanarFaceShape";
                    // Kết thúc giao dịch
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    // Hủy giao dịch nếu có lỗi
                    trans.RollBack();
                    throw ex;
                }
            }
        }
        public static PlanarFace CreateDirectShapeFromPlanarFace(PlanarFace planarFace, Document doc, Element hostElement, string nameComment)
        {
            // Bắt đầu giao dịch
            using (Transaction trans = new Transaction(doc, "Create DirectShape from PlanarFace"))
            {
                try
                {
                    trans.Start();
                    // Lấy các CurveLoops từ PlanarFace
                    IList<CurveLoop> curveLoops = planarFace.GetEdgesAsCurveLoops();
                    List<CurveLoop> validCurveLoops = new List<CurveLoop>();
                    foreach (CurveLoop curveLoop in curveLoops)
                        if (!curveLoop.IsOpen()) validCurveLoops.Add(curveLoop);
                    // Tạo hoặc lấy Material
                    Material material = GetOrCreateMaterial(doc, "CustomMaterial", new Color(255, 0, 0)); // Đỏ
                    // Tạo một Solid từ các CurveLoop hợp lệ
                    SolidOptions solidOptions = new SolidOptions(material.Id, ElementId.InvalidElementId);
                    Solid planarFaceSolid = GeometryCreationUtilities.CreateExtrusionGeometry(validCurveLoops, planarFace.FaceNormal, 1, solidOptions); // Chiều cao đùn ra
                    // Tạo DirectShape
                    DirectShape ds = DirectShape.CreateElement(doc, new ElementId(BuiltInCategory.OST_GenericModel));
                    // Gán thông tin metadata
                    ds.ApplicationId = "PlanarFaceToDirectShape"; // ID ứng dụng tạo ra đối tượng
                    ds.ApplicationDataId = "PlanarFaceData"; // ID dữ liệu liên quan
                    // Gán hình học cho DirectShape
                    ds.SetShape(new List<GeometryObject> { planarFaceSolid });
                    ds.Name = "PlanarFaceShape";
                    // Kết thúc giao dịch
                    trans.Commit();
                    // Lấy mặt đáy của DirectShape (PlanarFace mới)
                    return GetBottomFaceOfDirectShape(ds, planarFace.FaceNormal);
                }
                catch (Exception ex)
                {
                    // Hủy giao dịch nếu có lỗi
                    trans.RollBack();

                    // Ghi comment vào hostElement nếu không thể tạo DirectShape
                    using (Transaction commentTrans = new Transaction(doc, "Add Comment for Failure"))
                    {
                        commentTrans.Start();
                        hostElement.get_Parameter(BuiltInParameter.ALL_MODEL_INSTANCE_COMMENTS).Set(nameComment);
                        commentTrans.Commit();
                    }

                    //Msg.Show(ex.Message);
                    return null;
                }
            }
        }
        public static Solid CreateDirectShapeFromPlanarFaceSolid(PlanarFace planarFace, Document doc)
        {
            // Bắt đầu giao dịch
            using (Transaction trans = new Transaction(doc, "Create DirectShape from PlanarFace"))
            {
                try
                {
                    trans.Start();

                    // Lấy các CurveLoops từ PlanarFace
                    IList<CurveLoop> curveLoops = planarFace.GetEdgesAsCurveLoops();
                    List<CurveLoop> validCurveLoops = new List<CurveLoop>();

                    foreach (CurveLoop curveLoop in curveLoops)
                        if (!curveLoop.IsOpen()) validCurveLoops.Add(curveLoop);

                    // Tạo hoặc lấy Material
                    Material material = GetOrCreateMaterial(doc, "CustomMaterial", new Color(255, 0, 0)); // Đỏ

                    // Tạo một Solid từ các CurveLoop hợp lệ
                    SolidOptions solidOptions = new SolidOptions(material.Id, ElementId.InvalidElementId);
                    Solid planarFaceSolid = GeometryCreationUtilities.CreateExtrusionGeometry(validCurveLoops, planarFace.FaceNormal, 1, solidOptions); // Chiều cao đùn ra

                    // Tạo DirectShape
                    DirectShape ds = DirectShape.CreateElement(doc, new ElementId(BuiltInCategory.OST_GenericModel));

                    // Gán thông tin metadata
                    ds.ApplicationId = "PlanarFaceToDirectShape"; // ID ứng dụng tạo ra đối tượng
                    ds.ApplicationDataId = "PlanarFaceData"; // ID dữ liệu liên quan

                    // Gán hình học cho DirectShape
                    ds.SetShape(new List<GeometryObject> { planarFaceSolid });
                    ds.Name = "PlanarFaceShape";

                    // Kết thúc giao dịch
                    trans.Commit();

                    // Trả về Solid đã tạo
                    return planarFaceSolid;
                }
                catch (Exception ex)
                {
                    // Hủy giao dịch nếu có lỗi
                    trans.RollBack();
                    Msg.Show(ex.Message);
                    return null;
                }
            }
        }
        public static PlanarFace GetBottomFaceOfDirectShape(DirectShape directShape, XYZ faceNormal)
        {
            // Lấy hình học của DirectShape
            Options options = new Options();
            GeometryElement geomElement = directShape.get_Geometry(options);
            foreach (GeometryObject geomObj in geomElement)
            {
                if (geomObj is Solid solid)
                {
                    foreach (Face face in solid.Faces)
                    {
                        if (face is PlanarFace planarFace)
                        {
                            // Kiểm tra nếu mặt phẳng có pháp tuyến ngược với hướng đùn ban đầu
                            if (planarFace.FaceNormal.IsAlmostEqualTo(-faceNormal))
                            {
                                // Trả về mặt đáy (PlanarFace)
                                return planarFace;
                            }
                        }
                    }
                }
            }
            return null;
        }
        private static Material GetOrCreateMaterial(Document doc, string materialName, Color color)
        {
            // Tìm material theo tên
            FilteredElementCollector collector = new FilteredElementCollector(doc);
            collector.OfClass(typeof(Material));
            Material material = collector.FirstOrDefault(e => e.Name.Equals(materialName)) as Material;
            // Nếu không tìm thấy, tạo material mới
            if (material == null)
            {
                ElementId materialId = Material.Create(doc, materialName);
                material = doc.GetElement(materialId) as Material;
                material.Color = color;
            }
            else material.Color = color;
            return material;
        }
        public static void Create(Document doc, BoundingBoxXYZ bb)
        {
            XYZ max = bb.Max;
            XYZ min = bb.Min;
            double z = min.Z;
            double height = max.Z - z;

            XYZ min1 = new XYZ(min.X, max.Y, z);
            XYZ min2 = new XYZ(max.X, max.Y, z);
            XYZ min3 = new XYZ(max.X, min.Y, z);

            List<XYZ> points = new List<XYZ>() { min, min1, min2, min3 };

            CurveLoop curveLoop = CurveUtils.GetCurveLoop(points);
            List<CurveLoop> curveLoops = new List<CurveLoop>() { curveLoop };
            Solid solid = CreateSolid.Create(curveLoops, XYZ.BasisZ, height);
            Create(doc, new List<Solid>() { solid });
        }
        public static Face GetLargestFace(Solid solid)
        {
            Face largestFace = null;
            double maxArea = 0;

            // Duyệt qua tất cả các Faces để tìm RuledFace hoặc PlanarFace có diện tích lớn nhất
            foreach (Face face in solid.Faces)
            {
                double area = face.Area;
                if (face is RuledFace || (face is PlanarFace planarFace && planarFace.FaceNormal.Z > 0))
                {
                    if (area > maxArea)
                    {
                        maxArea = area;
                        largestFace = face;
                    }
                }
            }

            return largestFace;
        }
        public static void ProcessSolid(Solid solid, Document doc)
        {
            // Lấy mặt có diện tích lớn nhất từ Solid
            Face face = GetLargestFace(solid);

            if (face != null)
            {
                // Truyền Face vào hàm CreateDirectShapeFromFace
                CreateDirectShapeFromFace(face, doc);
            }
        }
        public static void CreateDirectShapeFromFace(Face face, Document doc)
        {
            using (Transaction trans = new Transaction(doc, "Create DirectShape from Face"))
            {
                try
                {
                    trans.Start();
                    // Lấy các CurveLoops từ Face
                    IList<CurveLoop> curveLoops = face.GetEdgesAsCurveLoops();
                    List<CurveLoop> validCurveLoops = new List<CurveLoop>();
                    foreach (CurveLoop curveLoop in curveLoops)
                    {
                        if (!curveLoop.IsOpen()) validCurveLoops.Add(curveLoop);
                    }
                    // Tạo hoặc lấy Material
                    Material material = GetOrCreateMaterial(doc, "CustomMaterial", new Color(255, 0, 0));
                    // Tạo một Solid từ các CurveLoop hợp lệ
                    SolidOptions solidOptions = new SolidOptions(material.Id, ElementId.InvalidElementId);
                    Solid faceSolid = GeometryCreationUtilities.CreateExtrusionGeometry(validCurveLoops, face.ComputeNormal(new UV(0, 0)), 1, solidOptions);
                    // Tạo DirectShape
                    DirectShape ds = DirectShape.CreateElement(doc, new ElementId(BuiltInCategory.OST_GenericModel));
                    ds.ApplicationId = "FaceToDirectShape";
                    ds.ApplicationDataId = "FaceData";
                    ds.SetShape(new List<GeometryObject> { faceSolid });
                    ds.Name = "FaceShape";
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.RollBack();
                    Msg.Show(ex.Message);
                }
            }
        }
    }
}