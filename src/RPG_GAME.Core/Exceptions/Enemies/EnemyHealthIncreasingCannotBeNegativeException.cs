namespace RPG_GAME.Core.Exceptions.Enemies
{
    internal sealed class EnemyHealthIncreasingCannotBeNegativeException : DomainException
    {
        public int IncreasingState { get; }

        public EnemyHealthIncreasingCannotBeNegativeException(int increasingState) : base($"Enemy experience IncreasingState: '{increasingState}' cannot be negative")
        {
            IncreasingState = increasingState;
        }
    }
}
