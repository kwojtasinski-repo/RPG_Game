using RPG_GAME.Core.Exceptions.Players;
using System;

namespace RPG_GAME.Core.Entities.Players
{
    public class SkillHeroAssign
    {
        public Guid Id { get; }
        public string Name { get; }
        public int Attack { get; private set; }

        public SkillHeroAssign(Guid id, string name, int attack)
        {
            Id = id;
            Name = name;
            ChangeAttack(attack);
        }

        public void ChangeAttack(int attack)
        {
            if (attack is <= 0)
            {
                throw new PlayerSkillAttackCannotBeZeroOrNegativeException(attack);
            }

            Attack = attack;
        }
    }
}
