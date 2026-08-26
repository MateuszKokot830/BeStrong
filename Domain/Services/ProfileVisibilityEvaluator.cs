using Domain.Common;

namespace Domain.Services
{
    public static class ProfileVisibilityEvaluator
    {
        public static bool CanView(ProfileVisibility visibility, bool isOwnerOrAdmin, bool isFollower) =>
            isOwnerOrAdmin || visibility switch
            {
                ProfileVisibility.Public => true,
                ProfileVisibility.FollowersOnly => isFollower,
                ProfileVisibility.Private => false,
                _ => false
            };
    }
}
