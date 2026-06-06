using Application.Dto.Auth;
using Application.Dto.Comment;
using Application.Dto.Exercise;
using Application.Dto.Follower;
using Application.Dto.Photo;
using Application.Dto.Post;
using Application.Dto.User;
using Application.Dto.Workout;
using Application.Dto.WorkoutPlan;
using AutoMapper;
using Domain.Aggregates;
using Domain.Entities;
using Domain.ValueObjects;

namespace Application.Mappings;

public class MappingProfile : Profile
{
        public MappingProfile()
    {
        CreateMap<User, UserDto>()
            .ForMember(x => x.ProfilePhotoUrl, opt => opt.MapFrom((src, dest) =>
                src.Photos?.FirstOrDefault(y => y.IsProfilePhoto)?.Url))
            .ReverseMap();

        CreateMap<UserUpdateDto, User>().ReverseMap();
        CreateMap<MeasurementsDto, Measurements>().ReverseMap();
        CreateMap<User, UserRegisterRequestDto>().ReverseMap();
        CreateMap<CommentDto, Comment>().ReverseMap();
        CreateMap<CommentCreateDto, Comment>().ReverseMap();
        CreateMap<ExerciseDto, Exercise>().ReverseMap();
        CreateMap<FollowerDto, Follower>().ReverseMap();
        CreateMap<PhotoDto, Photo>().ReverseMap();
        CreateMap<PostDto, Post>().ReverseMap();
        CreateMap<PostCreateDto, Post>().ReverseMap();
        CreateMap<WorkoutDto, Workout>().ReverseMap();
        CreateMap<WorkoutExerciseDto, WorkoutExercise>().ReverseMap();
        CreateMap<WorkoutPlanDto, WorkoutPlan>().ReverseMap();
        CreateMap<WorkoutPlanCreateDto, WorkoutPlan>().ReverseMap();
    }
}