using RPG_GAME.Core.Exceptions.Common;

namespace RPG_GAME.Core.Entities.Common
{
    public class IncreasingState<T>
        where T : struct
    {
        public StrategyIncreasing StrategyIncreasing { get; private set; }
        public T Value { get; private set; }

        public IncreasingState(T value, StrategyIncreasing strategyIncreasing)
        {
            ChangeValue(value);
            ChangeStrategyIncreasing(strategyIncreasing);
        } 

        public void ChangeStrategyIncreasing(StrategyIncreasing strategyIncreasing)
        {
            StrategyIncreasing = strategyIncreasing;
        }

        public void ChangeValue(T value)
        {
            ValidateValue(value);
            Value = value;
        }

        public void ValidateValue(object value)
        {
            if (typeof(T) == typeof(int) || typeof(T) == typeof(decimal))
            {
                if((decimal) value < 0)
                {
                    throw new InvalidIncreasingStateValueException();
                }
            }
        }
    }
}
