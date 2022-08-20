namespace RPG_GAME.Application.Exceptions.Heroes
{
    public sealed class InvalidHeroStateException : BusinessException
    {
        public InvalidHeroStateException() : base("Invalid hero state")
        {
        }
    }
}
