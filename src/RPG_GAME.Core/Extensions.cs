using Microsoft.Extensions.DependencyInjection;
using RPG_GAME.Core.Services.Enemies;
using RPG_GAME.Core.Services.Heroes;
using RPG_GAME.Core.Services.Maps;
using RPG_GAME.Core.Services.Players;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("RPG_GAME.UnitTests")]
namespace RPG_GAME.Core
{
    public static class Extensions
    {
        public static IServiceCollection AddCore(this IServiceCollection services)
        {
            services.AddTransient<IPlayerAllocatorDomainService, PlayerAllocatorDomainService>();
            services.AddTransient<IMapAllocatorDomainService, MapAllocatorDomainService>();
            services.AddTransient<IEnemyAssignUpdaterDomainService, EnemyAssignUpdaterDomainService>();
            services.AddTransient<IHeroAssignUpdaterDomainService, HeroAssignUpdaterDomainService>();
            return services;
        }
    }
}
