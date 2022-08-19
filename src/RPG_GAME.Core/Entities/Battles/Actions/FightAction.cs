using RPG_GAME.Core.Entities.Common;
using RPG_GAME.Core.Exceptions.Battles;
using System;

namespace RPG_GAME.Core.Entities.Battles.Actions
{
    public class FightAction
    {
        public Guid CharacterId { get; }
        public CharacterType Character { get; private set; }
        public string Name { get; }
        public int DamageDealt { get; }
        public int Health { get; }
        public string AttackInfo { get; }

        public FightAction(Guid characterId, string character, string name, int damageDealt, int health, string attackInfo)
        {
            CharacterId = characterId;
            Name = name;
            DamageDealt = damageDealt;
            Health = health;
            AttackInfo = attackInfo;
            ChangeCharacter(character);
        }

        private void ChangeCharacter(string character)
        {
            var parsed = Enum.TryParse<CharacterType>(character, out var characterParsed);

            if (!parsed)
            {
                throw new InvalidCharacterTypeException(character);
            }

            Character = characterParsed;
        }
    }
}
