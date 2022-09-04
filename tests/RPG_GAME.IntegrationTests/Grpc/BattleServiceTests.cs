using Grpc.Core;
using Humanizer;
using RPG_GAME.Application.Exceptions.Auth;
using RPG_GAME.IntegrationTests.Common;
using RPG_GAME.IntegrationTests.Common.Protos;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace RPG_GAME.IntegrationTests.Grpc
{
    public class BattleServiceTests : GrpcTestBase
    {
        public BattleServiceTests(TestApplicationFactory<Program> fixture)
              : base(fixture)
        {
        }

        [Fact]
        public async Task given_not_existing_user_should_throw_an_exception()
        {
            // Arrange
            var client = new Battle.BattleClient(Channel);
            var userId = Guid.NewGuid();
            var exceptionThrown = new UserNotFoundException(userId);
            var expectedException = new RpcException(new Status(StatusCode.FailedPrecondition, exceptionThrown.Message));
            var task = client.PrepareBattleAsync(new PrepareBattleRequest { MapId = Guid.NewGuid().ToString(), UserId = userId.ToString() });

            // Act
            var exception = await Record.ExceptionAsync(() => task.ResponseAsync);
            var headers = await task.ResponseHeadersAsync;

            // Assert
            Assert.NotNull(exception);
            Assert.IsType(expectedException.GetType(), exception);
            Assert.Equal(expectedException.Message, exception.Message);
            Assert.NotNull(headers);
            Assert.NotEmpty(headers);
            Assert.Contains(headers.First().Key, typeof(UserNotFoundException).Name.Underscore());
            Assert.Contains(headers.First().Value, expectedException.Message);
        }
    }
}
