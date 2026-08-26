using Application.Dto.User;
using Application.Helpers;
using Application.Helpers.Criteria;

namespace Application.Interfaces.Searchers
{
    public interface IUserSearcher
    {
        Task<IReadOnlyList<UserDto>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<UserDto?> FindByUsernameAsync(string username, CancellationToken cancellationToken = default);
        Task<UserDto?> FindByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<UserDto>> FindByIdsAsync(IReadOnlyCollection<int> ids, CancellationToken cancellationToken = default);
        Task<PaginationList<UserDto>> GetPagedAsync(UserSearchCriteria criteria, CancellationToken cancellationToken = default);
        Task<bool> ExistsAsync(int userId, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<int>> GetFollowedUserIdsAsync(int userId, CancellationToken cancellationToken = default);
        Task<DateTime?> GetWorkoutStartDateAsync(int userId, CancellationToken cancellationToken = default);
        Task<UserSettingsDto> GetSettingsAsync(int userId, CancellationToken cancellationToken = default);
    }
}
