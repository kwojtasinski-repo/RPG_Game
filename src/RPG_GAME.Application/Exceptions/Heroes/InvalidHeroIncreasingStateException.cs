namespace RPG_GAME.Application.Exceptions.Heroes
{
    public sealed class InvalidHeroIncreasingStateException : BusinessException
    {
        public InvalidHeroIncreasingStateException() : base("Invalid hero increasing state")
        {
        }
    }
}
