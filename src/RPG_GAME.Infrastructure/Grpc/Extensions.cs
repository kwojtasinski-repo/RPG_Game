using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using RPG_GAME.Infrastructure.Grpc.Interceptors;
using RPG_GAME.Infrastructure.Grpc.Services;

namespace RPG_GAME.Infrastructure.Grpc
{
    internal static class Extensions
    {
        public static IServiceCollection AddGrpcCommunication(this IServiceCollection services)
        {
            services.AddGrpc(options =>
            {
                options.Interceptors.Add<GrpcExceptionInterceptor>();
            });
            return services;
        }

        public static WebApplication UseGrpc(this WebApplication app, string corsPolicy)
        {
            app.MapGrpcService<BattleService>()
                .EnableGrpcWeb()
                .RequireCors(corsPolicy);
            return app;
        }

        public static IEndpointRouteBuilder MapGrpcServices(this IEndpointRouteBuilder endpoints, string corsPolicy)
        {
            endpoints.MapGrpcService<BattleService>()
                .EnableGrpcWeb().RequireCors(corsPolicy);
                return endpoints;
        }
    }
}
