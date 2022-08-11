using FluentAssertions;
using RPG_GAME.Application.DTO.Maps;
using RPG_GAME.Application.Events.Maps;
using RPG_GAME.Application.Exceptions.Enemies;
using RPG_GAME.Application.Exceptions.Maps;
using RPG_GAME.Application.Services;
using RPG_GAME.Core.Entities.Enemies;
using RPG_GAME.Core.Entities.Maps;
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
    public class MapServiceFlowTests
    {
        [Fact]
        public async Task should_create()
        {
            var enemies = await AddTestEnemies();
            var enmiesList = enemies.Select(e => new AddEnemyDto() { EnemyId = e.Id, Quantity = 1 });
            var dto = new AddMapDto { Name = "Map#1", Difficulty = "EASY", Enemies = enmiesList };

            await _mapService.AddAsync(dto);

            dto.Id.Should().NotBe(Guid.Empty);
            var enemy = await _mapService.GetAsync(dto.Id);
            enemy.Should().NotBeNull();
            enemy.Name.Should().Be(dto.Name);
            enemy.Difficulty.Should().Be(dto.Difficulty);
        }

        [Fact]
        public async Task should_create_and_publish_events()
        {
            var enemies = await AddTestEnemies();
            var enmiesList = enemies.Select(e => new AddEnemyDto() { EnemyId = e.Id, Quantity = 1 });
            var dto = new AddMapDto { Name = "Map#1", Difficulty = "EASY", Enemies = enmiesList };

            await _mapService.AddAsync(dto);

            var messages = _messageBrokerStub.GetPublishedMessages();
            messages.Should().HaveCount(4);
            var ids = messages.Where(m => typeof(EnemyAddedToMap).IsAssignableFrom(m.GetType()))
                .Select(m => ((EnemyAddedToMap)m).EnemyId);
            ids.Should().Contain(enemies.Select(e => e.Id));
        }

        [Fact]
        public async Task given_duplicated_enemy_id_when_create_should_throw_an_exception()
        {
            var enemies = await AddTestEnemies();
            var enemy = enemies.First();
            var enmiesList = new List<AddEnemyDto> { new AddEnemyDto() { EnemyId = enemy.Id, Quantity = 1 }, new AddEnemyDto() { EnemyId = enemy.Id, Quantity = 1 } };
            var dto = new AddMapDto { Name = "Map#1", Difficulty = "EASY", Enemies = enmiesList };
            var expectedException = new EnemiesShouldntHaveDuplicatedEnemyIdException();

            var exception = await Record.ExceptionAsync(() => _mapService.AddAsync(dto));

            exception.Should().NotBeNull();
            exception.Should().BeOfType(expectedException.GetType());
            exception.Message.Should().Be(expectedException.Message);
        }

        [Fact]
        public async Task given_not_existing_enemy_when_create_should_throw_an_exception()
        {
            var enemyId = Guid.NewGuid();
            var enemiesList = new List<AddEnemyDto> { new AddEnemyDto() { EnemyId = enemyId, Quantity = 1 } };
            var dto = new AddMapDto { Name = "Map#1", Difficulty = "EASY", Enemies = enemiesList };
            var expectedException = new EnemyNotFoundException(enemyId);

            var exception = await Record.ExceptionAsync(() => _mapService.AddAsync(dto));

            exception.Should().NotBeNull();
            exception.Should().BeOfType(expectedException.GetType());
            exception.Message.Should().Be(expectedException.Message);
        }

        [Fact]
        public async Task should_update()
        {
            var map = await AddDefaultMap();
            var enemy = EntitiesFixture.CreateDefaultEnemy();
            await _enemyRepository.AddAsync(enemy);
            var enemiesList = new List<AddEnemyDto> { new AddEnemyDto { EnemyId = map.Enemies.First().Enemy.Id, Quantity = 10 }, new AddEnemyDto { EnemyId = enemy.Id, Quantity = 2 } };
            var dto = new AddMapDto { Id = map.Id, Name = "Map#1abc", Difficulty = "HARD", Enemies = enemiesList };

            await _mapService.UpdateAsync(dto);

            var mapUpdated = await _mapService.GetAsync(map.Id);
            mapUpdated.Name.Should().Be(dto.Name);
            mapUpdated.Difficulty.Should().Be(dto.Difficulty);
            mapUpdated.Enemies.Count().Should().BeGreaterThan(0);
            mapUpdated.Enemies.Should().HaveCount(2);
        }

        [Fact]
        public async Task should_update_and_publish_events()
        {
            var map = await AddDefaultMap();
            var enemy = EntitiesFixture.CreateDefaultEnemy();
            await _enemyRepository.AddAsync(enemy);
            var enemiesList = new List<AddEnemyDto> { new AddEnemyDto { EnemyId = map.Enemies.First().Enemy.Id, Quantity = 10 }, new AddEnemyDto { EnemyId = enemy.Id, Quantity = 2 } };
            var dto = new AddMapDto { Id = map.Id, Name = "Map#1abc", Difficulty = "HARD", Enemies = enemiesList };

            await _mapService.UpdateAsync(dto);

            var messages = _messageBrokerStub.GetPublishedMessages();
            messages.Should().HaveCount(4);
            var idsAdded = messages.Where(m => typeof(EnemyAddedToMap).IsAssignableFrom(m.GetType()))
                .Select(m => ((EnemyAddedToMap)m).EnemyId);
            idsAdded.Should().Contain(enemy.Id);
            var idsDeleted = messages.Where(m => typeof(EnemyRemovedFromMap).IsAssignableFrom(m.GetType()))
                .Select(m => ((EnemyRemovedFromMap)m).EnemyId);
            idsDeleted.Should().HaveCount(3);
        }

        [Fact]
        public async Task given_invalid_map_when_update_should_throw_an_exception()
        {
            var mapId = Guid.NewGuid();
            var enemy = EntitiesFixture.CreateDefaultEnemy();
            await _enemyRepository.AddAsync(enemy);
            var enemiesList = new List<AddEnemyDto> { new AddEnemyDto { EnemyId = enemy.Id, Quantity = 10 } };
            var dto = new AddMapDto { Id = mapId, Name = "Map#1abc", Difficulty = "HARD", Enemies = enemiesList };
            var expectedException = new MapNotFoundException(mapId);

            var exception = await Record.ExceptionAsync(() => _mapService.UpdateAsync(dto));

            exception.Should().NotBeNull();
            exception.Should().BeOfType(expectedException.GetType());
            exception.Message.Should().Be(expectedException.Message);
        }

        [Fact]
        public async Task given_duplicated_enemy_id_when_update_should_throw_an_exception()
        {
            var map = await AddDefaultMap();
            var enemy = EntitiesFixture.CreateDefaultEnemy();
            await _enemyRepository.AddAsync(enemy);
            var enemiesList = new List<AddEnemyDto> { new AddEnemyDto { EnemyId = enemy.Id, Quantity = 10 }, new AddEnemyDto { EnemyId = enemy.Id, Quantity = 120 } };
            var dto = new AddMapDto { Id = map.Id, Name = "Map#1abc", Difficulty = "HARD", Enemies = enemiesList };
            var expectedException = new EnemiesShouldntHaveDuplicatedEnemyIdException();

            var exception = await Record.ExceptionAsync(() => _mapService.UpdateAsync(dto));

            exception.Should().NotBeNull();
            exception.Should().BeOfType(expectedException.GetType());
            exception.Message.Should().Be(expectedException.Message);
        }

        [Fact]
        public async Task given_not_existing_enemy_when_update_should_throw_an_exception()
        {
            var map = await AddDefaultMap();
            var enemyId = Guid.NewGuid();
            var enemiesList = new List<AddEnemyDto> { new AddEnemyDto { EnemyId = enemyId, Quantity = 10 } };
            var dto = new AddMapDto { Id = map.Id, Name = "Map#1abc", Difficulty = "HARD", Enemies = enemiesList };
            var expectedException = new EnemyNotFoundException(enemyId);

            var exception = await Record.ExceptionAsync(() => _mapService.UpdateAsync(dto));

            exception.Should().NotBeNull();
            exception.Should().BeOfType(expectedException.GetType());
            exception.Message.Should().Be(expectedException.Message);
        }

        [Fact]
        public async Task should_return_maps()
        {
            var enemies = await AddTestEnemies();
            var enmiesList = enemies.Select(e => new AddEnemyDto() { EnemyId = e.Id, Quantity = 1 });
            var dto = new AddMapDto { Name = "Map#1", Difficulty = "EASY", Enemies = enmiesList };
            await _mapService.AddAsync(dto);
            var dto2 = new AddMapDto { Name = "Map#2", Difficulty = "EASY", Enemies = enmiesList };
            await _mapService.AddAsync(dto2);

            var maps = await _mapService.GetAllAsync();

            maps.Should().NotBeEmpty();
            maps.Should().HaveCount(2);
            var messages = _messageBrokerStub.GetPublishedMessages();
            messages.Should().NotBeEmpty();
        }

        private async Task<IEnumerable<Enemy>> AddTestEnemies()
        {
            var enemy1 = EntitiesFixture.CreateDefaultEnemy();
            var enemy2 = EntitiesFixture.CreateDefaultEnemy();
            var enemy3 = EntitiesFixture.CreateDefaultEnemy();
            var enemy4 = EntitiesFixture.CreateDefaultEnemy();

            await _enemyRepository.AddAsync(enemy1);
            await _enemyRepository.AddAsync(enemy2);
            await _enemyRepository.AddAsync(enemy3);
            await _enemyRepository.AddAsync(enemy4);

            return new List<Enemy> { enemy1, enemy2, enemy3, enemy4 };
        }


        private async Task<Map> AddDefaultMap()
        {
            var enemies = await AddTestEnemies();
            var enmiesList = enemies.Select(e => new AddEnemyDto() { EnemyId = e.Id, Quantity = 1 });
            var dto = new AddMapDto { Name = "Map#1", Difficulty = "EASY", Enemies = enmiesList };
            await _mapService.AddAsync(dto);
            _messageBrokerStub.ClearMessages();
            return await _mapRepository.GetAsync(dto.Id);
        }

        private readonly IMapService _mapService;
        private readonly IMapRepository _mapRepository;
        private readonly IEnemyRepository _enemyRepository;
        private readonly MessageBrokerStub _messageBrokerStub;

        public MapServiceFlowTests()
        {
            _mapRepository = new MapRepositoryStub();
            _enemyRepository = new EnemyRepositoryStub();
            _messageBrokerStub = new MessageBrokerStub();
            _mapService = new MapService(_mapRepository, _enemyRepository, _messageBrokerStub);
        }
    }
}
