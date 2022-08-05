namespace RPG_GAME.Core.Exceptions.Players
{
    internal sealed class PlayerCurrentExpCannotBeNegativeException : DomainException
    {
        public decimal CurrentExp { get; }

        public PlayerCurrentExpCannotBeNegativeException(decimal currentExp) : base($"CurrentExp: '{currentExp}' cannot be negative")
        {
            CurrentExp = currentExp;
        }
    }
}
