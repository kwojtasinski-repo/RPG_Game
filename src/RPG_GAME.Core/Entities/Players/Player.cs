using RPG_GAME.Core.Exceptions.Players;
using System;

namespace RPG_GAME.Core.Entities.Players
{
    public class Player
    {
        public Guid Id { get; }
        public string Name { get; private set; }
        public HeroAssign Hero { get; private set; }
        public int Level { get; private set; }
        public decimal CurrentExp { get; private set; }
        public decimal RequiredExp { get; private set; }
        public Guid UserId { get; }

        public Player(Guid id, string name, HeroAssign heroAssign, int level, decimal currentExp, decimal requiredExp, Guid userId)
        {
            Id = id;
            ChangeName(name);
            Hero = heroAssign;

            if (level is <= 0)
            {
                throw new PlayerLevelCannotZeroOrNegativeException(level);
            }

            Level = level;
            ChangeRequiredExp(requiredExp);
            IncreaseCurrentExpBy(currentExp);
            UserId = userId;
        }

        public static Player Create(string name, HeroAssign hero, decimal requiredExp, Guid userId)
        {
            return new Player(
                Guid.NewGuid(),
                name,
                hero,
                1,
                decimal.Zero,
                requiredExp,
                userId
            );
        }

        public void ChangeName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new InvalidPlayerNameException();
            }

            if (name.Length is < 3)
            {
                throw new TooShortPlayerNameException(name);
            }

            Name = name;
        }

        public decimal IncreaseCurrentExpBy(decimal currentExp)
        {
            if (currentExp is < 0)
            {
                throw new PlayerCurrentExpCannotBeNegativeException(currentExp);
            }

            var calculatedCurrentExp = CurrentExp + currentExp;

            if (calculatedCurrentExp >= RequiredExp)
            {
                Level++;
                CurrentExp = calculatedCurrentExp - RequiredExp;
                return currentExp - RequiredExp;
            }

            CurrentExp = calculatedCurrentExp;
            return decimal.Zero;
        }

        public void ChangeCurrentExp(decimal currentExp)
        {
            if (currentExp is < 0)
            {
                throw new PlayerCurrentExpCannotBeNegativeException(currentExp);
            }

            CurrentExp = currentExp;
        }

        public void ChangeRequiredExp(decimal requiredExp)
        {
            if (requiredExp is <= 0)
            {
                throw new PlayerRequiredExpCannotBeZeroOrNegativeException(requiredExp);
            }

            RequiredExp = requiredExp;
        }
    }
}
