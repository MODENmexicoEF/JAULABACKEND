using AutoMapper;
using JAULABACKEND.DTOs;
using JAULABACKEND.Models;
using JAULABACKEND.Repository;

namespace JAULABACKEND.Services
{
    public class TrabajadoresService : ICommonService<TrabajadorDto, TrabajadorInsertDto, TrabajadorUpdateDto>
    {
        private IRepository<Trabajador> _trabajadorRepository;
        private IMapper _mapper;

        public TrabajadoresService(IMapper mapper, IRepository<Trabajador> trabajadorRepository)
        {
            _mapper = mapper;
            _trabajadorRepository = trabajadorRepository;
        }

        public async Task<IEnumerable<TrabajadorDto>> Get()
        {
            var trabajadores = await _trabajadorRepository.Get();

            return trabajadores.Select(x => _mapper.Map<TrabajadorDto>(x));
        }

        public async Task<TrabajadorDto> GetById(int id)
        {
            var trabajador = await _trabajadorRepository.GetById(id);

            if(trabajador != null)
            {
                var trabajadorDto = _mapper.Map<TrabajadorDto>(trabajador);
                return trabajadorDto;
            }
            return null;
        }

        public async Task<TrabajadorDto> Add(TrabajadorInsertDto insertDto)
        {
            var trabajador = _mapper.Map<Trabajador>(insertDto);

            await _trabajadorRepository.Add(trabajador);
            await _trabajadorRepository.Save();

            var trabajadorDto = _mapper.Map<TrabajadorDto>(trabajador);

            return trabajadorDto;
        }

        public async Task<TrabajadorDto> Update(int id, TrabajadorUpdateDto trabajadorUpdateDto)
        {
            var trabajador = await _trabajadorRepository.GetById(id);

            if( trabajador != null)
            {
                trabajador = _mapper.Map<TrabajadorUpdateDto, Trabajador>(trabajadorUpdateDto, trabajador);

                _trabajadorRepository.Update(trabajador);
                await _trabajadorRepository.Save();

                var trabajadorDto = _mapper.Map<TrabajadorDto>(trabajador);

                return trabajadorDto;
            }
            return null;
        }

        public async Task<TrabajadorDto> Delete(int id)
        {
            var trabajador = await _trabajadorRepository.GetById(id);

            if( trabajador != null)
            {
                var trabajadorDto = _mapper.Map<TrabajadorDto>(trabajador);

                _trabajadorRepository.Delete(trabajador);

                await _trabajadorRepository.Save();

                return trabajadorDto;
            }
            return null;
        }



    }
}
