using Application.Dto.Follower;
using Application.Dto.Photo;
using Application.Dto.Post;
using Domain.Common;

namespace Application.Dto.User
{
    public record UserDto(
        int Id,
        string UserName,
        DateTime DateOfBirth,
        DateTime DateOfWorkoutStart,
        string? Name,
        string? Surname,
        Gender Gender,
        string? City,
        string? Country,
        string? Description,
        string? ProfilePhotoUrl,
        int Age,
        string? WorkoutSince,
        MeasurementsDto? Measurements,
        IReadOnlyCollection<PhotoDto> Photos,
        IReadOnlyCollection<PostDto> Posts,
        IReadOnlyCollection<FollowerDto> FollowedUsers,
        IReadOnlyCollection<FollowerDto> Followers
    );
}