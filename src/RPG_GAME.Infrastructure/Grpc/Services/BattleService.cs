using Grpc.Core;
using Google.Protobuf.WellKnownTypes;
using RPG_GAME.Application.Commands.Battles;
using RPG_GAME.Infrastructure.Commands;
using RPG_GAME.Infrastructure.Grpc.Protos;
using System.Globalization;

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
            var response = new BattleResponse() { Id = battleDetails.Id.ToString(), UserId = battleDetails.UserId.ToString(), BattleInfo = battleDetails.BattleInfo, StartDate = Timestamp.FromDateTime(battleDetails.StartDate) };
            response.Map = new Map { Id = battleDetails.Map.Id.ToString(), Difficulty = battleDetails.Map.Difficulty, Name = battleDetails.Map.Name };
            foreach(var enemies in battleDetails.Map.Enemies)
            {
                var enemy = new Enemies
                {
                    Enemy = new EnemyAssign
                    {
                        Id = enemies.Enemy.Id.ToString(),
                        Name = enemies.Enemy.EnemyName,
                        Attack = enemies.Enemy.BaseAttack,
                        Category = enemies.Enemy.Category,
                        Difficulty = enemies.Enemy.Difficulty,
                        HealLvl = enemies.Enemy.BaseHealLvl,
                        Health = enemies.Enemy.BaseHealth,
                        Experience = new DecimalValue { Units = long.Parse(enemies.Enemy.Experience.ToString("0.0000000", CultureInfo.InvariantCulture).Split('.')[0]), Nanos = int.Parse(enemies.Enemy.Experience.ToString("0.0000000", CultureInfo.InvariantCulture).Split('.')[1]) },
                    },
                    Quantity = enemies.Quantity
                };

                foreach (var skill in enemies.Enemy.Skills)
                {
                    enemy.Enemy.Skills.Add(new SkillEnemyAssign { Id = skill.Id.ToString(), Name = skill.Name, Attack = skill.BaseAttack, 
                        Probability = new DecimalValue { Units = long.Parse(skill.Probability.ToString("0.0000000", CultureInfo.InvariantCulture).Split('.')[0]), Nanos = int.Parse(skill.Probability.ToString("0.0000000", CultureInfo.InvariantCulture).Split('.')[1]) }});
                }

                response.Map.Enemies.Add(enemy);
            }

            foreach (var enemyKilled in battleDetails.EnemiesKilled)
            {
                var dict = new Dictionary();
                dict.Pairs.Add(new Pair { Key = enemyKilled.Key.ToString(), Value = enemyKilled.Value });
                response.EnemiesKilled.Add(dict);
            }

            foreach (var battle in battleDetails.BattleStates)
            {
                var battleState = new BattleState
                {
                    Id = battle.Id.ToString(),
                    BattleId = battle.BattleId.ToString(),
                    BattleStatus = battle.BattleStatus,
                    Created = Timestamp.FromDateTime(battle.Created),
                    Player = new Player
                    {
                        Id = battle.Player.Id.ToString(),
                        Name = battle.Player.Name,
                        UserId = battle.Player.UserId.ToString(),
                        Level = battle.Player.Level,
                        CurrentExp = new DecimalValue { Units = long.Parse(battle.Player.CurrentExp.ToString("0.0000000", CultureInfo.InvariantCulture).Split('.')[0]), Nanos = int.Parse(battle.Player.CurrentExp.ToString("0.0000000", CultureInfo.InvariantCulture).Split('.')[1]) },
                        RequiredExp = new DecimalValue { Units = long.Parse(battle.Player.RequiredExp.ToString("0.0000000", CultureInfo.InvariantCulture).Split('.')[0]), Nanos = int.Parse(battle.Player.RequiredExp.ToString("0.0000000", CultureInfo.InvariantCulture).Split('.')[1]) },
                        Hero = new HeroAssign
                        {
                            Id = battle.Player.Hero.Id.ToString(),
                            HeroName = battle.Player.Hero.HeroName,
                            Attack = battle.Player.Hero.Attack,
                            HealLvl = battle.Player.Hero.HealLvl,
                            Health = battle.Player.Hero.Health
                        }
                    }
                };

                foreach (var skill in battle.Player.Hero.Skills)
                {
                    battleState.Player.Hero.Skills.Add(new SkillHeroAssign { Id = skill.Id.ToString(), Name = skill.Name, Attack = skill.Attack });
                }

                if (battle.Modified.HasValue)
                {
                    battleState.Modified = Timestamp.FromDateTime(battle.Modified.Value);
                }

                response.BattleStates.Add(battleState);
            }

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
            var response = new BattleResponse() { Id = battleDetails.Id.ToString(), UserId = battleDetails.UserId.ToString(), BattleInfo = battleDetails.BattleInfo, StartDate = Timestamp.FromDateTime(battleDetails.StartDate), EndDate = Timestamp.FromDateTime(battleDetails.EndDate.Value) };
            response.Map = new Map { Id = battleDetails.Map.Id.ToString(), Difficulty = battleDetails.Map.Difficulty, Name = battleDetails.Map.Name };
            foreach (var enemies in battleDetails.Map.Enemies)
            {
                var enemy = new Enemies
                {
                    Enemy = new EnemyAssign
                    {
                        Id = enemies.Enemy.Id.ToString(),
                        Name = enemies.Enemy.EnemyName,
                        Attack = enemies.Enemy.BaseAttack,
                        Category = enemies.Enemy.Category,
                        Difficulty = enemies.Enemy.Difficulty,
                        HealLvl = enemies.Enemy.BaseHealLvl,
                        Health = enemies.Enemy.BaseHealth,
                        Experience = new DecimalValue { Units = long.Parse(enemies.Enemy.Experience.ToString("0.0000000", CultureInfo.InvariantCulture).Split('.')[0]), Nanos = int.Parse(enemies.Enemy.Experience.ToString("0.0000000", CultureInfo.InvariantCulture).Split('.')[1]) },
                    },
                    Quantity = enemies.Quantity
                };

                foreach (var skill in enemies.Enemy.Skills)
                {
                    enemy.Enemy.Skills.Add(new SkillEnemyAssign
                    {
                        Id = skill.Id.ToString(),
                        Name = skill.Name,
                        Attack = skill.BaseAttack,
                        Probability = new DecimalValue { Units = long.Parse(skill.Probability.ToString("0.0000000", CultureInfo.InvariantCulture).Split('.')[0]), Nanos = int.Parse(skill.Probability.ToString("0.0000000", CultureInfo.InvariantCulture).Split('.')[1]) }
                    });
                }

                response.Map.Enemies.Add(enemy);
            }

            foreach (var enemyKilled in battleDetails.EnemiesKilled)
            {
                var dict = new Dictionary();
                dict.Pairs.Add(new Pair { Key = enemyKilled.Key.ToString(), Value = enemyKilled.Value });
                response.EnemiesKilled.Add(dict);
            }

            foreach (var battle in battleDetails.BattleStates)
            {
                var battleState = new BattleState
                {
                    Id = battle.Id.ToString(),
                    BattleId = battle.BattleId.ToString(),
                    BattleStatus = battle.BattleStatus,
                    Created = Timestamp.FromDateTime(battle.Created),
                    Player = new Player
                    {
                        Id = battle.Player.Id.ToString(),
                        Name = battle.Player.Name,
                        UserId = battle.Player.UserId.ToString(),
                        Level = battle.Player.Level,
                        CurrentExp = new DecimalValue { Units = long.Parse(battle.Player.CurrentExp.ToString("0.0000000", CultureInfo.InvariantCulture).Split('.')[0]), Nanos = int.Parse(battle.Player.CurrentExp.ToString("0.0000000", CultureInfo.InvariantCulture).Split('.')[1]) },
                        RequiredExp = new DecimalValue { Units = long.Parse(battle.Player.RequiredExp.ToString("0.0000000", CultureInfo.InvariantCulture).Split('.')[0]), Nanos = int.Parse(battle.Player.RequiredExp.ToString("0.0000000", CultureInfo.InvariantCulture).Split('.')[1]) },
                        Hero = new HeroAssign
                        {
                            Id = battle.Player.Hero.Id.ToString(),
                            HeroName = battle.Player.Hero.HeroName,
                            Attack = battle.Player.Hero.Attack,
                            HealLvl = battle.Player.Hero.HealLvl,
                            Health = battle.Player.Hero.Health
                        }
                    }
                };

                foreach (var skill in battle.Player.Hero.Skills)
                {
                    battleState.Player.Hero.Skills.Add(new SkillHeroAssign { Id = skill.Id.ToString(), Name = skill.Name, Attack = skill.Attack });
                }

                if (battle.Modified.HasValue)
                {
                    battleState.Modified = Timestamp.FromDateTime(battle.Modified.Value);
                }

                response.BattleStates.Add(battleState);
            }

            return response;
        }

        public override async Task<AddBattleEventResponse> AddBattleEvent(AddBattleEventRequest request, ServerCallContext context)
        {
            var playerId = Guid.Parse(request.PlayerId);
            var battleId = Guid.Parse(request.BattleId);
            var enemyId = Guid.Parse(request.EnemyId);
            var battleEvent = await _commandDispatcher.SendAsync(new AddBattleEvent { BattleId = battleId, PlayerId = playerId, EnemyId = enemyId, Action = request.Action });
            var response = new AddBattleEventResponse
            {
                Id = battleEvent.Id.ToString(),
                Action = new FightAction
                {
                    CharacterId = battleEvent.Action.CharacterId.ToString(),
                    Name = battleEvent.Action.Name,
                    Character = battleEvent.Action.Character,
                    AttackInfo = battleEvent.Action.AttackInfo,
                    DamageDealt = battleEvent.Action.DamageDealt,
                    Health = battleEvent.Action.Health
                },
                BattleId = battleEvent.BattleId.ToString(),
                Created = Timestamp.FromDateTime(battleEvent.Created),
            };
            return response;
        }
    }
}
