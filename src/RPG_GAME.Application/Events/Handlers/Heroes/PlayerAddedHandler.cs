using RPG_GAME.Application.Events.Players;

namespace RPG_GAME.Application.Events.Handlers.Heroes
{
    internal sealed class PlayerAddedHandler : IEventHandler<PlayerAdded>
    {
        public PlayerAddedHandler()
        {

        }

        public Task HandleAsync(PlayerAdded @event)
        {
            return Task.CompletedTask;
        }
    }
}
