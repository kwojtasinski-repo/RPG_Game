using Grpc.Core;
using RPG_GAME.Infrastructure.Grpc.Protos;

namespace RPG_GAME.Infrastructure.Grpc.Services
{
    internal sealed class BattleService : Battle.BattleBase
    {
        public override Task<BattleResponse> PrepareBattle(BattleRequest request, ServerCallContext context)
        {
            return base.PrepareBattle(request, context);
        }

        public override Task<BattleResponse> StartBattle(BattleRequest request, ServerCallContext context)
        {
            return base.StartBattle(request, context);
        }

        public override Task<BattleResponse> CompleteBattle(BattleRequest request, ServerCallContext context)
        {
            return base.CompleteBattle(request, context);
        }

        public override Task<AddBattleEventResponse> AddBattleEvent(AddBattleEventRequest request, ServerCallContext context)
        {
            return base.AddBattleEvent(request, context);
        }
    }
}
