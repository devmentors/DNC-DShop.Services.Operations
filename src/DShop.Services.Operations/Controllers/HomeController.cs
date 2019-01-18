using Microsoft.AspNetCore.Mvc;

namespace DShop.Services.Operations.Controllers
{
    [Route("")]
    public class HomeController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get() => Ok("DShop Operations Service");
    }
}