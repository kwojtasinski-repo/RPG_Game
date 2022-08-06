using RPG_GAME.Application.Messaging.Clients;
using RPG_GAME.Application.Messaging.Subscribers;

namespace RPG_GAME.Infrastructure.Messaging.Clients
{
    internal sealed class InternalClient : IInternalClient
    {
        private readonly IMessageSerializer _messageSerializer;
        private readonly ISubscriberRegitration _subscriberRegitration;

        public InternalClient(IMessageSerializer messageSerializer, ISubscriberRegitration subscriberRegitration)
        {
            _messageSerializer = messageSerializer;
            _subscriberRegitration = subscriberRegitration;
        }

        public async Task PublishAsync(object message)
        {
            var key = message.GetType().Name;
            var registrations = _subscriberRegitration.GetBroadcastRegistrations(key);

            var tasks = new List<Task>();

            foreach (var registration in registrations)
            {
                var action = registration.Action;
                var receiverMessage = TranslateType(message, registration.ReceiverType);
                tasks.Add(action(receiverMessage));
            }

            await Task.WhenAll(tasks);
        }

        private object TranslateType(object value, Type type)
            => _messageSerializer.Deserialize(_messageSerializer.Serialize(value), type);
    }
}
