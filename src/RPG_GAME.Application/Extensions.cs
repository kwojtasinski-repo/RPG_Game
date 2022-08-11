using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using RPG_GAME.Application.Services;
using RPG_GAME.Core.Entities.Users;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("RPG_GAME.UnitTests")]
namespace RPG_GAME.Application
{
    public static class Extensions
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddTransient<IIdentityService, IdentityService>();
            services.AddSingleton<IPasswordHasher<User>, PasswordHasher<User>>();
            services.AddTransient<IEnemyService, EnemyService>();
            services.AddTransient<IHeroService, HeroService>();
            services.AddTransient<IPlayerService, PlayerService>();
            services.AddTransient<IMapService, MapService>();
            return services;
        }
    }
}