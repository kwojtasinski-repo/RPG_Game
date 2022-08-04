namespace RPG_GAME.Core.Exceptions.Heroes
{
    internal sealed class InvalidSkillHeroNameException : DomainException
    {
        public InvalidSkillHeroNameException() : base("Invalid SkillName")
        {
        }
    }
}
