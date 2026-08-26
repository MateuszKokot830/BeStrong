using Application.Dto.User;
using Domain.Common;

namespace Application.Tests.TestDoubles
{
    internal static class UserDtoFactory
    {
        public static UserDto Create(int id, string username, bool isAdmin = false) => new(
            id, username, DateTime.UtcNow.AddYears(-25), null, null, null, Gender.Male, null, null, null,
            null, 0, null, null, null, [], [], [], [], isAdmin, null, null, true, true, true, true);
    }
}
