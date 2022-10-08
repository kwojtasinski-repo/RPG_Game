using FluentAssertions;
using Microsoft.AspNetCore.Identity;
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
    public class IdentityServiceTests
    {
        [Fact]
        public async Task should_sign_in()
        {
            var dto = new SignInDto { Email = "email@email.com", Password = "pasWo9Rd!26" };
            var user = new User(Guid.NewGuid(), dto.Email, dto.Password, "user", DateTime.UtcNow);
            _userRepository.Setup(u => u.GetAsync(It.IsAny<string>())).Returns(Task.FromResult(user));
            _passwordHasher.Setup(p => p.VerifyHashedPassword(It.IsAny<User>(), It.IsAny<string>(), It.IsAny<string>())).Returns(PasswordVerificationResult.Success);
            _authManager.Setup(a => a.CreateToken(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IDictionary<string, IEnumerable<string>>>()))
                .Returns(new JsonWebToken());

            var jwt = await _identityService.SignInAsync(dto);

            jwt.Should().NotBeNull();
            _refreshTokenService.Verify(r => r.CreateAsync(It.IsAny<Guid>()), times: Times.Once);
        }

        [Fact]
        public async Task given_not_activated_user_when_sign_in_should_throw_an_exception()
        {
            var dto = new SignInDto { Email = "email@email.com", Password = "pasWo9Rd!26" };
            var user = new User(Guid.NewGuid(), dto.Email, dto.Password, "user", DateTime.UtcNow, false);
            _userRepository.Setup(u => u.GetAsync(It.IsAny<string>())).Returns(Task.FromResult(user));
            _passwordHasher.Setup(p => p.VerifyHashedPassword(It.IsAny<User>(), It.IsAny<string>(), It.IsAny<string>())).Returns(PasswordVerificationResult.Success);
            var expectedException = new UserNotActiveException(user.Id);

            var exception = await Record.ExceptionAsync(() => _identityService.SignInAsync(dto));

            exception.Should().NotBeNull();
            exception.Should().BeOfType(expectedException.GetType());
            exception.Message.Should().Be(expectedException.Message);
        }

        [Fact]
        public async Task given_not_existed_user_when_sign_in_should_throw_an_exception()
        {
            var dto = new SignInDto { Email = "email@email.com", Password = "pasWo9Rd!26" };
            var expectedException = new InvalidCredentialsException();

            var exception = await Record.ExceptionAsync(() => _identityService.SignInAsync(dto));

            exception.Should().NotBeNull();
            exception.Should().BeOfType(expectedException.GetType());
            exception.Message.Should().Be(expectedException.Message);
        }

        [Fact]
        public async Task should_sing_up()
        {
            var dto = new SignUpDto { Email = "email@email.com", Password = "Paq214241312312312412" };
            _passwordHasher.Setup(p => p.HashPassword(It.IsAny<User>(), dto.Password)).Returns(dto.Password);

            await _identityService.SignUpAsync(dto);

            _userRepository.Verify(u => u.AddAsync(It.IsAny<User>()), times: Times.Once);
        }

        [Fact]
        public async Task given_existed_user_email_when_sign_up_should_throw_an_exception()
        {
            var dto = new SignUpDto { Email = "email@email.com", Password = "Paq214241312312312412" };
            _userRepository.Setup(u => u.GetAsync(dto.Email)).Returns(Task.FromResult(User.Create(dto.Email, dto.Password, DateTime.UtcNow, "user")));
            var expectedException = new EmailInUseException();

            var exception = await Record.ExceptionAsync(() => _identityService.SignUpAsync(dto));

            exception.Should().NotBeNull();
            exception.Should().BeOfType(expectedException.GetType());
            exception.Message.Should().Be(expectedException.Message);
        }

        [Fact]
        public async Task given_invalid_password_when_sign_up_should_throw_an_exception()
        {
            var dto = new SignUpDto { Email = "email@email.com", Password = "" };
            var expectedException = new Core.Exceptions.Common.InvalidPasswordException();

            var exception = await Record.ExceptionAsync(() => _identityService.SignUpAsync(dto));

            exception.Should().NotBeNull();
            exception.Should().BeOfType(expectedException.GetType());
            exception.Message.Should().Be(expectedException.Message);
        }

        private readonly IIdentityService _identityService;
        private readonly Mock<IUserRepository> _userRepository;
        private readonly Mock<IPasswordHasher<User>> _passwordHasher;
        private readonly Mock<IAuthManager> _authManager;
        private readonly Mock<IClock> _clock;
        private readonly Mock<IRefreshTokenService> _refreshTokenService;

        public IdentityServiceTests()
        {
            _userRepository = new Mock<IUserRepository>();
            _passwordHasher = new Mock<IPasswordHasher<User>>();
            _authManager = new Mock<IAuthManager>();
            _clock = new Mock<IClock>();
            _clock.Setup(c => c.CurrentDate()).Returns(DateTime.UtcNow);
            _refreshTokenService = new Mock<IRefreshTokenService>();
            _identityService = new IdentityService(_userRepository.Object, _passwordHasher.Object,
                _authManager.Object, _clock.Object, _refreshTokenService.Object);
        }
    }
}
