using RPG_GAME.Application.Messaging;

namespace RPG_GAME.Application.Commands
{
    // marker
    public interface ICommand : IMessage
    {
    }

    public interface ICommand<T> : IMessage
    {
    }
}
