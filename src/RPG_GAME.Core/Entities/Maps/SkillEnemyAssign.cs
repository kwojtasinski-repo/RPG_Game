using RPG_GAME.Core.Exceptions.Maps;
using System;

namespace RPG_GAME.Core.Entities.Maps
{
    public class SkillEnemyAssign
    {
        public Guid Id { get; }
        public string Name { get; }
        public int Attack { get; private set; }
        public decimal Probability { get; }

        public SkillEnemyAssign(Guid id, string name, int attack, decimal probability)
        {
            Id = id;
            Name = name;
            ChangeAttack(attack);
            Probability = probability;
        }

        public void ChangeAttack(int attack)
        {
            if (attack is <= 0)
            {
                throw new AttackCannotBeZeroOrNegativeException(attack);
            }

            Attack = attack;
        }
    }
}
