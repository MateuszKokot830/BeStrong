using Application.Dto;

namespace Application.Interfaces
{
    public interface IAppUserService
    {
        Task<IEnumerable<AppUserDto>> GetAllUsers();
        Task<AppUserDto> GetUserById(int id);
        Task<AppUserDto> GetUserByUsername(string username);
        Task<AppUserDto> AddUser(RegisterDto registerDto);
        bool IsPasswordCorrect(AppUserDto appUserDto, LoginDto loginDto);
    }
}