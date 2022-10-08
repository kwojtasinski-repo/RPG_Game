using RPG_GAME.Application.Auth;
using RPG_GAME.Application.DTO.Auth;
using RPG_GAME.Application.Exceptions.Auth;
using RPG_GAME.Application.Time;
using RPG_GAME.Core.Entities.Users;
using RPG_GAME.Core.Repositories;
using System.Security.Cryptography;

namespace RPG_GAME.Application.Services
{
    internal sealed class RefreshTokenService : IRefreshTokenService
    {
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IUserRepository _userRepository;
        private readonly IAuthManager _authManager;
        private readonly IClock _clock;
        private static readonly string[] SpecialChars = new[] { "/", "\\", "=", "+", "?", ":", "&" };

        public RefreshTokenService(IRefreshTokenRepository refreshTokenRepository, IUserRepository userRepository, IAuthManager authManager,
            IClock clock)
        {
            _refreshTokenRepository = refreshTokenRepository;
            _userRepository = userRepository;
            _authManager = authManager;
            _clock = clock;
        }

        public async Task<string> CreateAsync(Guid userId)
        {
            var token = GenerateRefreshToken(30, true);
            var refreshToken = new RefreshToken(Guid.NewGuid(), userId, token, _clock.CurrentDate());
            await _refreshTokenRepository.AddAsync(refreshToken);
            return token;
        }

        public async Task<JsonWebToken> UseAsync(string refreshToken)
        {
            var token = await _refreshTokenRepository.GetAsync(refreshToken);

            if (token is null)
            {
                throw new InvalidRefreshTokenException();
            }

            if (token.Revoked)
            {
                throw new RevokedRefreshTokenException();
            }

            var user = await _userRepository.GetAsync(token.UserId);

            if (user is null)
            {
                throw new UserNotFoundException(token.UserId);
            }

            var jwt = _authManager.CreateToken(user.Id.ToString(), user.Email, user.Role);
            var newRefreshToken = await CreateAsync(user.Id);
            jwt.RefreshToken = newRefreshToken;
            token.Revoke(_clock.CurrentDate());
            await _refreshTokenRepository.UpdateAsync(token);

            return jwt;
        }

        private string GenerateRefreshToken(int length = 50, bool removeSpecialChars = true)
        {
            using var rng = RandomNumberGenerator.Create();
            var bytes = new byte[length];
            rng.GetBytes(bytes);
            var result = Convert.ToBase64String(bytes);

            return removeSpecialChars
                ? SpecialChars.Aggregate(result, (current, chars) => current.Replace(chars, string.Empty))
                : result;
        }
    }
}
