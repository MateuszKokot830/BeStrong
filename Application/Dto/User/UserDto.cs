using Application.Dto.Follower;
using Application.Dto.Photo;
using Application.Dto.Post;
using Domain.Common;

namespace Application.Dto.User
{
    public class UserDto
    {
        public int Id { get; set; }
        public string? UserName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime DateOfWorkoutStart { get; set; }
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public Gender Gender { get; set; }
        public string? City { get; set; }
        public string? Country { get; set; }
        public string? Description { get; set; }
        public string? ProfilePhotoUrl { get; set; }
        public int Age { get; set; }
        public string? WorkoutSince { get; set; }
        public MeasurementsDto? Measurements { get; set; }
        public ICollection<PhotoDto> Photos { get; set; } = [];
        public ICollection<PostDto> Posts { get; set; } = [];
        public ICollection<FollowerDto> FollowedUsers { get; set; } = [];
        public ICollection<FollowerDto> Followers { get; set; } = [];

    }
}