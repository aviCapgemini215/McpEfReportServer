using Dapper;
using Microsoft.Data.SqlClient;
using McpEfReportServer.Services;

namespace McpEfReportServer.Services
{
    public class DynamicQueryTool
    {
        private readonly IConfiguration _config;
        private readonly SqlGuardService _guard;

        public DynamicQueryTool(
            IConfiguration config,
            SqlGuardService guard)
        {
            _config = config;
            _guard = guard;
        }

        public async Task<IEnumerable<dynamic>> ExecuteAsync(string sql)
        {
            // ✅ Clean + Validate SQL
            sql = _guard.ValidateReadOnly(sql);

            using var conn = new SqlConnection(
                _config.GetConnectionString("Default"));

            return await conn.QueryAsync(sql);
        }
    }
}
