using JAULABACKEND.DTOs;

namespace JAULABACKEND.Services
{
    public interface IJugarService
    {
        Task InitAsync();
        Task JugadorJuega(JugadorDto jugador); 
        Task JugadorCompraATrabajador(JugadorDto jugadorSelected); 
        Task ComprarBoleto(JugadorDto jugadorSelected, TrabajadorDto trabajadorAsignado);
        Task<IEnumerable<TrabajadorDto>> GetTrabajador();
        Task <IEnumerable<JugadorDto>> GetJugadores();

    }
}
