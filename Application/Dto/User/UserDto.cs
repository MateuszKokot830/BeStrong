using Application.Dto.Comment;
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
        DateTime? DateOfWorkoutStart,
        string? Name,
        string? Surname,
        Gender Gender,
        string? City,
        string? Country,
        string? Description,
        string? ProfilePhotoUrl,
        int Age,
        string? WorkoutSince,
        DateTime? UpdatedDate,
        MeasurementsDto? Measurements,
        IReadOnlyCollection<PhotoDto> Photos,
        IReadOnlyCollection<PostDto> Posts,
        IReadOnlyCollection<FollowerDto> FollowedUsers,
        IReadOnlyCollection<FollowerDto> Followers,
        bool IsAdmin,
        int? WorkoutPlanId,
        string? WorkoutPlanName,
        bool CanViewPhotos,
        bool CanViewWorkouts,
        bool CanViewWorkoutPlan,
        bool CanViewMeasurements
    );
}
