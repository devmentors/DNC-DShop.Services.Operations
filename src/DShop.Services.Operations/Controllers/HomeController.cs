using DShop.Common.Dispatchers;
using Microsoft.AspNetCore.Mvc;

namespace DShop.Services.Operations.Controllers
{
    [Route("")]
    public class HomeController : BaseController
    {
        public HomeController(IDispatcher dispatcher) : base(dispatcher)
        {
        }
        
        [HttpGet("")]
        public IActionResult Get() => Ok("DShop Operations Service");
    }
}