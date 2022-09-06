using FluentAssertions;
using RPG_GAME.Application.DTO.Players;
using RPG_GAME.Application.Events.Players;
using RPG_GAME.Application.Exceptions.Auth;
using RPG_GAME.Application.Exceptions.Heroes;
using RPG_GAME.Application.Exceptions.Players;
using RPG_GAME.Application.Mappings;
using RPG_GAME.Application.Services;
using RPG_GAME.Core.Entities.Heroes;
using RPG_GAME.Core.Entities.Players;
using RPG_GAME.Core.Entities.Users;
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
    public class PlayerServiceFlowTests
    {
        [Fact]
        public async Task should_add()
        {
            var user = await AddDefaultUser();
            var hero = await AddDefaultHero();
            var dto = new AddPlayerDto { Name = "Player#1", HeroId = hero.Id, UserId = user.Id };

            await _playerService.AddAsync(dto);

            var player = await _playerService.GetAsync(dto.Id);
            player.Should().NotBeNull();
            player.Hero.Id.Should().Be(dto.HeroId);
            player.UserId.Should().Be(dto.UserId);
        }

        [Fact]
        public async Task given_not_existing_hero_when_add_should_throw_an_exception()
        {
            var dto = new AddPlayerDto { Name = "Player#1", HeroId = Guid.NewGuid(), UserId = Guid.NewGuid() };
            var expectedException = new HeroNotFoundException(dto.HeroId);

            var exception = await Record.ExceptionAsync(() => _playerService.AddAsync(dto));

            exception.Should().NotBeNull();
            exception.Should().BeOfType(expectedException.GetType());
            exception.Message.Should().Be(expectedException.Message);
        }

        [Fact]
        public async Task given_not_existing_user_when_add_should_throw_an_exception()
        {
            var hero = await AddDefaultHero();
            var dto = new AddPlayerDto { Name = "Player#1", HeroId = hero.Id, UserId = Guid.NewGuid() };
            var expectedException = new UserNotFoundException(dto.UserId);

            var exception = await Record.ExceptionAsync(() => _playerService.AddAsync(dto));

            exception.Should().NotBeNull();
            exception.Should().BeOfType(expectedException.GetType());
            exception.Message.Should().Be(expectedException.Message);
        }

        [Fact]
        public async Task should_add_and_public_event()
        {
            var user = await AddDefaultUser();
            var hero = await AddDefaultHero();
            var dto = new AddPlayerDto { Name = "Player#1", HeroId = hero.Id, UserId = user.Id };

            await _playerService.AddAsync(dto);

            var messages = _messageBrokerStub.GetPublishedMessages();
            messages.Should().HaveCount(1);
            var ids = messages.Where(m => typeof(PlayerAdded).IsAssignableFrom(m.GetType()))
                .Select(m => ((PlayerAdded)m).PlayerId);
            ids.Should().Contain(dto.Id);
        }

        [Fact]
        public async Task should_update()
        {
            var player = await AddDefaultPlayer();
            var dto = new UpdatePlayerDto { Id = player.Id, CurrentExp = 100, HeroAttack = 200, HeroHealth = 300, HeroHealLvl = 100,
                Name = "Player#23", RequiredExp = 1200 };

            await _playerService.UpdateAsync(dto);

            var playerUpdated = await _playerService.GetAsync(dto.Id);
            playerUpdated.Should().NotBeNull();
            playerUpdated.Name.Should().Be(dto.Name);
            playerUpdated.Hero.Attack.Should().Be(dto.HeroAttack);
            playerUpdated.Hero.Health.Should().Be(dto.HeroHealth);
            playerUpdated.CurrentExp.Should().Be(dto.CurrentExp);
            playerUpdated.RequiredExp.Should().Be(dto.RequiredExp);
        }

        [Fact]
        public async Task given_not_existing_player_when_update_should_throw_an_exception()
        {
            var dto = new UpdatePlayerDto { Id = Guid.NewGuid(), CurrentExp = 100, HeroAttack = 200, HeroHealth = 300, HeroHealLvl = 100, Name = "Player#23", RequiredExp = 1200 };
            var expectedException = new PlayerNotFoundException(dto.Id);

            var exception = await Record.ExceptionAsync(() => _playerService.UpdateAsync(dto));

            exception.Should().NotBeNull();
            exception.Should().BeOfType(expectedException.GetType());
            exception.Message.Should().Be(expectedException.Message);
        }

        [Fact]
        public async Task given_not_existing_hero_skill_when_update_should_throw_an_exception()
        {
            var player = await AddDefaultPlayer();
            var skillId = Guid.NewGuid();
            var dto = new UpdatePlayerDto { Id = player.Id, CurrentExp = 100, HeroAttack = 200, HeroHealth = 300, HeroHealLvl = 100, Name = "Player#23", RequiredExp = 1200,
                HeroSkills = new List<UpdateHeroSkillDto> { new UpdateHeroSkillDto { SkillId = skillId, Attack = 1000 } } };
            var expectedException = new HeroSkillNotFoundException(skillId);

            var exception = await Record.ExceptionAsync(() => _playerService.UpdateAsync(dto));

            exception.Should().NotBeNull();
            exception.Should().BeOfType(expectedException.GetType());
            exception.Message.Should().Be(expectedException.Message);
        }

        [Fact]
        public async Task should_delete()
        {
            var player = await AddDefaultPlayer();

            await _playerService.RemoveAsync(player.Id);

            var playerDeleted = await _playerService.GetAsync(player.Id);
            playerDeleted.Should().BeNull();
        }

        [Fact]
        public async Task should_delete_and_publish_event()
        {
            var player = await AddDefaultPlayer();

            await _playerService.RemoveAsync(player.Id);

            var messages = _messageBrokerStub.GetPublishedMessages();
            messages.Should().HaveCount(1);
            var ids = messages.Where(m => typeof(PlayerDeleted).IsAssignableFrom(m.GetType()))
                .Select(m => ((PlayerDeleted)m).PlayerId);
            ids.Should().Contain(player.Id);
        }

        [Fact]
        public async Task given_not_existing_player_when_delete_should_throw_an_exception()
        {
            var id = Guid.NewGuid();
            var expectedException = new PlayerNotFoundException(id);

            var exception = await Record.ExceptionAsync(() => _playerService.RemoveAsync(id));

            exception.Should().NotBeNull();
            exception.Should().BeOfType(expectedException.GetType());
            exception.Message.Should().Be(expectedException.Message);
        }

        [Fact]
        public async Task should_get_all()
        {
            var player = await AddDefaultPlayer();
            var player2 = await AddDefaultPlayer();

            var players = await _playerService.GetAllAsync();

            players.Should().NotBeEmpty();
            players.Should().HaveCount(2);
            players.Select(p => p.Id).Should().Contain(new Guid[] { player.Id, player2.Id });
        }

        public async Task<Player> AddDefaultPlayer()
        {
            var hero = await AddDefaultHero();
            var user = await AddDefaultUser();
            var player = Player.Create("Player#1", hero.AsAssign(), 1000, user.Id);
            await _playerRepository.AddAsync(player);
            return player;
        }

        public async Task<Hero> AddDefaultHero()
        {
            var hero = EntitiesFixture.CreateDefaultHero();
            await _heroRepository.AddAsync(hero);
            return hero;
        }

        public async Task<User> AddDefaultUser()
        {
            var user = User.Create("email@email.com", "PasW0RDd12", DateTime.UtcNow, "user");
            await _userRepository.AddAsync(user);
            return user;
        }

        private readonly IPlayerService _playerService;
        private readonly IPlayerRepository _playerRepository;
        private readonly IHeroRepository _heroRepository;
        private readonly IUserRepository _userRepository;
        private readonly MessageBrokerStub _messageBrokerStub;
        
        public PlayerServiceFlowTests()
        {
            _playerRepository = new PlayerRepositoryStub();
            _heroRepository = new HeroRepositoryStub();
            _userRepository = new UserRepositoryStub();
            _messageBrokerStub = new MessageBrokerStub();
            _playerService = new PlayerService(_playerRepository, _heroRepository, _userRepository, _messageBrokerStub);
        }
    }
}
