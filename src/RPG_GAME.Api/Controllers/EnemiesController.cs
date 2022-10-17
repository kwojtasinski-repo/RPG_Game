using Microsoft.AspNetCore.Mvc;
using RPG_GAME.Application.Services;
using RPG_GAME.Application.DTO.Enemies;
using Microsoft.AspNetCore.Authorization;

namespace RPG_GAME.Api.Controllers
{
    [Authorize]
    public class EnemiesController : BaseController
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

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<EnemyDetailsDto>> Get(Guid id)
        {
            return OkOrNotFound(await _enemyService.GetAsync(id));
        }

        [Authorize(Policy = "is-admin")]
        [HttpPost]
        public async Task<ActionResult> Add(EnemyDto enemyDto)
        {
            await _enemyService.AddAsync(enemyDto);
            return CreatedAtAction(nameof(Get), new { Id = enemyDto.Id }, default);
        }

        [Authorize(Policy = "is-admin")]
        [HttpPut("{id:guid}")]
        public async Task<ActionResult> Update(Guid id, EnemyDto enemyDto)
        {
            enemyDto.Id = id;
            await _enemyService.UpdateAsync(enemyDto);
            return NoContent();
        }

        [Authorize(Policy = "is-admin")]
        [HttpDelete("{id:guid}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            await _enemyService.RemoveAsync(id);
            return NoContent();
        }
    }
}
