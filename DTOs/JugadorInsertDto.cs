namespace JAULABACKEND.DTOs
{
    public class JugadorInsertDto
    {
        public string? Nombre { get; set; }
        public string? ApellidoPaterno { get; set; }
        public string? ApellidoMaterno { get; set; }
        public int Edad { get; set; }
        public decimal Estatura { get; set; }
        public decimal Peso { get; set; }
        public int Habilidad { get; set; }

    }
}
