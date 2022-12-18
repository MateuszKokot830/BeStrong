using Microsoft.AspNetCore.Identity;
using Domain.Entities;
using Domain.ValueObjects;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Domain.Common.Extensions;
using Domain.Common;

namespace Domain.Aggregates
{
    [Table("Users")]
    public class User : IdentityUser<int>
    {
        public DateTime DateOfBirth { get; set; }
        public DateTime DateOfWorkoutStart { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public Gender Gender { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Description { get; set; }
        public int Age => DateOfBirth.GetAgeFromDate();
        public string WorkoutSince => DateOfWorkoutStart.GetTimeDifferenceString();
        [Required]
        public Measurements Measurements { get; set; }
        public virtual ICollection<Photo> Photos { get; set; }
        public virtual ICollection<Role> Roles { get; set; }
        public virtual ICollection<Post> Posts { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<Follower> FollowedUsers { get; set; }
        public virtual ICollection<Follower> Followers { get; set; }
        public virtual ICollection<Workout> Workouts { get; set; }
        public int? WorkoutPlanId { get; set; }
        public virtual WorkoutPlan WorkoutPlan { get; set; }
        public virtual ICollection<WorkoutPlan> CreatedWorkoutPlans { get; set; }
    }
}