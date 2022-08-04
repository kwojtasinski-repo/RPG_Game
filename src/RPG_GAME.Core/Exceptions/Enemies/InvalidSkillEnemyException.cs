namespace RPG_GAME.Core.Exceptions.Enemies
{
    internal sealed class InvalidSkillEnemyException : DomainException
    {
        public InvalidSkillEnemyException() : base("Invalid SkillEnemy")
        {
        }
    }
}
