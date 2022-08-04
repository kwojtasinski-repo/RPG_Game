namespace RPG_GAME.Core.Exceptions.Heroes
{
    internal sealed class InvalidSkillHeroIncreasingStateException : DomainException
    {
        public InvalidSkillHeroIncreasingStateException() : base("Invalid skill hero IncreasingState")
        {
        }
    }
}
