using JAULABACKEND.DTOs;
using JAULABACKEND.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JAULABACKEND.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JugarController : ControllerBase
    {
        IJugarService _jugarService;

        public JugarController(
            [FromKeyedServices("Jugar2Service")]IJugarService jugarService)
        {
            _jugarService = jugarService;
        }
        
        [HttpPost("init")]
        public async Task<IActionResult> Init()
        {
            await _jugarService.InitAsync();
            return Ok("Se cargaron las listas");
        }
        [HttpGet("jugadores")]
        public async Task<IEnumerable<JugadorDto>> GetJugadores(){
            var x = await _jugarService.GetJugadores();
            return x;
        }
        [HttpGet("trabajadores")]
        public async Task<IEnumerable<TrabajadorDto>> get()
        {
            var x = await _jugarService.GetTrabajador();
            return x;
        }

        [HttpPost("{id}")]
        public async Task<IActionResult> JugadorCompraATrabajadorYJuega(int id)
        {

            var jugador = await _jugarService.GetJugadores();
            var jugadorEncontrado = jugador.FirstOrDefault(x => x.JugadorId == id);

            if (jugadorEncontrado != null)
            {
                ///await _jugarService.JugadorCompraATrabajador(jugadorEncontrado);

                _ = Task.Run(() =>
                {
                    _jugarService.JugadorCompraATrabajador(jugadorEncontrado);
                });
                return Ok("Proceso en segundo plano, revisar la consola");
            }
            return NotFound("Ese ID no existe, consulta get");

        }
    }
}
