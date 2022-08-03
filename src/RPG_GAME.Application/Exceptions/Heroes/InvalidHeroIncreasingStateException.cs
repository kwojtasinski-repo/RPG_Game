namespace RPG_GAME.Application.Exceptions.Heroes
{
    internal sealed class InvalidHeroIncreasingStateException : BusinessException
    {
        public InvalidHeroIncreasingStateException() : base("Invalid hero increasing state")
        {
        }
    }
}
