namespace RPG_GAME.Core.Exceptions.Enemies
{
    internal sealed class EnemyAttackIncreasingCannotBeNegativeException : DomainException
    {
        public int IncreasingState { get; }

        public EnemyAttackIncreasingCannotBeNegativeException(int increasingState) : base($"Enemy Attack IncreasingState: '{increasingState}' cannot be negative")
        {
            IncreasingState = increasingState;
        }
    }
}
