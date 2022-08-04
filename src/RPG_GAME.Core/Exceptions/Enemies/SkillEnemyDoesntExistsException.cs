using System;

namespace RPG_GAME.Core.Exceptions.Enemies
{
    internal sealed class SkillEnemyDoesntExistsException : DomainException
    {
        public Guid Id { get; }
        public string Name { get; }

        public SkillEnemyDoesntExistsException(Guid id, string name) : base($"SkillEnemy with id: '{id}' and name: '{name}' already exists")
        {
            Id = id;
            Name = name;
        }
    }
}
