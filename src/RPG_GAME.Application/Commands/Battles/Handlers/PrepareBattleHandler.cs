using RPG_GAME.Application.DTO.Battles;
using RPG_GAME.Application.Exceptions.Auth;
using RPG_GAME.Application.Exceptions.Battles;
using RPG_GAME.Application.Exceptions.Enemies;
using RPG_GAME.Application.Exceptions.Maps;
using RPG_GAME.Application.Managers;
using RPG_GAME.Application.Mappings;
using RPG_GAME.Application.Time;
using RPG_GAME.Core.Entities.Battles;
using RPG_GAME.Core.Entities.Maps;
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
        private readonly IEnemyIncreaseStatsManager _enemyIncreaseStatsManager;
        private readonly IEnemyRepository _enemyRepository;

        public PrepareBattleHandler(IUserRepository userRepository, IMapRepository mapRepository, IPlayerRepository playerRepository, IClock clock,
            IBattleRepository battleRepository, IEnemyIncreaseStatsManager enemyIncreaseStatsManager,
            IEnemyRepository enemyRepository)
        {
            _userRepository = userRepository;
            _mapRepository = mapRepository;
            _playerRepository = playerRepository;
            _clock = clock;
            _battleRepository = battleRepository;
            _enemyIncreaseStatsManager = enemyIncreaseStatsManager;
            _enemyRepository = enemyRepository;
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

            if (map is null)
            {
                throw new MapNotFoundException(command.MapId);
            }

            await CalculateMapEnemies(map, player.Level);
            var battle = Battle.Create(_clock.CurrentDate(), command.UserId, map);
            var battleState = BattleState.Prepare(battle.Id, player, _clock.CurrentDate());
            battle.AddBattleStateAtPrepare(battleState);
            await _battleRepository.AddAsync(battle);

            return battle.AsDetailsDto();
        }

        private async Task CalculateMapEnemies(Map map, int level)
        {
            if (!map.Enemies.Any())
            {
                throw new CannotPrepareBattleForMapWithEmptyEnemiesException(map.Id);
            }

            foreach(var enemies in map.Enemies)
            {
                // TODO change getting data to get all for map not 1 by 1 from db
                var enemy = await _enemyRepository.GetAsync(enemies.Enemy.Id);

                if (enemy is null)
                {
                    throw new EnemyNotFoundException(enemies.Enemy.Id);
                }

                _enemyIncreaseStatsManager.IncreaseEnemyStats(level, enemies.Enemy, enemy);
            }
        }
    }
}
