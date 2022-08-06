using RPG_GAME.Core.Entities.Heroes;
using System;
using System.Threading.Tasks;

namespace RPG_GAME.Core.Services.Heroes
{
    internal sealed class PlayerAllocatorDomainService : IPlayerAllocatorDomainService
    {
        public Task AddPlayer(Hero hero, Guid playerId)
        {
            hero.AddPlayer(playerId);
            return Task.CompletedTask;
        }

        public Task DeletePlayer(Hero hero, Guid playerId)
        {
            hero.RemovePlayer(playerId);
            return Task.CompletedTask;
        }
    }
}
