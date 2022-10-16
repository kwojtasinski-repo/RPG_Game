using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RPG_GAME.Application.DTO.Players;
using RPG_GAME.Application.Services;

namespace RPG_GAME.Api.Controllers
{
    [Authorize]
    public class PlayersController : BaseController
    {
        private readonly IPlayerService _playerService;

        public PlayersController(IPlayerService playerService)
        {
            _playerService = playerService;
        }

        [HttpGet]
        public async Task<IEnumerable<PlayerDto>> GetAll()
        {
            return await _playerService.GetAllAsync();
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<PlayerDto>> Get(Guid id)
        {
            return OkOrNotFound(await _playerService.GetAsync(id));
        }

        [HttpPost]
        public async Task<ActionResult> Add(AddPlayerDto playerDto)
        {
            await _playerService.AddAsync(playerDto);
            return CreatedAtAction(nameof(Get), new { Id = playerDto.Id }, default);
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult> Update(Guid id, UpdatePlayerDto playerDto)
        {
            playerDto.Id = id;
            await _playerService.UpdateAsync(playerDto);
            return NoContent();
        }

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            await _playerService.RemoveAsync(id);
            return NoContent();
        }
    }
}
