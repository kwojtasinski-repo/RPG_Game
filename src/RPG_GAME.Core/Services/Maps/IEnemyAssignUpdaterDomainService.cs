using RPG_GAME.Core.Entities.Maps;
using System.Threading.Tasks;

namespace RPG_GAME.Core.Services.Maps
{
    public interface IEnemyAssignUpdaterDomainService
    {
        Task ChangeEnemyAssignFieldsAsync(EnemyAssign enemyAssign, EnemyAssignFieldsToUpdate enemyAssignFieldsToUpdate);
    }
}
