using System;

namespace RPG_GAME.Core.Entities.Battles.Actions
{
    public class FightAction
    {
        public Guid CharacterId { get; }
        public string Name { get; }
        public int DamageDealt { get; }
        public int Health { get; }
        public string AttackInfo { get; }

        public FightAction(Guid characterId, string name, int damageDealt, int health, string attackInfo)
        {
            CharacterId = characterId;
            Name = name;
            DamageDealt = damageDealt;
            Health = health;
            AttackInfo = attackInfo;
        }
    }
}
