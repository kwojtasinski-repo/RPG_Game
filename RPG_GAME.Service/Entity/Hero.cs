using RPG_GAME.Core.Common;
using System;


namespace RPG_GAME.Core.Entity
{
    public class Hero : BaseEntity, IPerson
    {
        public int MaxHealth { get; set; }
        public string Name { get; set; }
        public int Health { get; set; }
        public int Attack { get; set; }
        public int HealLvl { get; set; }
        public int Exp { get; set; }
        public int Level { get; set; }
        public int RequiredExp { get; set; }
        public string Profession { get; set; }
        public int TypeId { get; set; }

        public Hero()
        {

        }

        public Hero(int id, string name, int attack, int health, string profession, int typeId)
        {
            Id = id;
            Name = name;
            Attack = attack;
            Health = health;
            MaxHealth = health;
            Profession = profession;
            TypeId = typeId;
            Exp = 0;
            Level = 1;
            RequiredExp = 100;
            HealLvl = 2;
            Console.WriteLine($"Welocome hero {Name}!");
        }

        public void ChangeName(string name)
        {
            Name = name;
        }

        public void Heal()
        {
            if (Health < MaxHealth)
            {
                int max_heal_lv;

                if (Health + HealLvl > MaxHealth)
                    max_heal_lv = MaxHealth - Health;
                else
                    max_heal_lv = HealLvl;

                Health += max_heal_lv;
                Console.WriteLine($"Your current Health: {Health}/{MaxHealth}");
            }
            else
                Console.WriteLine($"You cannot heal yourself you have {Health}/{MaxHealth}");
        }

        public void PrintStats()
        {
            Console.WriteLine($"{Name} {Profession} stats:");
            Console.WriteLine($"Level {Level}, Exp: {Exp}/{RequiredExp}");
            Console.WriteLine($"Attack value is: {Attack}");
            Console.WriteLine($"Health value is: {Health}");
            Console.WriteLine($"Can Heal by {HealLvl} point health");
        }

        public void ReceiveDmg(int dmg)
        {
            Health -= dmg;
        }

        public void Reset()
        {
            if(Health <= 0)
            {
                Console.WriteLine("The hero is dead. The healer have healed him a moment ago.");
                SetHealth(MaxHealth);
            }
            else
            {
                Console.WriteLine("The Hero rested after the battle.");
                SetHealth(MaxHealth);
            }
           
        }

        public void SetHealth(int health)
        {
            Health = health;
        }

        public void UpgradeAttack(int attack)
        {
            Attack += attack;
        }

        public void UpgradeMaxHealth(int health)
        {
            MaxHealth += health;
        }

        public void UpgradeHeal(int heal)
        {
            HealLvl += heal;
        }

        public void GainExp(int exp)
        {
            Console.WriteLine($"You gained +{exp} exp");
            Exp += exp;
        }

        public void LevelUp()
        {
            int baseRequiredExp = 100;

            if (Exp >= RequiredExp)
            {
                Exp -= RequiredExp;
                Level++;
                Console.WriteLine("You leveled up!");
                Console.WriteLine($"You have recently reached level {Level}. Congrats");
                Console.WriteLine("Attack +4");
                Console.WriteLine("Max health +10");
                Console.WriteLine("Heal +5");
                Console.ReadLine();

                UpgradeAttack(4 + Level);
                UpgradeMaxHealth(10 + Level);
                Health += 10 + Level;
                HealLvl += 5 + Level;
                RequiredExp += baseRequiredExp + Level * 100;
            }
        }

        // Battle Functions
        public void NormAttack(IPerson target)
        {
            target.ReceiveDmg(Attack);
        }

        public void SpinAttack(IPerson target)
        {
            target.ReceiveDmg((Attack - 2) * 3);
        }

        public void DoubleSlash(IPerson target)
        {
            target.ReceiveDmg(Attack * 2);
        }

        public void HeroTurn(int choice, IPerson target)
        {
            switch (choice)
            {
                case 1:
                    NormAttack(target);
                    Console.WriteLine($"Hero {Name} hit {target.Name} dealing {Attack} damage!");
                    break;
                case 2:
                    Heal(); 
                    break;
                case 4:
                    SpinAttack(target);
                    Console.WriteLine($"Hero {Name} hit {target.Name} with spin attack  dealing {(Attack - 2) * 3} damage!");
                    break;
                case 5:
                    DoubleSlash(target);
                    Console.WriteLine($"Hero {Name} hit {target.Name} with double slash  dealing {Attack * 2} damage!");
                    break;
            }
        }
    }
}
