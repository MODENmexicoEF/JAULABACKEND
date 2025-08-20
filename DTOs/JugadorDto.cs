using static JAULABACKEND.Models.Jugador;

namespace JAULABACKEND.DTOs
{
    public class JugadorDto
    {
        public int JugadorId { get; set; }
        public string? Nombre { get; set; }
        public string ApellidoPaterno {  get; set; }
        public string ApellidoMaterno { get; set; }
        public int Edad {  get; set; }
        public decimal Estatura { get; set; }
        public decimal Peso { get; set; }
        public int Habilidad { get; set; }
        public Estado estadoJugador {  get; set; } = new Estado();
    }
}
