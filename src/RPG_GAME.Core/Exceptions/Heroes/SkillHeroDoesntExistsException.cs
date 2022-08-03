using System;

namespace RPG_GAME.Core.Exceptions.Heroes
{
    internal sealed class SkillHeroDoesntExistsException : DomainException
    {
        public Guid Id { get; }
        public string Name { get; }

        public SkillHeroDoesntExistsException(Guid id, string name) : base($"SkillHero with id: '{id}' and name: '{name}' doesnt exists")
        {
            Id = id;
            Name = name;
        }
    }
}
