using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using RPG_GAME.Infrastructure.Auth;
using RPG_GAME.Infrastructure.Commands;
using RPG_GAME.Infrastructure.Conventions;
using RPG_GAME.Infrastructure.Database;
using RPG_GAME.Infrastructure.Events;
using RPG_GAME.Infrastructure.Grpc;
using RPG_GAME.Infrastructure.Messaging;
using RPG_GAME.Infrastructure.Middlewares;
using RPG_GAME.Infrastructure.Mongo;
using RPG_GAME.Infrastructure.Queries;
using RPG_GAME.Infrastructure.Time;

namespace RPG_GAME.Infrastructure
{
    public static class Extensions
    {
        private const string SectionName = "mongo";
        private const string CorsPolicy = "cors";

        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddControllers(options => options.UseDashedConventionInRouting());
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

            var assemblies = AppDomain.CurrentDomain.GetAssemblies().ToList();
            services.AddRepositories();
            services.AddCommands(assemblies);
            services.AddQueries(assemblies);
            services.AddEvents(assemblies);
            services.AddMessaging();

            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            services.AddGrpcCommunication();

            services.AddCors(cors =>
            {
                cors.AddPolicy(CorsPolicy, policy =>
                {
                    policy.WithOrigins("*")
                          .WithMethods("POST", "PUT", "PATCH", "DELETE")
                          //.WithHeaders("Content-Type", "Authorization")
                          .AllowAnyHeader()
                          .WithExposedHeaders("Location")
                          .WithExposedHeaders("Grpc-Status", "Grpc-Message", "Grpc-Encoding", "Grpc-Accept-Encoding");
                });
            });

            return services;
        }

        public static WebApplication UseInfrastructure(this WebApplication app)
        {
            app.UseRouting();
            app.UseGrpcWeb();
            app.UseCors(CorsPolicy);
            //app.UseGrpc(CorsPolicy);
            app.UseMiddleware<ErrorHandlerMiddleware>();
            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGrpcServices(CorsPolicy);
                endpoints.MapControllers();
            });
            //app.MapControllers();
            app.UseMongo();
            return app;
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