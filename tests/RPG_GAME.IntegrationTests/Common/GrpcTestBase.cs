using Grpc.Net.Client;
using System;
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
                HttpHandler = Fixture.Server.CreateHandler()
            });
        }

        public GrpcTestBase(TestApplicationFactory<Program> fixture)
        {
            Fixture = fixture;
        }

        public void Dispose()
        {
            _channel = null;
        }
    }
}
