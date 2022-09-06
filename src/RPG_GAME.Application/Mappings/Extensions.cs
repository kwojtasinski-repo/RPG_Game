using RPG_GAME.Application.DTO.Battles;
using RPG_GAME.Application.DTO.Common;
using RPG_GAME.Application.DTO.Enemies;
using RPG_GAME.Application.DTO.Heroes;
using RPG_GAME.Application.DTO.Maps;
using RPG_GAME.Application.DTO.Players;
using RPG_GAME.Core.Entities.Battles;
using RPG_GAME.Core.Entities.Battles.Actions;
using RPG_GAME.Core.Entities.Common;
using RPG_GAME.Core.Entities.Enemies;
using RPG_GAME.Core.Entities.Heroes;
using RPG_GAME.Core.Entities.Maps;
using RPG_GAME.Core.Entities.Players;

namespace RPG_GAME.Application.Mappings
{
    public static class Extensions
    {
        public static Player AsEntity(this PlayerDto playerDto)
        {
            return new Player(
                playerDto.Id,
                playerDto.Name,
                playerDto.Hero.AsAssignEntity(),
                playerDto.Level,
                playerDto.CurrentExp,
                playerDto.RequiredExp,
                playerDto.UserId
            );
        }

        public static PlayerDto AsDto(this Player player)
        {
            return new PlayerDto()
            {
                Id = player.Id,
                UserId = player.UserId,
                Name = player.Name,
                Level = player.Level,
                CurrentExp = player.CurrentExp,
                RequiredExp = player.RequiredExp,
                Hero = player.Hero.AsAssignDto()
            };
        }

        public static Hero AsEntity(this HeroDto heroDto)
        {
            return new Hero(heroDto.Id, heroDto.HeroName,
                heroDto.Health.AsEntity(), heroDto.Attack.AsEntity(),
                heroDto.BaseRequiredExperience.AsEntity(),
                heroDto.Skills.Select(s => s.AsEntity()));
        }

        public static HeroAssign AsAssign(this Hero hero)
        {
            return new HeroAssign(
                hero.Id,
                hero.HeroName,
                hero.Health.Value,
                hero.Attack.Value,
                hero.Skills.Select(s => s.AsAssign())
            );
        }

        public static HeroAssign AsAssignEntity(this HeroAssignDto heroDto)
        {
            return new HeroAssign(
                heroDto.Id,
                heroDto.HeroName,
                heroDto.Health,
                heroDto.Attack,
                heroDto.Skills.Select(s => s.AsAssignEntity())
            );
        }

        public static SkillHeroAssign AsAssignEntity(this SkillHeroAssignDto skillHeroDto)
        {
            return new SkillHeroAssign(
                skillHeroDto.Id,
                skillHeroDto.Name,
                skillHeroDto.Attack
            );
        }

        public static SkillHeroAssign AsAssign(this SkillHero skillHero)
        {
            return new SkillHeroAssign(skillHero.Id, skillHero.Name, skillHero.BaseAttack);
        }

        public static HeroAssignDto AsAssignDto(this HeroAssign hero)
        {
            return new HeroAssignDto()
            {
                Id = hero.Id,
                HeroName = hero.HeroName,
                Attack = hero.Attack,
                Health = hero.Health,
                Skills = hero.Skills.Select(s => s.AsDto())
            };
        }

        public static SkillHeroAssignDto AsDto(this SkillHeroAssign skill)
        {
            return new SkillHeroAssignDto()
            {
                Id = skill.Id,
                Name = skill.Name,
                Attack = skill.Attack
            };
        }

        public static State<T> AsEntity<T>(this StateDto<T> fieldDto)
            where T : struct
        {
            return new State<T>(fieldDto.Value, fieldDto.IncreasingState.AsEntity());
        }

        public static IncreasingState<T> AsEntity<T>(this IncreasingStateDto<T> fieldDto)
            where T : struct
        {
            return new IncreasingState<T>(fieldDto.Value, fieldDto.StrategyIncreasing);
        }

        public static SkillEnemy AsEntity(this SkillEnemyDto skill)
        {
            return new SkillEnemy(
                skill.Id,
                skill.Name,
                skill.BaseAttack,
                skill.Probability,
                skill.IncreasingState.AsEntity()
            );
        }

        public static SkillHero AsEntity(this SkillHeroDto skill)
        {
            return new SkillHero(skill.Id, skill.Name, skill.BaseAttack, skill.IncreasingState.AsEntity());
        }

        public static HeroDto AsDto(this Hero hero)
        {
            return new HeroDto()
            {
                Id = hero.Id,
                Attack = hero.Attack.AsDto(),
                Health = hero.Health.AsDto(),
                HeroName = hero.HeroName,
                BaseRequiredExperience = hero.BaseRequiredExperience.AsDto(),
                Skills = hero.Skills.Select(s => s.AsDto())
            };
        }

        public static HeroDetailsDto AsDetailsDto(this Hero hero)
        {
            return new HeroDetailsDto()
            {
                Id = hero.Id,
                Attack = hero.Attack.AsDto(),
                Health = hero.Health.AsDto(),
                HeroName = hero.HeroName,
                BaseRequiredExperience = hero.BaseRequiredExperience.AsDto(),
                Skills = hero.Skills.Select(s => s.AsDto()),
                PlayersAssignedTo = hero.PlayersAssignedTo
            };
        }

        public static StateDto<T> AsDto<T>(this State<T> field)
            where T : struct
        {
            return new StateDto<T>
            {
                IncreasingState = field.IncreasingState.AsDto(),
                Value = field.Value
            };
        }

        public static IncreasingStateDto<T> AsDto<T>(this IncreasingState<T> field)
            where T : struct
        {
            return new IncreasingStateDto<T>
            {
                Value = field.Value,
                StrategyIncreasing = field.StrategyIncreasing.ToString()
            };
        }

        public static SkillEnemyDto AsDto(this SkillEnemy skill)
        {
            return new SkillEnemyDto()
            {
                Id = skill.Id,
                Name = skill.Name,
                BaseAttack = skill.BaseAttack,
                Probability = skill.Probability,
                IncreasingState = skill.IncreasingState.AsDto()
            };
        }

        public static SkillHeroDto AsDto(this SkillHero skill)
        {
            return new SkillHeroDto()
            {
                Id = skill.Id,
                Name = skill.Name,
                BaseAttack = skill.BaseAttack,
                IncreasingState = skill.IncreasingState.AsDto()
            };
        }

        public static Enemy AsEntity(this EnemyDto enemyDto)
        {
            return new Enemy(
                enemyDto.Id,
                enemyDto.EnemyName,
                enemyDto.BaseHealth.AsEntity(),
                enemyDto.BaseAttack.AsEntity(),
                enemyDto.BaseHealLvl.AsEntity(),
                enemyDto.Experience.AsEntity(),
                enemyDto.Difficulty,
                enemyDto.Category,
                enemyDto.Skills.Select(s => s.AsEntity())
            );
        }

        public static EnemyDto AsDto(this Enemy enemy)
        {
            return new EnemyDto()
            {
                Id = enemy.Id,
                EnemyName = enemy.EnemyName,
                Difficulty = enemy.Difficulty.ToString(),
                BaseAttack = enemy.BaseAttack.AsDto(),
                BaseHealLvl = enemy.BaseHealLvl.AsDto(),
                BaseHealth = enemy.BaseHealth.AsDto(),
                Experience = enemy.Experience.AsDto(),
                Category = enemy.Category.ToString(),
                Skills = enemy.Skills.Select(s => s.AsDto())
            };
        }

        public static EnemyDetailsDto AsDetailsDto(this Enemy enemy)
        {
            return new EnemyDetailsDto()
            {
                Id = enemy.Id,
                EnemyName = enemy.EnemyName,
                BaseAttack = enemy.BaseAttack.AsDto(),
                BaseHealLvl = enemy.BaseHealLvl.AsDto(),
                BaseHealth = enemy.BaseHealth.AsDto(),
                Difficulty = enemy.Difficulty.ToString(),
                Category = enemy.Category.ToString(),
                Experience = enemy.Experience.AsDto(),
                Skills = enemy.Skills.Select(s => s.AsDto()),
                MapsAssignedTo = enemy.MapsAssignedTo
            };
        }

        public static MapDto AsDto(this Map map)
        {
            return new MapDto()
            {
                Id = map.Id,
                Name = map.Name,
                Enemies = map.Enemies.Select(e => e.AsDto()),
                Difficulty = map.Difficulty.ToString()
            };
        }

        public static EnemiesDto AsDto(this Enemies enemies)
        {
            return new EnemiesDto()
            {
                Enemy = enemies.Enemy.AsDto(),
                Quantity = enemies.Quantity
            };
        }

        public static EnemyAssignDto AsDto(this EnemyAssign enemyAssign)
        {
            return new EnemyAssignDto()
            {
                Id = enemyAssign.Id,
                EnemyName = enemyAssign.EnemyName,
                BaseAttack = enemyAssign.Attack,
                BaseHealLvl = enemyAssign.HealLvl,
                BaseHealth = enemyAssign.Health,
                Difficulty = enemyAssign.Difficulty.ToString(),
                Experience = enemyAssign.Experience,
                Category = enemyAssign.Category.ToString(),
                Skills = enemyAssign.Skills.Select(s => s.AsDto())
            };
        }

        public static SkillEnemyAssignDto AsDto(this SkillEnemyAssign skillEnemyAssign)
        {
            return new SkillEnemyAssignDto()
            {
                Id = skillEnemyAssign.Id,
                Name = skillEnemyAssign.Name,
                BaseAttack = skillEnemyAssign.Attack,
                Probability = skillEnemyAssign.Probability
            };
        }

        public static Enemies AsEntity(this EnemiesDto enemiesDto)
        {
            return new Enemies(enemiesDto.Enemy.AsEntity(), enemiesDto.Quantity);
        }

        public static EnemyAssign AsEntity(this EnemyAssignDto enemyDto)
        {
            return new EnemyAssign(enemyDto.Id, enemyDto.EnemyName, enemyDto.BaseAttack,
                enemyDto.BaseHealth, enemyDto.BaseHealLvl, enemyDto.Experience,
                enemyDto.Difficulty, enemyDto.Category, enemyDto.Skills?.Select(s => s.AsEntity()));
        }

        public static SkillEnemyAssign AsEntity(this SkillEnemyAssignDto skillEnemyAssignDto)
        {
            return new SkillEnemyAssign(skillEnemyAssignDto.Id, skillEnemyAssignDto.Name,
                skillEnemyAssignDto.BaseAttack, skillEnemyAssignDto.Probability);
        }

        public static EnemyAssign AsAssign(this Enemy enemy)
        {
            return new EnemyAssign(enemy.Id, enemy.EnemyName, enemy.BaseAttack.Value, enemy.BaseHealth.Value,
                enemy.BaseHealLvl.Value, enemy.Experience.Value, enemy.Difficulty.ToString(),
                enemy.Category.ToString(), enemy.Skills.Select(s => s.AsAssign()));
        }

        public static SkillEnemyAssign AsAssign(this SkillEnemy skillEnemy)
        {
            return new SkillEnemyAssign(skillEnemy.Id, skillEnemy.Name, skillEnemy.BaseAttack,
                skillEnemy.Probability);
        }

        public static BattleDetailsDto AsDetailsDto(this Battle battle)
        {
            return new BattleDetailsDto
            {
                Id = battle.Id,
                BattleInfo = battle.BattleInfo.ToString(),
                StartDate = battle.StartDate,
                EndDate = battle.EndDate,
                Map = battle.Map.AsDto(),
                EnemiesKilled = battle.EnemiesKilled,
                BattleStates = battle.BattleStates.Select(bs => bs.AsDto()),
                UserId = battle.UserId
            };
        }

        public static BattleStateDto AsDto(this BattleState battleState)
        {
            return new BattleStateDto
            {
                Id = battleState.Id,
                BattleStatus = battleState.BattleStatus.ToString(),
                BattleId = battleState.BattleId,
                Created = battleState.Created,
                Player = battleState.Player.AsDto(),
                Modified = battleState.Modified
            };
        }

        public static BattleEventDto AsDto(this BattleEvent battleEvent)
        {
            return new BattleEventDto
            { 
                Id = battleEvent.Id,
                Action = battleEvent.Action.AsDto(),
                BattleId = battleEvent.BattleId,
                Created = battleEvent.Created
            };
        }

        public static FightActionDto AsDto(this FightAction fightAction)
        {
            return new FightActionDto
            {
                CharacterId = fightAction.CharacterId,
                Character = fightAction.Character.ToString(),
                Name = fightAction.Name,
                AttackInfo = fightAction.AttackInfo,
                DamageDealt = fightAction.DamageDealt,
                Health = fightAction.Health
            };
        }
    }
}
