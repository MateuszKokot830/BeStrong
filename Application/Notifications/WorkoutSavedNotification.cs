using MediatR;

namespace Application.Notifications
{
    public record WorkoutSavedNotification(int WorkoutId, int UserId, string? Description) : INotification;
}
