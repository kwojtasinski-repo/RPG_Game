using Microsoft.Extensions.DependencyInjection;
using RPG_GAME.Application.Time;

namespace RPG_Game.Infrastructure.Time
{
    internal static class Extensions
    {
        public static IServiceCollection AddTime(this IServiceCollection services)
        {
            services.AddSingleton<IClock, Clock>();
            return services;
        }
    }
}
