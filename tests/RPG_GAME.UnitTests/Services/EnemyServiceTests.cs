using FluentAssertions;
using Moq;
using RPG_GAME.Application.DTO.Common;
using RPG_GAME.Application.DTO.Enemies;
using RPG_GAME.Application.Events;
using RPG_GAME.Application.Exceptions.Enemies;
using RPG_GAME.Application.Mappings;
using RPG_GAME.Application.Messaging;
using RPG_GAME.Application.Services;
using RPG_GAME.Core.Entities.Enemies;
using RPG_GAME.Core.Repositories;
using RPG_GAME.UnitTests.Fixtures;
using System;
using System.Threading.Tasks;
using Xunit;

namespace RPG_GAME.UnitTests.Services
{
    public class EnemyServiceTests
    {
        [Fact]
        public async Task should_add()
        {
            var enemy = new EnemyDto
            {
                Id = Guid.NewGuid(),
                EnemyName = "Enemy",
                BaseAttack = new StateDto<int> { Value = 10, IncreasingState = new IncreasingStateDto<int> { Value = 2, StrategyIncreasing = "ADDITIVE" } },
                BaseHealth = new StateDto<int> { Value = 20, IncreasingState = new IncreasingStateDto<int> { Value = 5, StrategyIncreasing = "ADDITIVE" } },
                BaseHealLvl = new StateDto<int> { Value = 5, IncreasingState = new IncreasingStateDto<int> { Value = 1, StrategyIncreasing = "ADDITIVE" } },
                Experience = new StateDto<decimal> { Value = 100, IncreasingState = new IncreasingStateDto<decimal> { Value = 20, StrategyIncreasing = "ADDITIVE" } },
                Difficulty = "EASY"
            };

            await _enemyService.AddAsync(enemy);

            _enemyRepository.Verify(h => h.AddAsync(It.IsAny<Enemy>()), times: Times.Once);
        }

        [Fact]
        public async Task given_null_attack_when_add_should_throw_exception()
        {
            var enemy = new EnemyDto
            {
                BaseAttack = null,
                BaseHealth = new StateDto<int> { Value = 20, IncreasingState = new IncreasingStateDto<int> { Value = 5, StrategyIncreasing = "ADDITIVE" } },
                BaseHealLvl = new StateDto<int> { Value = 5, IncreasingState = new IncreasingStateDto<int> { Value = 1, StrategyIncreasing = "ADDITIVE" } },
                Experience = new StateDto<decimal> { Value = 100, IncreasingState = new IncreasingStateDto<decimal> { Value = 20, StrategyIncreasing = "ADDITIVE" } },
                Difficulty = "HARD"
            };
            var expectedException = new InvalidEnemyStateException();

            var exception = await Record.ExceptionAsync(() => _enemyService.AddAsync(enemy));

            exception.Should().NotBeNull();
            exception.Should().BeOfType(expectedException.GetType());
            exception.Message.Should().Be(expectedException.Message);
        }

        [Fact]
        public async Task given_invalid_difficulty_when_add_should_throw_exception()
        {
            var enemy = new EnemyDto
            {
                BaseAttack = new StateDto<int> { Value = 10, IncreasingState = new IncreasingStateDto<int> { Value = 2, StrategyIncreasing = "ADDITIVE" } },
                BaseHealth = new StateDto<int> { Value = 20, IncreasingState = new IncreasingStateDto<int> { Value = 5, StrategyIncreasing = "ADDITIVE" } },
                BaseHealLvl = new StateDto<int> { Value = 5, IncreasingState = new IncreasingStateDto<int> { Value = 1, StrategyIncreasing = "ADDITIVE" } },
                Experience = new StateDto<decimal> { Value = 100, IncreasingState = new IncreasingStateDto<decimal> { Value = 20, StrategyIncreasing = "ADDITIVE" } },
                Difficulty = "abc"
            };
            var expectedException = new InvalidEnemyDifficultyException(enemy.Difficulty);

            var exception = await Record.ExceptionAsync(() => _enemyService.AddAsync(enemy));

            exception.Should().NotBeNull();
            exception.Should().BeOfType(expectedException.GetType());
            exception.Message.Should().Be(expectedException.Message);
        }

        [Fact]
        public async Task given_attack_with_null_increasing_state_when_add_should_throw_exception()
        {
            var enemy = new EnemyDto
            {
                BaseAttack = new StateDto<int> { Value = 20 },
                BaseHealth = new StateDto<int> { Value = 20, IncreasingState = new IncreasingStateDto<int> { Value = 5, StrategyIncreasing = "ADDITIVE" } },
                BaseHealLvl = new StateDto<int> { Value = 5, IncreasingState = new IncreasingStateDto<int> { Value = 1, StrategyIncreasing = "ADDITIVE" } },
                Experience = new StateDto<decimal> { Value = 100, IncreasingState = new IncreasingStateDto<decimal> { Value = 20, StrategyIncreasing = "ADDITIVE" } },
                Difficulty = "MEDIUM"
            };
            var expectedException = new InvalidEnemyIncreasingStateException();

            var exception = await Record.ExceptionAsync(() => _enemyService.AddAsync(enemy));

            exception.Should().NotBeNull();
            exception.Should().BeOfType(expectedException.GetType());
            exception.Message.Should().Be(expectedException.Message);
        }

        [Fact]
        public async Task given_attack_with_invalid_increasing_strategy_when_add_should_throw_exception()
        {
            var enemy = new EnemyDto
            {
                BaseAttack = new StateDto<int> { Value = 20, IncreasingState = new IncreasingStateDto<int> { Value = 5, StrategyIncreasing = "" } },
                BaseHealth = new StateDto<int> { Value = 20, IncreasingState = new IncreasingStateDto<int> { Value = 5, StrategyIncreasing = "ADDITIVE" } },
                BaseHealLvl = new StateDto<int> { Value = 5, IncreasingState = new IncreasingStateDto<int> { Value = 1, StrategyIncreasing = "ADDITIVE" } },
                Experience = new StateDto<decimal> { Value = 100, IncreasingState = new IncreasingStateDto<decimal> { Value = 20, StrategyIncreasing = "ADDITIVE" } },
                Difficulty = "MEDIUM"
            };
            var expectedException = new InvalidEnemyStrategyIncreasingException(enemy.BaseAttack.IncreasingState.StrategyIncreasing);

            var exception = await Record.ExceptionAsync(() => _enemyService.AddAsync(enemy));

            exception.Should().NotBeNull();
            exception.Should().BeOfType(expectedException.GetType());
            exception.Message.Should().Be(expectedException.Message);
        }

        [Fact]
        public async Task should_update()
        {
            var enemy = EntitiesFixture.CreateDefaultEnemy();
            var dto = enemy.AsDto();
            var attack = 200;
            dto.BaseAttack.Value = attack;
            _enemyRepository.Setup(h => h.GetAsync(enemy.Id)).ReturnsAsync(enemy);

            await _enemyService.UpdateAsync(dto);

            _enemyRepository.Verify(h => h.UpdateAsync(It.IsAny<Enemy>()), times: Times.Once);
            _messageBroker.Verify(h => h.PublishAsync(It.IsAny<IEvent>()), times: Times.Once);
        }

        [Fact]
        public async Task given_not_existed_enemy_when_update_should_throw_an_exception()
        {
            var enemy = EntitiesFixture.CreateDefaultEnemy();
            var dto = enemy.AsDto();
            var attack = 200;
            dto.BaseAttack.Value = attack;
            var expectedException = new EnemyNotFoundException(dto.Id);

            var exception = await Record.ExceptionAsync(() => _enemyService.UpdateAsync(dto));

            exception.Should().NotBeNull();
            exception.Should().BeOfType(expectedException.GetType());
            exception.Message.Should().Be(expectedException.Message);
        }

        [Fact]
        public async Task should_delete()
        {
            var enemy = EntitiesFixture.CreateDefaultEnemy();
            _enemyRepository.Setup(h => h.GetAsync(enemy.Id)).ReturnsAsync(enemy);

            await _enemyService.RemoveAsync(enemy.Id);

            _enemyRepository.Verify(h => h.DeleteAsync(enemy.Id), times: Times.Once);
        }

        [Fact]
        public async Task given_not_existed_enemy_when_delete_should_throw_an_exception()
        {
            var enemyId = Guid.NewGuid();
            var expectedException = new EnemyNotFoundException(enemyId);

            var exception = await Record.ExceptionAsync(() => _enemyService.RemoveAsync(enemyId));

            exception.Should().NotBeNull();
            exception.Should().BeOfType(expectedException.GetType());
            exception.Message.Should().Be(expectedException.Message);
        }

        [Fact]
        public async Task given_enemy_assigned_to_players_when_delete_should_throw_an_exception()
        {
            var enemy = EntitiesFixture.CreateDefaultEnemy();
            enemy.AddMap(Guid.NewGuid());
            _enemyRepository.Setup(h => h.GetAsync(enemy.Id)).ReturnsAsync(enemy);
            var expectedException = new EnemyCannotBeDeletedException(enemy.Id);

            var exception = await Record.ExceptionAsync(() => _enemyService.RemoveAsync(enemy.Id));

            exception.Should().NotBeNull();
            exception.Should().BeOfType(expectedException.GetType());
            exception.Message.Should().Be(expectedException.Message);
        }

        private readonly IEnemyService _enemyService;
        private readonly Mock<IEnemyRepository> _enemyRepository;
        private readonly Mock<IMessageBroker> _messageBroker;

        public EnemyServiceTests()
        {
            _enemyRepository = new Mock<IEnemyRepository>();
            _messageBroker = new Mock<IMessageBroker>();
            _enemyService = new EnemyService(_enemyRepository.Object, _messageBroker.Object);
        }
    }
}
