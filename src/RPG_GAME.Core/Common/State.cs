namespace RPG_GAME.Core.Entities.Common
{
    public class State<T>
        where T : struct
    {
        public T Value { get; set; }
        public IncreasingState<T> IncreasingStats { get; set; }
    }
}
