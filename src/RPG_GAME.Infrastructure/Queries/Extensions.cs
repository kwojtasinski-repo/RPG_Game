using Microsoft.Extensions.DependencyInjection;
using RPG_GAME.Application.Queries;
using System.Reflection;

namespace RPG_GAME.Infrastructure.Queries
{
    internal static class Extensions
    {
        public static IServiceCollection AddQueries(this IServiceCollection services, IEnumerable<Assembly> assemblies)
        {
            services.AddSingleton<IQueryDispatcher, QueryDispatcher>();
            services.Scan(s => s.FromAssemblies(assemblies)
                                .AddClasses(c => c.AssignableTo(typeof(IQueryHandler<,>)))
                                .AsImplementedInterfaces()
                                .WithScopedLifetime());
            return services;
        }
    }
}
