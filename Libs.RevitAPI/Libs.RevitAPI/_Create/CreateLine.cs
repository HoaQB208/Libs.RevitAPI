using Autodesk.Revit.DB;

namespace Libs.RevitAPI._Create
{
    public class CreateLine
    {
        public static void Create(Document doc, XYZ start, XYZ end)
        {
            using Transaction tr = new (doc);
            tr.Start("CreateGrids Line");
            Line curve = Line.CreateBound(start, end);
            doc.Create.NewDetailCurve(doc.ActiveView, curve);
            tr.Commit();
        }

        public static void CreateModelLine(Document doc, XYZ point1, XYZ point2)
        {
            // Bắt đầu một transaction
            using (Transaction trans = new Transaction(doc, "Create Model Line"))
            {
                trans.Start();
                // Tạo vector pháp tuyến cho mặt phẳng chứa đoạn thẳng
                XYZ lineDirection = (point2 - point1).Normalize();
                XYZ upDirection = XYZ.BasisZ;
                // Kiểm tra nếu lineDirection và upDirection song song hoặc gần như song song
                XYZ normal = lineDirection.CrossProduct(upDirection);
                if (normal.IsZeroLength())
                {
                    // Nếu normal có độ dài bằng không, chọn một vector khác làm upDirection
                    upDirection = XYZ.BasisX;
                    normal = lineDirection.CrossProduct(upDirection);
                }
                // Chuẩn hóa vector pháp tuyến
                normal = normal.Normalize();
                // Tạo SketchPlane với vector pháp tuyến và điểm gốc
                Plane plane = Plane.CreateByNormalAndOrigin(normal, point1);
                SketchPlane sketchPlane = SketchPlane.Create(doc, plane);
                // Tạo đường thẳng (Line) từ point1 đến point2
                Line geomLine = Line.CreateBound(point1, point2);
                // Tạo Model Line
                doc.Create.NewModelCurve(geomLine, sketchPlane);
                // Kết thúc transaction
                trans.Commit();
            }
        }


        public static void CreateVLine(Document doc, XYZ point)
        {
            XYZ point2 = new XYZ(point.X, point.Y, point.Z + 100);
            CreateModelLine(doc, point, point2);
        }
    }
}