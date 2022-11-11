using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using RPG_GAME.Infrastructure.Grpc.Interceptors;
using RPG_GAME.Infrastructure.Grpc.Services;

namespace RPG_GAME.Infrastructure.Grpc
{
    internal static class Extensions
    {
        public static IServiceCollection AddGrpcCommunication(this IServiceCollection services, string corsPolicy)
        {
            services.AddCors(cors =>
            {
                cors.AddPolicy(corsPolicy, policy =>
                {
                    policy.WithOrigins("*")
                          .WithMethods("POST")
                          .WithHeaders("Content-Type", "Authorization", "x-grpc-web", "x-user-agent")
                          .WithExposedHeaders("Grpc-Status", "Grpc-Message", "Grpc-Encoding", "Grpc-Accept-Encoding");
                });
            });
            services.AddGrpc(options =>
            {
                options.Interceptors.Add<GrpcExceptionInterceptor>();
            });
            return services;
        }

        public static WebApplication UseGrpc(this WebApplication app, string corsPolicy)
        {
            app.UseGrpcWeb();
            app.MapGrpcService<BattleService>()
                .EnableGrpcWeb()
                .RequireCors(corsPolicy);
            return app;
        }
    }
}
