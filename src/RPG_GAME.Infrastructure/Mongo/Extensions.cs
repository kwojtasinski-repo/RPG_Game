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
        public static UserDocument AsDocument(this User user)
        {
            return new UserDocument
            {
                Id = user.Id,
                Email = user.Email,
                Password = user.Password,
                Role = user.Role,
                CreatedAt = user.CreatedAt,
                IsActive = user.IsActive
            };
        }

        public static User AsEntity(this UserDocument userDocument)
        {
            return new User
            {
                Id = userDocument.Id,
                Email = userDocument.Email,
                Password = userDocument.Password,
                Role = userDocument.Role,
                CreatedAt = userDocument.CreatedAt,
                IsActive = userDocument.IsActive
            };
        }

        public static IServiceCollection AddMongo(this IServiceCollection services)
        {
            RegisterConventions();
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddMongoRepository<UserDocument, Guid>("users");
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
