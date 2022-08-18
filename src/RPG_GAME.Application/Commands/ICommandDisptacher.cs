namespace RPG_GAME.Application.Commands
{
    public interface ICommandDispatcher
    {
        Task SendAsync<TCommand>(TCommand command) where TCommand : class, ICommand;
        Task<TResult> SendAsync<TResult>(ICommand<TResult> command);
    }
}
