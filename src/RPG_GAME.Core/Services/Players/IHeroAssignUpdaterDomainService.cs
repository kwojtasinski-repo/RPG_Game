using RPG_GAME.Core.Entities.Players;
using System.Threading.Tasks;

namespace RPG_GAME.Core.Services.Players
{
    public interface IHeroAssignUpdaterDomainService
    {
        Task ChangeHeroAssignFieldsAsync(HeroAssign heroAssign, HeroAssignFieldsToUpdate heroAssignFieldsToUpdate);
    }
}
