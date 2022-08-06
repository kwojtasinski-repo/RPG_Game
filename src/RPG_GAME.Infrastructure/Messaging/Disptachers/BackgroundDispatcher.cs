using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RPG_GAME.Application.Messaging.Clients;

namespace RPG_GAME.Infrastructure.Messaging.Disptachers
{
    internal sealed class BackgroundDispatcher : BackgroundService
    {
        private readonly IMessageChannel _messageChannel;
        private readonly IInternalClient _internalClient;
        private readonly ILogger<BackgroundDispatcher> _logger;

        public BackgroundDispatcher(IMessageChannel messageChannel, IInternalClient internalClient, ILogger<BackgroundDispatcher> logger)
        {
            _messageChannel = messageChannel;
            _internalClient = internalClient;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Running the background dispatcher");

            await foreach (var message in _messageChannel.Reader.ReadAllAsync())
            {
                try
                {
                    await _internalClient.PublishAsync(message);
                }
                catch (Exception exception)
                {
                    _logger.LogError(exception, exception.Message);
                }
            }

            _logger.LogInformation("Finished the background dispatcher");
        }
    }
}
