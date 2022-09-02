namespace RPG_GAME.Core.Exceptions.Battles
{
    internal sealed class InvalidModifiedDateException : DomainException
    {
        public InvalidModifiedDateException() : base("Invalid ModifiedDate")
        {
        }
    }
}
