using MongoDB.Driver;

namespace RPG_GAME.Infrastructure.Database
{
    public interface IMongoSessionFactory
    {
        Task<IClientSessionHandle> CreateAsync();
    }
}
