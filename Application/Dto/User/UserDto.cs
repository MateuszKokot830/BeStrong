using Application.Dto.Comment;
using Application.Dto.Follower;
using Application.Dto.Photo;
using Application.Dto.Post;
using Domain.Common;
using System.Linq.Expressions;

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
    )
    {
        public static Expression<Func<Domain.Aggregates.User, UserDto>> Selector => user => new UserDto(
            user.Id,
            user.UserName ?? string.Empty,
            user.DateOfBirth,
            user.DateOfWorkoutStart,
            user.Name,
            user.Surname,
            user.Gender,
            user.City,
            user.Country,
            user.Description,
            user.Photos.Where(p => p.IsProfilePhoto).Select(p => p.Url).FirstOrDefault(),
            DateTime.Now.Year - user.DateOfBirth.Year,
            null,
            user.Measurements != null
                ? new MeasurementsDto(
                    user.Measurements.Height,
                    user.Measurements.Weight,
                    user.Measurements.Chest,
                    user.Measurements.Shoulders,
                    user.Measurements.Arms,
                    user.Measurements.Waist,
                    user.Measurements.Hips,
                    user.Measurements.Thights)
                : null,
            user.Photos
                .Select(p => new PhotoDto(p.Id, p.PublicId, p.Url, p.IsProfilePhoto))
                .ToList(),
            user.Posts
                .Select(p => new PostDto(
                    p.Id,
                    p.UserId,
                    p.Description,
                    p.CreatedDate,
                    p.WorkoutId,
                    p.WorkoutPlanId,
                    p.Likes,
                    p.Comments
                        .Select(c => new CommentDto(c.Id, c.UserId, c.Description, c.CreatedDate, c.Likes, c.PostId))
                        .ToList()))
                .ToList(),
            user.FollowedUsers
                .Select(f => new FollowerDto(f.UserId, f.FollowedUserId))
                .ToList(),
            user.Followers
                .Select(f => new FollowerDto(f.UserId, f.FollowedUserId))
                .ToList()
        );
    }
}