using AutoMapper;
using MagicVilla_API.Data;
using MagicVilla_API.Models;
using MagicVilla_API.Models.Dto;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MagicVilla_API.Controllers
{
    [Route("api/VillaAPI")]
    [ApiController]
    public class VillaAPIController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;
        public VillaAPIController(ApplicationDbContext dbContext, IMapper mapper)
        {
            _mapper = mapper;
            _dbContext = dbContext;

        }
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<VillaDto>>> GetVillas()
        {
            IEnumerable<Villa> villas = await _dbContext.Villas.ToListAsync();
            return Ok(_mapper.Map<List<VillaDto>>(villas));
        }
        [HttpGet("id", Name = "GetVilla")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<VillaDto>> GetVilla(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }
            var villa = await _dbContext.Villas.FirstOrDefaultAsync(v => v.Id == id);
            if (villa == null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<VillaDto>(villa));
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<VillaDto>> CreateVilla([FromBody] VillaCreateDto createDto)
        {
            if (createDto == null) return BadRequest();
            Villa villa = _mapper.Map<Villa>(createDto);

            await _dbContext.Villas.AddAsync(villa);
            await _dbContext.SaveChangesAsync();

            return CreatedAtRoute("GetVilla", new { id = villa.Id }, villa);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [HttpDelete("{id}", Name = "DeleteVilla")]
        public async Task<IActionResult> DeleteVilla(int id)
        {
            if (id == 0) return BadRequest();
            var villa = await _dbContext.Villas.FirstOrDefaultAsync(v => v.Id == id);
            if (villa == null) return NotFound();
            _dbContext.Villas.Remove(villa);
            await _dbContext.SaveChangesAsync();
            return NoContent();
        }

        [HttpPut("{id}", Name = "UpdateVilla")]
        public async Task<IActionResult> UpdateVilla([FromBody] VillaUpdateDto updateVilla, int id)
        {
            if (updateVilla == null || updateVilla.Id != id) return BadRequest();

            if (!ModelState.IsValid) return BadRequest();

            Villa model = _mapper.Map<Villa>(updateVilla);
            _dbContext.Villas.Update(model);
            await _dbContext.SaveChangesAsync();

            return NoContent();
        }
        [HttpPatch("{id}", Name = "UpdatePartialVilla")]
        public async Task<IActionResult> UpdatePartialVilla(JsonPatchDocument<VillaUpdateDto> patchVilla, int id)
        {
            if (patchVilla == null || id == 0) return BadRequest();

            var villa = await _dbContext.Villas.AsNoTracking().FirstOrDefaultAsync(v => v.Id == id);
            if (villa == null) return BadRequest();

            VillaUpdateDto villaDto = _mapper.Map<VillaUpdateDto>(villa);
            patchVilla.ApplyTo(villaDto, ModelState);

            Villa model = _mapper.Map<Villa>(villaDto);
            _dbContext.Villas.Update(model);
            await _dbContext.SaveChangesAsync();

            if (!ModelState.IsValid) return BadRequest(ModelState);
            return NoContent();
        }
    }

}