namespace RPG_GAME.Core.Exceptions.Heroes
{
    internal class HeroHealLvlIncreasingCannotBeNegativeException : DomainException
    {
        public int IncreasingState { get; }

        public HeroHealLvlIncreasingCannotBeNegativeException(int increasingState) : base($"Hero healLvl IncreasingState: '{increasingState}' cannot be negative")
        {
            IncreasingState = increasingState;
        }
    }
}
