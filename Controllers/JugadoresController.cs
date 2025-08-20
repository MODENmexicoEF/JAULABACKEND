using FluentValidation;
using JAULABACKEND.DTOs;
using JAULABACKEND.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JAULABACKEND.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JugadoresController : ControllerBase
    {
        private IValidator<JugadorInsertDto> _jugadorInsertValidator;
        private IValidator<JugadorUpdateDto> _jugadorUpdateValidator;
        private ICommonService<JugadorDto, JugadorInsertDto, JugadorUpdateDto> _jugadorService;

        public JugadoresController(IValidator<JugadorInsertDto> jugadorInsertValidator,
            IValidator<JugadorUpdateDto> jugadorUpdateValidator,
            [FromKeyedServices("jugadoresService")] ICommonService<JugadorDto, JugadorInsertDto, JugadorUpdateDto> jugadorService)
        {
            _jugadorInsertValidator = jugadorInsertValidator;
            _jugadorUpdateValidator = jugadorUpdateValidator;
            _jugadorService = jugadorService;
        }

        [HttpGet]
        public async Task<IEnumerable<JugadorDto>> Get()=>
            await _jugadorService.Get();

        [HttpGet("{id}")]
        public async Task<ActionResult<JugadorDto>> GetById(int id)
        {
            var jugadorDto = await _jugadorService.GetById(id);
            
            return jugadorDto == null ? NotFound() : Ok(jugadorDto);
        }


        [HttpPost]
        public async Task<ActionResult<JugadorDto>> Add(JugadorInsertDto jugadorInsert)
        {
            var validationResult = await _jugadorInsertValidator.ValidateAsync(jugadorInsert);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            var jugadorDto = await _jugadorService.Add(jugadorInsert);

            return CreatedAtAction(nameof(GetById), new {id = jugadorDto.JugadorId}, jugadorDto);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<JugadorDto>> Update(int id, JugadorUpdateDto jugadorUpdateDto)
        {
            var validationResult = await _jugadorUpdateValidator.ValidateAsync(jugadorUpdateDto);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            var jugadorDTO = await _jugadorService.Update(id, jugadorUpdateDto);

            return jugadorDTO == null ? NotFound() : Ok(jugadorDTO);
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult<JugadorDto>> Delete(int id)
        {
            var jugadorDto = await _jugadorService.Delete(id);

            return jugadorDto == null ? NotFound() : Ok(jugadorDto);
        }
    }
}
