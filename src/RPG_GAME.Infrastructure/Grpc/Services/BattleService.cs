using Grpc.Core;
using RPG_GAME.Application.Commands.Battles;
using RPG_GAME.Infrastructure.Commands;
using RPG_GAME.Infrastructure.Grpc.Protos;
using RPG_GAME.Infrastructure.Grpc.Mappings;
using RPG_GAME.Core.ValueObjects;
using Microsoft.AspNetCore.Authorization;
using RPG_GAME.Infrastructure.Queries;
using RPG_GAME.Application.Queries.Battles;

namespace RPG_GAME.Infrastructure.Grpc.Services
{
    internal sealed class BattleService : Battle.BattleBase
    {
        private readonly ICommandDispatcher _commandDispatcher;
        private readonly IQueryDispatcher _queryDispatcher;

        public BattleService(ICommandDispatcher commandDispatcher, IQueryDispatcher queryDispatcher)
        {
            _commandDispatcher = commandDispatcher;
            _queryDispatcher = queryDispatcher;
        }

        [Authorize]
        public override async Task<BattleResponse> PrepareBattle(PrepareBattleRequest request, ServerCallContext context)
        {
            var mapId = new MapId(request.MapId);
            var userId = new UserId(request.UserId);
            var battleDetails = await _commandDispatcher.SendAsync(new PrepareBattle { MapId = mapId, UserId = userId });
            var response = battleDetails.AsResponse();
            return response;
        }

        public override async Task<StartBattleResponse> StartBattle(BattleRequest request, ServerCallContext context)
        {
            var battleId = new BattleId(request.BattleId);
            var userId = new UserId(request.UserId);
            var battleStatus = await _commandDispatcher.SendAsync(new StartBattle { BattleId = battleId, UserId = userId });
            var response = new StartBattleResponse() { BattleId = battleStatus.BattleId.ToString(), PlayerId = battleStatus.PlayerId.ToString(), PlayerHealth = battleStatus.PlayerHealth, EnemyId = battleStatus.EnemyId.ToString(), EnemyHealth = battleStatus.EnemyHealth };
            return response;
        }

        public override async Task<BattleResponse> CompleteBattle(BattleRequest request, ServerCallContext context)
        {
            var battleId = new BattleId(request.BattleId);
            var userId = new UserId(request.UserId);
            var battleDetails = await _commandDispatcher.SendAsync(new CompleteBattle { BattleId = battleId, UserId = userId });
            var response = battleDetails.AsResponse();
            return response;
        }

        public override async Task<AddBattleEventResponse> AddBattleEvent(AddBattleEventRequest request, ServerCallContext context)
        {
            var playerId = new PlayerId(request.PlayerId);
            var battleId = new BattleId(request.BattleId);
            var enemyId = new EnemyId(request.EnemyId);
            var battleEvent = await _commandDispatcher.SendAsync(new AddBattleEvent { BattleId = battleId, PlayerId = playerId, EnemyId = enemyId, Action = request.Action });
            var response = battleEvent.AsResponse();
            return response;
        }

        public override async Task<GetCurrentBattlesResponse> GetCurrentBattles(GetCurrentBattlesRequest request, ServerCallContext context)
        {
            var userId = new UserId(request.UserId);
            var battles = await _queryDispatcher.QueryAsync(new GetCurrentBattles() { UserId = userId });
            return battles.AsResponse();
        }

        public override async Task<GetBattleStateResponse> GetBattleState(GetBattleStateRequest request, ServerCallContext context)
        {
            var battleId = new BattleId(request.BattleId);
            var battle = await _queryDispatcher.QueryAsync(new GetBattleState() { BattleId = battleId });
            return battle.AsResponse();
        }
    }
}
