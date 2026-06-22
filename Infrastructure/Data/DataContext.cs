using Domain.Aggregates;
using Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class DataContext(DbContextOptions<DataContext> options) : IdentityDbContext<User, Role, int>(options)
    {
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Follower>(entity =>
            {
                entity.HasOne(x => x.User)
                .WithMany(x => x.FollowedUsers)
                .HasForeignKey(x => x.FollowedUserId)
                .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(x => x.FollowedUser)
                .WithMany(x => x.Followers)
                .HasForeignKey(x => x.FollowedUserId)
                .OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<Comment>(entity =>
            {
                entity.HasOne(x => x.Post)
                .WithMany(x => x.Comments)
                .HasForeignKey(x => x.PostId)
                .OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<PostLike>(entity =>
            {
                entity.HasOne(x => x.User)
                .WithMany()
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(x => x.Post)
                .WithMany(x => x.Likes)
                .HasForeignKey(x => x.PostId)
                .OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<CommentLike>(entity =>
            {
                entity.HasOne(x => x.User)
                .WithMany()
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(x => x.Comment)
                .WithMany(x => x.Likes)
                .HasForeignKey(x => x.CommentId)
                .OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<WorkoutExercise>(entity =>
            {
                entity.HasOne(x => x.Workout)
                .WithMany(x => x.WorkoutExercises)
                .HasForeignKey(x => x.WorkoutId)
                .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(x => x.Exercise)
                .WithMany(x => x.WorkoutExercise)
                .HasForeignKey(x => x.ExerciseId)
                .OnDelete(DeleteBehavior.NoAction);
            });

            builder.Entity<WorkoutSet>(entity =>
            {
                entity.HasOne(x => x.WorkoutExercise)
                .WithMany(x => x.Sets)
                .HasForeignKey(x => x.WorkoutExerciseId)
                .OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<User>(entity =>
            {
                entity.OwnsOne(x => x.Measurements);
                entity.HasOne(x => x.WorkoutPlan)
                .WithMany(x => x.UsedBy)
                .HasForeignKey(x => x.WorkoutPlanId)
                .OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<WorkoutPlan>(entity =>
            {
                entity.HasOne(x => x.CreatedBy)
                .WithMany(x => x.CreatedWorkoutPlans)
                .HasForeignKey(x => x.CreatedById)
                .OnDelete(DeleteBehavior.NoAction);
            });

            base.OnModelCreating(builder);
        }

        public DbSet<Post> Posts { get; set; }
        public DbSet<Workout> Workouts { get; set; }
        public DbSet<WorkoutPlan> WorkoutPlans { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Exercise> Excercises { get; set; }
        public DbSet<Follower> Followers { get; set; }
        public DbSet<Photo> Photos { get; set; }
        public DbSet<WorkoutExercise> WorkoutExercises { get; set; }
        public DbSet<WorkoutSet> WorkoutSets { get; set; }
        public DbSet<PostLike> PostLikes { get; set; }
        public DbSet<CommentLike> CommentLikes { get; set; }
    }
}
