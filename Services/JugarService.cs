using AutoMapper;
using JAULABACKEND.DTOs;
using JAULABACKEND.Models;
using JAULABACKEND.Repository;
using Microsoft.Extensions.Caching.Memory;

namespace JAULABACKEND.Services
{
    public class JugarService : IJugarService
    {
        private readonly Random random = new Random();
        private readonly IRepository<Jugador> _jugadoresRepository;
        private readonly IRepository<Trabajador> _trabajadoresRepository;
        private readonly IMapper _mapper;
        private readonly IMemoryCache _cache;

        private readonly object _lockTrabajadores = new();

        public JugarService(IRepository<Jugador> jugadoresRepository,
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
            var cola = _cache.Get<Queue<JugadorDto>>("ColaJugadores");

            if (cola == null)
            {
                cola = new Queue<JugadorDto>();
                _cache.Set("ColaJugadores", cola);
            }
            cola.Enqueue(jugadorSelected);
            //await InitAsync();
            //var jugadorSelected = jugadoresDtos.FirstOrDefault(x => x.JugadorId == idJugador);
            string nombreCom = jugadorSelected.Nombre + " " + jugadorSelected.ApellidoPaterno + " " + jugadorSelected.ApellidoMaterno;
            TrabajadorDto trabajadorAsignado = null;

            if (jugadorSelected == null) return;

            if (jugadorSelected.estadoJugador != Jugador.Estado.Desocupado)
            {
                Console.WriteLine($"{nombreCom} esta ocupado");
                return;
            }


            
            while (trabajadorAsignado == null)
            {
                lock (_lockTrabajadores)
                {
                    var trabajadores = _cache.Get<List<TrabajadorDto>>("Trabajadores");
                    trabajadorAsignado = trabajadores?.FirstOrDefault(x => x.Estado == false);
                    if (trabajadorAsignado != null)
                    {
                        trabajadorAsignado.Estado = true;
                    }
                    /*
                    var trabajadores = _cache.Get<List<TrabajadorDto>>("Trabajadores");
                    trabajadorAsignado = trabajadores?.FirstOrDefault(x => x.Estado == false);

                    if (trabajadorAsignado == null)
                    {
                        Console.WriteLine($"{nombreCom}\t\tEsperando por un trabajador");

                        await Task.Delay(1000);
                    }
                    */
                }
                if(trabajadorAsignado == null)
                {
                    var nombreComCola = $"{cola.First().Nombre} {cola.First().ApellidoPaterno} {cola.First().ApellidoMaterno}"; 
                    Console.WriteLine($"{nombreCom}\tEsperando por un trabajador");
                    await Task.Delay(1000);
                }
            }
            await ComprarBoleto(cola.First(), trabajadorAsignado);



        }
        public async Task ComprarBoleto(JugadorDto jugadorSelected, TrabajadorDto trabajadorAsignado)
        {
            var cola = _cache.Get<Queue<JugadorDto>>("ColaJugadores");
            string nombreCom = jugadorSelected.Nombre + " " + jugadorSelected.ApellidoPaterno + " " + jugadorSelected.ApellidoMaterno;
            string nombreComT = trabajadorAsignado.Nombre + " " + trabajadorAsignado.ApellidoPaterno + " " + trabajadorAsignado.ApellidoMaterno;
            try
            {
                //trabajadorAsignado.Estado = true;
                jugadorSelected.estadoJugador = Jugador.Estado.Comprando;
                if (!(jugadorSelected.estadoJugador == Jugador.Estado.Comprando ||
                    jugadorSelected.estadoJugador == Jugador.Estado.Jugando))
                {
                    Console.WriteLine($"{nombreCom}\t\tEsta ocupado");
                    return;
                }

                Console.WriteLine($"{nombreCom}\t\tEsta comprando boletos con {nombreComT}");

                await Task.Delay(7000);
                lock (_lockTrabajadores)
                {
                    if (cola == null)
                    {
                        cola = new Queue<JugadorDto>();
                        _cache.Set("ColaJugadores", cola);
                    }
                    
                    trabajadorAsignado.Estado = false; //Trabajador se libera
                }

                Console.WriteLine($"{nombreCom}\t\tEmpezara a jugar");
                var jugadorEntra = cola.First();
                cola.Dequeue();
                await JugadorJuega(jugadorEntra);
                //var trabajadorD
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException?.Message ?? ex.Message);
            }
        }

        public async Task JugadorJuega(JugadorDto jugador)
        {
            string nombreCom = jugador.Nombre + " " + jugador.ApellidoPaterno + " " + jugador.ApellidoMaterno;

            int count = 0;

            for (int i = 1; i <= 8; i++)
            {
                int tiro = random.Next(0, 101);
                bool tiroBola = tiro <= jugador.Habilidad;

                if (tiroBola) count++;
                await Task.Delay(1000);

                Console.WriteLine($"{nombreCom}\t {tiro} {tiroBola}");
            }
            Console.WriteLine($"{nombreCom}\t\t atino un total de {count} pelotas");
            jugador.estadoJugador = Jugador.Estado.Desocupado;
        }

        public  Task <IEnumerable<TrabajadorDto>> GetTrabajador()=>
            Task.FromResult(_cache.Get<List<TrabajadorDto>>("Trabajadores").AsEnumerable());
        

        public Task<IEnumerable<JugadorDto>> GetJugadores() =>
            Task.FromResult(_cache.Get<List<JugadorDto>>("Jugadores").AsEnumerable());


    }
}
