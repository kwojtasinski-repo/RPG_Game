using Microsoft.AspNetCore.Mvc;
using RPG_GAME.Application.DTO;
using RPG_GAME.Application.Services;

namespace RPG_GAME.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountsController : ControllerBase
    {
        private readonly IIdentityService _identityService;

        public AccountsController(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        [HttpPost]
        public async Task<ActionResult> Add(SignUpDto signUpDto)
        {
            await _identityService.SignUpAsync(signUpDto);
            return Ok();
        }
    }
}
