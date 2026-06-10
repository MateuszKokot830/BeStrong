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
        /// <summary>
        /// EF Core-translatable selector for list/paginated queries.
        ///
        /// Notes:
        ///   • Measurements: the null-conditional on owned entities cannot be translated
        ///     to SQL; individual columns are projected directly instead. An all-null
        ///     MeasurementsDto is equivalent to no measurements being set.
        ///   • Posts: never loaded in list queries (matches original repository behaviour).
        ///     Use a dedicated endpoint to retrieve a user's full post history.
        ///   • WorkoutSince: formatted string computed by an extension method — left null
        ///     here; single-user detail handlers supply it via AutoMapper.
        ///   • Age: approximated as (current year − birth year).
        /// </summary>
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
            new MeasurementsDto(
                user.Measurements.Height,
                user.Measurements.Weight,
                user.Measurements.Chest,
                user.Measurements.Shoulders,
                user.Measurements.Arms,
                user.Measurements.Waist,
                user.Measurements.Hips,
                user.Measurements.Thights),
            user.Photos
                .Select(p => new PhotoDto(p.Id, p.PublicId, p.Url, p.IsProfilePhoto))
                .ToList(),
            Array.Empty<PostDto>(),
            user.FollowedUsers
                .Select(f => new FollowerDto(f.UserId, f.FollowedUserId))
                .ToList(),
            user.Followers
                .Select(f => new FollowerDto(f.UserId, f.FollowedUserId))
                .ToList()
        );
    }
}
