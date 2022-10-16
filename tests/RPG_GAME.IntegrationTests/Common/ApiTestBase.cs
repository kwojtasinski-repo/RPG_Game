using Flurl.Http;
using Microsoft.Extensions.DependencyInjection;
using RPG_GAME.Application.Auth;
using Shouldly;
using System;
using System.Linq;
using Xunit;

namespace RPG_GAME.IntegrationTests.Common
{
    [Collection("TestCollection")]
    public class ApiTestBase
    {
        protected IFlurlClient _client;
        private readonly TestApplicationFactory<Program> _factory;

        public ApiTestBase(TestApplicationFactory<Program> factory)
        {
            _factory = factory;
            _client = new FlurlClient(factory.Client);
            Authorize(_client);
        }

        protected static Guid GetIdFromHeader(IFlurlResponse response, string path)
        {
            var (responseHeaderName, responseHeaderValue) = response.Headers.Where(h => h.Name == "Location").FirstOrDefault();
            responseHeaderValue.ShouldNotBeNull();
            responseHeaderValue = responseHeaderValue.ToLowerInvariant();
            path = path.ToLowerInvariant();
            var splitted = responseHeaderValue.Split(path + '/');
            var id= Guid.Parse(splitted[1]);
            return id;
        }

        private void Authorize(IFlurlClient client)
        {
            var authManager = _factory.Services.GetRequiredService<IAuthManager>();
            // Based on DataInitializer
            var userId = new Guid("00000000-0000-0000-0000-000000000001");
            var auth = authManager.CreateToken(userId.ToString(), "email-admin-predefined@email.com", "admin");
            client.WithOAuthBearerToken(auth.AccessToken);
        }
    }
}
