using System.Text.Json;
using Domain.Aggregates;
using Domain.Common;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.SeedData
{
    public class SeedData
    {
        private const string SeedPassword = "Pa$$w0rd";

        public static async Task SeedUserData(UserManager<User> userManager)
        {
            if (await userManager.Users.AnyAsync()) return;

            var userData = await File.ReadAllTextAsync("../Infrastructure/SeedData/UserSeedData.json");
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var users = JsonSerializer.Deserialize<List<User>>(userData, options) ?? [];

            foreach (var user in users)
            {
                if (user.UserName == null)
                    continue;

                user.UserName = user.UserName.ToLower();

                if (await userManager.FindByNameAsync(user.UserName) != null)
                    continue;

                var result = await userManager.CreateAsync(user, SeedPassword);
                if (!result.Succeeded)
                    throw new InvalidOperationException(
                        $"Failed to seed user '{user.UserName}': {string.Join("; ", result.Errors.Select(e => e.Description))}");
            }
        }

        public static async Task SeedRolesAsync(RoleManager<Role> roleManager, UserManager<User> userManager)
        {
            foreach (var roleName in new[] { Roles.Admin, Roles.Member })
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                    await roleManager.CreateAsync(new Role { Name = roleName });
            }

            var users = userManager.Users.ToList();
            foreach (var user in users)
            {
                if (!await userManager.IsInRoleAsync(user, Roles.Member))
                    await userManager.AddToRoleAsync(user, Roles.Member);
            }

            var firstUser = users.FirstOrDefault();
            if (firstUser != null && !await userManager.IsInRoleAsync(firstUser, Roles.Admin))
                await userManager.AddToRoleAsync(firstUser, Roles.Admin);
        }

        public static async Task SeedExerciseData(DataContext context)
        {
            if (await context.Excercises.AnyAsync()) return;

            var exercisesData = await File.ReadAllTextAsync("../Infrastructure/SeedData/ExerciseSeedData.json");
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var exercises = JsonSerializer.Deserialize<List<Exercise>>(exercisesData, options) ?? [];

            var seenNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            foreach (var exercise in exercises)
            {
                if (seenNames.Add(exercise.Name))
                    context.Excercises.Add(exercise);
            }

            await context.SaveChangesAsync();
        }
    }
}
