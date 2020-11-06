using RPG_GAME.Core.Common;
using System;

namespace RPG_GAME.Core.Entity
{
    public class Archer : Enemy
    {
        public Archer(string name, int attack, int health, string category, int diffLvl, int healLvl = 0) : base(name, attack, health, category, diffLvl, healLvl)
        {
        }

        // Battle Functions
        public int TwoArrows(IPerson target)
        {
            target.ReceiveDmg(Attack + 1);
            return Attack + 1;
        }

        public int SpecialArrow(IPerson target)
        {
            target.ReceiveDmg(Attack + 3);
            return Attack + 3;
        }

        public override void EnemyTurn(int choice, IPerson target)
        {
            base.EnemyTurn(choice, target);
            int dealtDmg;
            switch (choice)
            {
                case 1:
                    NormAttack(target);
                    Heal();
                    Console.WriteLine($"Archer hit you and dealt {Attack} damage!");
                    target.PrintStats();
                    break;
                case 2:
                    dealtDmg = TwoArrows(target);
                    Heal();
                    Console.WriteLine($"Archer hit you with two arrows and dealt {dealtDmg} damage!");
                    target.PrintStats();
                    break;
                case 3:
                    dealtDmg = SpecialArrow(target);
                    Heal();
                    Console.WriteLine($"Archer hit you with special arrow and dealt {dealtDmg} damage!");
                    target.PrintStats();
                    break;
            }
        }
    }
}
