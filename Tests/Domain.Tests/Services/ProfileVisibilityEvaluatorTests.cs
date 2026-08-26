using Domain.Common;
using Domain.Services;

namespace Domain.Tests.Services
{
    public class ProfileVisibilityEvaluatorTests
    {
        [Theory]
        [InlineData(ProfileVisibility.Public, false, false, true)]
        [InlineData(ProfileVisibility.Public, false, true, true)]
        [InlineData(ProfileVisibility.FollowersOnly, false, true, true)]
        [InlineData(ProfileVisibility.FollowersOnly, false, false, false)]
        [InlineData(ProfileVisibility.Private, false, true, false)]
        [InlineData(ProfileVisibility.Private, false, false, false)]
        public void CanView_ForNonOwnerViewer_RespectsVisibilityAndFollowState(
            ProfileVisibility visibility, bool isOwnerOrAdmin, bool isFollower, bool expected)
        {
            var result = ProfileVisibilityEvaluator.CanView(visibility, isOwnerOrAdmin, isFollower);

            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(ProfileVisibility.Public)]
        [InlineData(ProfileVisibility.FollowersOnly)]
        [InlineData(ProfileVisibility.Private)]
        public void CanView_WhenOwnerOrAdmin_AlwaysReturnsTrueRegardlessOfVisibility(ProfileVisibility visibility)
        {
            var result = ProfileVisibilityEvaluator.CanView(visibility, isOwnerOrAdmin: true, isFollower: false);

            Assert.True(result);
        }
    }
}
