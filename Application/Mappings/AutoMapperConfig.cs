using Application.Dto;
using Application.Helpers;
using AutoMapper;
using Domain.Aggregates;
using Domain.Entities;
using Domain.ValueObjects;

namespace Application.Mappings
{
    public class AutoMapperConfig
    {
        public static IMapper Initialize() => new MapperConfiguration(config =>
        {
            config.CreateMap<User, UserDto>()
                .ForMember(x => x.ProfilePhotoUrl, opt => opt.MapFrom(src => 
                    src.Photos.FirstOrDefault(y => y.IsProfilePhoto).Url)).ReverseMap();
            config.CreateMap<UserUpdateDto, User>().ReverseMap();
            config.CreateMap<MeasurementsDto, Measurements>().ReverseMap();
            config.CreateMap<User, UserRegisterRequestDto>().ReverseMap();
            config.CreateMap<CommentDto, Comment>().ReverseMap();
            config.CreateMap<CommentCreateDto, Comment>().ReverseMap();
            config.CreateMap<ExerciseDto, Exercise>().ReverseMap();
            config.CreateMap<FollowerDto, Follower>().ReverseMap();
            config.CreateMap<PhotoDto, Photo>().ReverseMap();
            config.CreateMap<PostDto, Post>().ReverseMap();
            config.CreateMap<PostCreateDto, Post>().ReverseMap();
            config.CreateMap<WorkoutDto, Workout>().ReverseMap();
            config.CreateMap<WorkoutExerciseDto, WorkoutExercise>().ReverseMap();
        })
        .CreateMapper();
    }
}