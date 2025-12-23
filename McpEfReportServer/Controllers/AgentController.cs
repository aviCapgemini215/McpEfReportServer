using McpEfReportServer.Models;
using McpEfReportServer.Services;
using Microsoft.AspNetCore.Mvc;

namespace McpEfReportServer.Controllers
{
    public class AgentController : Controller
    {
        private readonly AdaptiveAgentService _agent;

        public AgentController(AdaptiveAgentService agent)
        {
            _agent = agent;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(string prompt)
        {
            AgentResultVm result = await _agent.RunWithSqlAsync(prompt);
            return View(result);
        }
    }
}
