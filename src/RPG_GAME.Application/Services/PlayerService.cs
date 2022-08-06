using RPG_GAME.Application.DTO.Players;
using RPG_GAME.Application.Exceptions.Auth;
using RPG_GAME.Application.Exceptions.Heroes;
using RPG_GAME.Application.Exceptions.Players;
using RPG_GAME.Application.Mappings;
using RPG_GAME.Core.Entities.Players;
using RPG_GAME.Core.Repositories;

namespace RPG_GAME.Application.Services
{
    internal sealed class PlayerService : IPlayerService
    {
        private readonly IPlayerRepository _playerRepository;
        private readonly IHeroRepository _heroRepository;
        private readonly IUserRepository _userRepository;

        public PlayerService(IPlayerRepository playerRepository, IHeroRepository heroRepository, IUserRepository userRepository)
        {
            _playerRepository = playerRepository;
            _heroRepository = heroRepository;
            _userRepository = userRepository;
        }

        public async Task AddAsync(AddPlayerDto playerDto)
        {
            var hero = await _heroRepository.GetAsync(playerDto.HeroId);

            if (hero is null)
            {
                throw new HeroNotFoundException(playerDto.HeroId);
            }

            var userExists = await _userRepository.ExistsAsync(playerDto.UserId);

            if (!userExists)
            {
                throw new UserNotFoundException(playerDto.UserId);
            }

            var heroAssing = hero.AsAssign();
            var player = Player.Create(playerDto.Name, heroAssing, hero.BaseRequiredExperience.Value, playerDto.UserId);
            await _playerRepository.AddAsync(player);
            playerDto.Id = player.Id;
        }

        public async Task RemoveAsync(Guid id)
        {
            var playerExists = await _playerRepository.GetAsync(id);

            if (playerExists is null)
            {
                throw new PlayerNotFoundException(id);
            }

            await _playerRepository.DeleteAsync(id);
        }

        public async Task UpdateAsync(UpdatePlayerDto playerDto)
        {
            var player = await _playerRepository.GetAsync(playerDto.Id);

            if (player is null)
            {
                throw new PlayerNotFoundException(playerDto.Id);
            }

            player.ChangeName(playerDto.Name);
            player.ChangeCurrentExp(playerDto.CurrentExp);
            player.ChangeRequiredExp(playerDto.RequiredExp);
            player.Hero.ChangeAttack(playerDto.HeroAttack);
            player.Hero.ChangeHealth(playerDto.HeroHealth);
            player.Hero.ChangeHealLvl(playerDto.HeroHealLvl);

            if (playerDto.HeroSkills is null)
            {
                await _playerRepository.UpdateAsync(player);
                return;
            }

            foreach (var skill in playerDto.HeroSkills)
            {
                var skillHero = player.Hero.Skills.FirstOrDefault(s => s.Id == skill.SkillId);

                if (skillHero is null)
                {
                    throw new HeroSkillNotFoundException(skill.SkillId);
                }

                skillHero.ChangeAttack(skill.Attack);
            }

            await _playerRepository.UpdateAsync(player);
        }

        public async Task<PlayerDto> GetAsync(Guid id)
        {
            var player = await _playerRepository.GetAsync(id);
            return player?.AsDto();
        }

        public async Task<IEnumerable<PlayerDto>> GetAllAsync()
        {
            var players = await _playerRepository.GetAllAsync();
            return players.Select(p => p.AsDto());
        }
    }
}
