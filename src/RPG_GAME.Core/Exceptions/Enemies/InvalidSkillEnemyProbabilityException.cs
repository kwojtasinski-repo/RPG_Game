namespace RPG_GAME.Core.Exceptions.Enemies
{
    internal sealed class InvalidSkillEnemyProbabilityException : DomainException
    {
        public InvalidSkillEnemyProbabilityException() : base("Invalid SkillEnemyProbabilty")
        {
        }
    }
}
