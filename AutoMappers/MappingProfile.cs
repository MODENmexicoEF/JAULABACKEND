using JAULABACKEND.DTOs;
using JAULABACKEND.Models;
using AutoMapper;

namespace JAULABACKEND.AutoMappers
{
    public class MappingProfile : Profile
    {
        public MappingProfile() 
        {
            CreateMap<JugadorInsertDto, Jugador>();
            CreateMap<Jugador,JugadorDto>()
                .ForMember(dto => dto.JugadorId,
                    m => m.MapFrom(b => b.JugadorId));

            CreateMap<JugadorUpdateDto, Jugador>();

            CreateMap<TrabajadorInsertDto, Trabajador>();
            CreateMap<Trabajador, TrabajadorDto>()
                .ForMember(dto => dto.TrabajadorId,
                    m => m.MapFrom(b => b.TrabajadorId));

            CreateMap<TrabajadorUpdateDto, Trabajador>();

        }
    }
}
