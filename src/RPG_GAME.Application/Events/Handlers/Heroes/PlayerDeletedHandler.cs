using Microsoft.Extensions.Logging;
using RPG_GAME.Application.Events.Players;
using RPG_GAME.Core.Repositories;
using RPG_GAME.Core.Services.Heroes;

namespace RPG_GAME.Application.Events.Handlers.Heroes
{
    internal sealed class PlayerDeletedHandler : IEventHandler<PlayerDeleted>
    {
        private readonly IPlayerAllocatorDomainService _playerAllocatorDomainService;
        private readonly IHeroRepository _heroRepository;
        private readonly ILogger<PlayerDeletedHandler> _logger;

        public PlayerDeletedHandler(IPlayerAllocatorDomainService playerAllocatorDomainService,
                    IHeroRepository heroRepository, ILogger<PlayerDeletedHandler> logger)
        {
            _playerAllocatorDomainService = playerAllocatorDomainService;
            _heroRepository = heroRepository;
            _logger = logger;
        }

        public async Task HandleAsync(PlayerDeleted @event)
        {
            var hero = await _heroRepository.GetAsync(@event.HeroId);

            if (hero is null)
            {
                _logger.LogInformation($"Hero with id '{@event.HeroId}' doesnt exists");
                return;
            }

            await _playerAllocatorDomainService.DeletePlayer(hero, @event.PlayerId);
            await _heroRepository.UpdateAsync(hero);
        }
    }
}
