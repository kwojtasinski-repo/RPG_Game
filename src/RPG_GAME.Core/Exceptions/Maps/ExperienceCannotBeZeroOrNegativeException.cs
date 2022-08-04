namespace RPG_GAME.Core.Exceptions.Maps
{
    internal sealed class ExperienceCannotBeZeroOrNegativeException : DomainException
    {
        public decimal Experience { get; }

        public ExperienceCannotBeZeroOrNegativeException(decimal experience) : base($"Experience '{experience}' cannot be zero or negative")
        {
            Experience = experience;
        }
    }
}
