using ArchUnitNET.Domain;
using ArchUnitNET.xUnit;
using static ArchUnitNET.Fluent.ArchRuleDefinition;

namespace ArchitectureTests
{
    public class GeneralConventionTests
    {
        private static readonly Architecture Architecture = ArchitectureFixture.Architecture;

        [Fact]
        public void Interfaces_ShouldHaveNameStartingWith_I()
        {
            Interfaces().That().ResideInNamespaceMatching(@"^(Domain|Application|Infrastructure|WebAPI)(\..*)?$")
                .Should().HaveNameStartingWith("I")
                .Because("standard .NET convention for interface naming")
                .Check(Architecture);
        }

        [Fact]
        public void RepositoryInterfaces_ShouldResideIn_ApplicationInterfacesRepositoriesNamespace()
        {
            Interfaces().That().HaveNameEndingWith("Repository")
                .Should().ResideInNamespace("Application.Interfaces.Repositories")
                .Because("repository abstractions are owned by the Application layer, per the Dependency Inversion Principle")
                .Check(Architecture);
        }

        [Fact]
        public void RepositoryImplementations_ShouldResideIn_InfrastructureRepositoriesNamespace()
        {
            Classes().That().HaveNameEndingWith("Repository")
                .Should().ResideInNamespace("Infrastructure.Repositories")
                .Because("concrete data-access implementations belong in Infrastructure")
                .Check(Architecture);
        }
    }
}
