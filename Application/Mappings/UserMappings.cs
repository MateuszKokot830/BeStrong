using Application.Dto.Follower;
using Application.Dto.Photo;
using Application.Dto.Post;
using Application.Dto.User;
using Domain.Aggregates;
using Domain.Common;
using Domain.Common.Extensions;
using Domain.ValueObjects;
using System.Linq.Expressions;

namespace Application.Mappings
{
    public static class UserMappings
    {
        public static Expression<Func<User, UserDto>> Selector => user => new UserDto(
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
            user.UpdatedDate,
            new MeasurementsDto(
                user.Measurements!.Height,
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
                .Select(f => new FollowerDto(f.UserId, f.FollowedUserId, f.FollowedAt))
                .ToList(),
            user.Followers
                .Select(f => new FollowerDto(f.UserId, f.FollowedUserId, f.FollowedAt))
                .ToList(),
            user.Roles.Any(r => r.Name == Roles.Admin),
            user.WorkoutPlanId,
            user.WorkoutPlan != null ? user.WorkoutPlan.Name : null,
            true,
            true,
            true,
            true
        );

        public static UserDto WithComputedWorkoutSince(this UserDto dto) =>
            dto with { WorkoutSince = dto.DateOfWorkoutStart?.GetTimeDifferenceString() };

        public static UserDto ToDto(this User user) => new(
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
            user.Photos?.FirstOrDefault(p => p.IsProfilePhoto)?.Url,
            user.Age,
            user.WorkoutSince,
            user.UpdatedDate,
            user.Measurements?.ToDto(),
            user.Photos?.Select(p => p.ToDto()).ToList() ?? [],
            user.Posts?.Select(p => p.ToDto()).ToList() ?? [],
            user.FollowedUsers?.Select(f => f.ToDto()).ToList() ?? [],
            user.Followers?.Select(f => f.ToDto()).ToList() ?? [],
            user.Roles?.Any(r => r.Name == Roles.Admin) ?? false,
            user.WorkoutPlanId,
            user.WorkoutPlan?.Name,
            true,
            true,
            true,
            true
        );

        public static User ToEntity(this Dto.Auth.UserRegisterRequestDto dto) => new()
        {
            UserName = dto.UserName
        };

        public static void ApplyTo(this UserUpdateDto dto, User user)
        {
            user.DateOfBirth = dto.DateOfBirth;
            user.DateOfWorkoutStart = dto.DateOfWorkoutStart;
            user.Name = dto.Name;
            user.Surname = dto.Surname;
            user.Gender = dto.Gender;
            user.City = dto.City;
            user.Country = dto.Country;
            user.Description = dto.Description;
            user.Measurements = dto.Measurements?.ToEntity();
        }

        public static MeasurementsDto ToDto(this Measurements m) => new(
            m.Height, m.Weight, m.Chest, m.Shoulders, m.Arms, m.Waist, m.Hips, m.Thights
        );

        public static Measurements ToEntity(this MeasurementsDto dto) => new(
            dto.Height, dto.Weight, dto.Chest, dto.Shoulders, dto.Arms, dto.Waist, dto.Hips, dto.Thights
        );
    }
}
