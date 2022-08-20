using RPG_GAME.Core.Entities.Enemies;
using RPG_GAME.Core.Entities.Maps;

namespace RPG_GAME.Application.Managers
{
    public interface IEnemyIncreaseStatsManager
    {
        void IncreaseEnemyStats(int level, EnemyAssign enemyAssign, Enemy enemy);
    }
}
