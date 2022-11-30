using Application.Dto;
using AutoMapper;
using Domain.Aggregates;
using Domain.Entities;

namespace Application.Mappings
{
    public class AutoMapperConfig
    {
        public static IMapper Initialize() => new MapperConfiguration(config =>
        {
            config.CreateMap<User, UserDto>()
                .ForMember(x => x.ProfilePhotoUrl, opt => opt.MapFrom(src => 
                src.Photos.FirstOrDefault(y => y.IsProfilePhoto).Url));
            config.CreateMap<UserDto, User>();
            config.CreateMap<User, UserRegisterRequestDto>();
            config.CreateMap<UserRegisterRequestDto, User>();
            config.CreateMap<PhotoDto, Photo>();
        })
        .CreateMapper();
    }
}