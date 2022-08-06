namespace RPG_GAME.Application.Messaging
{
    public interface IMessageBroker
    {
        Task PublishAsync(params IMessage[] messages);
    }
}
