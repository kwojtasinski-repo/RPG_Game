using RPG_GAME.Application.Queries;

namespace RPG_GAME.Infrastructure.Queries
{
    public interface IQueryDispatcher
    {
        Task<TResult> QueryAsync<TResult>(IQuery<TResult> query);
    }
}
