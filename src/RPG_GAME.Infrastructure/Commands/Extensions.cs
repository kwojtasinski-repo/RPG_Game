using Microsoft.Extensions.DependencyInjection;
using RPG_GAME.Application.Commands;
using System.Reflection;

namespace RPG_GAME.Infrastructure.Commands
{
    internal static class Extensions
    {
        public static IServiceCollection AddCommands(this IServiceCollection services, IEnumerable<Assembly> assemblies)
        {
            services.AddSingleton<ICommandDispatcher, CommandDispatcher>();
            services.Scan(s => s.FromAssemblies(assemblies)
                                .AddClasses(c => c.AssignableTo(typeof(ICommandHandler<>)))
                                .AsImplementedInterfaces()
                                .WithTransientLifetime());
            services.Scan(s => s.FromAssemblies(assemblies)
                                .AddClasses(c => c.AssignableTo(typeof(ICommandHandler<,>)))
                                .AsImplementedInterfaces()
                                .WithTransientLifetime());
            return services;
        }
    }
}
