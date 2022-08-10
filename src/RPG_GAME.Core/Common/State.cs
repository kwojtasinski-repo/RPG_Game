using RPG_GAME.Core.Exceptions.Common;

namespace RPG_GAME.Core.Entities.Common
{
    public class State<T>
        where T : struct
    {
        public T Value { get; private set; }
        public IncreasingState<T> IncreasingState { get; private set; }

        public State(T value, IncreasingState<T> increasingState)
        {
            ChangeValue(value);
            ChangeIncreasingState(increasingState);
        }

        public void ChangeValue(T value)
        {
            ValidateValue(value);
            Value = value;
        }

        public void ChangeIncreasingState(IncreasingState<T> increasingState)
        {
            if (increasingState is null)
            {
                throw new InvalidIncreasingStateException();
            }

            IncreasingState = increasingState;
        }

        private void ValidateValue(object value)
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
