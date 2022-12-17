using Domain.Aggregates;
using Domain.Entities;
using Domain.ValueObjects;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Data
{
    public class DataContext : IdentityDbContext<User, Role, int>
    {
        public DataContext()
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                                                    .AddJsonFile("appsettings.Development.json");
            IConfiguration configuration = builder.Build();
            optionsBuilder.UseSqlite(configuration.GetConnectionString("DefaultConnection"));
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Follower>(entity => {
                entity.HasOne(x => x.User)
                .WithMany(x => x.FollowedUsers)
                .HasForeignKey(x => x.FollowedUserId)
                .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(x => x.FollowedUser)
                .WithMany(x => x.Followers)
                .HasForeignKey(x => x.FollowedUserId)
                .OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<User>(entity => {
                entity.HasOne(x => x.WorkoutPlan)
                .WithMany(x => x.UsedBy)
                .HasForeignKey(x => x.WorkoutPlanId)
                .OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<WorkoutPlan>(entity => {
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
    }
}