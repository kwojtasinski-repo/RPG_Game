using Flurl.Http;
using Microsoft.Extensions.DependencyInjection;
using RPG_GAME.Application.DTO.Maps;
using RPG_GAME.Application.Exceptions.Enemies;
using RPG_GAME.Application.Mappings;
using RPG_GAME.Core.Common;
using RPG_GAME.Core.Entities.Common;
using RPG_GAME.Core.Entities.Enemies;
using RPG_GAME.Core.Entities.Maps;
using RPG_GAME.Core.Repositories;
using RPG_GAME.IntegrationTests.Common;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace RPG_GAME.IntegrationTests.Api
{
    public class MapsControllerTests : ApiTestBase
    {
        [Fact]
        public async Task should_add_map_and_return_201()
        {
            var enemy = await AddDefaultEnemyAsync();
            var dto = new AddMapDto { Name = "map#1", Difficulty = Difficulty.EASY.ToString(), Enemies = new List<AddEnemyDto> { new AddEnemyDto { EnemyId = enemy.Id, Quantity = 2 } } };

            var response = await _client.Request($"{Path}").PostJsonAsync(dto);

            response.StatusCode.ShouldBe((int)HttpStatusCode.Created);
            var id = GetIdFromHeader(response, Path);
            id.ShouldNotBe(Guid.Empty);
        }

        [Fact]
        public async Task should_add_map_to_database()
        {
            var enemy = await AddDefaultEnemyAsync();
            var dto = new AddMapDto { Name = "map#2", Difficulty = Difficulty.EASY.ToString(), Enemies = new List<AddEnemyDto> { new AddEnemyDto { EnemyId = enemy.Id, Quantity = 2 } } };

            var response = await _client.Request($"{Path}").PostJsonAsync(dto);

            var id = GetIdFromHeader(response, Path);
            var map = await _repository.GetAsync(id);
            map.ShouldNotBeNull();
            map.Name.ShouldBe(dto.Name);
            map.Enemies.ShouldContain(e => e.Enemy.Id == dto.Enemies.First().EnemyId);
        }

        [Fact]
        public async Task should_update_map_and_return_204()
        {
            var map = await AddDefaultMap();
            var updatePlayer = new AddMapDto
            {
                Name = "map#3",
                Difficulty = Difficulty.EASY.ToString(),
                Enemies = new List<AddEnemyDto> { new AddEnemyDto { EnemyId = map.Enemies.First().Enemy.Id, Quantity = 3 } }
            };

            var response = await _client.Request($"{Path}/{map.Id}").PutJsonAsync(updatePlayer);

            response.StatusCode.ShouldBe((int)HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task should_update_map()
        {
            var map = await AddDefaultMap();
            var updateMap = new AddMapDto
            {
                Name = "map#4",
                Difficulty = Difficulty.EASY.ToString(),
                Enemies = new List<AddEnemyDto> { new AddEnemyDto { EnemyId = map.Enemies.First().Enemy.Id, Quantity = 3 } }
            };

            await _client.Request($"{Path}/{map.Id}").PutJsonAsync(updateMap);

            var mapUpdated = await _repository.GetAsync(map.Id);
            mapUpdated.Name.ShouldBe(updateMap.Name);
            mapUpdated.Difficulty.ToString().ShouldBe(updateMap.Difficulty);
            var enemy = updateMap.Enemies.First();
            mapUpdated.Enemies.ShouldContain(e => e.Enemy.Id == enemy.EnemyId && e.Quantity == enemy.Quantity);
        }

        [Fact]
        public async Task should_get_map()
        {
            var map = await AddDefaultMap();

            var response = await _client.Request($"{Path}/{map.Id}").GetAsync();

            response.StatusCode.ShouldBe((int)HttpStatusCode.OK);
            var dto = await response.GetJsonAsync<MapDto>();
            dto.Id.ShouldBe(map.Id);
            dto.Name.ShouldBe(map.Name);
        }

        [Fact]
        public async Task should_get_maps()
        {
            var map1 = await AddDefaultMap();
            var map2 = await AddDefaultMap();

            var response = await _client.Request($"{Path}").GetAsync();

            response.StatusCode.ShouldBe((int)HttpStatusCode.OK);
            var dtos = await response.GetJsonAsync<IEnumerable<MapDto>>();
            dtos.ShouldContain(m => m.Id == map1.Id);
            dtos.ShouldContain(m => m.Id == map2.Id);
        }

        [Fact]
        public async Task given_invalid_map_id_should_return_404()
        {
            var mapId = Guid.NewGuid();

            var response = await _client.AllowHttpStatus("404").Request($"{Path}/{mapId}").GetAsync();

            response.StatusCode.ShouldBe((int)HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task given_invalid_map_id_when_add_should_return_400()
        {
            var enemy = new AddEnemyDto { EnemyId = Guid.NewGuid(), Quantity = 2 };
            var addMap = new AddMapDto { Enemies = new List<AddEnemyDto> { enemy, enemy } };
            var expectedException = new EnemiesShouldntHaveDuplicatedEnemyIdException();

            var response = await _client.AllowHttpStatus("400").Request($"{Path}").PostJsonAsync(addMap);

            response.StatusCode.ShouldBe((int)HttpStatusCode.BadRequest);
            var message = JsonSerializer.Deserialize<Dictionary<string, string>>(await response.ResponseMessage.Content.ReadAsStringAsync());
            message.Values.ShouldContain(expectedException.Message);
        }

        private async Task<Map> AddDefaultMap()
        {
            var enemy = await AddDefaultEnemyAsync();
            var map = new Map(Guid.NewGuid(), $"map-{Guid.NewGuid().ToString("N")}", Difficulty.HARD.ToString(),
                            new List<Enemies> { new Enemies(enemy.AsAssign(), 2) });
            await _repository.AddAsync(map);
            return map;
        }

        private async Task<Enemy> AddDefaultEnemyAsync()
        {
            var enemy = new Enemy
            (
                Guid.NewGuid(),
                $"DefaultEnemy{Guid.NewGuid().ToString("N")}",
                new State<int>(10, new IncreasingState<int>(2, StrategyIncreasing.ADDITIVE.ToString())),
                new State<int>(10, new IncreasingState<int>(2, StrategyIncreasing.ADDITIVE.ToString())),
                new State<int>(10, new IncreasingState<int>(2, StrategyIncreasing.ADDITIVE.ToString())),
                new State<decimal>(1000, new IncreasingState<decimal>(200, StrategyIncreasing.ADDITIVE.ToString())),
                Difficulty.MEDIUM.ToString(),
                Category.Archer.ToString(),
                new List<SkillEnemy> { new SkillEnemy(Guid.NewGuid(), "skill", 20, 50, new IncreasingState<int>(10, StrategyIncreasing.ADDITIVE.ToString())) }
            );
            await _enemyRepository.AddAsync(enemy);
            return enemy;
        }

        private const string Path = "api/Maps";
        private readonly IMapRepository _repository;
        private readonly IEnemyRepository _enemyRepository;

        public MapsControllerTests(TestApplicationFactory<Program> factory) : base(factory)
        {
            _repository = factory.Services.GetRequiredService<IMapRepository>();
            _enemyRepository = factory.Services.GetRequiredService<IEnemyRepository>();
        }
    }
}
