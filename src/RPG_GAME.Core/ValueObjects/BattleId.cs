using RPG_GAME.Core.Exceptions.Common;
using System;

namespace RPG_GAME.Core.ValueObjects
{
    public sealed class BattleId
    {
        public Guid Value { get; }

        public BattleId(string value)
        {
            var parsed = Guid.TryParse(value, out var guidParsed);

            if (!parsed)
            {
                throw new InvalidBattleIdException(value);
            }

            Value = guidParsed;
        }

        public BattleId(Guid value)
        {
            if (value == Guid.Empty)
            {
                throw new InvalidBattleIdException(value);
            }

            Value = value;
        }

        public static BattleId Create() => new(Guid.NewGuid());

        public static implicit operator Guid(BattleId date)
            => date.Value;

        public static implicit operator BattleId(Guid value)
            => new(value);

        public static implicit operator BattleId(string value)
            => new(value);
    }
}
