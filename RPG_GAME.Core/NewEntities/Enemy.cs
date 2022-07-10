using System;

namespace RPG_GAME.Core.NewEntities
{
    // Abstract?
    public class Enemy
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int BaseAttack { get; set; }
        public int BaseHealth { get; set; }
        public int BaseHealLvl { get; set; }
    }
}
