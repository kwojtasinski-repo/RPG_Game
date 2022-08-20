using RPG_GAME.Application.Commands;

namespace RPG_GAME.Infrastructure.Commands
{
    internal interface ICommandDispatcher
    {
        Task SendAsync<TCommand>(TCommand command) where TCommand : class, ICommand;
        Task<TResult> SendAsync<TResult>(ICommand<TResult> command);
    }
}
