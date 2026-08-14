using Application.Dto.Post;
using Domain.Aggregates;

namespace Application.Mappings
{
    public static class PostMappings
    {
        public static PostDto ToDto(this Post post) => new(
            post.Id,
            post.UserId,
            post.Type,
            post.Description,
            post.CreatedDate,
            post.UpdatedDate,
            post.WorkoutId,
            post.WorkoutPlanId,
            post.Workout?.ToDto(),
            post.Likes?.Count ?? 0,
            post.Comments?.Select(c => c.ToDto()).ToList() ?? []
        );
    }
}
