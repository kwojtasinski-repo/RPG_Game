using RPG_GAME.Core.Exceptions.Common;
using System;

namespace RPG_GAME.Core.Entities.Common
{
    public class IncreasingState<T>
        where T : struct
    {
        public StrategyIncreasing StrategyIncreasing { get; private set; }
        public T Value { get; private set; }

        public IncreasingState(T value, string strategyIncreasing)
        {
            ChangeValue(value);
            ChangeStrategyIncreasing(strategyIncreasing);
        } 

        public void ChangeStrategyIncreasing(string strategyIncreasing)
        {
            var parsed = Enum.TryParse<StrategyIncreasing>(strategyIncreasing, out var strategyIncreasingParsed);

            if (!parsed)
            {
                throw new InvalidStrategyIncreasingException(strategyIncreasing);
            }

            StrategyIncreasing = strategyIncreasingParsed;
        }

        public void ChangeValue(T value)
        {
            ValidateValue(value);
            Value = value;
        }

        public void ValidateValue(object value)
        {
            if (typeof(T) == typeof(int))
            {
                if ((int)value < 0)
                {
                    throw new InvalidStateValueException();
                }
            }

            if (typeof(T) == typeof(decimal))
            {
                if ((decimal)value < 0)
                {
                    throw new InvalidStateValueException();
                }
            }
        }
    }
}
