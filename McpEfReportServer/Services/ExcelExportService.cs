using ClosedXML.Excel;
using System.Data;
namespace McpEfReportServer.Services
{
    public class ExcelExportService
    {
        public byte[] Export(IEnumerable<dynamic> data)
        {
            using var wb = new XLWorkbook();
            var ws = wb.Worksheets.Add("Report");

            var table = new DataTable();

            var first = data.FirstOrDefault();
            if (first == null) return Array.Empty<byte>();

            foreach (var prop in ((IDictionary<string, object>)first).Keys)
                table.Columns.Add(prop);

            foreach (IDictionary<string, object> row in data)
                table.Rows.Add(row.Values.ToArray());

            ws.Cell(1, 1).InsertTable(table);

            using var ms = new MemoryStream();
            wb.SaveAs(ms);
            return ms.ToArray();
        }
    }
}
