using Microsoft.AspNetCore.Mvc;
using RPG_GAME.Application.DTO.Auth;
using RPG_GAME.Application.Services;

namespace RPG_GAME.Api.Controllers
{
    public class AccountController : BaseController
    {
        private readonly IIdentityService _identityService;

        public AccountController(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        [HttpPost]
        public async Task<ActionResult> SignUp(SignUpDto signUpDto)
        {
            await _identityService.SignUpAsync(signUpDto);
            return Ok();
        }

        [HttpPost("me")]
        public async Task<JsonWebToken> SignIn(SignInDto signInDto)
        {
            var token = await _identityService.SignInAsync(signInDto);
            return token;
        }
    }
}
