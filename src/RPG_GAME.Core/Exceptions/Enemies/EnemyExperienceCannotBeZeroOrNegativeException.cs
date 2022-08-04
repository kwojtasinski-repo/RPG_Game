namespace RPG_GAME.Core.Exceptions.Enemies
{
    internal sealed class EnemyExperienceCannotBeZeroOrNegativeException : DomainException
    {
        public decimal Experience { get; }

        public EnemyExperienceCannotBeZeroOrNegativeException(decimal experience) : base($"Enemy Experience: '{experience}' cannot be zero or negative")
        {
            Experience = experience;
        }
    }
}
