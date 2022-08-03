namespace RPG_GAME.Core.Exceptions.Heroes
{
    internal sealed class HeroHealthIncreasingCannotBeNegativeException : DomainException
    {
        public int IncreasingState { get; }

        public HeroHealthIncreasingCannotBeNegativeException(int increasingState) : base($"Hero health IncreasingState: '{increasingState}' cannot be negative")
        {
            IncreasingState = increasingState;
        }
    }
}
