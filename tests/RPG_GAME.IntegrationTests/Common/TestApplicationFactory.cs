using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MongoDB.Driver;
using RPG_GAME.Infrastructure.Database;
using System.Net.Http;
using System.Threading.Tasks;

namespace RPG_GAME.IntegrationTests.Common
{
    public class TestApplicationFactory<TEntryPoint> : WebApplicationFactory<TEntryPoint> where TEntryPoint : class
    {
        public HttpClient Client { get; }

        public TestApplicationFactory()
        {
            WithWebHostBuilder(builder =>
            {
                builder.UseEnvironment("test")
                       .UseTestServer();
            });
            Client = Server.CreateClient();
        }

        public override async ValueTask DisposeAsync()
        {
            var options = Services.GetRequiredService<MongoDbOptions>();
            var client = new MongoClient(options.ConnectionString);
            await client.DropDatabaseAsync(options.Database);
            await base.DisposeAsync();
        }
    }
}
