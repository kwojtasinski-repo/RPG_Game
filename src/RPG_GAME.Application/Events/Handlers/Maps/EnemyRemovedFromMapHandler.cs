using Microsoft.Extensions.Logging;
using RPG_GAME.Application.Events.Maps;
using RPG_GAME.Core.Repositories;
using RPG_GAME.Core.Services.Enemies;

namespace RPG_GAME.Application.Events.Handlers.Maps
{
    internal class EnemyRemovedFromMapHandler : IEventHandler<EnemyRemovedFromMap>
    {
        private readonly IMapAllocatorDomainService _mapAllocatorDomainService;
        private readonly IEnemyRepository _enemyRepository;
        private readonly ILogger<EnemyRemovedFromMapHandler> _logger;

        public EnemyRemovedFromMapHandler(IMapAllocatorDomainService mapAllocatorDomainService, IEnemyRepository enemyRepository,
            ILogger<EnemyRemovedFromMapHandler> logger)
        {
            _mapAllocatorDomainService = mapAllocatorDomainService;
            _enemyRepository = enemyRepository;
            _logger = logger;
        }

        public async Task HandleAsync(EnemyRemovedFromMap @event)
        {
            var enemy = await _enemyRepository.GetAsync(@event.EnemyId);

            if (enemy is null)
            {
                _logger.LogInformation($"Enemy with id '{@event.EnemyId}' doesnt exists");
                return;
            }

            await _mapAllocatorDomainService.DeleteMap(enemy, @event.MapId);
            await _enemyRepository.UpdateAsync(enemy);
        }
    }
}
