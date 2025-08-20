using Microsoft.EntityFrameworkCore;

namespace JAULABACKEND.Models
{
    public class JaulaContext : DbContext
    {
        public JaulaContext(DbContextOptions<JaulaContext> options)
            : base(options)
        { }
        public DbSet<Jugador> Jugadores { get; set; }
        public DbSet<Trabajador> Trabajadores { get; set; }
    }
}
