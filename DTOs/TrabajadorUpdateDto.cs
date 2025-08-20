using System.ComponentModel.DataAnnotations.Schema;

namespace JAULABACKEND.DTOs
{
    public class TrabajadorUpdateDto
    {
        public int TrabajadorId { get; set; }
        public string Nombre { get; set; }
        public string ApellidoPaterno { get; set; }
        public string ApellidoMaterno { get; set; }
        public int Edad { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Salario { get; set; }
    }
}
