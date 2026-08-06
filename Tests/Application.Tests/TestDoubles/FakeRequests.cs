using ErrorOr;
using MediatR;

namespace Application.Tests.TestDoubles
{
    public sealed record FakeCommand(string Value) : IRequest<ErrorOr<string>>;

    public sealed record FakeQuery(string Value) : IRequest<ErrorOr<string>>;
}
