using System;
using System.Collections.Generic;
using System.Text;

namespace RPG_GAME
{
    public class Knight : Enemy
    {

        public Knight(string _name, int _attack, int _health, string _category) : base(_name, _attack, _health, _category)
        {
        }

        // Battle Functions
        public void DoubleSwordStrike(Hero target)
        {
            target.ReceiveDmg(Attack + 3);
        }

        public void SpecialSpin(Hero target)
        {
            target.ReceiveDmg(Attack + 5);
        }

        public override void EnemyTurn(int choice, Hero target)
        {
            base.EnemyTurn(choice, target);
            switch (choice)
            {
                case 1:
                    NormAttack(target);
                    Heal();
                    Console.WriteLine("Knight hit you!");
                    ShowHeroHealth(target);
                    break;
                case 2:
                    DoubleSwordStrike(target);
                    Heal();
                    Console.WriteLine("Knight hit you with double sword strike!");
                    ShowHeroHealth(target);
                    break;
                case 3:
                    SpecialSpin(target);
                    Heal();
                    Console.WriteLine("Knight hit you with special spin!");
                    ShowHeroHealth(target);
                    break;
            }
        }
    }
}
