using Microsoft.Extensions.Logging;
using RPG_GAME.Application.Events.Enemies;
using RPG_GAME.Core.Repositories;
using RPG_GAME.Core.Services.Maps;

namespace RPG_GAME.Application.Events.Handlers.Players
{
    internal sealed class EnemyUpdatedHandler : IEventHandler<EnemyUpdated>
    {
        private readonly IMapRepository _mapRepository;
        private readonly ILogger<EnemyUpdatedHandler> _logger;
        private readonly IEnemyAssignUpdaterDomainService _enemyAssignUpdaterDomainService;

        public EnemyUpdatedHandler(IMapRepository mapRepository, ILogger<EnemyUpdatedHandler> logger,
                IEnemyAssignUpdaterDomainService enemyAssignUpdaterDomainService)
        {
            _mapRepository = mapRepository;
            _logger = logger;
            _enemyAssignUpdaterDomainService = enemyAssignUpdaterDomainService;
        }

        public async Task HandleAsync(EnemyUpdated @event)
        {
            var maps = await _mapRepository.GetAllMapsByEnemyId(@event.EnemyId);

            if (!maps.Any())
            {
                _logger.LogInformation($"Maps for enemy with id '{@event.EnemyId}' was not found");
                return;
            }

            foreach (var map in maps)
            {
                var enemies = map.Enemies.SingleOrDefault(e => e.Enemy.Id == @event.EnemyId);

                if (enemies is null)
                {
                    _logger.LogInformation($"Map with id: '{map.Id}' for enemy with id '{@event.EnemyId}' was not found");
                    continue;
                }

                enemies.Enemy.ChangeAttack(@event.BaseAttack);
                enemies.Enemy.ChangeHealth(@event.BaseHealth);
                enemies.Enemy.ChangeHealLvl(@event.BaseHealLvl);
                enemies.Enemy.ChangeExperience(@event.Experience);
                await _enemyAssignUpdaterDomainService.ChangeEnemyAssignFieldsAsync(enemies.Enemy, new EnemyAssignFieldsToUpdate(@event.EnemyName, @event.Difficulty, @event.Skills));

                await _mapRepository.UpdateAsync(map);
            }
        }
    }
}
