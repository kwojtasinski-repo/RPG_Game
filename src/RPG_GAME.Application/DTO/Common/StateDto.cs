namespace RPG_GAME.Application.DTO.Common
{
    public class StateDto<T>
        where T : struct
    {
        public T Value { get; set; }
        public IncreasingStateDto<T> IncreasingState { get; set; }
    }
}
