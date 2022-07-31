using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using RPG_GAME.Application.Services;
using RPG_GAME.Core.Entities;

namespace RPG_Game.Application
{
    public static class Extensions
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddTransient<IIdentityService, IdentityService>();
            services.AddSingleton<IPasswordHasher<User>, PasswordHasher<User>>();
            return services;
        }
    }
}