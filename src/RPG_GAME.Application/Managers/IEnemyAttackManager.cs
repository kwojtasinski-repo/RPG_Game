using RPG_GAME.Application.DTO.Battles;
using RPG_GAME.Core.Entities.Maps;

namespace RPG_GAME.Application.Managers
{
    public interface IEnemyAttackManager
    {
        EnemyAttackDto AttackHeroWithDamage(EnemyAssign enemyAssign);
    }
}
