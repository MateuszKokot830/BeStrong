using ArchUnitNET.Domain;
using ArchUnitNET.xUnit;
using WebAPI.Controllers;
using static ArchUnitNET.Fluent.ArchRuleDefinition;

namespace ArchitectureTests
{
    public class ControllerConventionTests
    {
        private static readonly Architecture Architecture = ArchitectureFixture.Architecture;

        [Fact]
        public void ClassesNamedController_ShouldResideIn_WebApiControllersNamespace()
        {
            Classes().That().HaveNameEndingWith("Controller")
                .Should().ResideInNamespace("WebAPI.Controllers")
                .Because("controllers belong in the presentation layer's Controllers folder")
                .Check(Architecture);
        }

        [Fact]
        public void ClassesNamedController_ExceptBaseApiController_ShouldBeAssignableTo_BaseApiController()
        {
            Classes().That().HaveNameEndingWith("Controller").And().DoNotHaveName(nameof(BaseApiController))
                .Should().BeAssignableTo(typeof(BaseApiController))
                .Because("all API controllers must inherit the shared error-mapping behavior from BaseApiController")
                .Check(Architecture);
        }
    }
}
