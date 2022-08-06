using Microsoft.Extensions.DependencyInjection;
using RPG_GAME.Core.Services.Enemies;
using RPG_GAME.Core.Services.Heroes;

namespace RPG_GAME.Core
{
    public static class Extensions
    {
        public static IServiceCollection AddCore(this IServiceCollection services)
        {
            services.AddTransient<IPlayerAllocatorDomainService, PlayerAllocatorDomainService>();
            services.AddTransient<IMapAllocatorDomainService, MapAllocatorDomainService>();
            return services;
        }
    }
}
