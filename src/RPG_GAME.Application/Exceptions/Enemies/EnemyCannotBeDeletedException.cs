﻿namespace RPG_GAME.Application.Exceptions.Enemies
{
    public sealed class EnemyCannotBeDeletedException : BusinessException
    {
        public Guid Id { get; }

        public EnemyCannotBeDeletedException(Guid id) : base($"Enemy with id '{id}' cannot be deleted")
        {
            Id = id;
        }
    }
}
