using Application.Dto;

namespace Application.Interfaces
{
    public interface IAppUserService
    {
        Task<IEnumerable<AppUserDto>> GetAllUsers();
        Task<AppUserDto> GetUserById(int id);
    }
}