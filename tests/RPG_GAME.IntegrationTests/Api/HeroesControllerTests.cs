using Flurl.Http;
using Microsoft.Extensions.DependencyInjection;
using RPG_GAME.Application.DTO.Common;
using RPG_GAME.Application.DTO.Heroes;
using RPG_GAME.Application.Exceptions.Heroes;
using RPG_GAME.Core.Entities.Common;
using RPG_GAME.Core.Entities.Heroes;
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
    public class HeroesControllerTests : ApiTestBase
    {
        [Fact]
        public async Task should_add_hero_add_and_return_201()
        {
            var dto = new HeroDto
            {
                HeroName = "Suop",
                Attack = new StateDto<int> { Value = 10, IncreasingState = new IncreasingStateDto<int> { Value = 2, StrategyIncreasing = StrategyIncreasing.ADDITIVE.ToString() } },
                Health = new StateDto<int> { Value = 10, IncreasingState = new IncreasingStateDto<int> { Value = 2, StrategyIncreasing = StrategyIncreasing.ADDITIVE.ToString() } },
                HealLvl = new StateDto<int> { Value = 10, IncreasingState = new IncreasingStateDto<int> { Value = 2, StrategyIncreasing = StrategyIncreasing.ADDITIVE.ToString() } },
                BaseRequiredExperience = new StateDto<decimal> { Value = 1000, IncreasingState = new IncreasingStateDto<decimal> { Value = 200, StrategyIncreasing = StrategyIncreasing.ADDITIVE.ToString() } },
                Skills = new List<SkillHeroDto> { new SkillHeroDto { Name = "skill", BaseAttack = 20, IncreasingState = new IncreasingStateDto<int> { Value = 10, StrategyIncreasing = StrategyIncreasing.ADDITIVE.ToString() } } }
            };

            var response = await _client.Request($"{Path}").PostJsonAsync(dto);

            response.StatusCode.ShouldBe((int)HttpStatusCode.Created);
            var id = GetIdFromHeader(response, Path);
            id.ShouldNotBe(Guid.Empty);
        }

        [Fact]
        public async Task should_add_hero_to_database()
        {
            var dto = new HeroDto
            {
                HeroName = "SuopA",
                Attack = new StateDto<int> { Value = 10, IncreasingState = new IncreasingStateDto<int> { Value = 2, StrategyIncreasing = StrategyIncreasing.ADDITIVE.ToString() } },
                Health = new StateDto<int> { Value = 10, IncreasingState = new IncreasingStateDto<int> { Value = 2, StrategyIncreasing = StrategyIncreasing.ADDITIVE.ToString() } },
                HealLvl = new StateDto<int> { Value = 10, IncreasingState = new IncreasingStateDto<int> { Value = 2, StrategyIncreasing = StrategyIncreasing.ADDITIVE.ToString() } },
                BaseRequiredExperience = new StateDto<decimal> { Value = 1000, IncreasingState = new IncreasingStateDto<decimal> { Value = 200, StrategyIncreasing = StrategyIncreasing.ADDITIVE.ToString() } },
                Skills = new List<SkillHeroDto> { new SkillHeroDto { Name = "skill", BaseAttack = 20, IncreasingState = new IncreasingStateDto<int> { Value = 10, StrategyIncreasing = StrategyIncreasing.ADDITIVE.ToString() } } }
            };

            var response = await _client.Request($"{Path}").PostJsonAsync(dto);

            var id = GetIdFromHeader(response, Path);
            var hero = await _repository.GetAsync(id);
            hero.Attack.Value.ShouldBe(dto.Attack.Value);
            hero.Health.Value.ShouldBe(dto.Health.Value);
            hero.HealLvl.Value.ShouldBe(dto.HealLvl.Value);
            hero.BaseRequiredExperience.Value.ShouldBe(dto.BaseRequiredExperience.Value);
            hero.Skills.ShouldNotBeEmpty();
            hero.Skills.Count().ShouldBe(dto.Skills.Count());
            hero.Skills.First().Name.ShouldBe(dto.Skills.First().Name);
        }

        [Fact]
        public async Task should_update_hero_and_return_204()
        {
            var hero = await AddDefaultHeroAsync();
            var dto = new HeroDto
            {
                HeroName = "SuopAC",
                Attack = new StateDto<int> { Value = 10, IncreasingState = new IncreasingStateDto<int> { Value = 2, StrategyIncreasing = StrategyIncreasing.ADDITIVE.ToString() } },
                Health = new StateDto<int> { Value = 10, IncreasingState = new IncreasingStateDto<int> { Value = 2, StrategyIncreasing = StrategyIncreasing.ADDITIVE.ToString() } },
                HealLvl = new StateDto<int> { Value = 10, IncreasingState = new IncreasingStateDto<int> { Value = 2, StrategyIncreasing = StrategyIncreasing.ADDITIVE.ToString() } },
                BaseRequiredExperience = new StateDto<decimal> { Value = 1000, IncreasingState = new IncreasingStateDto<decimal> { Value = 200, StrategyIncreasing = StrategyIncreasing.ADDITIVE.ToString() } },
                Skills = new List<SkillHeroDto> { new SkillHeroDto { Name = "skill", BaseAttack = 20, IncreasingState = new IncreasingStateDto<int> { Value = 10, StrategyIncreasing = StrategyIncreasing.ADDITIVE.ToString() } } }
            };

            var response = await _client.Request($"{Path}/{hero.Id}").PutJsonAsync(dto);

            response.StatusCode.ShouldBe((int)HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task should_update_hero()
        {
            var hero = await AddDefaultHeroAsync();
            var dto = new HeroDto
            {
                Id = hero.Id,
                HeroName = "SuopB",
                Attack = new StateDto<int> { Value = 12, IncreasingState = new IncreasingStateDto<int> { Value = 2, StrategyIncreasing = StrategyIncreasing.ADDITIVE.ToString() } },
                Health = new StateDto<int> { Value = 13, IncreasingState = new IncreasingStateDto<int> { Value = 2, StrategyIncreasing = StrategyIncreasing.ADDITIVE.ToString() } },
                HealLvl = new StateDto<int> { Value = 14, IncreasingState = new IncreasingStateDto<int> { Value = 2, StrategyIncreasing = StrategyIncreasing.ADDITIVE.ToString() } },
                BaseRequiredExperience = new StateDto<decimal> { Value = 1020, IncreasingState = new IncreasingStateDto<decimal> { Value = 200, StrategyIncreasing = StrategyIncreasing.ADDITIVE.ToString() } },
                Skills = new List<SkillHeroDto> { new SkillHeroDto { Name = "skillAbc", BaseAttack = 20, IncreasingState = new IncreasingStateDto<int> { Value = 10, StrategyIncreasing = StrategyIncreasing.ADDITIVE.ToString() } } }
            };

            var response = await _client.Request($"{Path}/{hero.Id}").PutJsonAsync(dto);

            response.StatusCode.ShouldBe((int)HttpStatusCode.NoContent);
            var heroUpdated = await _repository.GetAsync(hero.Id);
            heroUpdated.Attack.Value.ShouldBe(dto.Attack.Value);
            heroUpdated.Health.Value.ShouldBe(dto.Health.Value);
            heroUpdated.HealLvl.Value.ShouldBe(dto.HealLvl.Value);
            heroUpdated.BaseRequiredExperience.Value.ShouldBe(dto.BaseRequiredExperience.Value);
            heroUpdated.Skills.ShouldNotBeEmpty();
            heroUpdated.Skills.Count().ShouldBe(dto.Skills.Count());
            var skillHero = hero.Skills.First();
            var skillHeroDto = heroUpdated.Skills.First();
            var skillHeroUpdated = heroUpdated.Skills.First();
            skillHeroUpdated.Id.ShouldNotBe(skillHero.Id);
            skillHeroUpdated.Name.ShouldBe(skillHeroDto.Name);
        }

        [Fact]
        public async Task should_delete_hero_and_return_204()
        {
            var hero = await AddDefaultHeroAsync();

            var response = await _client.Request($"{Path}/{hero.Id}").DeleteAsync();

            response.StatusCode.ShouldBe((int)HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task should_delete_hero_and_delete_from_database()
        {
            var hero = await AddDefaultHeroAsync();

            await _client.Request($"{Path}/{hero.Id}").DeleteAsync();

            var heroDeteled = await _repository.GetAsync(hero.Id);
            heroDeteled.ShouldBeNull();
        }

        [Fact]
        public async Task should_get_hero_and_return_200()
        {
            var hero = await AddDefaultHeroAsync();

            var response = await _client.Request($"{Path}/{hero.Id}").GetAsync();

            response.StatusCode.ShouldBe((int)HttpStatusCode.OK);
        }

        [Fact]
        public async Task should_get_hero_and_return_dto()
        {
            var hero = await AddDefaultHeroAsync();

            var dto = await _client.Request($"{Path}/{hero.Id}").GetJsonAsync<HeroDetailsDto>();

            dto.ShouldNotBeNull();
            dto.Id.ShouldBe(hero.Id);
            dto.HeroName.ShouldBe(hero.HeroName);
        }

        [Fact]
        public async Task should_get_heroes_and_return_200()
        {
            await AddDefaultHeroAsync();
            await AddDefaultHeroAsync();

            var response = await _client.Request($"{Path}").GetAsync();

            response.StatusCode.ShouldBe((int)HttpStatusCode.OK);
        }

        [Fact]
        public async Task should_get_heroes_and_return_dtos()
        {
            var hero1 = await AddDefaultHeroAsync();
            var hero2 = await AddDefaultHeroAsync();

            var dtos = await _client.Request($"{Path}").GetJsonAsync<IEnumerable<HeroDto>>();

            dtos.ShouldNotBeEmpty();
            dtos.Count().ShouldBeGreaterThan(0);
            dtos.ShouldContain(h => h.Id == hero1.Id);
            dtos.ShouldContain(h => h.Id == hero2.Id);
        }

        [Fact]
        public async Task given_invalid_dto_when_add_hero_should_return_400()
        {
            var dto = new HeroDto
            {
                HeroName = "SuopA"
            };
            var expectedException = new InvalidHeroStateException(nameof(HeroDto.Attack));

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

        private async Task<Hero> AddDefaultHeroAsync()
        {
            var hero = new Hero
            (
                Guid.NewGuid(),
                $"DefaultHero{Guid.NewGuid().ToString("N")}",
                new State<int>(10, new IncreasingState<int>(2, StrategyIncreasing.ADDITIVE.ToString())),
                new State<int>(10, new IncreasingState<int>(2, StrategyIncreasing.ADDITIVE.ToString())),
                new State<int>(10, new IncreasingState<int>(2, StrategyIncreasing.ADDITIVE.ToString())),
                new State<decimal>(1000, new IncreasingState<decimal>(200, StrategyIncreasing.ADDITIVE.ToString())),
                new List<SkillHero> { new SkillHero(Guid.NewGuid(), "skill", 20, new IncreasingState<int>(10, StrategyIncreasing.ADDITIVE.ToString())) }
            );
            await _repository.AddAsync(hero);
            return hero;
        }

        private const string Path = "api/Heroes";
        private readonly IHeroRepository _repository;

        public HeroesControllerTests(TestApplicationFactory<Program> factory) : base(factory)
        {
            _repository = factory.Services.GetRequiredService<IHeroRepository>();
        }
    }
}
