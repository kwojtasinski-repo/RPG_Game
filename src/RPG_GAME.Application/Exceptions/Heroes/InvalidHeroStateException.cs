namespace RPG_GAME.Application.Exceptions.Heroes
{
    internal sealed class InvalidHeroStateException : BusinessException
    {
        public InvalidHeroStateException() : base("Invalid hero state")
        {
        }
    }
}
