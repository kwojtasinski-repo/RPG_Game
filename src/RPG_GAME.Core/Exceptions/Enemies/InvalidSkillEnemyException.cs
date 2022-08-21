using System;

namespace RPG_GAME.Core.Exceptions.Enemies
{
    internal sealed class InvalidSkillEnemyException : DomainException
    {
        public InvalidSkillEnemyException() : base("Invalid SkillEnemy")
        {
        }

        public InvalidSkillEnemyException(Guid id) : base($"Invalid SkillEnemy with: id: '{id}'")
        {
        }
    }
}
