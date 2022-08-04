namespace RPG_GAME.Core.Exceptions.Enemies
{
    internal class InvalidSkillEnemyAttackException : DomainException
    {
        public InvalidSkillEnemyAttackException() : base("Invalid SkillEnemyAttack")
        {
        }
    }
}
