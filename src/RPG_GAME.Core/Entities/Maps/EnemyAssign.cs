using RPG_GAME.Core.Entities.Common;
using System;
using System.Collections.Generic;

namespace RPG_GAME.Core.Entities.Maps
{
    public class EnemyAssign
    {
        public Guid Id { get; set; }
        public CharacterType Character => CharacterType.ENEMY;
        public string EnemyName { get; set; }
        public int BaseAttack { get; set; }
        public int BaseHealth { get; set; }
        public int BaseHealLvl { get; set; }
        public decimal Experience { get; set; }
        public Difficulty Difficulty { get; set; }
        public IEnumerable<SkillEnemyAssign> Skills { get; set; }
    }
}
