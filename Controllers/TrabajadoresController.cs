using FluentValidation;
using JAULABACKEND.DTOs;
using JAULABACKEND.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JAULABACKEND.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrabajadoresController : ControllerBase
    {
        private IValidator<TrabajadorUpdateDto> _trabajadorUpdateValidator;
        private IValidator<TrabajadorInsertDto> _trabajadorInsertValidator;
        private ICommonService<TrabajadorDto, TrabajadorInsertDto, TrabajadorUpdateDto> _trabajadorService;

        public TrabajadoresController(IValidator<TrabajadorUpdateDto> trabajadorUpdateValidator,
            IValidator<TrabajadorInsertDto> trabajadorInsertValidator,
            [FromKeyedServices("trabajadoresService")] ICommonService<TrabajadorDto, TrabajadorInsertDto, TrabajadorUpdateDto> trabajadorService)
        {
            _trabajadorUpdateValidator = trabajadorUpdateValidator;
            _trabajadorInsertValidator = trabajadorInsertValidator;
            _trabajadorService = trabajadorService;
        }

        [HttpGet]
        public async Task<IEnumerable<TrabajadorDto>> Get()=>
            await _trabajadorService.Get();

        [HttpGet("{id}")]
        public async Task<ActionResult<TrabajadorDto>> GetById(int id)
        {
            var trabajadorDto = await _trabajadorService.GetById(id);

            return trabajadorDto == null ? NotFound() : Ok(trabajadorDto);
        }

        [HttpPost]
        public async Task<ActionResult<TrabajadorDto>> Add(TrabajadorInsertDto trabajadorInsert)
        {
            var validationResult = await _trabajadorInsertValidator.ValidateAsync(trabajadorInsert);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            var trabajadorDto = await _trabajadorService.Add(trabajadorInsert);

            return CreatedAtAction(nameof(GetById), new { id = trabajadorDto.TrabajadorId }, trabajadorDto);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<TrabajadorDto>> Update(int id, TrabajadorUpdateDto trabajadorUpdateDto)
        {
            var validationResult = await _trabajadorUpdateValidator.ValidateAsync(trabajadorUpdateDto);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            var trabajadorDto = await _trabajadorService.Update(id, trabajadorUpdateDto);

            return trabajadorDto == null ? NotFound() : Ok(trabajadorDto);
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult<TrabajadorDto>> Delete(int id)
        {
            var trabajadorDto = await _trabajadorService.Delete(id);

            return trabajadorDto == null ? NotFound() : Ok(trabajadorDto);
        }


    }
}
