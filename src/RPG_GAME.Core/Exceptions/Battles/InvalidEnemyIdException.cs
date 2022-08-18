namespace RPG_GAME.Core.Exceptions.Battles
{
    internal sealed class InvalidEnemyIdException : DomainException
    {
        public InvalidEnemyIdException() : base("Invalid Enemy Id")
        {
        }
    }
}
