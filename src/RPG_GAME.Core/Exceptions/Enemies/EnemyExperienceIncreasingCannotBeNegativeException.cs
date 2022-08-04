namespace RPG_GAME.Core.Exceptions.Enemies
{
    internal sealed class EnemyExperienceIncreasingCannotBeNegativeException : DomainException
    {
        public decimal IncreasingState { get; }

        public EnemyExperienceIncreasingCannotBeNegativeException(decimal increasingState) : base($"Enemy experience IncreasingState: '{increasingState}' cannot be negative")
        {
            IncreasingState = increasingState;
        }
    }
}
