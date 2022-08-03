using System;

namespace RPG_GAME.Core.Exceptions.Heroes
{
    internal sealed class SkillHeroAlreadyExistsException : DomainException
    {
        public Guid Id { get; }
        public string Name { get; }

        public SkillHeroAlreadyExistsException(Guid id, string name) : base($"SkillHero with id: '{id}' and name: '{name}' already exists")
        {
            Id = id;
            Name = name;
        }
    }
}
