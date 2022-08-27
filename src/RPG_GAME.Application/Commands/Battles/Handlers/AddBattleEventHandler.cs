using RPG_GAME.Application.DTO.Battles;
using RPG_GAME.Application.Exceptions.Battles;
using RPG_GAME.Application.Managers;
using RPG_GAME.Application.Mappings;
using RPG_GAME.Core.Repositories;

namespace RPG_GAME.Application.Commands.Battles.Handlers
{
    internal sealed class AddBattleEventHandler : ICommandHandler<AddBattleEvent, BattleEventDto>
    {
        private readonly IBattleRepository _battleRepository;
        private readonly IPlayerRepository _playerRepository;
        private readonly IBattleManager _battleManager;
        private readonly IBattleEventRepository _battleEventRepository;

        public AddBattleEventHandler(IBattleRepository battleRepository, IPlayerRepository playerRepository,
            IBattleManager battleManager, IBattleEventRepository battleEventRepository)
        {
            _battleRepository = battleRepository;
            _playerRepository = playerRepository;
            _battleManager = battleManager;
            _battleEventRepository = battleEventRepository;
        }

        public async Task<BattleEventDto> HandleAsync(AddBattleEvent command)
        {
            var battle = await _battleRepository.GetAsync(command.BattleId);

            if (battle is null)
            {
                throw new BattleNotFoundException(command.BattleId);
            }

            if (battle.BattleInfo != Core.Entities.Battles.BattleInfo.InProgress)
            {
                throw new InvalidOperationException("Battle is not in correct state");
            }

            var player = await _playerRepository.GetAsync(command.PlayerId);
            
            if (player is null)
            {
                throw new PlayerForUserNotFoundException(command.PlayerId);
            }

            var battleEvent = await _battleManager.CreateBattleEvent(battle, command.EnemyId, player, command.Action);
            await _battleEventRepository.AddAsync(battleEvent);
            await _battleRepository.UpdateAsync(battle);

            return battleEvent.AsDto();
        }
    }
}
