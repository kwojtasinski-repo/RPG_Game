using System;
using System.Collections.Generic;
using System.Text;

namespace RPG_GAME
{
    public class Hero : Person
    {
        private int _healLvl = 9;
        private int _exp;
        private int _level;

        public int Exp { get { return _exp; } }

        public Hero(string _name, int _attack, int _health) : base(_name, _attack, _health)
        {
            _exp = 0;
            _level = 1;
            Console.WriteLine($"Welocome hero {Name}!");
        }

        public int ChoiceAction()
        {
            int choice = -1;
            bool chosen = false;
            while(!chosen)
            {
                Console.WriteLine("What would you like to do?");
                Console.WriteLine("1. Attack");
                Console.WriteLine("2. Heal");
                Console.WriteLine("3. Special attack");

                var operation = Console.ReadKey();
                Console.WriteLine();
                switch (operation.KeyChar)
                {
                    case '1':
                        Int32.TryParse(operation.KeyChar.ToString(), out choice);
                        chosen = true;
                        break;
                    case '2':
                        choice = 4;
                        chosen = true;
                        break;
                    case '3':
                        Console.WriteLine("Choose special attack:");
                        Console.WriteLine("1. Spin Attack");
                        Console.WriteLine("2. Double Slash");
                        Console.WriteLine("3. <- Go back");
                        var operation2 = Console.ReadKey();
                        if(operation2.KeyChar == '1' || operation2.KeyChar == '2')
                        {
                            Int32.TryParse(operation2.KeyChar.ToString(), out choice);
                            choice += 1;
                            chosen = true;
                        }
                        Console.WriteLine();
                        break;
                    default:
                        Console.WriteLine("Action you entered does not exist");
                        break;
                }
            }
            return choice;
        }

        public override void Heal()
        {
            if (Health < MaxHealth)
            {
                int max_heal_lv;

                if (Health + _healLvl > MaxHealth)
                    max_heal_lv = MaxHealth - Health;
                else
                    max_heal_lv = _healLvl;

                this.UpgradeHealth(max_heal_lv);
                Console.WriteLine($"Your current Health: {Health}/{MaxHealth}");
            }
            else
                Console.WriteLine($"You cannot heal yourself you have {Health}/{MaxHealth}");
        }

        public void GainExp(int exp)
        {
            Console.WriteLine($"You gained +{exp} exp");
            _exp += exp;
        }

        public void LevelUp()
        {
            _exp -= 100;
            _level++;
            Console.WriteLine("You leveled up!");
            Console.WriteLine($"You have recently reached level {_level}. Congrats");
            Console.WriteLine("Attack +4");
            Console.WriteLine("Max health +10");
            Console.WriteLine("Heal +5");
            Console.ReadLine();

            this.UpgradeAttack(4);
            this.UpgradeMaxHealth(10);
            this.UpgradeHealth(10);
            _healLvl += 5;
        }

        // Battle Functions
        public void SpinAttack(Enemy target)
        {
            target.ReceiveDmg((Attack - 2) * 3);
        }

        public void DoubleSlash(Enemy target)
        {
            target.ReceiveDmg(Attack * 2);
        }

        public void HeroTurn(int choice, Enemy target)
        {
            switch (choice)
            {
                case 1:
                    NormAttack(target);
                    Console.WriteLine($"Hero {Name} hit {target.Name}!");
                    break;
                case 2:
                    SpinAttack(target);
                    Console.WriteLine($"Hero {Name} hit {target.Name} with spin attack!");
                    break;
                case 3:
                    DoubleSlash(target);
                    Console.WriteLine($"Hero {Name} hit {target.Name} with double slash!");
                    break;
                case 4:
                    Heal();
                    break;
            }
        }
    }
}
