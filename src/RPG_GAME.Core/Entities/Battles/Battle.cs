using RPG_GAME.Core.Common;
using RPG_GAME.Core.Entities.Maps;
using RPG_GAME.Core.Entities.Players;
using RPG_GAME.Core.Exceptions.Battles;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
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
        public Map Map { get; private set; }
        public IEnumerable<BattleState> BattleStates => GetBattleStates();
        public IImmutableDictionary<Guid, int> EnemiesKilled => _enemiesKilled.GroupBy(e => e).ToImmutableDictionary(e => e.Key, e => e.Count());

        private IList<Guid> _enemiesKilled = new List<Guid>();
        private IList<BattleState> _battleStates = new List<BattleState>();

        public Battle(Guid id, DateTime startDate, Guid userId, string battleInfo, Map map, DateTime? endDate = null, IEnumerable<BattleState> battleStates = null, IEnumerable<Guid> enemiesKilled = null)
        {
            Id = id;
            StartDate = startDate;
            UserId = userId;
            ChangeBattleInfo(battleInfo);
            ChangeMap(map);
            EndDate = endDate;

            if (battleStates is not null)
            {
                _battleStates = new List<BattleState>(battleStates);
            }

            if (enemiesKilled is not null)
            {
                _enemiesKilled = new List<Guid>(enemiesKilled);
            }
        }

        public BattleState GetLatestBattleState()
        {
            if (!_battleStates.Any())
            {
                throw new BattleStatesNotFoundException(Id);
            }

            var battleStateCompleted = _battleStates.SingleOrDefault(b => b.BattleStatus == BattleStatus.Completed);

            if (battleStateCompleted is not null)
            {
                return battleStateCompleted;
            }

            var battleStateInAction = _battleStates.SingleOrDefault(b => b.BattleStatus == BattleStatus.InAction);

            if (battleStateInAction is not null)
            {
                return battleStateInAction;
            }

            var battleStatePrepare = _battleStates.SingleOrDefault(b => b.BattleStatus == BattleStatus.Prepare);
            return battleStatePrepare;
        }

        public static Battle Create(DateTime startDate, Guid userId, Map map)
        {
            return new Battle(Guid.NewGuid(), startDate, userId, "Starting", map);
        }

        public void AddBattleStateAtPrepare(BattleState battleState)
        {
            if (battleState is null)
            {
                throw new InvalidBattleStateException();
            }

            if (battleState.BattleStatus != BattleStatus.Prepare)
            {
                throw new InvalidBattleStatusException(battleState.BattleStatus.ToString());
            }

            PolicyAddingBattleStates(battleState);
            _battleStates.Add(battleState);
        }

        public void AddBattleStateAtInAction(BattleState battleState)
        {
            if (battleState is null)
            {
                throw new InvalidBattleStateException();
            }

            if (battleState.BattleStatus != BattleStatus.InAction)
            {
                throw new InvalidBattleStatusException(battleState.BattleStatus.ToString());
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
                var player = new Player(battleState.Player.Id, battleState.Player.Name, hero, 
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

        public Player EndBattle(DateTime endDate, string battleInfo, BattleState battleState)
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

            if (BattleInfo != BattleInfo.Won)
            {
                var battleStateAtPrepare = _battleStates.SingleOrDefault(b => b.BattleStatus == BattleStatus.Prepare);
                return battleStateAtPrepare.Player;
            }

            return battleState.Player;
        }

        public void ChangeMap(Map map)
        {
            if (map is null)
            {
                throw new InvalidMapException();
            }

            Map = map;
        }

        public void AddKilledEnemy(Guid enemyId)
        {
            if (enemyId == Guid.Empty)
            {
                throw new InvalidEnemyIdException();
            }

            _enemiesKilled.Add(enemyId);
        }

        public EnemyAssign GetEnemyToFight()
        {
            EnemyAssign enemy = null;

            foreach(var category in Enum.GetValues<Category>())
            {
                enemy = GetEnemyByCategory(category);

                if (enemy is not null)
                {
                    break;
                }
            }

            if (enemy is null)
            {
                throw new MapHasNoEnemiesException(Map.Id);
            }

            return enemy;
        }

        private EnemyAssign GetEnemyByCategory(Category category)
        {
            var enemies = Map.Enemies.Where(e => e.Enemy.Category == category)
                .FirstOrDefault(e =>
                {
                    var enemiesKilledCount = EnemiesKilled.Where(ek => ek.Key == e.Enemy.Id).Select(ek => ek.Value).FirstOrDefault();

                    if (enemiesKilledCount == default)
                    {
                        return true;
                    }

                    if (e.Quantity < enemiesKilledCount)
                    {
                        return true;
                    }

                    return false;
                });
            return enemies?.Enemy;
        }
    }
}
