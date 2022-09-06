using RPG_GAME.Core.Exceptions.Common;
using System;

namespace RPG_GAME.Core.ValueObjects
{
    public sealed class EnemyId
    {
        public Guid Value { get; }

        public EnemyId(string value)
        {
            var parsed = Guid.TryParse(value, out var guidParsed);

            if (!parsed)
            {
                throw new InvalidEnemyIdException(value);
            }

            Value = guidParsed;
        }

        public EnemyId(Guid value)
        {
            if (value == Guid.Empty)
            {
                throw new InvalidEnemyIdException(value);
            }

            Value = value;
        }

        public static EnemyId Create() => new(Guid.NewGuid());

        public static implicit operator Guid(EnemyId date)
            => date.Value;

        public static implicit operator EnemyId(Guid value)
            => new(value);

        public static implicit operator EnemyId(string value)
            => new(value);
    }
}
