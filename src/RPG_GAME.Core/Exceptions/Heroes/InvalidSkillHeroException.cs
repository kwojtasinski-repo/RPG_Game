namespace RPG_GAME.Core.Exceptions.Heroes
{
    internal sealed class InvalidSkillHeroException : DomainException
    {
        public InvalidSkillHeroException() : base($"Invalid SkillHero")
        {
        }
    }
}
