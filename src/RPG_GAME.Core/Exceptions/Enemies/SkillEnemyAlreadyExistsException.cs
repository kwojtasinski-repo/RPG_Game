using System;

namespace RPG_GAME.Core.Exceptions.Enemies
{
    internal sealed class SkillEnemyAlreadyExistsException : DomainException
    {
        public Guid Id { get; }
        public string Name { get; }

        public SkillEnemyAlreadyExistsException(Guid id, string name) : base($"SkilEnemy with id: '{id}' and name: '{name}' already exists")
        {
            Id = id;
            Name = name;
        }
    }
}
