using MongoDB.Driver;

namespace RPG_Game.Infrastructure.Database
{
    public interface IMongoSessionFactory
    {
        Task<IClientSessionHandle> CreateAsync();
    }
}
