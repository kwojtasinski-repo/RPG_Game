namespace RPG_GAME.Core.Exceptions.Enemies
{
    internal sealed class InvalidEnemyExperienceException : DomainException
    {
        public InvalidEnemyExperienceException() : base("Invalid Enemy Experience")
        {
        }
    }
}
