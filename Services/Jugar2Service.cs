using AutoMapper;
using JAULABACKEND.DTOs;
using JAULABACKEND.Models;
using JAULABACKEND.Repository;
using Microsoft.Extensions.Caching.Memory;

namespace JAULABACKEND.Services
{
    public class Jugar2Service : IJugarService
    {
        private readonly Random random = new Random();
        private readonly IRepository<Jugador> _jugadoresRepository;
        private readonly IRepository<Trabajador> _trabajadoresRepository;
        private readonly IMapper _mapper;
        private readonly IMemoryCache _cache;

        private readonly object _lockCola = new();
        private readonly object _lockTrabajadores = new();

        public Jugar2Service(IRepository<Jugador> jugadoresRepository,
            IRepository<Trabajador> trabajadoresRepository,
            IMapper mapper,
            IMemoryCache cache) 
        {
            _jugadoresRepository = jugadoresRepository;
            _trabajadoresRepository = trabajadoresRepository;

            _mapper = mapper;

            _cache = cache;
        }
        //Enlistar a todos los jugadores y trabajadores. en la seccion de la api de "jugar" ya no se necesita hacer ninguna conexion
        //Con la db y las propiedades de tipo .Estado se quedaran en local

        public async Task InitAsync()
        {
            if(!_cache.TryGetValue("Jugadores",out List<JugadorDto> jugadoresDtos))
            {
                var jugadores = await _jugadoresRepository.Get();
                jugadoresDtos = jugadores.Select(x => _mapper.Map<JugadorDto>(x)).ToList();
                _cache.Set("Jugadores", jugadoresDtos);
            }

            if (!_cache.TryGetValue("Trabajadores", out List<TrabajadorDto> trabajadoresDtos))
            {
                var trabajadores = await _trabajadoresRepository.Get();
                trabajadoresDtos = trabajadores.Select(x => _mapper.Map<TrabajadorDto>(x)).ToList();
                _cache.Set("Trabajadores", trabajadoresDtos);
            }

            if(!_cache.TryGetValue("ColaJugadores", out Queue<JugadorDto> colaJugadores))
            {
                colaJugadores = new Queue<JugadorDto>();
                _cache.Set("ColaJugadores", colaJugadores);
            }
        }

        public async Task JugadorCompraATrabajador(JugadorDto jugadorSelected)
        {
            if (!(jugadorSelected.estadoJugador == Jugador.Estado.Desocupado)) return;
            SetLastJugador(jugadorSelected);
            try
            {
                while (!await TryTrabajadorLibre())
                {

                    var jugadorQueue = await GetFirstJugador();
                    Console.WriteLine($"{nomCom(jugadorQueue)}\tEsperando turno");
                    await Task.Delay(2000);

                }
                var jugador = await GetFirstJugador();
                var trabajador = await TrabajadorLibre();
                lock (_lockTrabajadores)
                {
                    trabajador.Estado = true;
                }
                lock (_lockCola)
                {
                    _cache.Get<Queue<JugadorDto>>("ColaJugadores").Dequeue();
                    jugador.estadoJugador = Jugador.Estado.Comprando;
                }
                ComprarBoleto(jugador, trabajador);
            }
            catch (Exception ex) { }
        }
        public async Task ComprarBoleto(JugadorDto jugadorSelected, TrabajadorDto trabajadorAsignado)
        {
            if (jugadorSelected.estadoJugador != Jugador.Estado.Comprando) return;

            Console.WriteLine("{0}\t Esta siendo atentido por {1}", nomCom(jugadorSelected).ToUpper(),nomComT(trabajadorAsignado));
            await Task.Delay(7000);

            lock (_lockTrabajadores)
            {
                trabajadorAsignado.Estado = false;
            }
            lock (_lockCola)
            {
                jugadorSelected.estadoJugador = Jugador.Estado.Jugando;
            }
            await JugadorJuega(jugadorSelected);
        }

        public async Task JugadorJuega(JugadorDto jugador)
        {
            if(jugador.estadoJugador != Jugador.Estado.Jugando) return;
            Console.WriteLine("{0}\tComenzara a jugar",nomCom(jugador));
            
            int count  = 0;
            for(int i = 1; i < 8; i++)
            {
                int tiro = random.Next(0, 101);
                bool tiroBola = tiro <= jugador.Habilidad;

                if(tiroBola) count++;
                await Task.Delay(1000);

                //Console.WriteLine("{0}\t {1} {2}",nomCom(jugador), tiro, tiroBola);
            }
            Console.WriteLine("{0}\t\t atino un total de {1} pelotas", nomCom(jugador), count);

            lock (_lockCola)
            {
                jugador.estadoJugador = Jugador.Estado.Desocupado;
            }
        }

        public Task <IEnumerable<TrabajadorDto>> GetTrabajador()=>
            Task.FromResult(_cache.Get<List<TrabajadorDto>>("Trabajadores").AsEnumerable());
        

        public Task<IEnumerable<JugadorDto>> GetJugadores() =>
            Task.FromResult(_cache.Get<List<JugadorDto>>("Jugadores").AsEnumerable());

        private Task<JugadorDto> GetFirstJugador()
        {
            var cola = _cache.Get<Queue<JugadorDto>>("ColaJugadores");
            if(cola == null || cola.Count == 0)
            {
                return Task.FromResult<JugadorDto>(null);
            }
            return Task.FromResult(cola.Peek());
        }

        private void SetLastJugador(JugadorDto jugadorDto)
        {
            lock (_lockCola)
            {
                var cola = _cache.Get<Queue<JugadorDto>>("ColaJugadores");
                if (cola == null)
                {
                    cola = new Queue<JugadorDto>();
                    _cache.Set("ColaJugadores", cola);
                }
                cola.Enqueue(jugadorDto);
            }
        }

        private async Task<bool> TryTrabajadorLibre()
        {
            var x = await GetTrabajador();
            lock (_lockTrabajadores)
            {
                if (x.FirstOrDefault(x => x.Estado == false) != null){
                    return true;
                }
                return false;
            }
        }
        private async Task<TrabajadorDto> TrabajadorLibre()
        {
            var GT = await GetTrabajador();
            lock (_lockTrabajadores)
            {
                return GT.FirstOrDefault(x => x.Estado == false);
            }

        }

        private string nomCom(JugadorDto j) =>
            $"{j.Nombre} {j.ApellidoPaterno} {j.ApellidoMaterno}";

        private string nomComT(TrabajadorDto j)=>
            $"{j.Nombre} {j.ApellidoPaterno} {j.ApellidoMaterno}";
        
    }
}
