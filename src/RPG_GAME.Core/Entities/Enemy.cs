using System;
using System.Collections.Generic;

namespace RPG_GAME.Core.Entities
{
    public class Enemy : IIdentifiable<Guid>
    {
        public Guid Id { get; set; }
        public CharacterType Character => CharacterType.ENEMY;
        public string EnemyName { get; set; }
        public Field<int> BaseAttack { get; set; }
        public Field<int> BaseHealth { get; set; }
        public Field<int> BaseHealLvl { get; set; }
        public Field<decimal> Experience { get; set; }
        public Difficulty Difficulty { get; set; }
        public IEnumerable<Skill> Skills { get; set; }
    }
}
