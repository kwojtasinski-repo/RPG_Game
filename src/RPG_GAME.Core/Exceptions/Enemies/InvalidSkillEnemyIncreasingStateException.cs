namespace RPG_GAME.Core.Exceptions.Enemies
{
    internal sealed class InvalidSkillEnemyIncreasingStateException : DomainException
    {
        public InvalidSkillEnemyIncreasingStateException() : base("Invalid SkillEnemy Increasing State")
        {
        }
    }
}
