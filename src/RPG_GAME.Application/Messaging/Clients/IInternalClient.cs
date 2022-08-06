namespace RPG_GAME.Application.Messaging.Clients
{
    public interface IInternalClient
    {
        Task PublishAsync(object message);
    }
}
