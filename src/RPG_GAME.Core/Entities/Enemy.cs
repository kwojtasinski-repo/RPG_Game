using System;

namespace RPG_GAME.Core.NewEntities
{
    // Abstract?
    public class Enemy
    {
        public Guid Id { get; set; }
        public CharacterType Character => CharacterType.ENEMY;
        public string Name { get; set; }
        public int BaseAttack { get; set; }
        public int BaseHealth { get; set; }
        public int BaseHealLvl { get; set; }
        public Difficulty Difficulty { get; set; }
    }
}
