using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
namespace McpEfReportServer.Services
{
    public class PdfExportService
    {
        public byte[] Export(IEnumerable<dynamic> data)
        {
            QuestPDF.Settings.License = LicenseType.Community;

            return Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Margin(30);
                    page.Header().Text("AI Generated Report").FontSize(20).Bold();

                    page.Content().Table(table =>
                    {
                        var first = (IDictionary<string, object>)data.First();

                        table.ColumnsDefinition(cols =>
                        {
                            foreach (var _ in first.Keys)
                                cols.RelativeColumn();
                        });

                        table.Header(header =>
                        {
                            foreach (var key in first.Keys)
                                header.Cell().Text(key).Bold();
                        });

                        foreach (IDictionary<string, object> row in data)
                        {
                            foreach (var val in row.Values)
                                table.Cell().Text(val?.ToString());
                        }
                    });
                });
            }).GeneratePdf();
        }
    }

}
