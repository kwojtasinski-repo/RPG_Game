using RPG_GAME.Application.DTO.Battles;
using RPG_GAME.Application.Exceptions.Auth;
using RPG_GAME.Application.Exceptions.Battles;
using RPG_GAME.Application.Mappings;
using RPG_GAME.Application.Time;
using RPG_GAME.Core.Entities.Battles;
using RPG_GAME.Core.Repositories;

namespace RPG_GAME.Application.Commands.Battles.Handlers
{
    internal sealed class StartBattleHandler : ICommandHandler<StartBattle, BattleDetailsDto>
    {
        private readonly IUserRepository _userRepository;
        private readonly IBattleRepository _battleRepository;
        private readonly IPlayerRepository _playerRepository;
        private readonly IClock _clock;

        public StartBattleHandler(IUserRepository userRepository, IBattleRepository battleRepository,
            IPlayerRepository playerRepository, IClock clock)
        {
            _userRepository = userRepository;
            _battleRepository = battleRepository;
            _playerRepository = playerRepository;
            _clock = clock;
        }

        public async Task<BattleDetailsDto> HandleAsync(StartBattle command)
        {
            var userExitsts = await _userRepository.ExistsAsync(command.UserId);

            if (!userExitsts)
            {
                throw new UserNotFoundException(command.UserId);
            }

            var battle = await _battleRepository.GetAsync(command.BattleId);

            if (battle is null)
            {
                throw new BattleNotFoundException(command.BattleId);
            }

            if (battle.UserId != command.UserId)
            {
                throw new CannotStartBattleForUserException(command.BattleId, command.UserId);
            }

            var player = await _playerRepository.GetByUserId(command.UserId);

            if (player is null)
            {
                throw new PlayerForUserNotFoundException(command.UserId);
            }

            var battleState = BattleState.InAction(command.BattleId, player, _clock.CurrentDate());
            battle.AddBattleStateAtInProgress(battleState);

            return battle.AsDetailsDto();
        }
    }
}
