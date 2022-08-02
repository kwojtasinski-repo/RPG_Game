using RPG_GAME.Application.DTO.Auth;

namespace RPG_GAME.Application.Services
{
    public interface IIdentityService
    {
        Task<JsonWebToken> SignInAsync(SignInDto dto);
        Task SignUpAsync(SignUpDto dto);
    }
}
