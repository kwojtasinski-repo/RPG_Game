using System;
using System.Collections.Generic;
using System.Text;

namespace RPG_GAME
{
    public abstract class Person
    {
        private string _name;
        private int _attack;
        private int _health;
        private int _maxHealth;
        public int MaxHealth { get { return _maxHealth; } }

        public string Name{ get
            {
                return _name;
            } }

        public int Attack { get { return _attack; } }
        public int Health { get { return _health; } }

        public void ChangeName(string name)
        {
            _name = name;
        }

        public void UpgradeAttack(int attack)
        {
            _attack += attack;
        }

        public void UpgradeHealth(int health)
        {
            _health += health;
        }

        public void UpgradeMaxHealth(int health)
        {
            _maxHealth += health;
        }

        public void ReceiveDmg(int dmg)
        {
            _health -= dmg;
        }

        public void SetHealth(int health) => _health = health;

        public Person(string _name, int _attack, int _health)
        {
            this._name = _name;
            this._attack = _attack;
            this._health = _health;
            _maxHealth = _health;
        }

        public void PrintStats()
        {
            Console.WriteLine($"{Name} stats:");
            Console.WriteLine("");
            Console.WriteLine($"Attack value is: {Attack}");
            Console.WriteLine($"Health value is: {Health}");
        }

        public void NormAttack(Person target)
        {
            target._health -= _attack;
        }

        public abstract void Heal();

        public virtual void Reset() 
        {
            Console.WriteLine($"The {Name} has respawned again");
            SetHealth(MaxHealth);
        }
    }
}
