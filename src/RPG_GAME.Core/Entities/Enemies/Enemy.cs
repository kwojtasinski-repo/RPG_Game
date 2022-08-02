using RPG_GAME.Core.Entities.Common;
using System;
using System.Collections.Generic;

namespace RPG_GAME.Core.Entities.Enemies
{
    public class Enemy : IIdentifiable<Guid>
    {
        public Guid Id { get; set; }
        public CharacterType Character => CharacterType.ENEMY;
        public string EnemyName { get; set; }
        public State<int> BaseAttack { get; set; }
        public State<int> BaseHealth { get; set; }
        public State<int> BaseHealLvl { get; set; }
        public State<decimal> Experience { get; set; }
        public Difficulty Difficulty { get; set; }
        public IEnumerable<SkillEnemy> Skills { get; set; }
    }
}
