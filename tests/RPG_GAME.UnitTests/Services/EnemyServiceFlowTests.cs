using FluentAssertions;
using RPG_GAME.Application.DTO.Enemies;
using RPG_GAME.Application.Events.Enemies;
using RPG_GAME.Application.Exceptions.Enemies;
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
    public class EnemyServiceFlowTests
    {
        [Fact]
        public async Task should_add()
        {
            var enemy = EntitiesFixture.CreateDefaultEnemy().AsDto();

            await _enemyService.AddAsync(enemy);

            var heroFromDb = await _enemyService.GetAsync(enemy.Id);
            heroFromDb.Should().NotBeNull();
            heroFromDb.EnemyName.Should().Be(enemy.EnemyName);
        }

        [Fact]
        public async Task should_update()
        {
            var enemy = EntitiesFixture.CreateDefaultEnemy().AsDto();
            enemy.Skills = new List<SkillEnemyDto> { EntitiesFixture.CreateDefaultSkillEnemy("skill1").AsDto(), EntitiesFixture.CreateDefaultSkillEnemy("skill2").AsDto() };
            var heroToUpdate = EntitiesFixture.Clone(enemy);
            heroToUpdate.Skills = new List<SkillEnemyDto> { enemy.Skills.First(), EntitiesFixture.CreateDefaultSkillEnemy().AsDto(), EntitiesFixture.CreateDefaultSkillEnemy("test123").AsDto() };
            await _enemyRepository.AddAsync(enemy.AsEntity());
            heroToUpdate.EnemyName = "Enemy123";

            await _enemyService.UpdateAsync(heroToUpdate);

            var heroUpdated = await _enemyService.GetAsync(enemy.Id);
            heroUpdated.Should().NotBeNull();
            heroUpdated.EnemyName.Should().Be(heroToUpdate.EnemyName);
            heroUpdated.Skills.Should().HaveCount(heroToUpdate.Skills.Count());
        }

        [Fact]
        public async Task should_update_and_publish_event()
        {
            var enemy = EntitiesFixture.CreateDefaultEnemy().AsDto();
            enemy.Skills = new List<SkillEnemyDto> { EntitiesFixture.CreateDefaultSkillEnemy("skill1").AsDto(), EntitiesFixture.CreateDefaultSkillEnemy("skill2").AsDto() };
            var heroToUpdate = EntitiesFixture.Clone(enemy);
            heroToUpdate.Skills = new List<SkillEnemyDto> { enemy.Skills.First(), EntitiesFixture.CreateDefaultSkillEnemy().AsDto(), EntitiesFixture.CreateDefaultSkillEnemy("test123").AsDto() };
            await _enemyRepository.AddAsync(enemy.AsEntity());
            heroToUpdate.EnemyName = "Enemy123";

            await _enemyService.UpdateAsync(heroToUpdate);

            var messages = _messageBrokerStub.GetPublishedMessages();
            messages.Should().HaveCount(1);
            var ids = messages.Where(m => typeof(EnemyUpdated).IsAssignableFrom(m.GetType()))
                .Select(m => ((EnemyUpdated)m).EnemyId);
            ids.Should().Contain(heroToUpdate.Id);
        }

        [Fact]
        public async Task given_not_existed_enemy_when_update_should_throw_an_exception()
        {
            var enemy = EntitiesFixture.CreateDefaultEnemy().AsDto();
            var expectedException = new EnemyNotFoundException(enemy.Id);

            var exception = await Record.ExceptionAsync(() => _enemyService.UpdateAsync(enemy));

            exception.Should().NotBeNull();
            exception.Should().BeOfType(expectedException.GetType());
            exception.Message.Should().Be(expectedException.Message);
        }

        [Fact]
        public async Task should_delete()
        {
            var enemy = EntitiesFixture.CreateDefaultEnemy().AsDto();
            await _enemyService.AddAsync(enemy);

            await _enemyService.RemoveAsync(enemy.Id);

            var heroDeleted = await _enemyService.GetAsync(enemy.Id);
            heroDeleted.Should().BeNull();
        }

        [Fact]
        public async Task given_not_existed_enemy_when_delete_should_throw_an_exception()
        {
            var enemy = EntitiesFixture.CreateDefaultEnemy().AsDto();
            var expectedException = new EnemyNotFoundException(enemy.Id);

            var exception = await Record.ExceptionAsync(() => _enemyService.UpdateAsync(enemy));

            exception.Should().NotBeNull();
            exception.Should().BeOfType(expectedException.GetType());
            exception.Message.Should().Be(expectedException.Message);
        }

        [Fact]
        public async Task given_enemy_with_maps_assigned_when_delete_should_throw_an_exception()
        {
            var enemy = EntitiesFixture.CreateDefaultEnemy();
            enemy.AddMap(Guid.NewGuid());
            await _enemyRepository.AddAsync(enemy);
            var expectedException = new EnemyCannotBeDeletedException(enemy.Id);

            var exception = await Record.ExceptionAsync(() => _enemyService.RemoveAsync(enemy.Id));

            exception.Should().NotBeNull();
            exception.Should().BeOfType(expectedException.GetType());
            exception.Message.Should().Be(expectedException.Message);
        }

        [Fact]
        public async Task should_return_all_enemies()
        {
            await _enemyService.AddAsync(EntitiesFixture.CreateDefaultEnemy().AsDto());
            await _enemyService.AddAsync(EntitiesFixture.CreateDefaultEnemy().AsDto());
            await _enemyService.AddAsync(EntitiesFixture.CreateDefaultEnemy().AsDto());

            var heroes = await _enemyService.GetAllAsync();

            heroes.Should().NotBeEmpty();
            heroes.Should().HaveCountGreaterThan(0);
        }

        private readonly IEnemyService _enemyService;
        private readonly IEnemyRepository _enemyRepository;
        private readonly MessageBrokerStub _messageBrokerStub;

        public EnemyServiceFlowTests()
        {
            _enemyRepository = new EnemyRepositoryStub();
            _messageBrokerStub = new MessageBrokerStub();
            _enemyService = new EnemyService(_enemyRepository, _messageBrokerStub);
        }
    }
}
