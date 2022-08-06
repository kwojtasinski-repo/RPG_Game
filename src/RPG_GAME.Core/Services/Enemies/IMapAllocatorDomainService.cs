using RPG_GAME.Core.Entities.Enemies;
using System;
using System.Threading.Tasks;

namespace RPG_GAME.Core.Services.Enemies
{
    public interface IMapAllocatorDomainService
    {
        Task AddMap(Enemy enemy, Guid mapId);
        Task DeleteMap(Enemy enemy, Guid mapId);
    }
}
