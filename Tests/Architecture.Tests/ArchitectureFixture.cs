using ArchUnitNET.Domain;
using ArchUnitNET.Loader;

namespace ArchitectureTests
{
    public static class ArchitectureFixture
    {
        public static readonly Architecture Architecture = new ArchLoader()
            .LoadAssemblies(
                typeof(Domain.Aggregates.User).Assembly,
                typeof(Application.Commands.Register.RegisterCommand).Assembly,
                typeof(Infrastructure.Repositories.UserRepository).Assembly,
                typeof(WebAPI.Controllers.AuthController).Assembly)
            .Build();
    }
}
