namespace RPG_GAME.Core.Exceptions.Enemies
{
    internal sealed class EnemyHealLvlIncreasingCannotBeNegativeException : DomainException
    {
        public int IncreasingState { get; }

        public EnemyHealLvlIncreasingCannotBeNegativeException(int increasingState) : base($"Enemy healLvl IncreasingState: '{increasingState}' cannot be negative")
        {
            IncreasingState = increasingState;
        }
    }
}
