using RPG_GAME.Application.DTO.Battles;
using RPG_GAME.Application.Exceptions.Auth;
using RPG_GAME.Application.Exceptions.Battles;
using RPG_GAME.Application.Mappings;
using RPG_GAME.Application.Time;
using RPG_GAME.Core.Entities.Battles;
using RPG_GAME.Core.Repositories;

namespace RPG_GAME.Application.Commands.Battles.Handlers
{
    internal sealed class PrepareBattleHandler : ICommandHandler<PrepareBattle, BattleDetailsDto>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapRepository _mapRepository;
        private readonly IPlayerRepository _playerRepository;
        private readonly IClock _clock;
        private readonly IBattleRepository _battleRepository;

        public PrepareBattleHandler(IUserRepository userRepository, IMapRepository mapRepository, IPlayerRepository playerRepository, IClock clock,
            IBattleRepository battleRepository)
        {
            _userRepository = userRepository;
            _mapRepository = mapRepository;
            _playerRepository = playerRepository;
            _clock = clock;
            _battleRepository = battleRepository;
        }

        public async Task<BattleDetailsDto> HandleAsync(PrepareBattle command)
        {
            var userExists = await _userRepository.ExistsAsync(command.UserId);

            if (!userExists)
            {
                throw new UserNotFoundException(command.UserId);
            }

            var player = await _playerRepository.GetByUserId(command.UserId);

            if (player is null)
            {
                throw new PlayerForUserNotFoundException(command.UserId);
            }

            var map = await _mapRepository.GetAsync(command.MapId);
            //TODO adapt map to existing lvl hero and store somewhere for example in battle
            var battle = Battle.Create(_clock.CurrentDate(), command.UserId, map);
            var battleState = BattleState.Prepare(battle.Id, player, _clock.CurrentDate());
            battle.AddBattleStateAtPrepare(battleState);
            await _battleRepository.AddAsync(battle);

            return battle.AsDetailsDto();
        }
    }
}
