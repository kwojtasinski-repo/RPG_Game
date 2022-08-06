using RPG_GAME.Core.Entities.Enemies;
using System;
using System.Threading.Tasks;

namespace RPG_GAME.Core.Services.Enemies
{
    internal sealed class MapAllocatorDomainService : IMapAllocatorDomainService
    {
        public Task AddMap(Enemy enemy, Guid mapId)
        {
            enemy.AddMap(mapId);
            return Task.CompletedTask;
        }

        public Task DeleteMap(Enemy enemy, Guid mapId)
        {
            enemy.RemoveMap(mapId);
            return Task.CompletedTask;
        }
    }
}
