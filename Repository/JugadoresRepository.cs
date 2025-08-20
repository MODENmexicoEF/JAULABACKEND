using JAULABACKEND.Models;
using Microsoft.EntityFrameworkCore;

namespace JAULABACKEND.Repository
{
    public class JugadoresRepository : IRepository<Jugador>
    {
        private JaulaContext _context;

        public JugadoresRepository(JaulaContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Jugador>> Get() =>
            await _context.Jugadores.ToListAsync();

        public async Task<Jugador> GetById(int id) =>
            await _context.Jugadores.FindAsync(id);

        public async Task Add(Jugador entity)
        {
            await _context.Jugadores.AddAsync(entity);
        }

        public void Delete(Jugador entity)=>
            _context.Jugadores.Remove(entity);


        public void Update(Jugador entity)
        {
            _context.Jugadores.Attach(entity);
            _context.Jugadores.Entry(entity).State = EntityState.Modified;
        }

        public async Task Save()=>
            await _context.SaveChangesAsync();

        public Task<Jugador?> GetFristDisponible()
        {
            throw new NotImplementedException();
        }
    }
}
