using RPG_GAME.Core.Common;
using System;

namespace RPG_GAME.Core.Entity
{
    public class Knight : Enemy
    {
        public Knight(string name, int attack, int health, string category, int diffLvl, int healLvl = 0) : base(name, attack, health, category, diffLvl, healLvl)
        {
        }


        // Battle Functions
        public int DoubleSwordStrike(IPerson target)
        {
            target.ReceiveDmg(Attack + 3);
            return Attack + 3;
        }

        public int SpecialSpin(IPerson target)
        {
            target.ReceiveDmg(Attack + 5);
            return Attack + 5;
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
                    Console.WriteLine($"Knight hit you and dealt {Attack} damage!");
                    target.PrintStats();
                    break;
                case 2:
                    dealtDmg = DoubleSwordStrike(target);
                    Heal();
                    Console.WriteLine($"Knight hit you with double sword strike and dealt {dealtDmg} damage!");
                    target.PrintStats();
                    break;
                case 3:
                    dealtDmg = SpecialSpin(target);
                    Heal();
                    Console.WriteLine($"Knight hit you with special spin and dealt {dealtDmg} damage!");
                    target.PrintStats();
                    break;
            }
        }
    }
}
