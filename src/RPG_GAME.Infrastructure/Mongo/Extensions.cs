using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using RPG_Game.Infrastructure.Mongo.Documents;
using RPG_Game.Infrastructure.Mongo.Repositories;
using RPG_GAME.Core.Entities;
using RPG_GAME.Core.Repositories;

namespace RPG_Game.Infrastructure.Mongo
{
    internal static class Extensions
    {
        public static IServiceCollection AddMongo(this IServiceCollection services)
        {
            RegisterConventions();
            return services;
        }

        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddMongoRepository<UserDocument, Guid>("users");

            services.AddTransient<IPlayerRepository, PlayerRepository>();
            services.AddMongoRepository<PlayerDocument, Guid>("players");

            services.AddTransient<IEnemyRepository, EnemyRepository>();
            services.AddMongoRepository<EnemyDocument, Guid>("enemies");

            services.AddTransient<IHeroRepository, HeroRepository>();
            services.AddMongoRepository<HeroDocument, Guid>("heroes");

            services.AddTransient<IMapRepository, MapRepository>();
            services.AddMongoRepository<MapDocument, Guid>("maps");

            return services;
        }

        private static void RegisterConventions()
        {
            BsonSerializer.RegisterSerializer(typeof(decimal), new DecimalSerializer(BsonType.Decimal128));
            BsonSerializer.RegisterSerializer(typeof(decimal?),
                new NullableSerializer<decimal>(new DecimalSerializer(BsonType.Decimal128)));
            ConventionRegistry.Register("rpg_game_convention", new MongoDbConventions(), t => true);
        }

        private static IServiceCollection AddMongoRepository<TEntity, TIdentifiable>(this IServiceCollection services,
            string collectionName)
            where TEntity : class, IIdentifiable<TIdentifiable>
        {
            services.AddTransient<IMongoRepository<TEntity, TIdentifiable>>(sp =>
            {
                var database = sp.GetService<IMongoDatabase>();
                return new MongoRepository<TEntity, TIdentifiable>(database, collectionName);
            });

            return services;
        }

        private class MongoDbConventions : IConventionPack
        {
            public IEnumerable<IConvention> Conventions => new List<IConvention>
            {
                new IgnoreExtraElementsConvention(true),
                new EnumRepresentationConvention(BsonType.String),
                new CamelCaseElementNameConvention()
            };
        }
    }
}
