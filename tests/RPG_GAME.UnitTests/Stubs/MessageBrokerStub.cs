using RPG_GAME.Application.Messaging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RPG_GAME.UnitTests.Stubs
{
    internal sealed class MessageBrokerStub : IMessageBroker
    {
        private readonly IList<IMessage> _messages = new List<IMessage>();

        public Task PublishAsync(params IMessage[] messages)
        {
            if (messages is null)
            {
                return Task.CompletedTask;
            }

            if (messages.Any(m => m is null))
            {
                return Task.CompletedTask;
            }

            foreach (var message in messages)
            {
                _messages.Add(message);
            }

            return Task.CompletedTask;
        }

        public IEnumerable<IMessage> GetPublishedMessages()
        {
            return _messages;
        }

        public void ClearMessages()
        {
            _messages.Clear();
        }
    }
}
