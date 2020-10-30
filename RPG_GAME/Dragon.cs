using System;
using System.Collections.Generic;
using System.Text;

namespace RPG_GAME
{
    public class Dragon : Enemy
    {

        public Dragon(string _name, int _attack, int _health, string _category) : base(_name, _attack, _health, _category)
        {
        }

        // Battle Functions
        public void Bite(Hero target)
        {
            target.ReceiveDmg(Attack + 5);
        }

        public void FireBreath(Hero target)
        {
            target.ReceiveDmg(Attack + 10);
        }

        public override void EnemyTurn(int choice, Hero target)
        {
            base.EnemyTurn(choice, target);
            switch (choice)
            {
                case 1:
                    NormAttack(target);
                    Heal();
                    Console.WriteLine("Dragon hit you!");
                    ShowHeroHealth(target);
                    break;
                case 2:
                    Bite(target);
                    Heal();
                    ShowHeroHealth(target);
                    break;
                case 3:
                    FireBreath(target);
                    Heal();
                    ShowHeroHealth(target);
                    break;
            }
        }

    }
}
