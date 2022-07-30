using System;
using System.Collections.Generic;

namespace RPG_GAME.Core.NewEntities
{
    public class Map
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Difficulty Difficulty { get; set; }
        public IList<RequiredEnemy> Enemies { get; set; }
    }
}
