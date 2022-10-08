using RPG_GAME.Core.Exceptions.Users;
using System;

namespace RPG_GAME.Core.Entities.Users
{
    public class RefreshToken
    {
        public Guid Id { get; }
        public Guid UserId { get; }
        public string Token { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? RevokedAt { get; private set; }
        public bool Revoked => RevokedAt.HasValue;

        public RefreshToken(Guid id, Guid userId, string token, DateTime createdAt,
            DateTime? revokedAt = null)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                throw new EmptyRefreshTokenException();
            }

            Id = id;
            UserId = userId;
            Token = token;
            CreatedAt = createdAt;
            RevokedAt = revokedAt;
        }

        public void Revoke(DateTime revokedAt)
        {
            if (Revoked)
            {
                throw new RevokedRefreshTokenException();
            }

            RevokedAt = revokedAt;
        }
    }
}
