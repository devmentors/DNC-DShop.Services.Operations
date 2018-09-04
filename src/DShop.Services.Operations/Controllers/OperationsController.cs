using System;
using System.Threading.Tasks;
using DShop.Services.Operations.Managers;
using DShop.Services.Operations.Services;
using Microsoft.AspNetCore.Mvc;

namespace DShop.Services.Operations.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class OperationsController : Controller
    {
        private readonly IOperationsService _operationsService;
        private readonly IProcessOrchestrator _processOrchestrator;

        public OperationsController(IOperationsService operationsService,
            IProcessOrchestrator processOrchestrator)
        {
            _operationsService = operationsService;
            _processOrchestrator = processOrchestrator;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
            => Ok(await _operationsService.GetAsync(id));
    }
}