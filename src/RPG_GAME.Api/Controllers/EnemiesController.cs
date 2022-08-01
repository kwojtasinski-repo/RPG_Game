using Microsoft.AspNetCore.Mvc;
using RPG_GAME.Application.Services;
using RPG_GAME.Application.DTO;

namespace RPG_GAME.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EnemiesController : ControllerBase
    {
        private readonly IEnemyService _enemyService;

        public EnemiesController(IEnemyService enemyService)
        {
            _enemyService = enemyService;
        }

        [HttpGet]
        public async Task<IEnumerable<EnemyDto>> GetAll()
        {
            return await _enemyService.GetAllAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<EnemyDetailsDto>> Get(Guid id)
        {
            return await _enemyService.GetAsync(id);
        }

        [HttpPost]
        public async Task<ActionResult> Add(EnemyDetailsDto enemyDetailsDto)
        {
            await _enemyService.AddAsync(enemyDetailsDto);
            return CreatedAtAction(nameof(Get), new { Id = enemyDetailsDto.Id }, default);
        }

        [HttpPut]
        public async Task<ActionResult> Update(EnemyDetailsDto enemyDetailsDto)
        {
            await _enemyService.UpdateAsync(enemyDetailsDto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            await _enemyService.RemoveAsync(id);
            return NoContent();
        }
    }
}
