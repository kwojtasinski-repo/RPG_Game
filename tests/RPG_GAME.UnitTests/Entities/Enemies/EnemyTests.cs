using FluentAssertions;
using RPG_GAME.Core.Entities.Common;
using RPG_GAME.Core.Entities.Enemies;
using RPG_GAME.Core.Exceptions.Enemies;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace RPG_GAME.UnitTests.Entities.Enemies
{
    public class EnemyTests
    {
        private Enemy Act(string enemyName, int health, int attack, int healLvl, decimal experience,
            string difficulty, string category = "Archer", IEnumerable<SkillEnemy> skills = null)
                => new Enemy(Guid.NewGuid(), enemyName, new State<int>(health, DefaultIncreasingState<int>()), new State<int>(attack, DefaultIncreasingState<int>()),
                    new State<int>(healLvl, DefaultIncreasingState<int>()), new State<decimal>(experience, DefaultIncreasingState<decimal>()),
                    difficulty, category, skills);

        private Enemy Act(string enemyName, int health, IncreasingState<int> healthIncreasing, int attack, IncreasingState<int> attackIncreasing, int healLvl, IncreasingState<int> healLvlIncreasing,
            decimal experience, IncreasingState<decimal> experienceIncreasing, string difficulty, string category = "Knight", IEnumerable<SkillEnemy> skills = null)
                => new Enemy(Guid.NewGuid(), enemyName, new State<int>(health, healthIncreasing), new State<int>(attack, attackIncreasing),
                    new State<int>(healLvl, healLvlIncreasing), new State<decimal>(experience, experienceIncreasing),
                    difficulty, category, skills);

        [Fact]
        public void should_create_enemy()
        {
            var name = "Enemy #1";
            var health = 100;
            var attack = 20;
            var heal = 5;
            var experience = 50M;
            var difficulty = "EASY";

            var enemy = Act(name, health, attack, heal, experience, difficulty);

            enemy.Should().NotBeNull();
            enemy.Id.Should().NotBe(Guid.Empty);
            enemy.EnemyName.Should().Be(name);
            enemy.BaseAttack.Value.Should().Be(attack);
            enemy.BaseHealth.Value.Should().Be(health);
            enemy.BaseHealLvl.Value.Should().Be(heal);
            enemy.Experience.Value.Should().Be(experience);
            enemy.Difficulty.ToString().Should().Be(difficulty);
        }

        [Fact]
        public void should_create_enemy_with_skills()
        {
            var name = "Enemy #1";
            var health = 100;
            var healthIncrease = new IncreasingState<int>(25, "ADDITIVE");
            var attack = 20;
            var attackIncrease = new IncreasingState<int>(5, "ADDITIVE");
            var heal = 5;
            var healIncrease = new IncreasingState<int>(2, "ADDITIVE");
            var experience = 50M;
            var experienceIncrease = new IncreasingState<decimal>(15, "ADDITIVE");
            var difficulty = "EASY";
            var category = "Archer";
            var skills = new List<SkillEnemy> { new SkillEnemy(Guid.NewGuid(), "Name#1", 30, 10, new IncreasingState<int>(10, "ADDITIVE")) };

            var enemy = Act(name, health, healthIncrease, attack, attackIncrease, heal, healIncrease,
                experience, experienceIncrease, difficulty, category, skills);

            enemy.Should().NotBeNull();
            enemy.Id.Should().NotBe(Guid.Empty);
            enemy.EnemyName.Should().Be(name);
            enemy.BaseAttack.Value.Should().Be(attack);
            enemy.BaseAttack.IncreasingState.Value.Should().Be(attackIncrease.Value);
            enemy.BaseHealLvl.Value.Should().Be(heal);
            enemy.BaseHealLvl.IncreasingState.Value.Should().Be(healIncrease.Value);
            enemy.BaseHealth.Value.Should().Be(health);
            enemy.BaseHealth.IncreasingState.Value.Should().Be(healthIncrease.Value);
            enemy.Experience.Value.Should().Be(experience);
            enemy.Experience.IncreasingState.Value.Should().Be(experienceIncrease.Value);
            enemy.Difficulty.ToString().Should().Be(difficulty);
            enemy.Skills.Should().NotBeNull();
            enemy.Skills.Should().NotBeEmpty();
            enemy.Skills.First().Name.Should().Be(skills.First().Name);
        }

        [Fact]
        public void given_empty_name_should_throw_an_exception()
        {
            var name = "";
            var health = 100;
            var attack = 20;
            var heal = 5;
            var experience = 50M;
            var difficulty = "EASY";
            var expectedException = new InvalidEnemyNameException();

            var exception = Record.Exception(() => Act(name, health, attack, heal, experience, difficulty));

            exception.Should().NotBeNull();
            exception.Should().BeOfType(expectedException.GetType());
            exception.Message.Should().Be(expectedException.Message);
        }

        [Fact]
        public void given_too_short_name_should_throw_an_exception()
        {
            var name = "t";
            var health = 100;
            var attack = 20;
            var heal = 5;
            var experience = 50M;
            var difficulty = "EASY";
            var expectedException = new TooShortEnemyNameException(name);

            var exception = Record.Exception(() => Act(name, health, attack, heal, experience, difficulty));

            exception.Should().NotBeNull();
            exception.Should().BeOfType(expectedException.GetType());
            exception.Message.Should().Be(expectedException.Message);
        }

        [Fact]
        public void given_null_health_should_throw_an_exception()
        {
            var name = "AbCDeF";
            var attack = 20;
            var heal = 5;
            var experience = 50M;
            var difficulty = "EASY";
            var category = "Dragon";
            var expectedException = new InvalidEnemyHealthException();

            var exception = Record.Exception(() => new Enemy(Guid.NewGuid(), name, null, new State<int>(attack, DefaultIncreasingState<int>()),
                    new State<int>(heal, DefaultIncreasingState<int>()), new State<decimal>(experience, DefaultIncreasingState<decimal>()),
                    difficulty, category));

            exception.Should().NotBeNull();
            exception.Should().BeOfType(expectedException.GetType());
            exception.Message.Should().Be(expectedException.Message);
        }

        [Fact]
        public void given_invalid_health_should_throw_an_exception()
        {
            var name = "AbCDeF";
            var health = 0;
            var attack = 20;
            var heal = 5;
            var experience = 50M;
            var difficulty = "EASY";
            var expectedException = new EnemyHealthCannotBeZeroOrNegativeException(health);

            var exception = Record.Exception(() => Act(name, health, attack, heal, experience, difficulty));

            exception.Should().NotBeNull();
            exception.Should().BeOfType(expectedException.GetType());
            exception.Message.Should().Be(expectedException.Message);
        }

        [Fact]
        public void given_valid_health_should_change_value()
        {
            var name = "Enemy #1";
            var health = 100;
            var healthModified = 120;
            var attack = 20;
            var heal = 5;
            var experience = 50M;
            var difficulty = "EASY";
            var enemy = Act(name, health, attack, heal, experience, difficulty);

            enemy.ChangeHealth(new State<int>(healthModified, DefaultIncreasingState<int>()));

            enemy.BaseHealth.Value.Should().Be(healthModified);
        }

        [Fact]
        public void given_null_attack_should_throw_an_exception()
        {
            var name = "AbCDeF";
            var health = 200;
            var heal = 5;
            var experience = 50M;
            var difficulty = "EASY";
            var category = "Archer";
            var expectedException = new InvalidEnemyAttackException();

            var exception = Record.Exception(() => new Enemy(Guid.NewGuid(), name, new State<int>(health, DefaultIncreasingState<int>()), null,
                    new State<int>(heal, DefaultIncreasingState<int>()), new State<decimal>(experience, DefaultIncreasingState<decimal>()),
                    difficulty, category));

            exception.Should().NotBeNull();
            exception.Should().BeOfType(expectedException.GetType());
            exception.Message.Should().Be(expectedException.Message);
        }

        [Fact]
        public void given_invalid_attack_should_throw_an_exception()
        {
            var name = "AbCDeF";
            var health = 120;
            var attack = 0;
            var heal = 5;
            var experience = 50M;
            var difficulty = "EASY";
            var expectedException = new EnemyAttackCannotBeZeroOrNegativeException(attack);

            var exception = Record.Exception(() => Act(name, health, attack, heal, experience, difficulty));

            exception.Should().NotBeNull();
            exception.Should().BeOfType(expectedException.GetType());
            exception.Message.Should().Be(expectedException.Message);
        }

        [Fact]
        public void given_valid_attack_should_change_value()
        {
            var name = "Enemy #1";
            var health = 100;
            var attack = 20;
            var attackModified = 20;
            var heal = 5;
            var experience = 50M;
            var difficulty = "EASY";
            var enemy = Act(name, health, attack, heal, experience, difficulty);

            enemy.ChangeAttack(new State<int>(attackModified, DefaultIncreasingState<int>()));

            enemy.BaseAttack.Value.Should().Be(attackModified);
        }

        [Fact]
        public void given_null_heal_should_throw_an_exception()
        {
            var name = "AbCDeF";
            var health = 5;
            var attack = 20;
            var experience = 50M;
            var difficulty = "EASY";
            var category = "Knight";
            var expectedException = new InvalidEnemyHealLvlException();

            var exception = Record.Exception(() => new Enemy(Guid.NewGuid(), name, new State<int>(health, DefaultIncreasingState<int>()), new State<int>(attack, DefaultIncreasingState<int>()),
                    null, new State<decimal>(experience, DefaultIncreasingState<decimal>()),
                    difficulty,category));

            exception.Should().NotBeNull();
            exception.Should().BeOfType(expectedException.GetType());
            exception.Message.Should().Be(expectedException.Message);
        }

        [Fact]
        public void given_invalid_heal_should_throw_an_exception()
        {
            var name = "AbCDeF";
            var health = 210;
            var attack = 20;
            var heal = 0;
            var experience = 50M;
            var difficulty = "EASY";
            var expectedException = new EnemyHealLvlCannotBeZeroOrNegativeException(heal);

            var exception = Record.Exception(() => Act(name, health, attack, heal, experience, difficulty));

            exception.Should().NotBeNull();
            exception.Should().BeOfType(expectedException.GetType());
            exception.Message.Should().Be(expectedException.Message);
        }

        [Fact]
        public void given_valid_heal_should_change_value()
        {
            var name = "Enemy #1";
            var health = 100;
            var attack = 20;
            var heal = 5;
            var healModified = 120;
            var experience = 50M;
            var difficulty = "EASY";
            var enemy = Act(name, health, attack, heal, experience, difficulty);

            enemy.ChangeHealLvl(new State<int>(healModified, DefaultIncreasingState<int>()));

            enemy.BaseHealLvl.Value.Should().Be(healModified);
        }

        [Fact]
        public void given_null_experience_should_throw_an_exception()
        {
            var name = "AbCDeF";
            var health = 100;
            var attack = 20;
            var heal = 5;
            var difficulty = "EASY";
            var category = "Archer";
            var expectedException = new InvalidEnemyExperienceException();

            var exception = Record.Exception(() => new Enemy(Guid.NewGuid(), name, new State<int>(health, DefaultIncreasingState<int>()), new State<int>(attack, DefaultIncreasingState<int>()),
                    new State<int>(heal, DefaultIncreasingState<int>()), null, difficulty, category));

            exception.Should().NotBeNull();
            exception.Should().BeOfType(expectedException.GetType());
            exception.Message.Should().Be(expectedException.Message);
        }

        [Fact]
        public void given_invalid_experience_should_throw_an_exception()
        {
            var name = "AbCDeF";
            var health = 210;
            var attack = 20;
            var heal = 5;
            var experience = 0M;
            var difficulty = "EASY";
            var expectedException = new EnemyExperienceCannotBeZeroOrNegativeException(experience);

            var exception = Record.Exception(() => Act(name, health, attack, heal, experience, difficulty));

            exception.Should().NotBeNull();
            exception.Should().BeOfType(expectedException.GetType());
            exception.Message.Should().Be(expectedException.Message);
        }

        [Fact]
        public void given_valid_experience_should_change_value()
        {
            var name = "Enemy #1";
            var health = 100;
            var attack = 20;
            var heal = 5;
            var experience = 50M;
            var experienceModified = 50M;
            var difficulty = "EASY";
            var enemy = Act(name, health, attack, heal, experience, difficulty);

            enemy.ChangeExperience(new State<decimal>(experienceModified, DefaultIncreasingState<decimal>()));

            enemy.Experience.Value.Should().Be(experienceModified);
        }

        [Fact]
        public void given_invalid_difficulty_should_throw_an_exception()
        {
            var name = "AbCDeF";
            var health = 210;
            var attack = 20;
            var heal = 10;
            var experience = 50M;
            var difficulty = "abcas";
            var expectedException = new InvalidEnemyDifficultyException(difficulty);

            var exception = Record.Exception(() => Act(name, health, attack, heal, experience, difficulty));

            exception.Should().NotBeNull();
            exception.Should().BeOfType(expectedException.GetType());
            exception.Message.Should().Be(expectedException.Message);
        }

        [Fact]
        public void given_valid_difficulty_should_change()
        {
            var name = "AbCDeF";
            var health = 210;
            var attack = 20;
            var heal = 10;
            var experience = 50M;
            var difficulty = "EASY";
            var difficultyModified = "HARD";
            var enemy = Act(name, health, attack, heal, experience, difficulty);

            enemy.ChangeDifficulty(difficultyModified);

            enemy.Difficulty.ToString().Should().Be(difficultyModified);
        }

        [Fact]
        public void given_valid_skill_should_add()
        {
            var enemy = CreateDefaultEnemy();
            var skill = new SkillEnemy(Guid.NewGuid(), "Name#1", 100, 10, DefaultIncreasingState<int>());

            enemy.AddSkill(skill);

            enemy.Skills.Should().NotBeEmpty();
            enemy.Skills.Should().HaveCount(1);
        }

        [Fact]
        public void given_same_as_added_skill_should_throw_an_exception()
        {
            var enemy = CreateDefaultEnemy();
            var skill = new SkillEnemy(Guid.NewGuid(), "Name#1", 100, 10, DefaultIncreasingState<int>());
            var skill2 = new SkillEnemy(skill.Id, "Name#1", 100, 10, DefaultIncreasingState<int>());
            enemy.AddSkill(skill);
            var expectedException = new SkillEnemyAlreadyExistsException(skill2.Id, skill2.Name);

            var exception = Record.Exception(() => enemy.AddSkill(skill2));

            exception.Should().NotBeNull();
            exception.Should().BeOfType(expectedException.GetType());
            exception.Message.Should().Be(expectedException.Message);
        }

        [Fact]
        public void given_null_skill_when_add_should_throw_an_exception()
        {
            var enemy = CreateDefaultEnemy();
            var expectedException = new InvalidSkillEnemyException();

            var exception = Record.Exception(() => enemy.AddSkill(null));

            exception.Should().NotBeNull();
            exception.Should().BeOfType(expectedException.GetType());
            exception.Message.Should().Be(expectedException.Message);
        }

        [Fact]
        public void given_valid_skill_should_remove()
        {
            var enemy = CreateDefaultEnemy();
            var skill = new SkillEnemy(Guid.NewGuid(), "Name#1", 100, 10, DefaultIncreasingState<int>());
            enemy.AddSkill(skill);

            enemy.RemoveSkill(new SkillEnemy(skill.Id, skill.Name, skill.BaseAttack, skill.Probability, skill.IncreasingState));

            enemy.Skills.Should().BeEmpty();
        }

        [Fact]
        public void given_not_existed_skill_when_delete_should_throw_an_exception()
        {
            var enemy = CreateDefaultEnemy();
            var skill = new SkillEnemy(Guid.NewGuid(), "Name#1", 100, 10, DefaultIncreasingState<int>());
            var expectedException = new SkillEnemyDoesntExistsException(skill.Id, skill.Name);

            var exception = Record.Exception(() => enemy.RemoveSkill(skill));

            exception.Should().NotBeNull();
            exception.Should().BeOfType(expectedException.GetType());
            exception.Message.Should().Be(expectedException.Message);
        }

        [Fact]
        public void given_null_skill_should_throw_an_exception()
        {
            var enemy = CreateDefaultEnemy();
            var expectedException = new InvalidSkillEnemyException();

            var exception = Record.Exception(() => enemy.RemoveSkill(null));

            exception.Should().NotBeNull();
            exception.Should().BeOfType(expectedException.GetType());
            exception.Message.Should().Be(expectedException.Message);
        }

        [Fact]
        public void given_default_map_id_when_add_should_throw_an_exception()
        {
            var enemy = CreateDefaultEnemy();
            var expectedException = new InvalidMapIdException();

            var exception = Record.Exception(() => enemy.AddMap(default));

            exception.Should().NotBeNull();
            exception.Should().BeOfType(expectedException.GetType());
            exception.Message.Should().Be(expectedException.Message);
        }

        [Fact]
        public void given_existed_map_id_when_add_should_throw_an_exception()
        {
            var enemy = CreateDefaultEnemy();
            var mapId = Guid.NewGuid();
            enemy.AddMap(mapId);
            var expectedException = new MapAlreadyExistsException(mapId);

            var exception = Record.Exception(() => enemy.AddMap(mapId));

            exception.Should().NotBeNull();
            exception.Should().BeOfType(expectedException.GetType());
            exception.Message.Should().Be(expectedException.Message);
        }

        [Fact]
        public void should_add_map()
        {
            var enemy = CreateDefaultEnemy();
            var mapId = Guid.NewGuid();

            enemy.AddMap(mapId);

            enemy.MapsAssignedTo.Should().NotBeEmpty();
            enemy.MapsAssignedTo.Should().HaveCount(1);
            enemy.MapsAssignedTo.Should().Contain(mapId);
        }

        [Fact]
        public void should_delete_map()
        {
            var enemy = CreateDefaultEnemy();
            var mapId = Guid.NewGuid();
            enemy.AddMap(mapId);

            enemy.RemoveMap(mapId);

            enemy.MapsAssignedTo.Should().BeEmpty();
        }

        [Fact]
        public void given_not_existed_map_id_when_delete_should_throw_an_exception()
        {
            var enemy = CreateDefaultEnemy();
            var mapId = Guid.NewGuid();
            var expectedException = new MapDoesntExistsException(mapId);

            var exception = Record.Exception(() => enemy.RemoveMap(mapId));

            exception.Should().NotBeNull();
            exception.Should().BeOfType(expectedException.GetType());
            exception.Message.Should().Be(expectedException.Message);
        }

        [Fact]
        public void given_default_map_id_when_delete_should_throw_an_exception()
        {
            var enemy = CreateDefaultEnemy();
            var expectedException = new InvalidMapIdException();

            var exception = Record.Exception(() => enemy.RemoveMap(default));

            exception.Should().NotBeNull();
            exception.Should().BeOfType(expectedException.GetType());
            exception.Message.Should().Be(expectedException.Message);
        }

        private static IncreasingState<T> DefaultIncreasingState<T>() 
            where T : struct
        {
            T value = default;

            if (typeof(T) == typeof(decimal))
            {
                object val = 1M;
                value = (T) val;
            }

            if (typeof(T) == typeof(int))
            {
                object val = 1;
                value = (T) val;
            }

            return new IncreasingState<T>(value, "ADDITIVE");
        }

        private static Enemy CreateDefaultEnemy()
        {
            return new Enemy(Guid.NewGuid(), "Enemy", new State<int>(100, DefaultIncreasingState<int>()), new State<int>(100, DefaultIncreasingState<int>()),
                    new State<int>(10, DefaultIncreasingState<int>()), new State<decimal>(1000, DefaultIncreasingState<decimal>()),
                    "EASY", "Knight");
        }
    }
}
