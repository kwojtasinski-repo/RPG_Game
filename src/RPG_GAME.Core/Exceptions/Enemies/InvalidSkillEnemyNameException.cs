namespace RPG_GAME.Core.Exceptions.Enemies
{
    internal sealed class InvalidSkillEnemyNameException : DomainException
    {
        public InvalidSkillEnemyNameException() : base("Invalid SkillEnemyName")
        {
        }
    }
}
