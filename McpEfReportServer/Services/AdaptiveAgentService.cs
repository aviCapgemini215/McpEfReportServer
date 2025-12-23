using McpEfReportServer.Models;
using OpenAI.Chat;
using System.Text.Json;
namespace McpEfReportServer.Services
{
    public class AdaptiveAgentService
    {
        private readonly SchemaToolService _schemaTool;
        private readonly DynamicQueryTool _queryTool;
        private readonly ChatClient _client;

        public AdaptiveAgentService(
            SchemaToolService schemaTool,
            DynamicQueryTool queryTool,
            IConfiguration config)
        {
            _schemaTool = schemaTool;
            _queryTool = queryTool;

            _client = new ChatClient(
                model: "gpt-4.1-mini",
                apiKey: config["OpenAI:ApiKey"]);
        }

        public async Task<string> RunAsync(string prompt)
        {
            // 1️⃣ Read DB schema dynamically
            var schema = await _schemaTool.GetSchemaAsync();

            // 2️⃣ Ask AI to generate SQL
            var sqlResponse = await _client.CompleteChatAsync(
                new List<ChatMessage>
                {
                    ChatMessage.CreateSystemMessage(
                        "You are an AI SQL expert. Generate ONLY a SELECT SQL query."),
                    ChatMessage.CreateUserMessage(
                        $"Database schema:\n{JsonSerializer.Serialize(schema)}\n\nUser request:\n{prompt}")
                });

            var sql = sqlResponse.Value.Content[0].Text.Trim();

            // 3️⃣ Execute SQL
            var data = await _queryTool.ExecuteAsync(sql);

            // 4️⃣ Ask AI to generate a readable report
            var reportResponse = await _client.CompleteChatAsync(
                new List<ChatMessage>
                {
                    ChatMessage.CreateSystemMessage(
                        "You are a business analyst AI."),
                    ChatMessage.CreateUserMessage(
                        $"Create a professional report from this data:\n{JsonSerializer.Serialize(data)}")
                });

            return reportResponse.Value.Content[0].Text;
        }
        public async Task<AgentResultVm> RunWithSqlAsync(string prompt)
        {
            var schema = await _schemaTool.GetSchemaAsync();

            var sqlResponse = await _client.CompleteChatAsync(
                new List<ChatMessage>
                {
                    ChatMessage.CreateSystemMessage("\"Generate ONLY a SELECT SQL query. \" +\r\n    \"DO NOT add comments, markdown, or explanations.\""),
                    ChatMessage.CreateUserMessage($"{JsonSerializer.Serialize(schema)}\n{prompt}")
                });

            var sql = sqlResponse.Value.Content[0].Text;

            var data = await _queryTool.ExecuteAsync(sql);

            var reportResponse = await _client.CompleteChatAsync(
                new List<ChatMessage>
                {
                    ChatMessage.CreateSystemMessage("Create business report"),
                    ChatMessage.CreateUserMessage(JsonSerializer.Serialize(data))
                });

            return new AgentResultVm
            {
                Sql = sql,
                Report = reportResponse.Value.Content[0].Text
            };
        }

    }
}
