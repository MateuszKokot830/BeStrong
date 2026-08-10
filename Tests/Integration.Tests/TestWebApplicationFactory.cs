using Application.Interfaces.Services;
using Infrastructure.Data;
using Integration.Tests.TestDoubles;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Integration.Tests
{
    public class TestWebApplicationFactory : WebApplicationFactory<Program>
    {
        // A single shared SqliteConnection object would force every concurrent request's DbContext
        // onto the same physical ADO.NET connection, which can only hold one active transaction at a
        // time — two requests racing into TransactionBehavior.BeginTransactionAsync() would collide
        // with "cannot start a transaction within a transaction", a test-harness artifact that has
        // nothing to do with the app (a real deployment gives each request its own connection).
        // A named, shared-cache in-memory database lets EF Core open a fresh connection per DbContext,
        // same as production, while every connection still sees the same in-memory data. The
        // keep-alive connection just stops the database from being dropped between requests.
        private readonly string _connectionString = $"Data Source=file:{Guid.NewGuid()};Mode=Memory;Cache=Shared";
        private readonly SqliteConnection _keepAliveConnection;

        public TestWebApplicationFactory()
        {
            // SeedData.SeedUserData/SeedExerciseData load JSON files via paths relative to the
            // working directory (e.g. "../Infrastructure/SeedData/UserSeedData.json"), which only
            // resolve correctly when the process is launched from inside the WebAPI folder. The test
            // host's working directory is the test assembly's output folder instead, so without this
            // the app crashes on startup trying to read a file that isn't there.
            Directory.SetCurrentDirectory(FindWebApiContentRoot());

            _keepAliveConnection = new SqliteConnection(_connectionString);
            _keepAliveConnection.Open();
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                services.RemoveAll<DbContextOptions<DataContext>>();

                services.AddDbContext<DataContext>(options => options.UseSqlite(_connectionString));

                // PhotoService constructs a real Cloudinary client directly from
                // IOptions<CloudinarySettings> with no seam to redirect via configuration, so it's
                // swapped here instead — the same pattern as DataContext above.
                services.RemoveAll<IPhotoService>();
                services.AddScoped<IPhotoService, FakePhotoService>();
            });
        }

        private static string FindWebApiContentRoot()
        {
            var directory = new DirectoryInfo(AppContext.BaseDirectory);
            while (directory is not null && !File.Exists(Path.Combine(directory.FullName, "BeStrong.sln")))
                directory = directory.Parent;

            if (directory is null)
                throw new InvalidOperationException("Could not locate the solution root from the test output directory.");

            return Path.Combine(directory.FullName, "WebAPI");
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (disposing)
                _keepAliveConnection.Dispose();
        }
    }
}
