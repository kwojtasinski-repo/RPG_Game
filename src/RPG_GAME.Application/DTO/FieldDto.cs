namespace RPG_GAME.Application.DTO
{
    public class FieldDto<T>
        where T : struct
    {
        public T Value { get; set; }
        public IncreasingStatsDto<T> IncreasingStats { get; set; }
    }
}
