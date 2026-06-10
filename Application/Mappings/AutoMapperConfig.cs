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
            .ConstructUsing((src, ctx) => new UserDto(
                src.Id,
                src.UserName ?? string.Empty,
                src.DateOfBirth,
                src.DateOfWorkoutStart,
                src.Name,
                src.Surname,
                src.Gender,
                src.City,
                src.Country,
                src.Description,
                src.Photos?.FirstOrDefault(p => p.IsProfilePhoto)?.Url,
                src.Age,
                src.WorkoutSince,
                ctx.Mapper.Map<MeasurementsDto>(src.Measurements),
                ctx.Mapper.Map<IReadOnlyCollection<PhotoDto>>(src.Photos),
                ctx.Mapper.Map<IReadOnlyCollection<PostDto>>(src.Posts),
                ctx.Mapper.Map<IReadOnlyCollection<FollowerDto>>(src.FollowedUsers),
                ctx.Mapper.Map<IReadOnlyCollection<FollowerDto>>(src.Followers)
            ))
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