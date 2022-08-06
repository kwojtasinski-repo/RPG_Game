using RPG_GAME.Application.Messaging;

namespace RPG_GAME.Infrastructure.Messaging.Disptachers
{
    internal interface IAsyncMessageDispatcher
    {
        Task PublishAsync<TMessage>(TMessage message) where TMessage : class, IMessage;
    }
}
