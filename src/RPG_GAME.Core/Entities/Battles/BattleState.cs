using RPG_GAME.Core.Entities.Players;
using RPG_GAME.Core.Exceptions.Battles;
using System;

namespace RPG_GAME.Core.Entities.Battles
{
    public class BattleState
    {
        public Guid Id { get; }
        public Guid BattleId { get; }
        public BattleStatus BattleStatus { get; private set; }
        public Player Player { get; private set; }
        public DateTime Created { get; }
        public DateTime? Modified { get; private set; }

        public BattleState(Guid id, string battleStatus, Guid battleId, Player player, DateTime created, DateTime? modified = null)
        {
            Id = id;
            BattleId = battleId;
            ChangeBattleStatus(battleStatus);
            Player = player;
            Created = created;
            Modified = modified;
        }

        public static BattleState Prepare(Guid battleId, Player player, DateTime created)
        {
            return new BattleState(Guid.NewGuid(), BattleStatus.Prepare.ToString(), battleId, player, created);
        }

        public static BattleState InAction(Guid battleId, Player player, DateTime created)
        {
            return new BattleState(Guid.NewGuid(), BattleStatus.InAction.ToString(), battleId, player, created);
        }

        public static BattleState Completed(Guid battleId, Player player, DateTime created)
        {
            return new BattleState(Guid.NewGuid(), BattleStatus.Completed.ToString(), battleId, player, created);
        }

        private void ChangeBattleStatus(string battleStatus)
        {
            var parsed = Enum.TryParse<BattleStatus>(battleStatus, out var battleStatusParsed);

            if (!parsed)
            {
                throw new InvalidBattleStatusException(battleStatus);
            }

            if (!Enum.IsDefined(battleStatusParsed))
            {
                throw new InvalidBattleStatusException(battleStatus);
            }

            BattleStatus = battleStatusParsed;
        }

        public void UpdatePlayer(Player player, DateTime modified)
        {
            if (BattleStatus != BattleStatus.InAction)
            {
                throw new CannotUpdatePlayerInCurrentBattleStateException(BattleStatus);
            }

            if (player is null)
            {
                throw new InvalidPlayerException();
            }

            if (player.Hero is null)
            {
                throw new InvalidPlayerException();
            }

            if (modified == default)
            {
                throw new InvalidModifiedDateException();
            }

            if (Modified is not null && Modified > modified)
            {
                throw new ModifiedDateCannotBeBeforeException(Modified.Value, modified);
            }

            Player = player;
            Modified = modified;
        }
    }
}
