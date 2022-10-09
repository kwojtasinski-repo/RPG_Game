using FluentAssertions;
using Moq;
using RPG_GAME.Application.Auth;
using RPG_GAME.Application.DTO.Auth;
using RPG_GAME.Application.Exceptions.Auth;
using RPG_GAME.Application.Services;
using RPG_GAME.Application.Time;
using RPG_GAME.Core.Entities.Users;
using RPG_GAME.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace RPG_GAME.UnitTests.Services
{
    public class RefreshTokenServiceTests
    {
        [Fact]
        public async Task should_create_refresh_token()
        {
            var userId = Guid.NewGuid();
            var date = _clock.Object.CurrentDate();

            var token = await _refreshTokenService.CreateAsync(userId);

            token.Should().NotBeNullOrWhiteSpace();
            _refreshTokenRepository.Verify(r => r.AddAsync(It.Is<RefreshToken>(rt => rt.CreatedAt == date)), times: Times.Once);
        }

        [Fact]
        public async Task should_use_refresh_token()
        {
            var user = AddDefaultUser();
            var date = _clock.Object.CurrentDate();
            var refreshToken = AddDefaultToken(user.Id);
            _authManager.Setup(a => a.CreateToken(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IDictionary<string, IEnumerable<string>>>()))
                .Returns(new JsonWebToken { AccessToken = "token" });

            var newToken = await _refreshTokenService.UseAsync(refreshToken.Token);

            _refreshTokenRepository.Verify(r => r.AddAsync(It.Is<RefreshToken>(rt => rt.CreatedAt == date)), times: Times.Once);
            _refreshTokenRepository.Verify(r => r.UpdateAsync(refreshToken), times: Times.Once);
            newToken.Should().NotBeNull();
            newToken.AccessToken.Should().NotBeNullOrWhiteSpace();
            newToken.RefreshToken.Should().NotBeNullOrWhiteSpace();
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
            var refreshToken = AddDefaultToken(Guid.NewGuid(), true);
            var expectedException = new RevokedRefreshTokenException();

            var exception = await Record.ExceptionAsync(() => _refreshTokenService.UseAsync(refreshToken.Token));

            exception.Should().NotBeNull();
            exception.Should().BeOfType(expectedException.GetType());
            exception.Message.Should().Be(expectedException.Message);
        }

        [Fact]
        public async Task given_token_with_invalid_user_id_when_use_should_throw_an_exception()
        {
            var refreshToken = AddDefaultToken(Guid.NewGuid(), false);
            var expectedException = new UserNotFoundException(refreshToken.UserId);

            var exception = await Record.ExceptionAsync(() => _refreshTokenService.UseAsync(refreshToken.Token));

            exception.Should().NotBeNull();
            exception.Should().BeOfType(expectedException.GetType());
            ((UserNotFoundException)exception).Message.Should().Be(expectedException.Message);
            ((UserNotFoundException)exception).UserId.Should().Be(expectedException.UserId);
        }

        private RefreshToken AddDefaultToken(Guid userId, bool revoked = false)
        {
            var refreshToken = new RefreshToken(Guid.NewGuid(), userId, "token", _clock.Object.CurrentDate(), revoked ? _clock.Object.CurrentDate() : null);
            _refreshTokenRepository.Setup(r => r.GetAsync(refreshToken.Token)).ReturnsAsync(refreshToken);
            return refreshToken;
        }

        private User AddDefaultUser()
        {
            var user = new User(Guid.NewGuid(), "email@email.com", "password", "user", DateTime.UtcNow);
            _userRepository.Setup(u => u.GetAsync(user.Id)).ReturnsAsync(user);
            return user;
        }

        private readonly IRefreshTokenService _refreshTokenService;
        private readonly Mock<IRefreshTokenRepository> _refreshTokenRepository;
        private readonly Mock<IUserRepository> _userRepository;
        private readonly Mock<IAuthManager> _authManager;
        private readonly Mock<IClock> _clock;

        public RefreshTokenServiceTests()
        {
            _refreshTokenRepository = new Mock<IRefreshTokenRepository>();
            _userRepository = new Mock<IUserRepository>();
            _authManager = new Mock<IAuthManager>();
            _clock = new Mock<IClock>();
            _clock.Setup(c => c.CurrentDate()).Returns(DateTime.UtcNow);
            _refreshTokenService = new RefreshTokenService(_refreshTokenRepository.Object, _userRepository.Object,
                _authManager.Object, _clock.Object);
        }
    }
}
