using RPG_GAME.Core.Exceptions.Common;
using System;

namespace RPG_GAME.Core.ValueObjects
{
    public sealed record MapId
    {
        public Guid Value { get; }

        public MapId(string value)
        {
            var parsed = Guid.TryParse(value, out var guidParsed);

            if (!parsed)
            {
                throw new InvalidMapIdException(value);
            }

            Value = guidParsed;
        }

        public MapId(Guid value)
        {
            if (value == Guid.Empty)
            {
                throw new InvalidMapIdException(value);
            }

            Value = value;
        }

        public static MapId Create() => new(Guid.NewGuid());

        public static implicit operator Guid(MapId date)
            => date.Value;

        public static implicit operator MapId(Guid value)
            => new(value);

        public static implicit operator MapId(string value)
            => new(value);
    }
}
