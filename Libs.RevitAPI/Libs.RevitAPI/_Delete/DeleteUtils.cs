using Autodesk.Revit.DB;
using System.Collections.Generic;
using System.Linq;

namespace Libs.RevitAPI._Delete
{
    public class DeleteUtils
    {
        public static void DeleteObject(Document doc, ElementFilter filter)
        {
            // Bắt đầu một giao dịch để xóa các đối tượng
            using (Transaction transaction = new Transaction(doc, "Delete Objects"))
            {
                transaction.Start();
                // Tạo một danh sách các đối tượng cần xóa
                FilteredElementCollector collector = new FilteredElementCollector(doc, doc.ActiveView.Id);
                List<ElementId> elementIdsToDelete = collector.WherePasses(filter).ToElementIds().ToList();
                elementIdsToDelete.Reverse();
                // Xóa từng đối tượng trong danh sách
                foreach (ElementId elementId in elementIdsToDelete)
                {
                    try
                    {
                        doc.Delete(elementId);
                    }
                    catch { }
                }
                transaction.Commit();
            }
        }
        public static void DeleteAllObjects(Document doc)
        {
            // Bắt đầu một giao dịch để xóa các đối tượng
            using (Transaction transaction = new Transaction(doc, "Delete All Objects"))
            {
                transaction.Start();
                // Tạo một FilteredElementCollector để thu thập tất cả các đối tượng trong tài liệu
                FilteredElementCollector collector = new FilteredElementCollector(doc);
                // Lấy tất cả các đối tượng (không phải loại "ElementType") trong tài liệu
                List<ElementId> elementIdsToDelete = collector.WhereElementIsNotElementType().ToElementIds().ToList();
                // Xóa từng đối tượng trong danh sách
                foreach (ElementId elementId in elementIdsToDelete)
                {
                    try
                    {
                        doc.Delete(elementId);
                    }
                    catch { }
                }
                // Kết thúc giao dịch
                transaction.Commit();
            }
        }
    }
}
