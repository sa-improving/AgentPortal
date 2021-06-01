using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AgentPortal.Models;
using AgentPortal.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace AgentPortal.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _configuration;
        private readonly AgentData _agentData;
        public HomeController(ILogger<HomeController> logger, IConfiguration configuration, AgentData agentData)
        {
            _logger = logger;
            _configuration = configuration;
            _agentData = agentData;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Agents()
        {
            var agents = _agentData.AllAgentData();
            var vm = new AgentListViewModel();
            vm.Agents = agents;
            return View(vm);
        }

        public IActionResult AgentsWithAjax() 
        {
            return View();
        }

        public IActionResult AgentData()
        {
            var agents = _agentData.AllAgentData();
            return Json(agents);
        }

        public IActionResult Agent(string id)
        {
            var vm = new AgentViewModel();
            vm.agent = _agentData.GetAgentByAgentCode(id);
            return View(vm);
        }

        [HttpGet]
        public IActionResult NewAgent()
        {
            var newCode = _agentData.newAgentCode();

            var vm = new AgentCodeViewModel();
            vm.AgentCode = "A" + newCode;

            return View(vm);
        }

        [HttpPost]
        public IActionResult NewAgent(Agent agent)
        {
            _agentData.CreateNewAgent(agent);
            return RedirectToAction("Agents");
        }

        public IActionResult HideAgent(string id)
        {
            _agentData.HideAgent(id);
            return RedirectToAction("Agents");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
