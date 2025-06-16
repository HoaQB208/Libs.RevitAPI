using Autodesk.Revit.DB.ExtensibleStorage;
using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Libs.RevitAPI.Samples
{
    // Ví dụ lưu dữ liệu vào bộ nhớ mở rộng
    internal class DataStorageSample
    {
        internal readonly static Guid id = new Guid("{2B999359-8204-4226-A9F3-A41C1A7AD882}");
        internal readonly static string name = "NAME"; //Schema Name
        // KHAI BÁO CÁC KEY KIỂU STRING
        internal readonly static string keyListId = "ListId";
        internal readonly static string keyExportType = "ExportType";
        internal readonly static string keyPDF_PrintSetting = "PDF_PrintSetting";
        internal static Schema GetSchema()
        {
            Schema schema = Schema.Lookup(id);
            if (schema == null)
            {
                SchemaBuilder schemaBuilder = new SchemaBuilder(id);
                schemaBuilder.SetSchemaName(name);
                schemaBuilder.AddArrayField(keyListId, typeof(string));             // KIỂU IList<string>
                schemaBuilder.AddSimpleField(keyExportType, typeof(bool));          // KIỂU bool
                schemaBuilder.AddSimpleField(keyPDF_PrintSetting, typeof(string));  // KIỂU string
                schema = schemaBuilder.Finish();
            }
            return schema;
        }
        internal static void Write(Document doc, IList<string> listId, bool TypePDF, string PDFSetting)
        {
            try
            {
                ExtensibleStorageFilter f = new ExtensibleStorageFilter(id);
                DataStorage dataStorage = new FilteredElementCollector(doc).OfClass(typeof(DataStorage)).WherePasses(f).Where(e => name.Equals(e.Name)).FirstOrDefault() as DataStorage;
                using (Transaction t = new Transaction(doc, "DataStorage"))
                {
                    t.Start();
                    if (dataStorage == null)
                    {
                        dataStorage = DataStorage.Create(doc);
                        dataStorage.Name = name;
                    }
                    Entity entity = new Entity(GetSchema());
                    entity.Set(keyListId, listId);
                    entity.Set(keyExportType, TypePDF);
                    entity.Set(keyPDF_PrintSetting, PDFSetting);
                    dataStorage.SetEntity(entity);
                    t.Commit();
                }
            }
            catch { }
        }
        internal static List<object> Read(Document doc)
        {
            List<object> list = new List<object>();
            try
            {
                ExtensibleStorageFilter f = new ExtensibleStorageFilter(id);
                DataStorage dataStorage = new FilteredElementCollector(doc).OfClass(typeof(DataStorage)).WherePasses(f).Where(e => name.Equals(e.Name)).FirstOrDefault() as DataStorage;
                if (dataStorage != null)
                {
                    Entity entity = dataStorage.GetEntity(GetSchema());
                    if (entity.IsValid())
                    {
                        IList<string> ls = entity.Get<IList<string>>(keyListId);
                        bool exportType = entity.Get<bool>(keyExportType);
                        string pdfSt = entity.Get<string>(keyPDF_PrintSetting);
                        list.Add(ls);
                        list.Add(exportType);
                        list.Add(pdfSt);
                    }
                }
            }
            catch { }
            return list;
        }
    }
}
