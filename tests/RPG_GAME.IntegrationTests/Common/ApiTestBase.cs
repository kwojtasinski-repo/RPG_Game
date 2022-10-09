using Flurl.Http;
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

        public ApiTestBase(TestApplicationFactory<Program> factory)
        {
            _client = new FlurlClient(factory.Client);
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
    }
}
