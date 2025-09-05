using Autodesk.Revit.DB;
using System.Collections.Generic;
using System;
using System.Linq;
using Libs.RevitAPI._Common;

namespace Libs.RevitAPI._Geometry
{
    public class GridUtils
    {
        public static Line GetLine(Grid grid, Document doc)
        {
            try
            {
                List<Line> lines = new List<Line>();
                var opt = new Options()
                {
                    ComputeReferences = true,
                    IncludeNonVisibleObjects = false,
                    View = doc.ActiveView,
                };
                foreach (var obj in grid.get_Geometry(opt))
                {
                    if (obj is Line ln) lines.Add(ln);
                }
                if (lines.Count == 0) return null;
                if (lines.Count == 1) return lines[0];

                List<XYZ> pts = new List<XYZ>();
                foreach (var line in lines)
                {
                    pts.Add(line.GetEndPoint(0));
                    pts.Add(line.GetEndPoint(1));
                }
                // Sắp xếp theo X -> Y -> Z (sau khi làm tròn 6 chữ số thập phân)
                var sortedPts = pts
                    .Distinct(new XYZComparer(6)) // Nếu muốn loại bỏ điểm trùng
                    .OrderBy(p => Math.Round(p.X, 6))
                    .ThenBy(p => Math.Round(p.Y, 6))
                    .ThenBy(p => Math.Round(p.Z, 6))
                    .ToList();

                // Trả về đường thẳng nối 2 điểm đầu - cuối sau khi sắp xếp
                return Line.CreateBound(sortedPts.First(), sortedPts.Last());
            }
            catch { }
            return null;
        }
    }
}
