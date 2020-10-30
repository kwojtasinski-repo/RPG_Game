using System;
using System.Collections.Generic;
using System.Text;

namespace RPG_GAME
{
    public class Enemy : Person
    {
        private int _healLvl = 0;
        private string _category;
        public string Category { get { return _category; } }
        public Enemy(string _name, int _attack, int _health, string _category) : base(_name, _attack, _health)
        {
            this._category = _category;
        }

        public void SetHeal(int heal) => _healLvl = heal;

        public override void Heal()
        {
            if (Health < MaxHealth)
            {
                this.UpgradeHealth(_healLvl);
            }
            if (_healLvl > 0)
                Console.WriteLine($"{Name} healed by {_healLvl}");
        }

        public void UpgradeHeal(int heal)
        {
            _healLvl += heal;
        }

        public void SetCategory(string category) => _category = category;

        public void ShowHeroHealth(Hero hero) => Console.WriteLine($"Hero Health {hero.Health}/{hero.MaxHealth}");

        public virtual void EnemyTurn(int choice, Hero target) 
        {
            Console.WriteLine($"{target.Name} vs {Name}");
        }
    }
}
