using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RPG_GAME.Application.DTO.Auth;
using RPG_GAME.Application.Services;

namespace RPG_GAME.Api.Controllers
{
    public class RefreshTokensController : BaseController
    {
        private readonly IRefreshTokenService _refreshTokenService;

        public RefreshTokensController(IRefreshTokenService refreshTokenService)
        {
            _refreshTokenService = refreshTokenService;
        }

        [AllowAnonymous]
        [HttpPost("use")]
        public async Task<JsonWebToken> RefreshTokenAsync(RefreshTokenDto refreshTokenDto)
        {
            var jwt = await _refreshTokenService.UseAsync(refreshTokenDto.Token);
            return jwt;
        }
    }
}
