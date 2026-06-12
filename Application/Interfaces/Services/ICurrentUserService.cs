namespace Application.Interfaces.Services
{
    public interface ICurrentUserService
    {
        int UserId { get; }
        bool IsAdmin { get; }
        bool IsOwnerOrAdmin(int resourceOwnerId);
    }
}
