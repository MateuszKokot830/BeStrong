using Application.Dto;
using AutoMapper;
using Domain.Aggregates;

namespace Application.Mappings
{
    public class AutoMapperConfig
    {
        public static IMapper Initialize() => new MapperConfiguration(config =>
        {
            config.CreateMap<UserAggregate, UserAggregateDto>();
            config.CreateMap<UserAggregateDto, UserAggregate>();
            config.CreateMap<UserAggregate, UserRegisterRequestDto>();
            config.CreateMap<UserRegisterRequestDto, UserAggregate>();
        })
        .CreateMapper();
    }
}