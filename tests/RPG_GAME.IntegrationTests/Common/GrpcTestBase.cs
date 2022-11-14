using Grpc.Net.Client;
using Microsoft.Extensions.DependencyInjection;
using RPG_GAME.Application.Auth;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace RPG_GAME.IntegrationTests.Common
{
    [Collection("TestCollection")]
    public class GrpcTestBase : IDisposable
    {
        private GrpcChannel _channel;

        protected TestApplicationFactory<Program> Fixture { get; set; }

        protected GrpcChannel Channel => _channel ??= CreateChannel();

        protected GrpcChannel CreateChannel()
        {
            return GrpcChannel.ForAddress("http://localhost", new GrpcChannelOptions
            {
                HttpHandler = new GrpcDelegatingHandler(Fixture.Server.CreateHandler())
            });
        }

        public GrpcTestBase(TestApplicationFactory<Program> fixture)
        {
            Fixture = fixture;
            SetBearerToken();
        }

        public void Dispose()
        {
            _channel = null;
        }

        public T GetService<T>()
        {
            return Fixture.Services.GetService<T>();
        }

        private void SetBearerToken()
        {
            var authManager = GetService<IAuthManager>();
            // Based on DataInitializer
            var userId = new Guid("00000000-0000-0000-0000-000000000001");
            var auth = authManager.CreateToken(userId.ToString(), "email-admin-predefined@email.com", "admin");
            _token = auth.AccessToken;
        }

        private static string _token;

        private class GrpcDelegatingHandler : DelegatingHandler
        {
            public GrpcDelegatingHandler(HttpMessageHandler handler)
            {
                InnerHandler = handler;
            }

            protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _token);
                return base.SendAsync(request, cancellationToken);
            }
        }
    }
}
