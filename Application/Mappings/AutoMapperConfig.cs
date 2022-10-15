using Application.Dto;
using AutoMapper;
using Domain.Entities;

namespace Application.Mappings
{
    public class AutoMapperConfig
    {
        public static IMapper Initialize() => new MapperConfiguration(config =>
        {
            config.CreateMap<AppUser, AppUserDto>();
        })
        .CreateMapper();
    }
}