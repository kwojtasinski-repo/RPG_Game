using FluentAssertions;
using RPG_GAME.Application.DTO.Heroes;
using RPG_GAME.Application.Exceptions.Heroes;
using RPG_GAME.Application.Mappings;
using RPG_GAME.Application.Services;
using RPG_GAME.Core.Repositories;
using RPG_GAME.UnitTests.Fixtures;
using RPG_GAME.UnitTests.Stubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace RPG_GAME.UnitTests.Services
{
    public class HeroServiceFlowTests
    {
        [Fact]
        public async Task should_add()
        {
            var hero = EntitiesFixture.CreateDefaultHero().AsDto();

            await _heroService.AddAsync(hero);

            var heroFromDb = await _heroService.GetAsync(hero.Id);
            heroFromDb.Should().NotBeNull();
            heroFromDb.HeroName.Should().Be(hero.HeroName);
        }

        [Fact]
        public async Task should_update()
        {
            var hero = EntitiesFixture.CreateDefaultHero().AsDto();
            hero.Skills = new List<SkillHeroDto> { EntitiesFixture.CreateDefaultSkillHero("skill1").AsDto(), EntitiesFixture.CreateDefaultSkillHero("skill2").AsDto() };
            var heroToUpdate = EntitiesFixture.Clone(hero);
            heroToUpdate.Skills = new List<SkillHeroDto> { hero.Skills.First(), EntitiesFixture.CreateDefaultSkillHero().AsDto(), EntitiesFixture.CreateDefaultSkillHero("test123").AsDto() };
            await _heroRepository.AddAsync(hero.AsEntity());
            heroToUpdate.HeroName = "Heros123";

            await _heroService.UpdateAsync(heroToUpdate);

            var heroUpdated = await _heroService.GetAsync(hero.Id);
            heroUpdated.Should().NotBeNull();
            heroUpdated.HeroName.Should().Be(heroToUpdate.HeroName);
            heroUpdated.Skills.Should().HaveCount(heroToUpdate.Skills.Count());
        }

        [Fact]
        public async Task given_not_existed_hero_when_update_should_throw_an_exception()
        {
            var hero = EntitiesFixture.CreateDefaultHero().AsDto();
            var expectedException = new HeroNotFoundException(hero.Id);

            var exception = await Record.ExceptionAsync(() => _heroService.UpdateAsync(hero));

            exception.Should().NotBeNull();
            exception.Should().BeOfType(expectedException.GetType());
            exception.Message.Should().Be(expectedException.Message);
        }

        [Fact]
        public async Task should_delete()
        {
            var hero = EntitiesFixture.CreateDefaultHero().AsDto();
            await _heroService.AddAsync(hero);
            
            await _heroService.RemoveAsync(hero.Id);

            var heroDeleted = await _heroService.GetAsync(hero.Id);
            heroDeleted.Should().BeNull();
        }

        [Fact]
        public async Task given_not_existed_hero_when_delete_should_throw_an_exception()
        {
            var hero = EntitiesFixture.CreateDefaultHero().AsDto();
            var expectedException = new HeroNotFoundException(hero.Id);

            var exception = await Record.ExceptionAsync(() => _heroService.UpdateAsync(hero));

            exception.Should().NotBeNull();
            exception.Should().BeOfType(expectedException.GetType());
            exception.Message.Should().Be(expectedException.Message);
        }

        [Fact]
        public async Task given_hero_with_players_assigned_when_delete_should_throw_an_exception()
        {
            var hero = EntitiesFixture.CreateDefaultHero();
            hero.AddPlayer(Guid.NewGuid());
            await _heroRepository.AddAsync(hero);
            var expectedException = new HeroCannotBeDeletedException(hero.Id);

            var exception = await Record.ExceptionAsync(() => _heroService.RemoveAsync(hero.Id));

            exception.Should().NotBeNull();
            exception.Should().BeOfType(expectedException.GetType());
            exception.Message.Should().Be(expectedException.Message);
        }

        [Fact]
        public async Task should_return_all_heroes()
        {
            await _heroService.AddAsync(EntitiesFixture.CreateDefaultHero().AsDto());
            await _heroService.AddAsync(EntitiesFixture.CreateDefaultHero().AsDto());
            await _heroService.AddAsync(EntitiesFixture.CreateDefaultHero().AsDto());

            var heroes = await _heroService.GetAllAsync();

            heroes.Should().NotBeEmpty();
            heroes.Should().HaveCountGreaterThan(0);
        }

        private readonly IHeroService _heroService;
        private readonly IHeroRepository _heroRepository;

        public HeroServiceFlowTests()
        {
            _heroRepository = new HeroRepositoryStub();
            _heroService = new HeroService(_heroRepository);
        }
    }
}
