using RPG_GAME.Core.Common;
using System;

namespace RPG_GAME.Core.Entity
{
    public class Dragon : Enemy
    {
        public Dragon(int id,string name, int attack, int health, string category, int diffLvl, int healLvl = 0) : base(id, name, attack, health, category, diffLvl, healLvl)
        {
        }

        // Battle Functions
        public int Bite(IPerson target)
        {
            target.ReceiveDmg(Attack + 5);
            return Attack + 5;
        }

        public int FireBreath(IPerson target)
        {
            target.ReceiveDmg(Attack + 10);
            return Attack + 10;
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
                    Console.WriteLine($"Dragon hit you and dealt {Attack} damage!");
                    target.PrintStats();
                    break;
                case 2:
                    dealtDmg = Bite(target);
                    Console.WriteLine($"Dragon bit you and dealt {dealtDmg} damage!");
                    Heal();
                    target.PrintStats();
                    break;
                case 3:
                    dealtDmg = FireBreath(target);
                    Console.WriteLine($"Dragon hit you with fire breath and dealt {dealtDmg} damage!");
                    Heal();
                    target.PrintStats();
                    break;
            }
        }
    }
}
