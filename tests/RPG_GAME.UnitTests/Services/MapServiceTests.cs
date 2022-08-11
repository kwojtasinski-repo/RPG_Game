using FluentAssertions;
using Moq;
using RPG_GAME.Application.DTO.Maps;
using RPG_GAME.Application.Exceptions.Enemies;
using RPG_GAME.Application.Exceptions.Maps;
using RPG_GAME.Application.Mappings;
using RPG_GAME.Application.Messaging;
using RPG_GAME.Application.Services;
using RPG_GAME.Core.Entities.Enemies;
using RPG_GAME.Core.Entities.Maps;
using RPG_GAME.Core.Repositories;
using RPG_GAME.UnitTests.Fixtures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace RPG_GAME.UnitTests.Services
{
    public class MapServiceTests
    {
        [Fact]
        public async Task should_create()
        {
            var enemies = AddTestEnemies();
            var enmiesList = enemies.Select(e => new AddEnemyDto() { EnemyId = e.Id, Quantity = 1 });
            var dto = new AddMapDto { Name = "Map#1", Difficulty = "EASY", Enemies = enmiesList };

            await _mapService.AddAsync(dto);

            dto.Id.Should().NotBe(Guid.Empty);
            _mapRepository.Verify(m => m.AddAsync(It.IsAny<Map>()), times: Times.Once);
            _messageBroker.Verify(m => m.PublishAsync(It.IsAny<IMessage>()), times: Times.Exactly(enemies.Count()));
        }

        [Fact]
        public async Task given_duplicated_enemy_id_when_create_should_throw_an_exception()
        {
            var enemies = AddTestEnemies();
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
            var map = AddDefaultMap();
            var enemy = EntitiesFixture.CreateDefaultEnemy();
            _enemyRepository.Setup(e => e.GetAsync(enemy.Id)).ReturnsAsync(enemy);
            var enemiesList = new List<AddEnemyDto> { new AddEnemyDto { EnemyId = map.Enemies.First().Enemy.Id, Quantity = 10 }, new AddEnemyDto { EnemyId = enemy.Id, Quantity = 2 } };
            var dto = new AddMapDto { Id = map.Id, Name = "Map#1abc", Difficulty = "HARD", Enemies = enemiesList };

            await _mapService.UpdateAsync(dto);

            var mapUpdated = await _mapService.GetAsync(map.Id);
            mapUpdated.Name.Should().Be(dto.Name);
            mapUpdated.Difficulty.Should().Be(dto.Difficulty);
            mapUpdated.Enemies.Count().Should().BeGreaterThan(0);
            mapUpdated.Enemies.Should().HaveCount(2);
            _messageBroker.Verify(m => m.PublishAsync(It.IsAny<IMessage>()), times: Times.Exactly(4));
        }

        [Fact]
        public async Task given_invalid_map_when_update_should_throw_an_exception()
        {
            var mapId = Guid.NewGuid();
            var enemy = EntitiesFixture.CreateDefaultEnemy();
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
            var map = AddDefaultMap();
            var enemy = EntitiesFixture.CreateDefaultEnemy();
            _enemyRepository.Setup(e => e.GetAsync(enemy.Id)).ReturnsAsync(enemy);
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
            var map = AddDefaultMap();
            var enemyId = Guid.NewGuid();
            var enemiesList = new List<AddEnemyDto> { new AddEnemyDto { EnemyId = enemyId, Quantity = 10 } };
            var dto = new AddMapDto { Id = map.Id, Name = "Map#1abc", Difficulty = "HARD", Enemies = enemiesList };
            var expectedException = new EnemyNotFoundException(enemyId);

            var exception = await Record.ExceptionAsync(() => _mapService.UpdateAsync(dto));

            exception.Should().NotBeNull();
            exception.Should().BeOfType(expectedException.GetType());
            exception.Message.Should().Be(expectedException.Message);
        }

        private IEnumerable<Enemy> AddTestEnemies()
        {
            var enemy1 = EntitiesFixture.CreateDefaultEnemy();
            var enemy2 = EntitiesFixture.CreateDefaultEnemy();
            var enemy3 = EntitiesFixture.CreateDefaultEnemy();
            var enemy4 = EntitiesFixture.CreateDefaultEnemy();

            _enemyRepository.Setup(e => e.GetAsync(enemy1.Id)).ReturnsAsync(enemy1);
            _enemyRepository.Setup(e => e.GetAsync(enemy2.Id)).ReturnsAsync(enemy2);
            _enemyRepository.Setup(e => e.GetAsync(enemy3.Id)).ReturnsAsync(enemy3);
            _enemyRepository.Setup(e => e.GetAsync(enemy4.Id)).ReturnsAsync(enemy4);

            return new List<Enemy> { enemy1, enemy2, enemy3, enemy4 };
        }

        private Map AddDefaultMap()
        {
            var enemies = AddTestEnemies();
            var map = Map.Create("Map#1", "EASY", enemies.Select(e => new Enemies(e.AsAssign(), 1)));
            _mapRepository.Setup(m => m.GetAsync(map.Id)).ReturnsAsync(map);
            return map;
        }

        private readonly IMapService _mapService;
        private readonly Mock<IMapRepository> _mapRepository;
        private readonly Mock<IEnemyRepository> _enemyRepository;
        private readonly Mock<IMessageBroker> _messageBroker;

        public MapServiceTests()
        {
            _mapRepository = new Mock<IMapRepository>();
            _enemyRepository = new Mock<IEnemyRepository>();
            _messageBroker = new Mock<IMessageBroker>();
            _mapService = new MapService(_mapRepository.Object, _enemyRepository.Object, _messageBroker.Object);
        }
    }
}
