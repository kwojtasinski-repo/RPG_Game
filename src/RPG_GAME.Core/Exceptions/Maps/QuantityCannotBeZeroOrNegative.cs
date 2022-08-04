namespace RPG_GAME.Core.Exceptions.Maps
{
    internal sealed class QuantityCannotBeZeroOrNegative : DomainException
    {
        public int Quantity { get; }

        public QuantityCannotBeZeroOrNegative(int quantity) : base($"Quantity: '{quantity}' cannot be zero or negative")
        {
            Quantity = quantity;
        }
    }
}
