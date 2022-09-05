using Flurl.Http;
using Microsoft.Extensions.DependencyInjection;
using RPG_GAME.Application.DTO.Common;
using RPG_GAME.Application.DTO.Enemies;
using RPG_GAME.Application.Exceptions.Enemies;
using RPG_GAME.Core.Common;
using RPG_GAME.Core.Entities.Common;
using RPG_GAME.Core.Entities.Enemies;
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
    public class EnemiesControllerTests : ApiTestBase
    {
        [Fact]
        public async Task should_add_enemy_add_and_return_201()
        {
            var dto = new EnemyDto
            {
                EnemyName = "Suoap",
                BaseAttack = new StateDto<int> { Value = 10, IncreasingState = new IncreasingStateDto<int> { Value = 2, StrategyIncreasing = StrategyIncreasing.ADDITIVE.ToString() } },
                BaseHealth = new StateDto<int> { Value = 10, IncreasingState = new IncreasingStateDto<int> { Value = 2, StrategyIncreasing = StrategyIncreasing.ADDITIVE.ToString() } },
                BaseHealLvl = new StateDto<int> { Value = 10, IncreasingState = new IncreasingStateDto<int> { Value = 2, StrategyIncreasing = StrategyIncreasing.ADDITIVE.ToString() } },
                Experience = new StateDto<decimal> { Value = 1000, IncreasingState = new IncreasingStateDto<decimal> { Value = 200, StrategyIncreasing = StrategyIncreasing.ADDITIVE.ToString() } },
                Category = Category.Knight.ToString(),
                Difficulty = Difficulty.HARD.ToString(),
                Skills = new List<SkillEnemyDto> { new SkillEnemyDto { Name = "skill", BaseAttack = 20, Probability = 90, IncreasingState = new IncreasingStateDto<int> { Value = 10, StrategyIncreasing = StrategyIncreasing.ADDITIVE.ToString() } } }
            };

            var response = await _client.Request($"{Path}").PostJsonAsync(dto);

            response.StatusCode.ShouldBe((int)HttpStatusCode.Created);
            var id = GetIdFromHeader(response, Path);
            id.ShouldNotBe(Guid.Empty);
        }

        [Fact]
        public async Task should_add_enemy_to_database()
        {
            var dto = new EnemyDto
            {
                EnemyName = "SxsuopA",
                BaseAttack = new StateDto<int> { Value = 10, IncreasingState = new IncreasingStateDto<int> { Value = 2, StrategyIncreasing = StrategyIncreasing.ADDITIVE.ToString() } },
                BaseHealth = new StateDto<int> { Value = 10, IncreasingState = new IncreasingStateDto<int> { Value = 2, StrategyIncreasing = StrategyIncreasing.ADDITIVE.ToString() } },
                BaseHealLvl = new StateDto<int> { Value = 10, IncreasingState = new IncreasingStateDto<int> { Value = 2, StrategyIncreasing = StrategyIncreasing.ADDITIVE.ToString() } },
                Experience = new StateDto<decimal> { Value = 1000, IncreasingState = new IncreasingStateDto<decimal> { Value = 200, StrategyIncreasing = StrategyIncreasing.ADDITIVE.ToString() } },
                Category = Category.Knight.ToString(),
                Difficulty = Difficulty.HARD.ToString(),
                Skills = new List<SkillEnemyDto> { new SkillEnemyDto { Name = "skill", BaseAttack = 20, Probability = 90, IncreasingState = new IncreasingStateDto<int> { Value = 10, StrategyIncreasing = StrategyIncreasing.ADDITIVE.ToString() } } }
            };

            var response = await _client.Request($"{Path}").PostJsonAsync(dto);

            var id = GetIdFromHeader(response, Path);
            var enemy = await _repository.GetAsync(id);
            enemy.BaseAttack.Value.ShouldBe(dto.BaseAttack.Value);
            enemy.BaseHealth.Value.ShouldBe(dto.BaseHealth.Value);
            enemy.BaseHealLvl.Value.ShouldBe(dto.BaseHealLvl.Value);
            enemy.Experience.Value.ShouldBe(dto.Experience.Value);
            enemy.Skills.ShouldNotBeEmpty();
            enemy.Skills.Count().ShouldBe(dto.Skills.Count());
            enemy.Skills.First().Name.ShouldBe(dto.Skills.First().Name);
        }

        [Fact]
        public async Task should_update_enemy_and_return_204()
        {
            var enemy = await AddDefaultEnemyAsync();
            var dto = new EnemyDto
            {
                EnemyName = "SudopAC",
                BaseAttack = new StateDto<int> { Value = 10, IncreasingState = new IncreasingStateDto<int> { Value = 2, StrategyIncreasing = StrategyIncreasing.ADDITIVE.ToString() } },
                BaseHealth = new StateDto<int> { Value = 10, IncreasingState = new IncreasingStateDto<int> { Value = 2, StrategyIncreasing = StrategyIncreasing.ADDITIVE.ToString() } },
                BaseHealLvl = new StateDto<int> { Value = 10, IncreasingState = new IncreasingStateDto<int> { Value = 2, StrategyIncreasing = StrategyIncreasing.ADDITIVE.ToString() } },
                Experience = new StateDto<decimal> { Value = 1000, IncreasingState = new IncreasingStateDto<decimal> { Value = 200, StrategyIncreasing = StrategyIncreasing.ADDITIVE.ToString() } },
                Category = Category.Knight.ToString(),
                Difficulty = Difficulty.EASY.ToString(),
                Skills = new List<SkillEnemyDto> { new SkillEnemyDto { Name = "skill", BaseAttack = 20, Probability = 90, IncreasingState = new IncreasingStateDto<int> { Value = 10, StrategyIncreasing = StrategyIncreasing.ADDITIVE.ToString() } } }
            };

            var response = await _client.Request($"{Path}/{enemy.Id}").PutJsonAsync(dto);

            response.StatusCode.ShouldBe((int)HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task should_update_enemy()
        {
            var enemy = await AddDefaultEnemyAsync();
            var dto = new EnemyDto
            {
                EnemyName = "abc",
                BaseAttack = new StateDto<int> { Value = 10, IncreasingState = new IncreasingStateDto<int> { Value = 2, StrategyIncreasing = StrategyIncreasing.ADDITIVE.ToString() } },
                BaseHealth = new StateDto<int> { Value = 10, IncreasingState = new IncreasingStateDto<int> { Value = 2, StrategyIncreasing = StrategyIncreasing.ADDITIVE.ToString() } },
                BaseHealLvl = new StateDto<int> { Value = 10, IncreasingState = new IncreasingStateDto<int> { Value = 2, StrategyIncreasing = StrategyIncreasing.ADDITIVE.ToString() } },
                Experience = new StateDto<decimal> { Value = 1000, IncreasingState = new IncreasingStateDto<decimal> { Value = 200, StrategyIncreasing = StrategyIncreasing.ADDITIVE.ToString() } },
                Category = Category.Knight.ToString(),
                Difficulty = Difficulty.EASY.ToString(),
                Skills = new List<SkillEnemyDto> { new SkillEnemyDto { Name = "skill", BaseAttack = 20, Probability = 90, IncreasingState = new IncreasingStateDto<int> { Value = 10, StrategyIncreasing = StrategyIncreasing.ADDITIVE.ToString() } } }
            };

            var response = await _client.Request($"{Path}/{enemy.Id}").PutJsonAsync(dto);

            response.StatusCode.ShouldBe((int)HttpStatusCode.NoContent);
            var enemyUpdated = await _repository.GetAsync(enemy.Id);
            enemyUpdated.BaseAttack.Value.ShouldBe(dto.BaseAttack.Value);
            enemyUpdated.BaseHealth.Value.ShouldBe(dto.BaseHealth.Value);
            enemyUpdated.BaseHealLvl.Value.ShouldBe(dto.BaseHealLvl.Value);
            enemyUpdated.Experience.Value.ShouldBe(dto.Experience.Value);
            enemyUpdated.Skills.ShouldNotBeEmpty();
            enemyUpdated.Skills.Count().ShouldBe(dto.Skills.Count());
            var skillEnemy = enemy.Skills.First();
            var skillEnemyDto = enemyUpdated.Skills.First();
            var skillEnemyUpdated = enemyUpdated.Skills.First();
            skillEnemyUpdated.Id.ShouldNotBe(skillEnemy.Id);
            skillEnemyUpdated.Name.ShouldBe(skillEnemyDto.Name);
        }

        [Fact]
        public async Task should_delete_enemy_and_return_204()
        {
            var enemy = await AddDefaultEnemyAsync();

            var response = await _client.Request($"{Path}/{enemy.Id}").DeleteAsync();

            response.StatusCode.ShouldBe((int)HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task should_delete_enemy_and_delete_from_database()
        {
            var enemy = await AddDefaultEnemyAsync();

            await _client.Request($"{Path}/{enemy.Id}").DeleteAsync();

            var enemyDeteled = await _repository.GetAsync(enemy.Id);
            enemyDeteled.ShouldBeNull();
        }

        [Fact]
        public async Task should_get_enemy_and_return_200()
        {
            var enemy = await AddDefaultEnemyAsync();

            var response = await _client.Request($"{Path}/{enemy.Id}").GetAsync();

            response.StatusCode.ShouldBe((int)HttpStatusCode.OK);
        }

        [Fact]
        public async Task should_get_enemy_and_return_dto()
        {
            var enemy = await AddDefaultEnemyAsync();

            var dto = await _client.Request($"{Path}/{enemy.Id}").GetJsonAsync<EnemyDetailsDto>();

            dto.ShouldNotBeNull();
            dto.Id.ShouldBe(enemy.Id);
            dto.EnemyName.ShouldBe(enemy.EnemyName);
        }

        [Fact]
        public async Task should_get_enemies_and_return_200()
        {
            await AddDefaultEnemyAsync();
            await AddDefaultEnemyAsync();

            var response = await _client.Request($"{Path}").GetAsync();

            response.StatusCode.ShouldBe((int)HttpStatusCode.OK);
        }

        [Fact]
        public async Task should_get_enemies_and_return_dtos()
        {
            var enemy1 = await AddDefaultEnemyAsync();
            var enemy2 = await AddDefaultEnemyAsync();

            var dtos = await _client.Request($"{Path}").GetJsonAsync<IEnumerable<EnemyDto>>();

            dtos.ShouldNotBeEmpty();
            dtos.Count().ShouldBeGreaterThan(0);
            dtos.ShouldContain(h => h.Id == enemy1.Id);
            dtos.ShouldContain(h => h.Id == enemy2.Id);
        }

        [Fact]
        public async Task given_invalid_dto_when_add_enemy_should_return_400()
        {
            var dto = new EnemyDto
            {
                EnemyName = "EneamyA",
                Difficulty = Difficulty.HARD.ToString(),
                Category = Category.Dragon.ToString()
            };
            var expectedException = new InvalidEnemyStateException(nameof(EnemyDto.BaseAttack));

            var response = await _client.AllowHttpStatus("400").Request($"{Path}").PostJsonAsync(dto);

            response.StatusCode.ShouldBe((int)HttpStatusCode.BadRequest);
            var message = JsonSerializer.Deserialize<Dictionary<string, string>>(await response.ResponseMessage.Content.ReadAsStringAsync());
            message.Values.ShouldContain(expectedException.Message);
        }

        [Fact]
        public async Task given_invalid_id_should_return_404()
        {
            var id = Guid.NewGuid();

            var response = await _client.AllowHttpStatus("404").Request($"{Path}/{id}").GetAsync();

            response.StatusCode.ShouldBe((int)HttpStatusCode.NotFound);
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
            await _repository.AddAsync(enemy);
            return enemy;
        }

        private const string Path = "api/Enemies";
        private readonly IEnemyRepository _repository;

        public EnemiesControllerTests(TestApplicationFactory<Program> factory) : base(factory)
        {
            _repository = factory.Services.GetRequiredService<IEnemyRepository>();
        }
    }
}
