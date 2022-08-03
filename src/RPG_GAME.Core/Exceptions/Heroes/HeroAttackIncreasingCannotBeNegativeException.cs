namespace RPG_GAME.Core.Exceptions.Heroes
{
    internal sealed class HeroAttackIncreasingCannotBeNegativeException : DomainException
    {
        public int IncreasingState { get; }

        public HeroAttackIncreasingCannotBeNegativeException(int increasingState) : base($"Hero attack IncreasingState: '{increasingState}' cannot be negative")
        {
            IncreasingState = increasingState;
        }
    }
}
