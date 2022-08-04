namespace RPG_GAME.Core.Exceptions.Heroes
{
    internal sealed class InvalidSkillHeroAttackException : DomainException
    {
        public InvalidSkillHeroAttackException() : base("Invalid SkillHeroAttack")
        {
        }
    }
}
