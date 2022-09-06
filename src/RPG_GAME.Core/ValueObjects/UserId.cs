using RPG_GAME.Core.Exceptions.Common;
using System;

namespace RPG_GAME.Core.ValueObjects
{
    public sealed class UserId
    {
        public Guid Value { get; }

        public UserId(string value)
        {
            var parsed = Guid.TryParse(value, out var guidParsed);

            if (!parsed)
            {
                throw new InvalidUserIdException(value);
            }

            Value = guidParsed;
        }

        public UserId(Guid value)
        {
            if (value == Guid.Empty)
            {
                throw new InvalidUserIdException(value);
            }

            Value = value;
        }

        public static UserId Create() => new(Guid.NewGuid());

        public static implicit operator Guid(UserId date)
            => date.Value;

        public static implicit operator UserId(Guid value)
            => new(value);

        public static implicit operator UserId(string value)
            => new(value);
    }
}
