using System;

namespace RPG_GAME.Core.Entities.Battles.Actions
{
    public class FightAction
    {
        public Guid Id { get; }
        public string Name { get; }
        public int DamageDealt { get; }
        public int Health { get; }
        public string AttackInfo { get; }

        public FightAction(Guid id, string name, int damageDealt, int health, string attackInfo)
        {
            Id = id;
            Name = name;
            DamageDealt = damageDealt;
            Health = health;
            AttackInfo = attackInfo;
        }
    }
}
