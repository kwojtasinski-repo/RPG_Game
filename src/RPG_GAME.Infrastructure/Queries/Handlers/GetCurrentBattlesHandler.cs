using RPG_GAME.Application.DTO.Battles;
using RPG_GAME.Application.Mappings;
using RPG_GAME.Application.Queries.Battles;
using RPG_GAME.Core.Entities.Battles;
using RPG_GAME.Core.Repositories;

namespace RPG_GAME.Infrastructure.Queries.Handlers
{
    internal sealed class GetCurrentBattlesHandler : IQueryHandler<GetCurrentBattles, IEnumerable<BattleDto>>
    {
        private readonly IBattleRepository _battleRepository;

        public GetCurrentBattlesHandler(IBattleRepository battleRepository)
        {
            _battleRepository = battleRepository;
        }

        public async Task<IEnumerable<BattleDto>> HandleAsync(GetCurrentBattles query)
        {
            var battles = await _battleRepository.GetByUserIdAsync(query.UserId, BattleInfo.InProgress);
            return battles.Select(b => b.AsDto());
        }
    }
}
