using Microsoft.Extensions.DependencyInjection;
using RPG_GAME.Application.Events;
using RPG_GAME.Application.Messaging;
using RPG_GAME.Application.Messaging.Clients;
using RPG_GAME.Application.Messaging.Subscribers;
using RPG_GAME.Infrastructure.Messaging.Brokers;
using RPG_GAME.Infrastructure.Messaging.Clients;
using RPG_GAME.Infrastructure.Messaging.Disptachers;
using RPG_GAME.Infrastructure.Messaging.Subscribers;
using System.Linq;
using System.Reflection;

namespace RPG_GAME.Infrastructure.Messaging
{
    internal static class Extensions
    {
        public static IServiceCollection AddMessaging(this IServiceCollection services)
        {
            services.AddSingleton<IInternalClient, InternalClient>();
            services.AddSingleton<IMessageSerializer, MessageSerializer>();
            services.AddSubscriberRegistration();
            services.AddSingleton<IMessageBroker, InMemoryMessageBroker>();
            services.AddSingleton<IMessageChannel, MessageChannel>();
            services.AddSingleton<IAsyncMessageDispatcher, AsyncMessageDispatcher>();

            services.AddHostedService<BackgroundDispatcher>();

            return services;
        }

        private static IServiceCollection AddSubscriberRegistration(this IServiceCollection services)
        {
            var registry = new SubscriberRegitration();

            var assemblies = AppDomain.CurrentDomain.GetAssemblies().ToList();
            var types = assemblies.SelectMany(t => t.GetTypes()).ToArray();
            var eventTypes = types.Where(et => et.IsClass && typeof(IEvent).IsAssignableFrom(et));

            services.AddSingleton<ISubscriberRegitration>(sp =>
            {
                var eventDispatcher = sp.GetRequiredService<IEventDispatcher>();
                var eventDispatcherType = eventDispatcher.GetType();

                foreach (var type in eventTypes)
                {
                    registry.AddBroadcastAction(type, @event =>
                        (Task)eventDispatcherType.GetMethod(nameof(eventDispatcher.PublishAsync))
                            ?.MakeGenericMethod(type)
                            .Invoke(eventDispatcher, new[] { @event }));
                }

                return registry;
            });

            return services;
        }
    }
}
