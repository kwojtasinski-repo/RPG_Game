using Flurl.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using RPG_GAME.Application.DTO.Auth;
using RPG_GAME.Application.Services;
using RPG_GAME.Core.Entities.Users;
using RPG_GAME.Core.Repositories;
using RPG_GAME.IntegrationTests.Common;
using Shouldly;
using System;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

namespace RPG_GAME.IntegrationTests.Api
{
    public class RefreshTokensControllerTests : ApiTestBase
    {
        [Fact]
        public async Task should_refresh_token_and_return_200()
        {
            var user = await AddDefaultUserAsync();
            var token = await _refreshTokenService.CreateAsync(user.Id);
            var dto = new RefreshTokenDto(token);

            var response = await _client.Request($"{Path}/use").PostJsonAsync(dto);

            response.StatusCode.ShouldBe(StatusCodes.Status200OK);
        }

        [Fact]
        public async Task should_refresh_token_and_return_new_token()
        {
            var user = await AddDefaultUserAsync();
            var token = await _refreshTokenService.CreateAsync(user.Id);
            var dto = new RefreshTokenDto(token);

            var response = await _client.Request($"{Path}/use").PostJsonAsync(dto);

            response.StatusCode.ShouldBe(StatusCodes.Status200OK);
            var newToken = await response.ResponseMessage.Content.ReadFromJsonAsync<JsonWebToken>();
            newToken.ShouldNotBeNull();
            newToken.AccessToken.ShouldNotBeNullOrWhiteSpace();
            newToken.RefreshToken.ShouldNotBeNullOrWhiteSpace();
            newToken.RefreshToken.ShouldNotBe(token);
        }

        [Fact]
        public async Task should_refresh_token_and_update_last_token_as_revoked()
        {
            var user = await AddDefaultUserAsync();
            var token = await _refreshTokenService.CreateAsync(user.Id);
            var dto = new RefreshTokenDto(token);

            var response = await _client.Request($"{Path}/use").PostJsonAsync(dto);

            response.StatusCode.ShouldBe(StatusCodes.Status200OK);
            var newToken = await response.ResponseMessage.Content.ReadFromJsonAsync<JsonWebToken>();
            var tokenAdded = await _repository.GetAsync(newToken.RefreshToken);
            var tokenUpdated = await _repository.GetAsync(token);
            tokenUpdated.Revoked.ShouldBeTrue();
            tokenUpdated.RevokedAt.ShouldNotBeNull();
            tokenUpdated.RevokedAt.ShouldNotBe(default);
            tokenAdded.UserId.ShouldBe(user.Id);
            tokenAdded.RevokedAt.ShouldBeNull();
        }

        private async Task<User> AddDefaultUserAsync()
        {
            var user = new User(Guid.NewGuid(), $"claude_{Guid.NewGuid().ToString("N")}@test.com", "password", "user", DateTime.UtcNow);
            await _userRepository.AddAsync(user);
            return user;
        }


        private const string Path = "api/Refresh-Tokens";
        private readonly IRefreshTokenRepository _repository;
        private readonly IUserRepository _userRepository;
        private readonly IRefreshTokenService _refreshTokenService;

        public RefreshTokensControllerTests(TestApplicationFactory<Program> factory) : base(factory)
        {
            _repository = factory.Services.GetRequiredService<IRefreshTokenRepository>();
            _userRepository = factory.Services.GetRequiredService<IUserRepository>();
            _refreshTokenService = factory.Services.GetRequiredService<IRefreshTokenService>();
        }
    }
}
