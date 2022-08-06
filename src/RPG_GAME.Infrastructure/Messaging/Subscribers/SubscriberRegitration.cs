using RPG_GAME.Application.Messaging.Subscribers;
using System.Collections.Concurrent;

namespace RPG_GAME.Infrastructure.Messaging.Subscribers
{
    internal sealed class SubscriberRegitration : ISubscriberRegitration
    {
        private readonly ConcurrentBag<BroadcastRegistration> _broadcastRegistrations = new();

        public void AddBroadcastAction(Type requestType, Func<object, Task> action)
        {
            if (string.IsNullOrWhiteSpace(requestType.Namespace))
            {
                throw new InvalidOperationException("Missing namespace.");
            }

            var registration = new BroadcastRegistration(requestType, action);
            _broadcastRegistrations.Add(registration);
        }

        public IEnumerable<BroadcastRegistration> GetBroadcastRegistrations(string key)
            => _broadcastRegistrations.Where(t => t.Key == key);
    }
}
