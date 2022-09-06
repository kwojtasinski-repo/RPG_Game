using RPG_GAME.Core.Exceptions.Common;
using System;

namespace RPG_GAME.Core.ValueObjects
{
    public sealed class PlayerId
    {
        public Guid Value { get; }

        public PlayerId(string value)
        {
            var parsed = Guid.TryParse(value, out var guidParsed);

            if (!parsed)
            {
                throw new InvalidPlayerIdException(value);
            }

            Value = guidParsed;
        }

        public PlayerId(Guid value)
        {
            if (value == Guid.Empty)
            {
                throw new InvalidPlayerIdException(value);
            }

            Value = value;
        }

        public static PlayerId Create() => new(Guid.NewGuid());

        public static implicit operator Guid(PlayerId date)
            => date.Value;

        public static implicit operator PlayerId(Guid value)
            => new(value);

        public static implicit operator PlayerId(string value)
            => new(value);
    }
}
