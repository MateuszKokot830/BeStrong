using Application.Dto.Post;
using Domain.Aggregates;

namespace Application.Mappings
{
    public static class PostMappings
    {
        public static PostDto ToDto(this Post post) => new(
            post.Id,
            post.UserId,
            post.Description,
            post.CreatedDate,
            post.WorkoutId,
            post.WorkoutPlanId,
            post.Likes,
            post.Comments?.Select(c => c.ToDto()).ToList() ?? []
        );

        public static Post ToEntity(this PostCreateDto dto) => new()
        {
            UserId = dto.UserId,
            Description = dto.Description,
            WorkoutId = dto.WorkoutId,
            WorkoutPlanId = dto.WorkoutPlan
        };
    }
}
