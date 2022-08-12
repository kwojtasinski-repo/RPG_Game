using RPG_GAME.Core.Entities.Maps;
using System.Linq;
using System.Threading.Tasks;

namespace RPG_GAME.Core.Services.Maps
{
    internal sealed class EnemyAssignUpdaterDomainService : IEnemyAssignUpdaterDomainService
    {
        public Task ChangeEnemyAssignFieldsAsync(EnemyAssign enemyAssign, EnemyAssignFieldsToUpdate enemyAssignFieldsToUpdate)
        {
            enemyAssign.ChangeName(enemyAssignFieldsToUpdate.EnemyName);
            enemyAssign.ChangeDifficulty(enemyAssignFieldsToUpdate.Difficulty);

            if (!enemyAssignFieldsToUpdate.Skills.Any())
            {
                enemyAssign.ChangeSkills(enemyAssignFieldsToUpdate.Skills);
            }

            return Task.CompletedTask;
        }
    }
}
