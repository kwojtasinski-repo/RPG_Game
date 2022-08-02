﻿using MongoDB.Bson.Serialization;
using RPG_GAME.Infrastructure.Mappings;
using RPG_GAME.Core.Entity;

namespace RPG_GAME.Infrastructure.Configurations
{
    internal class HeroConfiguration : IEntityMapConfiguration<Hero>
    {
        public void Map(BsonClassMap<Hero> bsonClassMap)
        {
            bsonClassMap.AutoMap();
            bsonClassMap.SetIdMember(bsonClassMap.GetMemberMap(c => c.Id));
        }
    }
}
