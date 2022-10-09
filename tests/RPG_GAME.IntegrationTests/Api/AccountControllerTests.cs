using Flurl.Http;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver.Linq;
using RPG_GAME.Application.DTO.Auth;
using RPG_GAME.Core.Repositories;
using RPG_GAME.IntegrationTests.Common;
using Shouldly;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

namespace RPG_GAME.IntegrationTests.Api
{
    public class AccountControllerTests : ApiTestBase
    {
        [Fact]
        public async Task should_sign_up()
        {
            var signUp = new SignUpDto { Email = "email@email.test.com", Password = "PasSw0rdAbc12", Role = "user" };

            var response = await _client.Request($"{Path}/sign-up").PostJsonAsync(signUp);

            response.StatusCode.ShouldBe((int)HttpStatusCode.OK);
            var user = await _repository.GetAsync(signUp.Email);
            user.ShouldNotBeNull();
            user.Email.Value.ShouldBe(signUp.Email);
            user.Role.ShouldBe(signUp.Role);
        }
        
        [Fact]
        public async Task should_sign_in()
        {
            var signUp = new SignUpDto { Email = "email@email2.test.com", Password = "passwordAB63", Role = "user" };
            var response = await _client.Request($"{Path}/sign-up").PostJsonAsync(signUp);
            response.StatusCode.ShouldBe((int)HttpStatusCode.OK);
            var signIn = new SignInDto { Email = signUp.Email, Password = signUp.Password };

            var responseSignIn = await _client.Request($"{Path}/sign-in").PostJsonAsync(signIn);

            responseSignIn.StatusCode.ShouldBe((int)HttpStatusCode.OK);
            var jwt = await responseSignIn.ResponseMessage.Content.ReadFromJsonAsync<JsonWebToken>();
            jwt.ShouldNotBeNull();
            jwt.AccessToken.ShouldNotBeNullOrWhiteSpace();
        }

        [Fact]
        public async Task when_sign_in_should_return_jwt_token_with_valid_content()
        {
            var timeBeforeSendRequest = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeMilliseconds();
            var signUp = new SignUpDto { Email = "email@email2abv.test.com", Password = "passwordAB63", Role = "user" };
            var response = await _client.Request($"{Path}/sign-up").PostJsonAsync(signUp);
            response.StatusCode.ShouldBe((int)HttpStatusCode.OK);
            var signIn = new SignInDto { Email = signUp.Email, Password = signUp.Password };

            var responseSignIn = await _client.Request($"{Path}/sign-in").PostJsonAsync(signIn);

            responseSignIn.StatusCode.ShouldBe((int)HttpStatusCode.OK);
            var jwt = await responseSignIn.ResponseMessage.Content.ReadFromJsonAsync<JsonWebToken>();
            jwt.ShouldNotBeNull();
            jwt.AccessToken.ShouldNotBeNullOrWhiteSpace();
            var handler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = handler.ReadJwtToken(jwt.AccessToken);
            jwtSecurityToken.Subject.ShouldNotBeNull();
            var emailClaim = jwtSecurityToken.Claims.Where(c => c.Type == JwtRegisteredClaimNames.Email).FirstOrDefault();
            emailClaim.Value.ShouldNotBeNull();
            emailClaim.Value.ShouldBe(signUp.Email);
            var issuedAtClaim = jwtSecurityToken.Claims.Where(c => c.Type == JwtRegisteredClaimNames.Iat).FirstOrDefault();
            issuedAtClaim.ShouldNotBeNull();
            var timeAfterSendRequest = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeMilliseconds();
            issuedAtClaim.Value.ShouldNotBeNull();
            var timeIssuedAt = long.Parse(issuedAtClaim.Value);
            timeIssuedAt.ShouldBeGreaterThan(timeBeforeSendRequest);
            timeIssuedAt.ShouldBeLessThan(timeAfterSendRequest);
            var claimJwtExpired = jwtSecurityToken.Claims.Where(c => c.Type == JwtRegisteredClaimNames.Exp).FirstOrDefault();
            claimJwtExpired.ShouldNotBeNull();
            var timeJwtExpired = long.Parse(claimJwtExpired.Value) * 1000;
            var expectedTimeJwtExpired = DateTimeOffset.FromUnixTimeMilliseconds(timeIssuedAt).AddHours(1).ToUnixTimeSeconds() * 1000;
            timeJwtExpired.ShouldBe(expectedTimeJwtExpired);
            jwt.RefreshToken.ShouldNotBeNullOrWhiteSpace();
        }

        private const string Path = "api/Account";
        private readonly IUserRepository _repository;

        public AccountControllerTests(TestApplicationFactory<Program> factory) : base(factory)
        {
            _repository = factory.Services.GetRequiredService<IUserRepository>();
        }
    }
}
