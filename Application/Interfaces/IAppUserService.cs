using Application.Dto;

namespace Application.Interfaces
{
    public interface IAppUserService
    {
        IEnumerable<AppUserDto> GetAllUsers();
        AppUserDto GetUserById(int id);
    }
}