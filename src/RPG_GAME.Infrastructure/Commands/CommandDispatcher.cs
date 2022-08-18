using Microsoft.Extensions.DependencyInjection;
using RPG_GAME.Application.Commands;

namespace RPG_GAME.Infrastructure.Commands
{
    internal sealed class CommandDispatcher : ICommandDispatcher
    {
        private readonly IServiceProvider _serviceProvider;

        public CommandDispatcher(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task<TResult> SendAsync<TResult>(ICommand<TResult> command)
        {
            if (command is null)
            {
                throw new ArgumentNullException($"Cannot execute when {typeof(ICommand<TResult>).Name} is null");
            }

            using var scope = _serviceProvider.CreateScope();
            var handlerType = typeof(ICommandHandler<,>).MakeGenericType(command.GetType(), typeof(TResult));
            var handler = scope.ServiceProvider.GetRequiredService(handlerType);
            var result = await (Task<TResult>)handlerType
                .GetMethod(nameof(ICommandHandler<ICommand<TResult>, TResult>.HandleAsync))
                ?.Invoke(handler, new[] { command });

            return result;
        }

        public async Task SendAsync<TCommand>(TCommand command) where TCommand : class, ICommand
        {
            if (command is null)
            {
                throw new ArgumentNullException($"Cannot execute when {typeof(ICommand).Name} is null");
            }

            using var scope = _serviceProvider.CreateScope();
            var handler = scope.ServiceProvider.GetRequiredService<ICommandHandler<TCommand>>();
            await handler.HandleAsync(command);
        }
    }
}
