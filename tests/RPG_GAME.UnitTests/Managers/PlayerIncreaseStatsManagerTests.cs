using FluentAssertions;
using RPG_GAME.Application.Managers;
using RPG_GAME.Application.Mappings;
using RPG_GAME.Core.Entities.Common;
using RPG_GAME.Core.Entities.Heroes;
using RPG_GAME.Core.Entities.Players;
using RPG_GAME.UnitTests.Stubs;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace RPG_GAME.UnitTests.Managers
{
    public class PlayerIncreaseStatsManagerTests
    {
        [Fact]
        public void should_calculate_hero_skills()
        {
            var skillHero = new SkillHero(Guid.NewGuid(), "skill", 100, new IncreasingState<int>(10, StrategyIncreasing.PERCENTAGE.ToString()));
            var heroAssign = new HeroAssign(Guid.NewGuid(), "hero", 120, 120, new List<SkillHeroAssign> { skillHero.AsAssign() });
            var level = 5;
            var calculatedSkillAttack = 140;

            _playerIncreaseStatsManager.CalculateHeroSkills(level, heroAssign, new List<SkillHero> { skillHero });

            var skillAfterCalculate = heroAssign.Skills.Single(s => s.Id == skillHero.Id);
            skillAfterCalculate.Attack.Should().Be(calculatedSkillAttack);
        }

        [Fact]
        public void should_hero_stats()
        {
            var skillHero = new SkillHero(Guid.NewGuid(), "skill", 1000, new IncreasingState<int>(10, StrategyIncreasing.PERCENTAGE.ToString()));
            var hero = new Hero(Guid.NewGuid(), "hero", new State<int>(100, new IncreasingState<int>(10, StrategyIncreasing.PERCENTAGE.ToString())), new State<int>(100, new IncreasingState<int>(10, StrategyIncreasing.PERCENTAGE.ToString())),
                    new State<decimal>(1000, new IncreasingState<decimal>(10, StrategyIncreasing.PERCENTAGE.ToString())),
                    new List<SkillHero> { skillHero });
            var heroToCalculate = hero.AsAssign();
            var level = 5;
            var calculatedAttack = 140;
            var calculatedHealth = 140;
            var calculatedHeal = 140;
            var calculatedSkillAttack = 1400;

            _playerIncreaseStatsManager.CalculateHeroStats(level, heroToCalculate, hero);

            var skillAfterCalculate = heroToCalculate.Skills.Single(s => s.Id == skillHero.Id);
            skillAfterCalculate.Attack.Should().Be(calculatedSkillAttack);
            heroToCalculate.Attack.Should().Be(calculatedAttack);
            heroToCalculate.Health.Should().Be(calculatedHealth);
        }

        [Fact]
        public void should_calculate_player_stats()
        {
            var skillHero = new SkillHero(Guid.NewGuid(), "skill", 1000, new IncreasingState<int>(10, StrategyIncreasing.PERCENTAGE.ToString()));
            var hero = new Hero(Guid.NewGuid(), "hero", new State<int>(100, new IncreasingState<int>(10, StrategyIncreasing.PERCENTAGE.ToString())), new State<int>(100, new IncreasingState<int>(10, StrategyIncreasing.PERCENTAGE.ToString())),
                    new State<decimal>(1000, new IncreasingState<decimal>(10, StrategyIncreasing.PERCENTAGE.ToString())),
                    new List<SkillHero> { skillHero });
            var level = 5;
            var player = new Player(Guid.NewGuid(), "player", hero.AsAssign(), level, 0, 1000, Guid.NewGuid());
            var calculatedAttack = 140;
            var calculatedHealth = 140;
            var calculatedHeal = 140;
            var calculatedSkillAttack = 1400;
            var calculatedRequiredExp = 1400;

            _playerIncreaseStatsManager.CalculatePlayerStats(player, hero);


            var skillAfterCalculate = player.Hero.Skills.Single(s => s.Id == skillHero.Id);
            skillAfterCalculate.Attack.Should().Be(calculatedSkillAttack);
            player.Hero.Attack.Should().Be(calculatedAttack);
            player.Hero.Health.Should().Be(calculatedHealth);
            player.RequiredExp.Should().Be(calculatedRequiredExp);
        }

        private readonly IPlayerIncreaseStatsManager _playerIncreaseStatsManager;

        public PlayerIncreaseStatsManagerTests()
        {
            _playerIncreaseStatsManager = new PlayerIncreaseStatsManager(LoggerStub<PlayerIncreaseStatsManager>.Create());
        }
    }
}
