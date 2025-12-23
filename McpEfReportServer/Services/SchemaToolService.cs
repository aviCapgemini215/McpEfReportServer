using Dapper;
using Microsoft.Data.SqlClient;
namespace McpEfReportServer.Services
{
    public class SchemaToolService
    {
        private readonly string _conn;

        public SchemaToolService(IConfiguration config)
        {
            _conn = config.GetConnectionString("Default");
        }

        public async Task<object> GetSchemaAsync()
        {
            using var db = new SqlConnection(_conn);

            var tables = await db.QueryAsync("""
            SELECT TABLE_NAME
            FROM INFORMATION_SCHEMA.TABLES
            WHERE TABLE_TYPE='BASE TABLE'
        """);

            var columns = await db.QueryAsync("""
            SELECT TABLE_NAME, COLUMN_NAME, DATA_TYPE
            FROM INFORMATION_SCHEMA.COLUMNS
        """);

            return new
            {
                Tables = tables,
                Columns = columns
            };
        }
    }

}
