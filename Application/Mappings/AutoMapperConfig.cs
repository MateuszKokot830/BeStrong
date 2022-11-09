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
        })
        .CreateMapper();
    }
}