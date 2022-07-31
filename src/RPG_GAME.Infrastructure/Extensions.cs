using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using RPG_Game.Infrastructure.Auth;
using RPG_Game.Infrastructure.Database;
using RPG_Game.Infrastructure.Mappings;
using RPG_Game.Infrastructure.Repositories;
using RPG_Game.Infrastructure.Time;
using RPG_GAME.Application.Time;
using RPG_GAME.Core.Entities;
using RPG_GAME.Core.Repositories;

namespace RPG_Game.Infrastructure
{
    public static class Extensions
    {
        private const string SectionName = "mongo";

        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            RegisterMappings();
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

            services.AddAuth();
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddMongoRepository<User, Guid>("users");
            services.AddSingleton<IClock, Clock>();

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

        private static IServiceCollection AddMongoRepository<TEntity, TIdentifiable>(this IServiceCollection services,
            string collectionName)
            where TEntity : class, IIdentifiable<TIdentifiable>
        {
            services.AddTransient<IMongoRepository<TEntity, TIdentifiable>>(sp =>
            {
                var database = sp.GetService<IMongoDatabase>();
                return new MongoRepository<TEntity, TIdentifiable>(database, collectionName);
            });

            return services;
        }
    }
}