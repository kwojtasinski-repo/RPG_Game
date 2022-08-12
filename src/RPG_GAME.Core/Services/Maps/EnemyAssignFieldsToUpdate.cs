using RPG_GAME.Core.Entities.Common;
using RPG_GAME.Core.Entities.Maps;
using System.Collections.Generic;

namespace RPG_GAME.Core.Services.Maps
{
    public sealed class EnemyAssignFieldsToUpdate
    {
        public string EnemyName { get; }
        public Difficulty Difficulty { get; }
        public IEnumerable<SkillEnemyAssign> Skills { get; } = new List<SkillEnemyAssign>();

        public EnemyAssignFieldsToUpdate(string enemyName, Difficulty difficulty, IEnumerable<SkillEnemyAssign> skills = null)
        {
            EnemyName = enemyName;
            Difficulty = difficulty;
            Skills = skills;
        }
    }
}
