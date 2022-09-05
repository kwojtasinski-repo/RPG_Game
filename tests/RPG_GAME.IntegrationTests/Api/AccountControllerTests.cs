using Flurl.Http;
using Microsoft.Extensions.DependencyInjection;
using RPG_GAME.Application.DTO.Auth;
using RPG_GAME.Core.Repositories;
using RPG_GAME.IntegrationTests.Common;
using Shouldly;
using System;
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

            var response = await _client.Request($"{Path}").PostJsonAsync(signUp);

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
            var response = await _client.Request($"{Path}").PostJsonAsync(signUp);
            response.StatusCode.ShouldBe((int)HttpStatusCode.OK);
            var signIn = new SignInDto { Email = signUp.Email, Password = signUp.Password };

            var responseSignIn = await _client.Request($"{Path}/me").PostJsonAsync(signIn);

            responseSignIn.StatusCode.ShouldBe((int)HttpStatusCode.OK);
            var jwt = await responseSignIn.ResponseMessage.Content.ReadFromJsonAsync<JsonWebToken>();
            jwt.ShouldNotBeNull();
            jwt.AccessToken.ShouldNotBeNullOrWhiteSpace();
            jwt.Email.ShouldBe(signUp.Email);
            jwt.Role.ShouldBe(signUp.Role);
        }

        private const string Path = "api/Account";
        private readonly IUserRepository _repository;

        public AccountControllerTests(TestApplicationFactory<Program> factory) : base(factory)
        {
            _repository = factory.Services.GetRequiredService<IUserRepository>();
        }
    }
}
