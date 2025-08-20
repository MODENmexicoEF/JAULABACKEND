using JAULABACKEND.Models;
using Microsoft.EntityFrameworkCore;
using System.Formats.Asn1;

namespace JAULABACKEND.Repository
{
    public class TrabajadoresRepository : IRepository<Trabajador>
    {
        private JaulaContext _context;

        public TrabajadoresRepository(JaulaContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Trabajador>> Get()=>
            await _context.Trabajadores.ToListAsync();
        

        public async Task<Trabajador> GetById(int id)=>
            await _context.Trabajadores.FindAsync(id);
        

        public async Task Add(Trabajador entity)
        {
            await _context.Trabajadores.AddAsync(entity);
        }

        public void Delete(Trabajador entity)=>
            _context.Trabajadores.Remove(entity);
        
        public void Update(Trabajador entity)
        {
            _context.Trabajadores.Attach(entity);
            _context.Trabajadores.Entry(entity).State = EntityState.Modified;
        }
        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<Trabajador?> GetFristDisponible()
        {
            return await _context.Trabajadores
                .FirstOrDefaultAsync(x => x.Estado == false);
        }
    }
}
