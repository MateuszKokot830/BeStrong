using System.Text.Json;
using Domain.Aggregates;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.SeedData
{
    public class SeedData
    {
        public static async Task SeedUserData(DataContext context)
        {
            if (await context.Users.AnyAsync()) return;

            var userData = await File.ReadAllTextAsync("../Infrastructure/SeedData/UserSeedData.json");
            var options = new JsonSerializerOptions{PropertyNameCaseInsensitive = true};
            var users = JsonSerializer.Deserialize<List<User>>(userData);

            foreach(var user in users)
            {
                if (!context.Users.Any(u => u.UserName.ToLower() == user.UserName.ToLower()))
                {
                    var password = new PasswordHasher<User>();
                    var hashed = password.HashPassword(user,"Password");
                    user.PasswordHash = hashed;
                    user.UserName = user.UserName.ToLower();
                    context.Users.Add(user);
                }
            }

            await context.SaveChangesAsync();
        }

        public static async Task SeedExerciseData(DataContext context)
        {
            if (await context.Excercises.AnyAsync()) return;

            var exercisesData = await File.ReadAllTextAsync("../Infrastructure/SeedData/ExerciseSeedData.json");
            var options = new JsonSerializerOptions{PropertyNameCaseInsensitive = true};
            var exercises = JsonSerializer.Deserialize<List<Exercise>>(exercisesData);

            foreach(var exercise in exercises)
            {
                if (!context.Excercises.Any(u => u.Name.ToLower() == exercise.Name.ToLower()))
                {
                     context.Excercises.Add(exercise);
                }
            }

            await context.SaveChangesAsync();
        }
    }
}