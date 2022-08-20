using RPG_GAME.Application.DTO.Battles;
using RPG_GAME.Application.Exceptions.Battles;
using RPG_GAME.Application.Mappings;
using RPG_GAME.Application.Queries.Battles;
using RPG_GAME.Core.Repositories;

namespace RPG_GAME.Infrastructure.Queries.Handlers
{
    internal sealed class GetBattleStateHandler : IQueryHandler<GetBattleState, BattleStateDto>
    {
        private readonly IBattleRepository _battleRepository;

        public GetBattleStateHandler(IBattleRepository battleRepository)
        {
            _battleRepository = battleRepository;
        }

        public async Task<BattleStateDto> HandleAsync(GetBattleState query)
        {
            var battle = await _battleRepository.GetAsync(query.BattleId);
            
            if (battle is null)
            {
                throw new BattleNotFoundException(query.BattleId);
            }

            return battle.GetLatestBattleState().AsDto();
        }
    }
}
