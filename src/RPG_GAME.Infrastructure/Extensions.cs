using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using RPG_GAME.Infrastructure.Auth;
using RPG_GAME.Infrastructure.Database;
using RPG_GAME.Infrastructure.Mappings;
using RPG_GAME.Infrastructure.Middlewares;
using RPG_GAME.Infrastructure.Mongo;
using RPG_GAME.Infrastructure.Time;

namespace RPG_GAME.Infrastructure
{
    public static class Extensions
    {
        private const string SectionName = "mongo";

        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddControllers();
            services.AddAuth();
            services.AddMongo();
            services.AddTime();
            services.AddSingleton<ErrorHandlerMiddleware>();
            var mongoDbOptions = services.GetOptions<MongoDbOptions>(SectionName);
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

            services.AddRepositories();

            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            
            return services;
        }

        public static WebApplication UseInfrastructure(this WebApplication app)
        {
            app.UseMiddleware<ErrorHandlerMiddleware>();
            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseAuthorization();
            app.MapControllers();
            return app;
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