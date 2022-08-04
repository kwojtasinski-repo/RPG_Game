namespace RPG_GAME.Core.Exceptions.Enemies
{
    internal sealed class EnemyExperienceCannotBeNegativeException : DomainException
    {
        public decimal Experience { get; }

        public EnemyExperienceCannotBeNegativeException(decimal experience) : base($"Enemy Experience: '{experience}' cannot be negative")
        {
            Experience = experience;
        }
    }
}
