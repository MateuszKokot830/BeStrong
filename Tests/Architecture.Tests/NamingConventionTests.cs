using ArchUnitNET.Domain;
using ArchUnitNET.xUnit;
using MediatR;
using static ArchUnitNET.Fluent.ArchRuleDefinition;

namespace ArchitectureTests
{
    public class NamingConventionTests
    {
        private static readonly Architecture Architecture = ArchitectureFixture.Architecture;

        [Fact]
        public void ClassesNamedCommandHandler_ShouldImplement_IRequestHandler()
        {
            Classes().That().HaveNameEndingWith("CommandHandler")
                .Should().ImplementInterface(typeof(IRequestHandler<,>))
                .Because("every *CommandHandler must be a MediatR request handler")
                .Check(Architecture);
        }

        [Fact]
        public void ClassesNamedQueryHandler_ShouldImplement_IRequestHandler()
        {
            Classes().That().HaveNameEndingWith("QueryHandler")
                .Should().ImplementInterface(typeof(IRequestHandler<,>))
                .Because("every *QueryHandler must be a MediatR request handler")
                .Check(Architecture);
        }

        [Fact]
        public void RequestHandlers_ShouldHaveNameEndingWith_CommandHandlerOrQueryHandler()
        {
            Classes().That().ImplementInterface(typeof(IRequestHandler<,>))
                .Should().HaveNameEndingWith("CommandHandler").OrShould().HaveNameEndingWith("QueryHandler")
                .Because("MediatR request handlers must be discoverable by name as either a command or a query handler")
                .Check(Architecture);
        }

        [Fact]
        public void ClassesInCommandsNamespace_NamedCommand_ShouldImplement_IRequest()
        {
            Classes().That().ResideInNamespaceMatching(@"^Application\.Commands(\..*)?$").And().HaveNameEndingWith("Command")
                .Should().ImplementInterface(typeof(IRequest<>))
                .Because("every *Command must be dispatchable through MediatR")
                .Check(Architecture);
        }

        [Fact]
        public void ClassesInQueriesNamespace_NamedQuery_ShouldImplement_IRequest()
        {
            Classes().That().ResideInNamespaceMatching(@"^Application\.Queries(\..*)?$").And().HaveNameEndingWith("Query")
                .Should().ImplementInterface(typeof(IRequest<>))
                .Because("every *Query must be dispatchable through MediatR")
                .Check(Architecture);
        }
    }
}
