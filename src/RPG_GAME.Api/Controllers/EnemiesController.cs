using Microsoft.AspNetCore.Mvc;
using RPG_GAME.Application.Services;
using RPG_GAME.Application.DTO.Enemies;

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
        public async Task<ActionResult> Add(EnemyDto enemyDto)
        {
            await _enemyService.AddAsync(enemyDto);
            return CreatedAtAction(nameof(Get), new { Id = enemyDto.Id }, default);
        }

        [HttpPut]
        public async Task<ActionResult> Update(EnemyDto enemyDto)
        {
            await _enemyService.UpdateAsync(enemyDto);
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
