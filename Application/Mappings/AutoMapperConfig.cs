using Application.Dto;
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
                src.Photos.FirstOrDefault(y => y.IsProfilePhoto).Url));
            config.CreateMap<UserDto, User>();
            config.CreateMap<User, UserDto>();
            config.CreateMap<UserUpdateDto, User>();
            config.CreateMap<MeasurementsDto, Measurements>();
            config.CreateMap<Measurements, MeasurementsDto>();
            config.CreateMap<User, UserRegisterRequestDto>();
            config.CreateMap<UserRegisterRequestDto, User>();
            config.CreateMap<CommentDto, Comment>();
            config.CreateMap<Comment, CommentDto>();
            config.CreateMap<ExerciseDto, Exercise>();
            config.CreateMap<Exercise, ExerciseDto>();
            config.CreateMap<FollowerDto, Follower>();
            config.CreateMap<Follower, FollowerDto>();
            config.CreateMap<PhotoDto, Photo>();
            config.CreateMap<Photo, PhotoDto>();
            config.CreateMap<PostDto, Post>();
            config.CreateMap<Post, PostDto>();
            config.CreateMap<PostCreateDto, Post>();
            config.CreateMap<WorkoutDto, Workout>();
            config.CreateMap<Workout, WorkoutDto>();
            config.CreateMap<WorkoutExerciseDto, WorkoutExercise>();
            config.CreateMap<WorkoutExercise, WorkoutExerciseDto>();
        })
        .CreateMapper();
    }
}