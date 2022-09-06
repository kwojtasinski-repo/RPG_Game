using Grpc.Core;
using Google.Protobuf.WellKnownTypes;
using RPG_GAME.Application.Commands.Battles;
using RPG_GAME.Infrastructure.Commands;
using RPG_GAME.Infrastructure.Grpc.Protos;
using System.Globalization;
using RPG_GAME.Infrastructure.Grpc.Mappings;

namespace RPG_GAME.Infrastructure.Grpc.Services
{
    internal sealed class BattleService : Battle.BattleBase
    {
        private readonly ICommandDispatcher _commandDispatcher;

        public BattleService(ICommandDispatcher commandDispatcher)
        {
            _commandDispatcher = commandDispatcher;
        }

        public override async Task<BattleResponse> PrepareBattle(PrepareBattleRequest request, ServerCallContext context)
        {
            // TODO: Try add explicit implicit cast string on guid and guid on string
            // TODO: Add some mappings
            var mapId = Guid.Parse(request.MapId);
            var userId = Guid.Parse(request.UserId);
            var battleDetails = await _commandDispatcher.SendAsync(new PrepareBattle { MapId = mapId, UserId = userId });
            var response = battleDetails.AsResponse();
            return response;
        }

        public override async Task<StartBattleResponse> StartBattle(BattleRequest request, ServerCallContext context)
        {
            var battleId = Guid.Parse(request.BattleId);
            var userId = Guid.Parse(request.UserId);
            var battleStatus = await _commandDispatcher.SendAsync(new StartBattle { BattleId = battleId, UserId = userId });
            var response = new StartBattleResponse() { BattleId = battleStatus.BattleId.ToString(), PlayerId = battleStatus.PlayerId.ToString(), PlayerHealth = battleStatus.PlayerHealth, EnemyId = battleStatus.EnemyId.ToString(), EnemyHealth = battleStatus.EnemyHealth };
            return response;
        }

        public override async Task<BattleResponse> CompleteBattle(BattleRequest request, ServerCallContext context)
        {
            var battleId = Guid.Parse(request.BattleId);
            var userId = Guid.Parse(request.UserId);
            var battleDetails = await _commandDispatcher.SendAsync(new CompleteBattle { BattleId = battleId, UserId = userId });
            var response = battleDetails.AsResponse();
            return response;
        }

        public override async Task<AddBattleEventResponse> AddBattleEvent(AddBattleEventRequest request, ServerCallContext context)
        {
            var playerId = Guid.Parse(request.PlayerId);
            var battleId = Guid.Parse(request.BattleId);
            var enemyId = Guid.Parse(request.EnemyId);
            var battleEvent = await _commandDispatcher.SendAsync(new AddBattleEvent { BattleId = battleId, PlayerId = playerId, EnemyId = enemyId, Action = request.Action });
            var response = battleEvent.AsResponse();
            return response;
        }
    }
}
