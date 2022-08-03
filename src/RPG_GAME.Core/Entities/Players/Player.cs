using System;

namespace RPG_GAME.Core.Entities.Players
{
    public class Player
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public HeroAssign Hero { get; set; }
        public int Level { get; set; }
        public decimal CurrentExp { get; set; }
        public decimal RequiredExp { get; set; }
        public Guid UserId { get; set; }

        public static Player Create(string name, HeroAssign hero, decimal requiredExp, Guid userId)
        {
            return new Player
            {
                Id = Guid.NewGuid(),
                Name = name,
                Hero = hero,
                CurrentExp = decimal.Zero,
                RequiredExp = requiredExp,
                Level = 1,
                UserId = userId
            };
        }
    }
}
