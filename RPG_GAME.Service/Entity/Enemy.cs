using RPG_GAME.Core.Common;
using System;


namespace RPG_GAME.Core.Entity
{
    public class Enemy : BaseEntity, IPerson
    {

        public int MaxHealth { get; set; }
        public string Name { get; set; }
        public int Health { get; set; }
        public int Attack { get; set; }
        public string Category { get; set; }
        public int HealLvl { get; set; }
        public int DiffLvl { get; set; }

        public Enemy(string name, int attack, int health, string category, int diffLvl, int healLvl = 0)
        {
            Id++;
            Name = name;
            Attack = attack;
            Health = health;
            MaxHealth = health;
            Category = category;
            HealLvl = healLvl;
            DiffLvl = diffLvl;
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
            }
            if (HealLvl > 0)
                Console.WriteLine($"{Name} healed by {HealLvl}");
        }

        public void NormAttack(IPerson target)
        {
            target.ReceiveDmg(Attack);
        }

        public void PrintStats()
        {
            Console.WriteLine($"{Name} stats:");
            Console.WriteLine("");
            Console.WriteLine($"Attack value is: {Attack}");
            Console.WriteLine($"Health value is: {Health}");
        }

        public void ReceiveDmg(int dmg)
        {
            Health -= dmg;
        }

        public void Reset()
        {
            Console.WriteLine($"The {Name} has respawned again");
            SetHealth(MaxHealth);
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

        public virtual void EnemyTurn(int choice, IPerson target)
        {
            Console.WriteLine($"{target.Name} vs {Name}");
        }
    }
}
