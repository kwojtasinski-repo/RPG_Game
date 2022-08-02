using RPG_GAME.Core.Entities.Common;
using System;
using System.Collections.Generic;

namespace RPG_GAME.Core.Entities.Maps
{
    public class Map
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Difficulty Difficulty { get; set; }
        public IList<Enemies> Enemies { get; set; }
    }
}
