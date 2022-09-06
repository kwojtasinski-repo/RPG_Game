using Flurl.Http;
using Microsoft.Extensions.DependencyInjection;
using RPG_GAME.Application.DTO.Players;
using RPG_GAME.Application.Exceptions.Heroes;
using RPG_GAME.Application.Mappings;
using RPG_GAME.Core.Entities.Common;
using RPG_GAME.Core.Entities.Heroes;
using RPG_GAME.Core.Entities.Players;
using RPG_GAME.Core.Entities.Users;
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
    public class PlayersControllerTests : ApiTestBase
    {
        [Fact]
        public async Task should_add_player_and_return_201()
        {
            var user = await AddDefaultUserAsync();
            var hero = await AddDefaultHeroAsync();
            var addPlayer = new AddPlayerDto { Name = "player", HeroId = hero.Id, UserId = user.Id };

            var response = await _client.Request($"{Path}").PostJsonAsync(addPlayer);

            response.StatusCode.ShouldBe((int)HttpStatusCode.Created);
            var id = GetIdFromHeader(response, Path);
            id.ShouldNotBe(Guid.Empty);
        }

        [Fact]
        public async Task should_add_player_to_database()
        {
            var user = await AddDefaultUserAsync();
            var hero = await AddDefaultHeroAsync();
            var addPlayer = new AddPlayerDto { Name = "player2", HeroId = hero.Id, UserId = user.Id };

            var response = await _client.Request($"{Path}").PostJsonAsync(addPlayer);

            var id = GetIdFromHeader(response, Path);
            var player = await _repository.GetAsync(id);
            player.Hero.Attack.ShouldBe(hero.Attack.Value);
            player.Hero.Health.ShouldBe(hero.Health.Value);
            player.RequiredExp.ShouldBe(hero.BaseRequiredExperience.Value);
            player.UserId.ShouldBe(user.Id);
        }

        [Fact]
        public async Task should_update_player_and_return_204()
        {
            var player = await AddDefaultPlayerAsync();
            var updatePlayer = new UpdatePlayerDto
            {
                Name = "test-player",
                CurrentExp = 1000,
                RequiredExp = 10000,
                HeroAttack = 200,
                HeroHealLvl = 50,
                HeroHealth = 1000,
                HeroSkills = new List<UpdateHeroSkillDto> { new UpdateHeroSkillDto { SkillId = player.Hero.Skills.First().Id, Attack = 5000 } }
            };

            var response = await _client.Request($"{Path}/{player.Id}").PutJsonAsync(updatePlayer);

            response.StatusCode.ShouldBe((int)HttpStatusCode.NoContent);
        }
        
        [Fact]
        public async Task should_update_player()
        {
            var player = await AddDefaultPlayerAsync();
            var updatePlayer = new UpdatePlayerDto
            {
                Name = $"test-player-{Guid.NewGuid().ToString("N")}",
                CurrentExp = 1000,
                RequiredExp = 10000,
                HeroAttack = 200,
                HeroHealLvl = 50,
                HeroHealth = 1000,
                HeroSkills = new List<UpdateHeroSkillDto> { new UpdateHeroSkillDto { SkillId = player.Hero.Skills.First().Id, Attack = 5000 } }
            };

            await _client.Request($"{Path}/{player.Id}").PutJsonAsync(updatePlayer);

            var playerUpdated = await _repository.GetAsync(player.Id);
            playerUpdated.Hero.Attack.ShouldBe(updatePlayer.HeroAttack);
            playerUpdated.Hero.Health.ShouldBe(updatePlayer.HeroHealth);
            playerUpdated.CurrentExp.ShouldBe(updatePlayer.CurrentExp);
            playerUpdated.RequiredExp.ShouldBe(updatePlayer.RequiredExp);
            playerUpdated.Hero.Skills.ShouldNotBeEmpty();
            playerUpdated.Hero.Skills.Count().ShouldBe(updatePlayer.HeroSkills.Count());
            var skillHeroDto = updatePlayer.HeroSkills.First();
            var skillHeroUpdated = playerUpdated.Hero.Skills.First();
            skillHeroUpdated.Attack.ShouldBe(skillHeroDto.Attack);
        }

        [Fact]
        public async Task should_delete_player()
        {
            var player = await AddDefaultPlayerAsync();

            var response = await _client.Request($"{Path}/{player.Id}").DeleteAsync();

            response.StatusCode.ShouldBe((int)HttpStatusCode.NoContent);
            var heroDeteled = await _repository.GetAsync(player.Id);
            heroDeteled.ShouldBeNull();
        }

        [Fact]
        public async Task should_get_player()
        {
            var player = await AddDefaultPlayerAsync();

            var response = await _client.Request($"{Path}/{player.Id}").GetAsync();

            response.StatusCode.ShouldBe((int)HttpStatusCode.OK);
            var dto = await response.GetJsonAsync<PlayerDto>();
            dto.Id.ShouldBe(player.Id);
            dto.Name.ShouldBe(player.Name);
        }

        [Fact]
        public async Task given_invalid_player_id_should_return_404()
        {
            var playerId = Guid.NewGuid();

            var response = await _client.AllowHttpStatus("404").Request($"{Path}/{playerId}").GetAsync();

            response.StatusCode.ShouldBe((int)HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task should_get_players()
        {
            var player1 = await AddDefaultPlayerAsync();
            var player2 = await AddDefaultPlayerAsync();

            var response = await _client.Request($"{Path}").GetAsync();

            response.StatusCode.ShouldBe((int)HttpStatusCode.OK);
            var dtos = await response.GetJsonAsync<IEnumerable<PlayerDto>>();
            dtos.ShouldContain(d => d.Id == player1.Id);
            dtos.ShouldContain(d => d.Id == player2.Id);
        }

        [Fact]
        public async Task given_invalid_player_name_should_return_400()
        {
            var addPlayer = new AddPlayerDto { HeroId = Guid.NewGuid() };
            var expectedException = new HeroNotFoundException(addPlayer.HeroId);

            var response = await _client.AllowHttpStatus("400").Request($"{Path}").PostJsonAsync(addPlayer);

            response.StatusCode.ShouldBe((int)HttpStatusCode.BadRequest);
            var message = JsonSerializer.Deserialize<Dictionary<string, string>>(await response.ResponseMessage.Content.ReadAsStringAsync());
            message.Values.ShouldContain(expectedException.Message);
        }

        private async Task<Player> AddDefaultPlayerAsync()
        {
            var user = await AddDefaultUserAsync();
            var hero = await AddDefaultHeroAsync();
            var player = new Player
            (
                Guid.NewGuid(),
                $"DefaultPlayer{Guid.NewGuid().ToString("N")}",
                hero.AsAssign(),
                1,
                0,
                1000,
                user.Id
            );
            await _repository.AddAsync(player);
            return player;
        }

        private async Task<Hero> AddDefaultHeroAsync()
        {
            var hero = new Hero
            (
                Guid.NewGuid(),
                $"DefaultHero{Guid.NewGuid().ToString("N")}",
                new State<int>(10, new IncreasingState<int>(2, StrategyIncreasing.ADDITIVE.ToString())),
                new State<int>(10, new IncreasingState<int>(2, StrategyIncreasing.ADDITIVE.ToString())),
                new State<decimal>(1000, new IncreasingState<decimal>(200, StrategyIncreasing.ADDITIVE.ToString())),
                new List<SkillHero> { new SkillHero(Guid.NewGuid(), "skill", 20, new IncreasingState<int>(10, StrategyIncreasing.ADDITIVE.ToString())) }
            );
            await _heroRepository.AddAsync(hero);
            return hero;
        }

        private async Task<User> AddDefaultUserAsync()
        {
            var user = new User(Guid.NewGuid(), $"velvet_{Guid.NewGuid().ToString("N")}@test.com", "password", "user", DateTime.UtcNow);
            await _userRepository.AddAsync(user);
            return user;
        }

        private const string Path = "api/Players";
        private readonly IPlayerRepository _repository;
        private readonly IHeroRepository _heroRepository;
        private readonly IUserRepository _userRepository;

        public PlayersControllerTests(TestApplicationFactory<Program> factory) : base(factory)
        {
            _repository = factory.Services.GetRequiredService<IPlayerRepository>();
            _heroRepository = factory.Services.GetRequiredService<IHeroRepository>();
            _userRepository = factory.Services.GetRequiredService<IUserRepository>();
        }
    }
}
