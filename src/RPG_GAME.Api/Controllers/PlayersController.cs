using Microsoft.AspNetCore.Mvc;
using RPG_GAME.Application.DTO.Players;
using RPG_GAME.Application.Services;

namespace RPG_GAME.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PlayersController : ControllerBase
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

        [HttpGet("{id}")]
        public async Task<ActionResult<PlayerDto>> Get(Guid id)
        {
            return Ok(await _playerService.GetAsync(id));
        }

        [HttpPost]
        public async Task<ActionResult> Add(PlayerDto playerDto)
        {
            await _playerService.AddAsync(playerDto);
            return CreatedAtAction(nameof(Get), new { Id = playerDto.Id }, default);
        }

        [HttpPut]
        public async Task<ActionResult> Update(PlayerDto playerDto)
        {
            await _playerService.UpdateAsync(playerDto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            await _playerService.RemoveAsync(id);
            return NoContent();
        }
    }
}
