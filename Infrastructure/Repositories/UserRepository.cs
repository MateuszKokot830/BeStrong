using Domain.Aggregates;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Domain.Entities;
using Application.Helpers;
using AutoMapper.QueryableExtensions;
using AutoMapper;
using Application.Dto.User;
using Application.Interfaces.Repositories;

namespace Infrastructure.Repositories
{
    public class UserRepository(DataContext context, UserManager<User> userManager, IMapper mapper) : BaseRepository<User>(context), IUserRepository
    {
        private readonly UserManager<User> _userManager = userManager;
        private readonly IMapper _mapper = mapper;

        public override async Task<User?> GetByIdAsync(int id)
        {
            return await _userManager.Users
            .Include(u => u.Photos)
            .Include(u => u.Measurements)
            .Include(u => u.FollowedUsers)
            .Include(u => u.Followers)
            .SingleOrDefaultAsync(x => x.Id == id);
        }

        public override async Task<IReadOnlyList<User>> GetAllAsync()
        {
            return await _userManager.Users
            .Include(u => u.Photos)
            .Include(u => u.Measurements)
            .Include(u => u.FollowedUsers)
            .Include(u => u.Followers)
            .ToListAsync();
        }

        public async Task<User?> GetByUsernameAsync(string? username)
        {
            var usernameLower = username?.ToLower() ?? string.Empty;
            return await _userManager.Users
            .Include(u => u.Photos)
            .Include(u => u.Measurements)
            .Include(u => u.FollowedUsers)
            .Include(u => u.Followers)
            .SingleOrDefaultAsync(x => x.UserName == usernameLower);
        }

        public async Task<IdentityResult> RegisterUserAsync(User user, string? password)
        {
            var pw = password ?? string.Empty;
            return await _userManager.CreateAsync(user, pw);
        }

        public async Task<bool> CheckPasswordAsync(User user, string? password)
        {
            var pw = password ?? string.Empty;
            return await _userManager.CheckPasswordAsync(user, pw);
        }

        public async Task AddFollowerAsync(Follower follower)
        {
            _context.Followers.Add(follower);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteFollowerAsync(Follower follower)
        {
            _context.Followers.Remove(follower);
            await _context.SaveChangesAsync();
        }

        public async Task AddPhoto(Photo photo)
        {
            _context.Photos.Add(photo);
            await _context.SaveChangesAsync();
        }

        public async Task DeletePhoto(Photo photo)
        {
            _context.Photos.Remove(photo);
            await _context.SaveChangesAsync();
        }

        public async Task<PaginationList<UserDto>> GetUsersAsync(PaginationParams paginationParams)
        {
            var query = _userManager.Users.AsQueryable();
            query = query.Where(x => x.UserName != paginationParams.Username);

            return await PaginationList<UserDto>.CreateAsync(
                query.ProjectTo<UserDto>(_mapper.ConfigurationProvider).AsNoTracking(),
                paginationParams.PageNumber, paginationParams.PageSize);
        }
    }
}