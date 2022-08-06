using Microsoft.Extensions.DependencyInjection;
using RPG_GAME.Application.Messaging;
using RPG_GAME.Application.Messaging.Clients;
using RPG_GAME.Infrastructure.Messaging.Brokers;
using RPG_GAME.Infrastructure.Messaging.Clients;
using RPG_GAME.Infrastructure.Messaging.Disptachers;

namespace RPG_GAME.Infrastructure.Messaging
{
    internal static class Extensions
    {
        public static IServiceCollection AddMessaging(this IServiceCollection services)
        {
            services.AddScoped<IInternalClient, InternalClient>();
            services.AddSingleton<IMessageBroker, InMemoryMessageBroker>();
            services.AddSingleton<IMessageChannel, MessageChannel>();
            services.AddSingleton<IAsyncMessageDispatcher, AsyncMessageDispatcher>();

            services.AddHostedService<BackgroundDispatcher>();

            return services;
        }
    }
}
