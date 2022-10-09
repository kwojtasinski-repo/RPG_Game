using FluentAssertions;
using RPG_GAME.Application.Auth;
using RPG_GAME.Application.Exceptions.Auth;
using RPG_GAME.Application.Services;
using RPG_GAME.Application.Time;
using RPG_GAME.Core.Entities.Users;
using RPG_GAME.Core.Repositories;
using RPG_GAME.UnitTests.Stubs;
using System;
using System.Threading.Tasks;
using Xunit;

namespace RPG_GAME.UnitTests.Services
{
    public class RefreshTokenServiceFlowTests
    {
        [Fact]
        public async Task should_create_refresh_token()
        {
            var userId = Guid.NewGuid();
            var date = _clock.CurrentDate();

            var token = await _refreshTokenService.CreateAsync(userId);

            token.Should().NotBeNullOrWhiteSpace();
            var tokenAdded = await _refreshTokenRepository.GetAsync(token);
            tokenAdded.Should().NotBeNull();
            tokenAdded.Token.Should().NotBeNullOrWhiteSpace();
            tokenAdded.UserId.Should().Be(userId);
            tokenAdded.CreatedAt.Should().Be(date);
        }

        [Fact]
        public async Task should_use_refresh_token()
        {
            var user = await AddDefaultUser();
            var date = _clock.CurrentDate();
            var refreshToken = await AddDefaultToken(user.Id);

            var newToken = await _refreshTokenService.UseAsync(refreshToken.Token);

            var refreshTokenUpdated = await _refreshTokenRepository.GetAsync(refreshToken.Token);
            refreshTokenUpdated.Should().NotBeNull();
            refreshTokenUpdated.RevokedAt.Should().NotBeNull();
            refreshTokenUpdated.RevokedAt.Should().Be(date);
            refreshTokenUpdated.Revoked.Should().BeTrue();
            var refreshTokenAdded = await _refreshTokenRepository.GetAsync(newToken.RefreshToken);
            refreshTokenAdded.Should().NotBeNull();
            refreshTokenAdded.RevokedAt.Should().BeNull();
            refreshTokenAdded.Revoked.Should().BeFalse();
            newToken.Should().NotBeNull();
            newToken.AccessToken.Should().NotBeNullOrWhiteSpace();
            newToken.RefreshToken.Should().NotBeNullOrWhiteSpace();
            newToken.RefreshToken.Should().Be(refreshTokenAdded.Token);
        }

        [Fact]
        public async Task given_invalid_token_when_use_should_throw_an_exception()
        {
            var refreshToken = "token";
            var expectedException = new InvalidRefreshTokenException();

            var exception = await Record.ExceptionAsync(() => _refreshTokenService.UseAsync(refreshToken));

            exception.Should().NotBeNull();
            exception.Should().BeOfType(expectedException.GetType());
            exception.Message.Should().Be(expectedException.Message);
        }

        [Fact]
        public async Task given_revoked_token_when_use_should_throw_an_exception()
        {
            var refreshToken = await AddDefaultToken(Guid.NewGuid(), true);
            var expectedException = new RevokedRefreshTokenException();

            var exception = await Record.ExceptionAsync(() => _refreshTokenService.UseAsync(refreshToken.Token));

            exception.Should().NotBeNull();
            exception.Should().BeOfType(expectedException.GetType());
            exception.Message.Should().Be(expectedException.Message);
        }

        [Fact]
        public async Task given_token_with_invalid_user_id_when_use_should_throw_an_exception()
        {
            var refreshToken = await AddDefaultToken(Guid.NewGuid());
            var expectedException = new UserNotFoundException(refreshToken.UserId);

            var exception = await Record.ExceptionAsync(() => _refreshTokenService.UseAsync(refreshToken.Token));

            exception.Should().NotBeNull();
            exception.Should().BeOfType(expectedException.GetType());
            ((UserNotFoundException)exception).Message.Should().Be(expectedException.Message);
            ((UserNotFoundException)exception).UserId.Should().Be(expectedException.UserId);
        }

        private async Task<RefreshToken> AddDefaultToken(Guid userId, bool revoked = false)
        {
            var refreshToken = new RefreshToken(Guid.NewGuid(), userId, "token", _clock.CurrentDate(), revoked ? _clock.CurrentDate() : null);
            await _refreshTokenRepository.AddAsync(refreshToken);
            return refreshToken;
        }

        private async Task<User> AddDefaultUser()
        {
            var user = new User(Guid.NewGuid(), "email@email.com", "password", "user", DateTime.UtcNow);
            await _userRepository.AddAsync(user);
            return user;
        }

        private readonly IRefreshTokenService _refreshTokenService;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IUserRepository _userRepository;
        private readonly IAuthManager _authManager;
        private readonly IClock _clock;

        public RefreshTokenServiceFlowTests()
        {
            _refreshTokenRepository = new RefreshTokenRepositoryStub();
            _userRepository = new UserRepositoryStub();
            _authManager = new AuthManagerStub();
            _clock = new ClockStub();
            _refreshTokenService = new RefreshTokenService(_refreshTokenRepository, _userRepository, _authManager, _clock);
        }
    }
}
