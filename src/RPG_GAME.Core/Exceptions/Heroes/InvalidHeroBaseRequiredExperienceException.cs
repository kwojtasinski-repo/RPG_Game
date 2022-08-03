namespace RPG_GAME.Core.Exceptions.Heroes
{
    internal sealed class InvalidHeroBaseRequiredExperienceException : DomainException
    {
        public InvalidHeroBaseRequiredExperienceException() : base("Invalid hero BaseRequiredExperience")
        {
        }
    }
}
