using RPG_GAME.Core.Entities.Players;
using System.Linq;
using System.Threading.Tasks;

namespace RPG_GAME.Core.Services.Players
{
    internal sealed class HeroAssignUpdaterDomainService : IHeroAssignUpdaterDomainService
    {
        public Task ChangeHeroAssignFieldsAsync(HeroAssign heroAssign, HeroAssignFieldsToUpdate heroAssignFieldsToUpdate)
        {
            heroAssign.ChangeHeroName(heroAssignFieldsToUpdate.HeroName);

            if (heroAssign.Skills.Any())
            {
                heroAssign.ChangeSkills(heroAssignFieldsToUpdate.Skills);
            }

            return Task.CompletedTask;
        }
    }
}
