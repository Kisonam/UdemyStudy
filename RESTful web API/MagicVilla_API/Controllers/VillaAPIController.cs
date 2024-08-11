using System.Net;
using AutoMapper;
using MagicVilla_API.Models;
using MagicVilla_API.Models.Dto;
using MagicVilla_API.Repository.IRepository;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace MagicVilla_API.Controllers
{
    [Route("api/VillaAPI")]
    [ApiController]
    public class VillaAPIController : ControllerBase
    {
        protected APIResponse _response;
        private readonly IVillaRepository _repository;
        private readonly IMapper _mapper;
        public VillaAPIController(IVillaRepository repository, IMapper mapper)
        {
            this._repository = repository;
            _mapper = mapper;
            _response = new();

        }
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> GetVillas()
        {
            try
            {
                var villas = await _repository.GetVillas();
                _response.Result = _mapper.Map<List<VillaDto>>(villas);
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = true;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }
            return _response;
        }
        [HttpGet("id", Name = "GetVilla")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<APIResponse>> GetVilla(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.DisplayMessage = "Id can not be zero";
                    _response.IsSuccess = false;

                    return BadRequest(_response);
                }
                var villa = await _repository.GetVilla(v => v.Id == id);
                if (villa == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.DisplayMessage = "Invalid Id";
                    _response.IsSuccess = false;

                    return NotFound(_response);
                }
                _response.Result = _mapper.Map<VillaDto>(villa);
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }
            return _response;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> CreateVilla([FromBody] VillaCreateDto createDto)
        {
            try
            {
                if (createDto == null)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsSuccess = false;
                    return BadRequest(_response);
                }
                Villa villa = _mapper.Map<Villa>(createDto);

                await _repository.Create(villa);
                _response.Result = _mapper.Map<VillaDto>(villa);
                _response.StatusCode = HttpStatusCode.Created;
                return CreatedAtRoute("GetVilla", new { id = villa.Id }, _response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }
            return _response;
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [HttpDelete("{id}", Name = "DeleteVilla")]
        public async Task<ActionResult<APIResponse>> DeleteVilla(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }
                var villa = await _repository.GetVilla(v => v.Id == id);
                if (villa == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

                await _repository.Remove(villa);
                _response.StatusCode = HttpStatusCode.NoContent;
                _response.IsSuccess = true;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }
            return _response;
        }

        [HttpPut("{id}", Name = "UpdateVilla")]
        public async Task<ActionResult<APIResponse>> UpdateVilla([FromBody] VillaUpdateDto updateVilla, int id)
        {
            try
            {
                if (updateVilla == null || updateVilla.Id != id)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest();
                }

                if (!ModelState.IsValid) return BadRequest();

                Villa model = _mapper.Map<Villa>(updateVilla);
                await _repository.Update(model);
                _response.StatusCode = HttpStatusCode.NoContent;
                _response.IsSuccess = true;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };

            }
            return _response;
        }
        [HttpPatch("{id}", Name = "UpdatePartialVilla")]
        public async Task<IActionResult> UpdatePartialVilla(JsonPatchDocument<VillaUpdateDto> patchVilla, int id)
        {
            if (patchVilla == null || id == 0) return BadRequest();

            var villa = await _repository.GetVilla(v => v.Id == id, tracked: false);
            if (villa == null) return BadRequest();

            VillaUpdateDto villaDto = _mapper.Map<VillaUpdateDto>(villa);
            patchVilla.ApplyTo(villaDto, ModelState);

            Villa model = _mapper.Map<Villa>(villaDto);
            await _repository.Update(model);


            if (!ModelState.IsValid) return BadRequest(ModelState);
            return NoContent();
        }
    }

}