using AutoMapper;
using JAULABACKEND.DTOs;
using JAULABACKEND.Models;
using JAULABACKEND.Repository;

namespace JAULABACKEND.Services
{
    public class JugadoresService : ICommonService<JugadorDto, JugadorInsertDto, JugadorUpdateDto>
    {
        private IRepository<Jugador> _jugadorRepository;
        private IMapper _mapper;


        public JugadoresService(IRepository<Jugador> jugadorRepository, IMapper mapper)
        {
            _jugadorRepository = jugadorRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<JugadorDto>> Get()
        {
            var jugadores = await _jugadorRepository.Get();

            return jugadores.Select(x => _mapper.Map<JugadorDto>(x));
        }

        public async Task<JugadorDto> GetById(int id)
        {
            var jugador = await _jugadorRepository.GetById(id);

            if (jugador != null)
            {
                var jugadorDto = _mapper.Map<JugadorDto>(jugador);
                return jugadorDto;
            }
            return null;
        }

        public async Task<JugadorDto> Add(JugadorInsertDto insertDto)
        {
            var jugador = _mapper.Map<Jugador>(insertDto);

            await _jugadorRepository.Add(jugador);
            await _jugadorRepository.Save();

            var jugadorDto = _mapper.Map<JugadorDto>(jugador);

            return jugadorDto;
        }

        public async Task<JugadorDto> Update(int id, JugadorUpdateDto jugadorUpdateDto)
        {
            var jugador = await _jugadorRepository.GetById(id);

            if(jugador != null)
            {
                jugador = _mapper.Map<JugadorUpdateDto, Jugador>(jugadorUpdateDto, jugador);

                _jugadorRepository.Update(jugador);
                await _jugadorRepository.Save();

                var jugadorDto = _mapper.Map<JugadorDto>(jugador);

                return jugadorDto;
            }
            return null;
        }

        public async Task<JugadorDto> Delete(int id)
        {
            var jugador = await _jugadorRepository.GetById(id);

            if(jugador != null)
            {
                var jugadorDto = _mapper.Map<JugadorDto>(jugador);

                _jugadorRepository.Delete(jugador);
                await _jugadorRepository.Save();

                return jugadorDto;
            }
            return null;
        }
    }
}
