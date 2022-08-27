using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using RPG_GAME.Infrastructure.Mongo.Documents.Enemies;
using RPG_GAME.Infrastructure.Mongo.Documents.Heroes;
using RPG_GAME.Infrastructure.Mongo.Documents.Maps;
using RPG_GAME.Infrastructure.Mongo.Documents.Players;
using RPG_GAME.Infrastructure.Mongo.Repositories;
using RPG_GAME.Core.Repositories;
using RPG_GAME.Infrastructure.Mongo.Documents;
using Microsoft.AspNetCore.Builder;
using RPG_GAME.Infrastructure.Mongo.Documents.Battles;

namespace RPG_GAME.Infrastructure.Mongo
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

            services.AddTransient<IBattleRepository, BattleRepository>();
            services.AddMongoRepository<BattleDocument, Guid>("battles");
            
            services.AddTransient<IBattleEventRepository, BattleEventRepository>();
            services.AddMongoRepository<BattleEventDocument, Guid>("battle-events");

            services.AddTransient<ICurrentBattleStateRepository, CurrentBattleStateRepository>();
            services.AddMongoRepository<CurrentBattleStateDocument, Guid>("current-battle-states");

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

        public static IApplicationBuilder UseMongo(this IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var userRepo = scope.ServiceProvider.GetService<IMongoRepository<UserDocument, Guid>>();
                var userBuilder = Builders<UserDocument>.IndexKeys;
                Task.Run(async () => await userRepo.Collection.Indexes.CreateOneAsync(
                    new CreateIndexModel<UserDocument>(userBuilder.Ascending(i => i.Email),
                        new CreateIndexOptions
                        {
                            Unique = true
                        })));

                var enemyRepo = scope.ServiceProvider.GetService<IMongoRepository<EnemyDocument, Guid>>();
                var enemyBuilder = Builders<EnemyDocument>.IndexKeys;
                Task.Run(async () => await enemyRepo.Collection.Indexes.CreateOneAsync(
                    new CreateIndexModel<EnemyDocument>(enemyBuilder.Ascending(i => i.EnemyName),
                        new CreateIndexOptions
                        {
                            Unique = true
                        })));

                var heroRepo = scope.ServiceProvider.GetService<IMongoRepository<HeroDocument, Guid>>();
                var heroBuilder = Builders<HeroDocument>.IndexKeys;
                Task.Run(async () => await heroRepo.Collection.Indexes.CreateOneAsync(
                    new CreateIndexModel<HeroDocument>(heroBuilder.Ascending(i => i.HeroName),
                        new CreateIndexOptions
                        {
                            Unique = true
                        })));

                var playerRepo = scope.ServiceProvider.GetService<IMongoRepository<PlayerDocument, Guid>>();
                var playerBuilder = Builders<PlayerDocument>.IndexKeys;
                Task.Run(async () =>
                {
                    await playerRepo.Collection.Indexes.CreateOneAsync(
                        new CreateIndexModel<PlayerDocument>(playerBuilder.Ascending(i => i.Name),
                            new CreateIndexOptions
                            {
                                Unique = true
                            }));

                    await playerRepo.Collection.Indexes.CreateOneAsync(
                        new CreateIndexModel<PlayerDocument>(playerBuilder.Ascending(i => i.UserId)));
                });

                var mapRepo = scope.ServiceProvider.GetService<IMongoRepository<MapDocument, Guid>>();
                var mapBuilder = Builders<MapDocument>.IndexKeys;
                Task.Run(async () => await mapRepo.Collection.Indexes.CreateOneAsync(
                    new CreateIndexModel<MapDocument>(mapBuilder.Ascending(i => i.Name),
                        new CreateIndexOptions
                        {
                            Unique = true
                        })));

                var battleRepo = scope.ServiceProvider.GetService<IMongoRepository<BattleDocument, Guid>>();
                var battleBuilder = Builders<BattleDocument>.IndexKeys;
                Task.Run(async () => await battleRepo.Collection.Indexes.CreateOneAsync(
                    new CreateIndexModel<BattleDocument>(battleBuilder.Ascending(i => i.UserId))));

                var battleEventRepo = scope.ServiceProvider.GetService<IMongoRepository<BattleEventDocument, Guid>>();
                var battleEventBuilder = Builders<BattleEventDocument>.IndexKeys;
                Task.Run(async () => await battleEventRepo.Collection.Indexes.CreateOneAsync(
                    new CreateIndexModel<BattleEventDocument>(battleEventBuilder.Ascending(i => i.BattleId))));
            }

            return app;
        }
    }
}
