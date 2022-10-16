using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using RPG_GAME.Application.DTO.Auth;
using Microsoft.IdentityModel.Tokens;
using RPG_GAME.Application.Time;
using RPG_GAME.Application.Auth;

namespace RPG_GAME.Infrastructure.Auth
{
    internal sealed class AuthManager : IAuthManager
    {
        private readonly AuthOptions _options;
        private readonly IClock _clock;
        private readonly SigningCredentials _signingCredentials;
        private readonly string _issuer;

        public AuthManager(AuthOptions authOptions, IClock clock)
        {
            var issuerSigningKey = authOptions.IssuerSigningKey;
            if (issuerSigningKey is null)
            {
                throw new InvalidOperationException("Issuer signing key not set.");
            }

            _options = authOptions;
            _clock = clock;
            _signingCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authOptions.IssuerSigningKey)), SecurityAlgorithms.HmacSha256);
            _issuer = authOptions.Issuer;
        }

        public JsonWebToken CreateToken(string userId, string userEmail, string role = null, string audience = null, IDictionary<string, IEnumerable<string>> claims = null)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                throw new ArgumentException("User ID claim (subject) cannot be empty.", nameof(userId));
            }

            var now = _clock.CurrentDate();
            var jwtClaims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Sub, userId),
                new(JwtRegisteredClaimNames.UniqueName, userId),
                new(JwtRegisteredClaimNames.Email, userEmail),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new(JwtRegisteredClaimNames.Iat, new DateTimeOffset(now).ToUnixTimeMilliseconds().ToString())
            };
            if (!string.IsNullOrWhiteSpace(role))
            {
                jwtClaims.Add(new Claim(ClaimTypes.Role, role));
            }

            if (!string.IsNullOrWhiteSpace(audience))
            {
                jwtClaims.Add(new Claim(JwtRegisteredClaimNames.Aud, audience));
            }

            if (claims?.Any() is true)
            {
                var customClaims = new List<Claim>();
                foreach (var (claim, values) in claims)
                {
                    customClaims.AddRange(values.Select(value => new Claim(claim, value)));
                }

                jwtClaims.AddRange(customClaims);
            }

            var expires = now.Add(_options.Expiry);

            var jwt = new JwtSecurityToken(
                _issuer,
                claims: jwtClaims,
                notBefore: now,
                expires: expires,
                signingCredentials: _signingCredentials
            );

            var token = new JwtSecurityTokenHandler().WriteToken(jwt);

            return new JsonWebToken
            {
                AccessToken = token,
                RefreshToken = string.Empty
            };
        }
    }
}
