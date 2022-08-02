using System;

namespace RPG_GAME.Core.Entities.Maps
{
    public class SkillEnemyAssign
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int BaseAttack { get; set; }
        public decimal Probability { get; set; }
    }
}
