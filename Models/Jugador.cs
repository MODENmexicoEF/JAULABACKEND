using AutoMapper.Configuration.Annotations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JAULABACKEND.Models
{

    public class Jugador
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int JugadorId {  get; set; }
        public string Nombre { get; set; }
        public string ApellidoPaterno { get; set; }
        public string ApellidoMaterno { get; set; }
        public int Edad {  get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Estatura {  get; set; }
        [Column(TypeName = "decimal(18,2)")]

        public decimal Peso { get; set; }
        public int Habilidad { get; set; }


         public enum Estado
        {
            Desocupado,
            Comprando,
            Jugando
        }
                
        public Estado _estadoJugador = Estado.Desocupado;

    }
}
