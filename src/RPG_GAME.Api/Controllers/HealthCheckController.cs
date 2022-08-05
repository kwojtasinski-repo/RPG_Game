using Microsoft.AspNetCore.Mvc;

namespace RPG_GAME.Api.Controllers
{
    [ApiController]
    [Route("api")]
    public class HealthCheckController : ControllerBase
    {
        [HttpGet]
        public string Get()
        {
            return "RPG_GAME Api!";
        }
    }
}
