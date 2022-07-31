namespace RPG_GAME.Core.Entities
{
    public class Field<T>
        where T : struct
    {
        public string Name { get; set; }
        public T Value { get; set; }
        public IncreasingStats<T> IncreasingStats { get; set; }
    }
}
