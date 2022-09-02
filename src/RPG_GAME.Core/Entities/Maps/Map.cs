using RPG_GAME.Core.Entities.Common;
using RPG_GAME.Core.Exceptions.Maps;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RPG_GAME.Core.Entities.Maps
{
    public class Map
    {
        public Guid Id { get; }
        public string Name { get; private set; }
        public Difficulty Difficulty { get; private set; }
        public IEnumerable<Enemies> Enemies => _enemies;

        private IList<Enemies> _enemies = new List<Enemies>();

        public Map(Guid id, string name, string difficulty, IEnumerable<Enemies> enemies = null)
        {
            Id = id;
            ChangeName(name);
            ChangeDifficulty(difficulty);

            if (enemies is not null)
            {
                foreach(var enemy in enemies)
                {
                    AddEnemies(enemy);
                }
            }
        }

        public static Map Create(string name, string difficulty, IEnumerable<Enemies> enemies = null)
        {
            return new Map(Guid.NewGuid(), name, difficulty, enemies);
        }

        public void ChangeName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new InvalidMapNameException();
            }

            if (name.Length < 3)
            {
                throw new TooShortMapNameException(name);
            }

            Name = name;
        }

        public void ChangeDifficulty(string difficulty)
        {
            var parsed = Enum.TryParse<Difficulty>(difficulty, out var difficultyType);

            if (!parsed)
            {
                throw new InvalidMapDifficultyException(difficulty);
            }

            if (!Enum.IsDefined(difficultyType))
            {
                throw new InvalidMapDifficultyException(difficulty);
            }

            Difficulty = difficultyType;
        }

        public void AddEnemies(Enemies enemies)
        {
            if (enemies is null)
            {
                throw new InvalidEnemiesException();
            }

            var enemiesExists = _enemies.SingleOrDefault(e => e.Enemy.Id == enemies.Enemy.Id);

            if (enemiesExists is null)
            {
                _enemies.Add(enemies);
                return;
            }

            enemiesExists.AddQuantity(enemies.Quantity);
        }

        public void ReplaceEnemies(Enemies enemies)
        {
            if (enemies is null)
            {
                throw new InvalidEnemiesException();
            }

            var enemiesExists = _enemies.SingleOrDefault(e => e.Enemy.Id == enemies.Enemy.Id);

            if (enemiesExists is null)
            {
                _enemies.Add(enemies);
                throw new EnemiesDoesntExistsException(enemies.Enemy.Id);
            }

            enemiesExists.SetQuantity(enemies.Quantity);
        }

        public void RemoveEnemy(Enemies enemies)
        {
            if (enemies is null)
            {
                throw new InvalidEnemiesException();
            }

            var enemiesExists = _enemies.SingleOrDefault(s => s.Enemy.Id == enemies.Enemy.Id);

            if (enemiesExists is null)
            {
                throw new EnemiesDoesntExistsException(enemies.Enemy.Id);
            }

            var quantity = enemiesExists.Quantity - enemies.Quantity;

            if (quantity <= 0)
            {
                _enemies.Remove(enemies);
                return;
            }

            enemiesExists.RemoveQuantity(enemies.Quantity);
        }
    }
}
