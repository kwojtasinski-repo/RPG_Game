using MongoDB.Driver;
using RPG_GAME.Core.Entities;
using System.Linq.Expressions;

namespace RPG_Game.Infrastructure.Repositories
{
	public interface IMongoRepository<TEntity, in TIdentifiable> where TEntity : IIdentifiable<TIdentifiable>
	{
		IMongoCollection<TEntity> Collection { get; }
		Task<TEntity> GetAsync(TIdentifiable id);
		Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> predicate);
		Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate);
		Task AddAsync(TEntity entity);
		Task UpdateAsync(TEntity entity);
		Task DeleteAsync(TIdentifiable id);
		Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate);
	}
}