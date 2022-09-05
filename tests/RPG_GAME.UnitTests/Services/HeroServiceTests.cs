using FluentAssertions;
using Moq;
using RPG_GAME.Application.DTO.Common;
using RPG_GAME.Application.DTO.Heroes;
using RPG_GAME.Application.Events;
using RPG_GAME.Application.Exceptions.Heroes;
using RPG_GAME.Application.Mappings;
using RPG_GAME.Application.Messaging;
using RPG_GAME.Application.Services;
using RPG_GAME.Core.Entities.Heroes;
using RPG_GAME.Core.Repositories;
using RPG_GAME.UnitTests.Fixtures;
using System;
using System.Threading.Tasks;
using Xunit;

namespace RPG_GAME.UnitTests.Services
{
    public class HeroServiceTests
    {
        [Fact]
        public async Task should_add()
        {
            var hero = new HeroDto{
                Id = Guid.NewGuid(), HeroName = "Hero",
                Attack = new StateDto<int> { Value = 10, IncreasingState = new IncreasingStateDto<int> { Value = 2, StrategyIncreasing = "ADDITIVE" } },
                Health = new StateDto<int> { Value = 20, IncreasingState = new IncreasingStateDto<int> { Value = 5, StrategyIncreasing = "ADDITIVE" } },
                HealLvl = new StateDto<int> { Value = 5, IncreasingState = new IncreasingStateDto<int> { Value = 1, StrategyIncreasing = "ADDITIVE" } },
                BaseRequiredExperience = new StateDto<decimal> { Value = 100, IncreasingState = new IncreasingStateDto<decimal> { Value = 20, StrategyIncreasing = "ADDITIVE" } }
                };

            await _heroService.AddAsync(hero);

            _heroRepository.Verify(h => h.AddAsync(It.IsAny<Hero>()), times: Times.Once);
        }

        [Fact]
        public async Task given_null_attack_when_add_should_throw_exception()
        {
            var hero = new HeroDto
            {
                Attack = null,
                Health = new StateDto<int> { Value = 20, IncreasingState = new IncreasingStateDto<int> { Value = 5, StrategyIncreasing = "ADDITIVE" } },
                HealLvl = new StateDto<int> { Value = 5, IncreasingState = new IncreasingStateDto<int> { Value = 1, StrategyIncreasing = "ADDITIVE" } },
                BaseRequiredExperience = new StateDto<decimal> { Value = 100, IncreasingState = new IncreasingStateDto<decimal> { Value = 20, StrategyIncreasing = "ADDITIVE" } }
            };
            var expectedException = new InvalidHeroStateException(nameof(HeroDto.Attack));

            var exception = await Record.ExceptionAsync(() => _heroService.AddAsync(hero));

            exception.Should().NotBeNull();
            exception.Should().BeOfType(expectedException.GetType());
            exception.Message.Should().Be(expectedException.Message);
        }

        [Fact]
        public async Task given_attack_with_null_increasing_state_when_add_should_throw_exception()
        {
            var hero = new HeroDto
            {
                Attack = new StateDto<int> { Value = 20 },
                Health = new StateDto<int> { Value = 20, IncreasingState = new IncreasingStateDto<int> { Value = 5, StrategyIncreasing = "ADDITIVE" } },
                HealLvl = new StateDto<int> { Value = 5, IncreasingState = new IncreasingStateDto<int> { Value = 1, StrategyIncreasing = "ADDITIVE" } },
                BaseRequiredExperience = new StateDto<decimal> { Value = 100, IncreasingState = new IncreasingStateDto<decimal> { Value = 20, StrategyIncreasing = "ADDITIVE" } }
            };
            var expectedException = new InvalidHeroIncreasingStateException(nameof(HeroDto.Attack));

            var exception = await Record.ExceptionAsync(() => _heroService.AddAsync(hero));

            exception.Should().NotBeNull();
            exception.Should().BeOfType(expectedException.GetType());
            exception.Message.Should().Be(expectedException.Message);
        }

        [Fact]
        public async Task given_attack_with_invalid_increasing_strategy_when_add_should_throw_exception()
        {
            var hero = new HeroDto
            {
                Attack = new StateDto<int> { Value = 20, IncreasingState = new IncreasingStateDto<int> { Value = 5, StrategyIncreasing = "" } },
                Health = new StateDto<int> { Value = 20, IncreasingState = new IncreasingStateDto<int> { Value = 5, StrategyIncreasing = "ADDITIVE" } },
                HealLvl = new StateDto<int> { Value = 5, IncreasingState = new IncreasingStateDto<int> { Value = 1, StrategyIncreasing = "ADDITIVE" } },
                BaseRequiredExperience = new StateDto<decimal> { Value = 100, IncreasingState = new IncreasingStateDto<decimal> { Value = 20, StrategyIncreasing = "ADDITIVE" } }
            };
            var expectedException = new InvalidHeroStrategyIncreasingException(hero.Attack.IncreasingState.StrategyIncreasing);

            var exception = await Record.ExceptionAsync(() => _heroService.AddAsync(hero));

            exception.Should().NotBeNull();
            exception.Should().BeOfType(expectedException.GetType());
            exception.Message.Should().Be(expectedException.Message);
        }

        [Fact]
        public async Task should_update()
        {
            var hero = EntitiesFixture.CreateDefaultHero();
            var dto = hero.AsDto();
            var attack = 200;
            dto.Attack.Value = attack;
            _heroRepository.Setup(h => h.GetAsync(hero.Id)).ReturnsAsync(hero);

            await _heroService.UpdateAsync(dto);

            _heroRepository.Verify(h => h.UpdateAsync(It.IsAny<Hero>()), times: Times.Once);
            _messageBroker.Verify(h => h.PublishAsync(It.IsAny<IEvent>()), times: Times.Once);
        }

        [Fact]
        public async Task given_not_existed_hero_when_update_should_throw_an_exception()
        {
            var hero = EntitiesFixture.CreateDefaultHero();
            var dto = hero.AsDto();
            var attack = 200;
            dto.Attack.Value = attack;
            var expectedException = new HeroNotFoundException(dto.Id);

            var exception = await Record.ExceptionAsync(() => _heroService.UpdateAsync(dto));

            exception.Should().NotBeNull();
            exception.Should().BeOfType(expectedException.GetType());
            exception.Message.Should().Be(expectedException.Message);
        }

        [Fact]
        public async Task should_delete()
        {
            var hero = EntitiesFixture.CreateDefaultHero();
            _heroRepository.Setup(h => h.GetAsync(hero.Id)).ReturnsAsync(hero);

            await _heroService.RemoveAsync(hero.Id);

            _heroRepository.Verify(h => h.DeleteAsync(hero.Id), times: Times.Once);
        }

        [Fact]
        public async Task given_not_existed_hero_when_delete_should_throw_an_exception()
        {
            var heroId = Guid.NewGuid();
            var expectedException = new HeroNotFoundException(heroId);

            var exception = await Record.ExceptionAsync(() => _heroService.RemoveAsync(heroId));

            exception.Should().NotBeNull();
            exception.Should().BeOfType(expectedException.GetType());
            exception.Message.Should().Be(expectedException.Message);
        }

        [Fact]
        public async Task given_hero_assigned_to_players_when_delete_should_throw_an_exception()
        {
            var hero = EntitiesFixture.CreateDefaultHero();
            hero.AddPlayer(Guid.NewGuid());
            _heroRepository.Setup(h => h.GetAsync(hero.Id)).ReturnsAsync(hero);
            var expectedException = new HeroCannotBeDeletedException(hero.Id);

            var exception = await Record.ExceptionAsync(() => _heroService.RemoveAsync(hero.Id));

            exception.Should().NotBeNull();
            exception.Should().BeOfType(expectedException.GetType());
            exception.Message.Should().Be(expectedException.Message);
        }

        private readonly IHeroService _heroService;
        private readonly Mock<IHeroRepository> _heroRepository;
        private readonly Mock<IMessageBroker> _messageBroker;

        public HeroServiceTests()
        {
            _heroRepository = new Mock<IHeroRepository>();
            _messageBroker = new Mock<IMessageBroker>();
            _heroService = new HeroService(_heroRepository.Object, _messageBroker.Object);
        }
    }
}
