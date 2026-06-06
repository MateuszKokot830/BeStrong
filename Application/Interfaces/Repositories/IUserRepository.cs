using Application.Dto.User;
using Application.Helpers;
using Domain.Aggregates;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Application.Interfaces.Repositories
{
    public interface IUserRepository : IAsyncRepository<User>
    {
        Task<User?> GetByUsernameAsync(string? username);
        Task<IdentityResult> RegisterUserAsync(User user, string? password);
        Task<bool> CheckPasswordAsync(User user, string? password);
        Task AddFollowerAsync(Follower follower);
        Task DeleteFollowerAsync(Follower follower);
        Task AddPhoto(Photo photo);
        Task DeletePhoto(Photo photo);
        Task<PaginationList<UserDto>> GetUsersAsync(PaginationParams paginationParams);
    }
}