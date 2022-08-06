using RPG_GAME.Application.Messaging;
using RPG_GAME.Infrastructure.Messaging.Disptachers;

namespace RPG_GAME.Infrastructure.Messaging.Brokers
{
    internal sealed class InMemoryMessageBroker : IMessageBroker
    {
        private readonly IAsyncMessageDispatcher _asyncMessageDispatcher;

        public InMemoryMessageBroker(IAsyncMessageDispatcher asyncMessageDispatcher)
        {
            _asyncMessageDispatcher = asyncMessageDispatcher;
        }

        public async Task PublishAsync(params IMessage[] messages)
        {
            if (messages is null)
            {
                return;
            }

            messages = messages.Where(m => m is not null).ToArray();

            foreach (var message in messages)
            {
                await _asyncMessageDispatcher.PublishAsync(message);
            }
        }
    }
}
