using Microsoft.Extensions.Logging;
using RPG_GAME.Application.Events.Heroes;
using RPG_GAME.Application.Managers;
using RPG_GAME.Core.Repositories;
using RPG_GAME.Core.Services.Players;

namespace RPG_GAME.Application.Events.Handlers.Players
{
    internal class HeroUpdatedHandler : IEventHandler<HeroUpdated>
    {
        private readonly IPlayerRepository _playerRepository;
        private readonly ILogger<HeroUpdatedHandler> _logger;
        private readonly IHeroAssignUpdaterDomainService _heroAssignUpdaterDomainService;
        private readonly IPlayerIncreaseStatsManager _playerIncreaseStatsManager;

        public HeroUpdatedHandler(IPlayerRepository playerRepository, ILogger<HeroUpdatedHandler> logger,
            IHeroAssignUpdaterDomainService heroAssignUpdaterDomainService)
        {
            _playerRepository = playerRepository;
            _logger = logger;
            _heroAssignUpdaterDomainService = heroAssignUpdaterDomainService;
        }

        public async Task HandleAsync(HeroUpdated @event)
        {
            var players = await _playerRepository.GetAllPlayersByHeroId(@event.HeroId);

            if (!players.Any())
            {
                _logger.LogInformation($"Players for hero with id '{@event.HeroId}' was not found");
                return;
            }

            foreach (var player in players)
            {
                if (player is null)
                {
                    _logger.LogInformation($"Player with id: '{player.Id}' for hero with id '{@event.HeroId}' was not found");
                    continue;
                }

                await _heroAssignUpdaterDomainService.ChangeHeroAssignFieldsAsync(player.Hero, new HeroAssignFieldsToUpdate(@event.HeroName, @event.SkillsToUpdate));
                _playerIncreaseStatsManager.IncreaseHeroSkills(player.Level, player.Hero, @event.Skills);
                await _playerRepository.UpdateAsync(player);
            }
        }
    }
}
