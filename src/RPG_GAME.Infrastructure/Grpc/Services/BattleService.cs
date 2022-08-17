using Grpc.Core;
using RPG_GAME.Infrastructure.Grpc.Protos;

namespace RPG_GAME.Infrastructure.Grpc.Services
{
    internal sealed class BattleService : Battle.BattleBase
    {
        public override Task<AddBattleEventResponse> AddBattleEvent(AddBattleEventRequest request, ServerCallContext context)
        {
            return base.AddBattleEvent(request, context);
        }

        public override Task<AddBattleStateResponse> AddBattleState(AddBattleStateRequest request, ServerCallContext context)
        {
            return base.AddBattleState(request, context);
        }

        public override Task<CheckBattleResponse> CheckBattle(CheckBattleRequest request, ServerCallContext context)
        {
            return base.CheckBattle(request, context);
        }

        public override Task<StartBattleResponse> StartBattle(StartBattleRequest request, ServerCallContext context)
        {
            return base.StartBattle(request, context);
        }

        public override Task<UpdateBattleResponse> UpdateBattle(UpdateBattleRequest request, ServerCallContext context)
        {
            return base.UpdateBattle(request, context);
        }

        public override Task<UpdateBattleStateResponse> UpdateBattleState(UpdateBattleStateRequest request, ServerCallContext context)
        {
            return base.UpdateBattleState(request, context);
        }
    }
}
