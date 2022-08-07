using RPG_GAME.Core.Entities.Players;
using RPG_GAME.Core.Exceptions.Battles;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RPG_GAME.Core.Entities.Battles
{
    public class Battle
    {
        public Guid Id { get; }
        public DateTime StartDate { get; }
        public Guid UserId { get; }
        public BattleInfo BattleInfo { get; private set; }
        public DateTime? EndDate { get; private set; }
        public IEnumerable<BattleState> BattleStates => GetBattleStates();

        private IList<BattleState> _battleStates = new List<BattleState>();

        public Battle(Guid id, DateTime startDate, Guid userId, string battleInfo, DateTime? endDate = null, IEnumerable<BattleState> battleStates = null)
        {
            Id = id;
            StartDate = startDate;
            UserId = userId;
            ChangeBattleInfo(battleInfo);
            EndDate = endDate;

            if (battleStates is not null)
            {
                _battleStates = new List<BattleState>(battleStates);
            }
        }

        public static Battle Create(DateTime startDate, Guid userId)
        {
            return new Battle(Guid.NewGuid(), startDate, userId, "Starting");
        }

        public void AddBattleStateAtPrepare(BattleState battleState)
        {
            if (battleState is null)
            {
                throw new InvalidBattleStateException();
            }

            PolicyAddingBattleStates(battleState);
            _battleStates.Add(battleState);
        }

        public void AddBattleStateAtInProgress(BattleState battleState)
        {
            if (battleState is null)
            {
                throw new InvalidBattleStateException();
            }

            BattleInfo = BattleInfo.InProgress;
            PolicyAddingBattleStates(battleState);
            _battleStates.Add(battleState);
        }

        public BattleState GetBattleStateInAction()
        {
            if (!(BattleInfo == BattleInfo.InProgress))
            {
                throw new CannotGetBattleStateInActionException(BattleInfo);
            }

            var battleStates = _battleStates.SingleOrDefault(b => b.BattleStatus == BattleStatus.InAction);

            if (battleStates is null)
            {
                throw new BattleStateInActionNotFoundException();
            }

            return battleStates;
        }

        private void PolicyAddingBattleStates(BattleState battleState)
        {
            var exists = _battleStates.Any(b => b.BattleStatus == battleState.BattleStatus);

            if (exists)
            {
                throw new BattleStateAlreadyExistsException(battleState.BattleStatus);
            }
        }

        private IEnumerable<BattleState> GetBattleStates()
        {
            var battleStates = new List<BattleState>();

            foreach(var battleState in _battleStates)
            {
                var heroSkills = battleState.Player.Hero.Skills.Select(s => new SkillHeroAssign(s.Id, s.Name, s.Attack));
                var hero = new HeroAssign(battleState.Player.Hero.Id, battleState.Player.Hero.HeroName, battleState.Player.Hero.Health, 
                    battleState.Player.Hero.Attack, battleState.Player.Hero.HealLvl, heroSkills);
                var player = new Players.Player(battleState.Player.Id, battleState.Player.Name, hero, 
                    battleState.Player.Level, battleState.Player.CurrentExp, battleState.Player.RequiredExp, battleState.Player.UserId);
                battleStates.Add(new BattleState(battleState.Id, battleState.BattleStatus.ToString(), battleState.BattleId,
                    player, battleState.Created, battleState.Modified));
            }

            return battleStates;
        }

        private void ChangeBattleInfo(string battleInfo)
        {
            var parsed = Enum.TryParse<BattleInfo>(battleInfo, out var battleInfoParsed);

            if (!parsed)
            {
                throw new InvalidBattleInfoException(battleInfo);
            }

            BattleInfo = battleInfoParsed;
        }

        public void Completed(DateTime endDate, string battleInfo, BattleState battleState)
        {
            if (battleState is null)
            {
                throw new InvalidBattleStateException();
            }

            if (battleState.BattleStatus != BattleStatus.Completed)
            {
                throw new InvalidBattleStatusException(battleState.BattleStatus.ToString());
            }

            PolicyAddingBattleStates(battleState);
            var allowedStates = new List<BattleInfo>() { BattleInfo.Won, BattleInfo.Lost, BattleInfo.Suspended };
            if (!allowedStates.Any(s => s.ToString() == battleInfo))
            {
                throw new InvalidBattleInfoException(battleInfo);
            }

            EndDate = endDate;
            ChangeBattleInfo(battleInfo);
            _battleStates.Add(battleState);
        }
    }
}
