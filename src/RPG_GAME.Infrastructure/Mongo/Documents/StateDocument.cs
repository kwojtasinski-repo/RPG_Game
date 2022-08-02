namespace RPG_GAME.Infrastructure.Mongo.Documents
{
    internal class StateDocument<T>
        where T : struct
    {
        public T Value { get; set; }
        public IncreasingStateDocument<T> IncreasingState { get; set; }
    }
}
