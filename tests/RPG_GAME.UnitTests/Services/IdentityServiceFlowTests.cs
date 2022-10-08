using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using RPG_GAME.Application.Auth;
using RPG_GAME.Application.DTO.Auth;
using RPG_GAME.Application.Exceptions.Auth;
using RPG_GAME.Application.Services;
using RPG_GAME.Application.Time;
using RPG_GAME.Core.Entities.Users;
using RPG_GAME.Core.Repositories;
using RPG_GAME.UnitTests.Stubs;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace RPG_GAME.UnitTests.Services
{
    public class IdentityServiceFlowTests
    {
        [Fact]
        public async Task should_sign_in()
        {
            await AddTestUsers();
            var dto = new SignInDto { Email = "email@email.com", Password = "PassWord!26" };

            var jwt = await _identityService.SignInAsync(dto);

            jwt.Should().NotBeNull();
            jwt.AccessToken.Should().NotBeEmpty();
            var token = await _refreshTokenRepository.GetAsync(jwt.RefreshToken);
            token.Should().NotBeNull();
            var user = await _userRepository.GetAsync(dto.Email);
            token.UserId.Should().Be(user.Id);
            token.Token.Should().NotBeNullOrWhiteSpace();
            token.Revoked.Should().BeFalse();
        }

        [Fact]
        public async Task given_not_existing_user_when_sign_in_should_throw_an_exception()
        {
            var dto = new SignInDto { Email = "email@email.com", Password = "PassWord!26" };
            var expectedException = new InvalidCredentialsException();

            var exception = await Record.ExceptionAsync(() => _identityService.SignInAsync(dto));

            exception.Should().NotBeNull();
            exception.Should().BeOfType(expectedException.GetType());
            exception.Message.Should().Be(expectedException.Message);
        }

        [Fact]
        public async Task given_invalid_user_password_when_sign_in_should_throw_an_exception()
        {
            await AddTestUsers();
            var dto = new SignInDto { Email = "email@email.com", Password = "PassWord!26afa" };
            var expectedException = new InvalidCredentialsException();

            var exception = await Record.ExceptionAsync(() => _identityService.SignInAsync(dto));

            exception.Should().NotBeNull();
            exception.Should().BeOfType(expectedException.GetType());
            exception.Message.Should().Be(expectedException.Message);
        }

        [Fact]
        public async Task given_not_active_user_when_sign_in_should_throw_an_exception()
        {
            await AddTestUsers();
            var dto = new SignInDto { Email = "email@email.com", Password = "PassWord!26" };
            var user = await _userRepository.GetAsync(dto.Email);
            user.ChangeUserActivity(false);
            var expectedException = new UserNotActiveException(user.Id);

            var exception = await Record.ExceptionAsync(() => _identityService.SignInAsync(dto));

            exception.Should().NotBeNull();
            exception.Should().BeOfType(expectedException.GetType());
            exception.Message.Should().Be(expectedException.Message);
        }

        [Fact]
        public async Task should_sign_up()
        {
            var dto = new SignUpDto { Email = "123@124.com", Password = "AbcasGSA123!" };

            await _identityService.SignUpAsync(dto);

            var auth = await _identityService.SignInAsync(new SignInDto { Email = dto.Email, Password = dto.Password });
            auth.Should().NotBeNull();
            auth.AccessToken.Should().NotBeEmpty();
        }

        [Fact]
        public async Task given_existed_email_when_sign_up_should_throw_an_exception()
        {
            await AddTestUsers();
            var dto = new SignUpDto { Email = "email@email.com", Password = "AbcasGSA123!" };
            var expectedException = new EmailInUseException();

            var exception = await Record.ExceptionAsync(() => _identityService.SignUpAsync(dto));

            exception.Should().NotBeNull();
            exception.Should().BeOfType(expectedException.GetType());
            exception.Message.Should().Be(expectedException.Message);
        }

        private IEnumerable<SignUpDto> GetTestUsers()
        {
            var admin = new SignUpDto { Email = "admin@email.com", Password = "AdmINnn!26", Role = "admin" };
            var user = new SignUpDto { Email = "email@email.com", Password = "PassWord!26", Role = "user" };
            var userTest = new SignUpDto { Email = "user@email.com", Password = "UussEErrR!26" };

            yield return admin;
            yield return user;
            yield return userTest;
        }

        private async Task AddTestUsers()
        {
            var users = GetTestUsers();

            foreach(var user in users)
            {
                await _identityService.SignUpAsync(user);
            }
        }

        private readonly IIdentityService _identityService;
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IAuthManager _authManager;
        private readonly IClock _clock;
        private readonly IRefreshTokenService _refreshTokenService;
        private readonly IRefreshTokenRepository _refreshTokenRepository;

        public IdentityServiceFlowTests()
        {
            _userRepository = new UserRepositoryStub();
            _passwordHasher = new PasswordHasher<User>();
            _authManager = new AuthManagerStub();
            _clock = new ClockStub();
            _refreshTokenRepository = new RefreshTokenRepositoryStub();
            _refreshTokenService = new RefreshTokenService(_refreshTokenRepository, _userRepository, _authManager, _clock);
            _identityService = new IdentityService(_userRepository, _passwordHasher, _authManager, _clock, _refreshTokenService);
        }
    }
}
