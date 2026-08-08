using ArchUnitNET.Domain;
using ArchUnitNET.xUnit;
using FluentValidation;
using static ArchUnitNET.Fluent.ArchRuleDefinition;

namespace ArchitectureTests
{
    public class ValidatorConventionTests
    {
        private static readonly Architecture Architecture = ArchitectureFixture.Architecture;

        [Fact]
        public void ClassesNamedValidator_ShouldBeAssignableTo_AbstractValidator()
        {
            Classes().That().HaveNameEndingWith("Validator")
                .Should().BeAssignableTo(typeof(AbstractValidator<>))
                .Because("every *Validator must actually be a FluentValidation validator")
                .Check(Architecture);
        }

        [Fact]
        public void AbstractValidatorImplementations_ShouldHaveNameEndingWith_Validator()
        {
            Classes().That().AreAssignableTo(typeof(AbstractValidator<>))
                .Should().HaveNameEndingWith("Validator")
                .Because("FluentValidation validators must be discoverable by name")
                .Check(Architecture);
        }

        [Fact]
        public void Validators_ShouldBeSealed()
        {
            Classes().That().HaveNameEndingWith("Validator")
                .Should().BeSealed()
                .Because("validators are not designed to be extended")
                .Check(Architecture);
        }
    }
}
