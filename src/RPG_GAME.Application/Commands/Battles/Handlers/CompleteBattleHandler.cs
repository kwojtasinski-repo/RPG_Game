using RPG_GAME.Application.DTO.Battles;
using RPG_GAME.Application.Exceptions.Auth;
using RPG_GAME.Application.Exceptions.Battles;
using RPG_GAME.Application.Managers;
using RPG_GAME.Application.Mappings;
using RPG_GAME.Core.Repositories;

namespace RPG_GAME.Application.Commands.Battles.Handlers
{
    internal sealed class CompleteBattleHandler : ICommandHandler<CompleteBattle, BattleDetailsDto>
    {
        private readonly IUserRepository _userRepository;
        private readonly IBattleRepository _battleRepository;
        private readonly IBattleManager _battleManager;
        private readonly IPlayerRepository _playerRepository;

        public CompleteBattleHandler(IUserRepository userRepository, IBattleRepository battleRepository,
            IBattleManager battleManager, IPlayerRepository playerRepository)
        {
            _userRepository = userRepository;
            _battleRepository = battleRepository;
            _battleManager = battleManager;
            _playerRepository = playerRepository;
        }

        public async Task<BattleDetailsDto> HandleAsync(CompleteBattle command)
        {
            var user = await _userRepository.GetAsync(command.UserId);

            if (user is null)
            {
                throw new UserNotFoundException(command.UserId);
            }

            var battle = await _battleRepository.GetAsync(command.BattleId);

            if (battle is null)
            {
                throw new BattleNotFoundException(command.BattleId);
            }

            if (battle.UserId != command.UserId && user.Role.ToLowerInvariant() != "admin")
            {
                throw new CannotCompleteBattleForUserException(command.BattleId, command.UserId);
            }

            var battleStateInAction = battle.GetBattleStateInAction();
            var playerToUpdate = await _battleManager.CompleteBattle(battle, battleStateInAction.Player);
            await _battleRepository.UpdateAsync(battle);
            await _playerRepository.UpdateAsync(playerToUpdate);
            
            return battle.AsDetailsDto();
        }
    }
}
