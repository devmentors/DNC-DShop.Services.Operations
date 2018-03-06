using System;
using System.Threading.Tasks;
using DShop.Services.Operations.Services;
using Microsoft.AspNetCore.Mvc;

namespace DShop.Services.Operations.Controllers
{
    [Route("[controller]")]
    public class OperationsController : Controller
    {
        private readonly IOperationsService _operationsService;

        public OperationsController(IOperationsService operationsService)
        {
            _operationsService = operationsService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
            => Ok(await _operationsService.GetAsync(id));
    }
}