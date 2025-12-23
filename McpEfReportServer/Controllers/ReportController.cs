using McpEfReportServer.Services;
using Microsoft.AspNetCore.Mvc;

namespace McpEfReportServer.Controllers
{
    public class ReportController : Controller
    {
        private readonly DynamicQueryTool _query;
        private readonly ExcelExportService _excel;
        private readonly PdfExportService _pdf;

        public ReportController(
            DynamicQueryTool query,
            ExcelExportService excel,
            PdfExportService pdf)
        {
            _query = query;
            _excel = excel;
            _pdf = pdf;
        }

        [HttpPost]
        public async Task<IActionResult> ExportExcel(string sql)
        {
            var data = await _query.ExecuteAsync(sql);

            return File(
                _excel.Export(data),
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                "Report.xlsx");
        }

        [HttpPost]
        public async Task<IActionResult> ExportPdf(string sql)
        {
            var data = await _query.ExecuteAsync(sql);

            return File(
                _pdf.Export(data),
                "application/pdf",
                "Report.pdf");
        }
    }
}
