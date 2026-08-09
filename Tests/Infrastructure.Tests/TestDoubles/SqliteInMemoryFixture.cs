using Domain.Aggregates;
using Domain.Common;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Tests.TestDoubles
{
    public abstract class SqliteInMemoryFixture : IDisposable
    {
        private readonly SqliteConnection _connection;
        protected readonly DataContext Context;

        protected SqliteInMemoryFixture()
        {
            _connection = new SqliteConnection("DataSource=:memory:");
            _connection.Open();

            var options = new DbContextOptionsBuilder<DataContext>()
                .UseSqlite(_connection)
                .Options;

            Context = new DataContext(options);
            Context.Database.EnsureCreated();
        }

        protected async Task<User> CreateUserAsync(string userName = "seeduser")
        {
            var user = new User { UserName = userName, NormalizedUserName = userName.ToUpperInvariant() };
            Context.Users.Add(user);
            await Context.SaveChangesAsync();
            return user;
        }

        protected async Task<Exercise> CreateExerciseAsync(string name = "Bench Press")
        {
            var exercise = new Exercise { Name = name, MuscleSubgroup = MuscleSubgroup.Chest };
            Context.Excercises.Add(exercise);
            await Context.SaveChangesAsync();
            return exercise;
        }

        public void Dispose()
        {
            Context.Dispose();
            _connection.Dispose();
        }
    }
}
