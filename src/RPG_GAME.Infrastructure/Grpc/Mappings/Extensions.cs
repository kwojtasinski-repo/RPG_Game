using Google.Protobuf.WellKnownTypes;
using RPG_GAME.Application.DTO.Battles;
using RPG_GAME.Application.DTO.Players;
using RPG_GAME.Infrastructure.Grpc.Protos;
using System.Globalization;

namespace RPG_GAME.Infrastructure.Grpc.Mappings
{
    internal static class Extensions
    {
        public static BattleResponse AsResponse(this BattleDetailsDto battleDetailsDto)
        {
            var response = new BattleResponse() { Id = battleDetailsDto.Id.ToString(), UserId = battleDetailsDto.UserId.ToString(), BattleInfo = battleDetailsDto.BattleInfo, StartDate = Timestamp.FromDateTime(battleDetailsDto.StartDate), EndDate = battleDetailsDto.EndDate.HasValue ? Timestamp.FromDateTime(battleDetailsDto.EndDate.Value) : null };
            response.Map = new Map { Id = battleDetailsDto.Map.Id.ToString(), Difficulty = battleDetailsDto.Map.Difficulty, Name = battleDetailsDto.Map.Name };
            foreach (var enemies in battleDetailsDto.Map.Enemies)
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
                        Experience = enemies.Enemy.Experience.AsDecimalValue(),
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
                        Probability = skill.Probability.AsDecimalValue()
                    });
                }

                response.Map.Enemies.Add(enemy);
            }

            foreach (var enemyKilled in battleDetailsDto.EnemiesKilled)
            {
                var dict = new Dictionary();
                dict.Pairs.Add(new Pair { Key = enemyKilled.Key.ToString(), Value = enemyKilled.Value });
                response.EnemiesKilled.Add(dict);
            }

            foreach (var battle in battleDetailsDto.BattleStates)
            {
                var battleState = new BattleState
                {
                    Id = battle.Id.ToString(),
                    BattleId = battle.BattleId.ToString(),
                    BattleStatus = battle.BattleStatus,
                    Created = Timestamp.FromDateTime(battle.Created),
                    Player = battle.Player.AsResponse()
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

        public static AddBattleEventResponse AsResponse(this BattleEventDto battleEventDto)
        {
            var response = new AddBattleEventResponse
            {
                Id = battleEventDto.Id.ToString(),
                Action = new FightAction
                {
                    CharacterId = battleEventDto.Action.CharacterId.ToString(),
                    Name = battleEventDto.Action.Name,
                    Character = battleEventDto.Action.Character,
                    AttackInfo = battleEventDto.Action.AttackInfo,
                    DamageDealt = battleEventDto.Action.DamageDealt,
                    Health = battleEventDto.Action.Health
                },
                BattleId = battleEventDto.BattleId.ToString(),
                Level = battleEventDto.Level,
                CurrentExp = battleEventDto.CurrentExp.AsDecimalValue(),
                RequiredExp = battleEventDto.RequiredExp.AsDecimalValue(),
                Created = Timestamp.FromDateTime(battleEventDto.Created),
            };

            return response;
        }

        public static GetCurrentBattlesResponse AsResponse(this IEnumerable<Application.DTO.Battles.BattleDto> dtos)
        {
            var response = new GetCurrentBattlesResponse();
            
            foreach(var battle in dtos)
            {
                response.Battles.Add(battle.AsResponse());
            }

            return response;
        }

        public static Protos.BattleDto AsResponse(this Application.DTO.Battles.BattleDto dto)
        {
            return new Protos.BattleDto
            {
                Id = dto.Id.ToString(),
                BattleInfo = dto.BattleInfo.ToString(),
                StartDate = Timestamp.FromDateTime(dto.StartDate),
                EndDate = dto.EndDate.HasValue ? Timestamp.FromDateTime(dto.EndDate.Value) : null,
                UserId = dto.UserId.ToString()
            };
        }

        public static GetBattleStateResponse AsResponse(this BattleStateDto dto)
        {
            return new GetBattleStateResponse
            {
                Id = dto.Id.ToString(),
                BattleStatus = dto.BattleStatus,
                BattleId = dto.BattleId.ToString(),
                Created = Timestamp.FromDateTime(dto.Created),
                Modified = dto.Modified.HasValue ? Timestamp.FromDateTime(dto.Modified.Value) : null,
                Player = dto.Player.AsResponse()
            };
        }

        public static Player AsResponse(this PlayerDto dto)
        {
            return new Player
            {
                Id = dto.Id.ToString(),
                Name = dto.Name,
                UserId = dto.UserId.ToString(),
                Level = dto.Level,
                CurrentExp = dto.CurrentExp.AsDecimalValue(),
                RequiredExp = dto.RequiredExp.AsDecimalValue(),
                Hero = new HeroAssign
                {
                    Id = dto.Hero.Id.ToString(),
                    HeroName = dto.Hero.HeroName,
                    Attack = dto.Hero.Attack,
                    Health = dto.Hero.Health
                }
            };
        }

        public static DecimalValue AsDecimalValue(this decimal value)
        {
            var stringValue = value.ToString("0.0000000", CultureInfo.InvariantCulture).Split('.');
            return new DecimalValue { Units = long.Parse(stringValue[0]), Nanos = int.Parse(stringValue[1]) };
        }
    }
}
