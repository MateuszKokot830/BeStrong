using ArchUnitNET.Domain;
using ArchUnitNET.xUnit;
using static ArchUnitNET.Fluent.ArchRuleDefinition;

namespace ArchitectureTests
{
    public class LayerDependencyTests
    {
        private static readonly Architecture Architecture = ArchitectureFixture.Architecture;

        private static readonly IObjectProvider<IType> DomainLayer =
            Types().That().ResideInNamespaceMatching(@"^Domain(\..*)?$").As("Domain Layer");

        private static readonly IObjectProvider<IType> ApplicationLayer =
            Types().That().ResideInNamespaceMatching(@"^Application(\..*)?$").As("Application Layer");

        private static readonly IObjectProvider<IType> InfrastructureLayer =
            Types().That().ResideInNamespaceMatching(@"^Infrastructure(\..*)?$").As("Infrastructure Layer");

        private static readonly IObjectProvider<IType> WebApiLayer =
            Types().That().ResideInNamespaceMatching(@"^WebAPI(\..*)?$").As("WebAPI Layer");

        [Fact]
        public void DomainLayer_ShouldNotHaveDependencyOn_ApplicationLayer()
        {
            Types().That().Are(DomainLayer)
                .Should().NotDependOnAny(ApplicationLayer)
                .Because("Domain sits at the center of Clean Architecture and must not know about Application")
                .Check(Architecture);
        }

        [Fact]
        public void DomainLayer_ShouldNotHaveDependencyOn_InfrastructureLayer()
        {
            Types().That().Are(DomainLayer)
                .Should().NotDependOnAny(InfrastructureLayer)
                .Because("Domain must not know about persistence, external services, or any other infrastructure concern")
                .Check(Architecture);
        }

        [Fact]
        public void DomainLayer_ShouldNotHaveDependencyOn_WebApiLayer()
        {
            Types().That().Are(DomainLayer)
                .Should().NotDependOnAny(WebApiLayer)
                .Because("Domain must not know about HTTP, controllers, or presentation concerns")
                .Check(Architecture);
        }

        [Fact]
        public void ApplicationLayer_ShouldNotHaveDependencyOn_InfrastructureLayer()
        {
            Types().That().Are(ApplicationLayer)
                .Should().NotDependOnAny(InfrastructureLayer)
                .Because("Application defines interfaces that Infrastructure implements — the dependency must not point back")
                .Check(Architecture);
        }

        [Fact]
        public void ApplicationLayer_ShouldNotHaveDependencyOn_WebApiLayer()
        {
            Types().That().Are(ApplicationLayer)
                .Should().NotDependOnAny(WebApiLayer)
                .Because("Application must stay usable from any presentation layer, not just WebAPI")
                .Check(Architecture);
        }

        [Fact]
        public void InfrastructureLayer_ShouldNotHaveDependencyOn_WebApiLayer()
        {
            Types().That().Are(InfrastructureLayer)
                .Should().NotDependOnAny(WebApiLayer)
                .Because("Infrastructure is wired up by WebAPI's composition root, not the other way round")
                .Check(Architecture);
        }
    }
}
