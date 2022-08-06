using RPG_GAME.Core.Entities.Heroes;
using System;
using System.Threading.Tasks;

namespace RPG_GAME.Core.Services.Heroes
{
    public interface IPlayerAllocatorDomainService
    {
        Task AddPlayer(Hero hero, Guid playerId);
        Task DeletePlayer(Hero hero, Guid playerId);
    }
}
