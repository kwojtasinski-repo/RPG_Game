using System;

namespace RPG_GAME
{
    public class Archer : Enemy
    {

        public Archer(string _name, int _attack, int _health, string _category) : base(_name, _attack, _health, _category)
        {
        }

        // Battle Functions
        public void TwoArrows(Hero target)
        {
            target.ReceiveDmg(Attack + 1);
        }

        public void SpecialArrow(Hero target)
        {
            target.ReceiveDmg(Attack + 3);
        }

        public override void EnemyTurn(int choice, Hero target)
        {
            base.EnemyTurn(choice, target);
            switch(choice)
            {
                case 1:
                    NormAttack(target);
                    Heal();
                    Console.WriteLine("Archer hit you!");
                    ShowHeroHealth(target);
                    break;
                case 2:
                    TwoArrows(target);
                    Heal();
                    Console.WriteLine("Archer hit you with two arrows!");
                    ShowHeroHealth(target);
                    break;
                case 3:
                    SpecialArrow(target);
                    Heal();
                    Console.WriteLine("Archer hit you with special arrow!");
                    ShowHeroHealth(target);
                    break;
            }    
        }
    }
}
