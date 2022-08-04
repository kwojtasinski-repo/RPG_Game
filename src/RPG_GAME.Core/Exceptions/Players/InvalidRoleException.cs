namespace RPG_GAME.Core.Exceptions.Players
{
    internal sealed class InvalidRoleException : DomainException
    {
        public InvalidRoleException() : base("Invalid Role")
        {
        }
    }
}
