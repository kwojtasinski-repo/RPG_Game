using Microsoft.Extensions.Logging;
using RPG_GAME.Application.Events.Maps;
using RPG_GAME.Core.Repositories;
using RPG_GAME.Core.Services.Enemies;

namespace RPG_GAME.Application.Events.Handlers.Maps
{
    internal class EnemyAddedToMapHandler : IEventHandler<EnemyAddedToMap>
    {
        private readonly IMapAllocatorDomainService _mapAllocatorDomainService;
        private readonly IEnemyRepository _enemyRepository;
        private readonly ILogger<EnemyAddedToMapHandler> _logger;

        public EnemyAddedToMapHandler(IMapAllocatorDomainService mapAllocatorDomainService, IEnemyRepository enemyRepository,
            ILogger<EnemyAddedToMapHandler> logger)
        {
            _mapAllocatorDomainService = mapAllocatorDomainService;
            _enemyRepository = enemyRepository;
            _logger = logger;
        }

        public async Task HandleAsync(EnemyAddedToMap @event)
        {
            var enemy = await _enemyRepository.GetAsync(@event.EnemyId);

            if (enemy is null)
            {
                _logger.LogInformation($"Enemy with id '{@event.EnemyId}' doesnt exists");
                return;
            }

            await _mapAllocatorDomainService.AddMap(enemy, @event.MapId);
            await _enemyRepository.UpdateAsync(enemy);
        }
    }
}
