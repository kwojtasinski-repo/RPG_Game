using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using RPG_Game.Infrastructure.Auth;
using RPG_Game.Infrastructure.Database;
using RPG_Game.Infrastructure.Mappings;
using RPG_Game.Infrastructure.Mongo;
using RPG_Game.Infrastructure.Time;

namespace RPG_Game.Infrastructure
{
    public static class Extensions
    {
        private const string SectionName = "mongo";

        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddAuth();
            services.AddMongo();
            services.AddTime();
            IConfiguration configuration = services.BuildServiceProvider().GetService<IConfiguration>();
            var mongoDbOptions = configuration.GetOptions<MongoDbOptions>(SectionName);
            services.AddSingleton(mongoDbOptions);

            services.AddSingleton<IMongoClient>(sp =>
            {
                var options = sp.GetService<MongoDbOptions>();
                return new MongoClient(options.ConnectionString);
            });

            services.AddTransient(sp =>
            {
                var options = sp.GetService<MongoDbOptions>();
                var client = sp.GetService<IMongoClient>();
                return client.GetDatabase(options.Database);
            });

            return services;
        }

        public static void RegisterMappings()
        {
            MongoDbClassMap.RegisterAllMappings(typeof(Extensions).Assembly);
        }

        public static T GetOptions<T>(this IServiceCollection services, string sectionName) where T : new()
        {
            using var serviceProvider = services.BuildServiceProvider();
            var configuration = serviceProvider.GetRequiredService<IConfiguration>();
            return configuration.GetOptions<T>(sectionName);
        }

        public static T GetOptions<T>(this IConfiguration configuration, string sectionName) where T : new()
        {
            var options = new T();
            configuration.GetSection(sectionName).Bind(options);
            return options;
        }
    }
}