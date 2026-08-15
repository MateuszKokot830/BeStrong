using Domain.Common;

namespace Application.Helpers.Criteria
{
    public enum CreatedByFilter
    {
        All,
        OnlyMyself,
        OnlyFollowers
    }

    public class WorkoutPlanSearchCriteria : PaginationCriteria
    {
        public WorkoutPlanCategory? Category { get; set; }
        public string? Name { get; set; }
        public CreatedByFilter CreatedBy { get; set; } = CreatedByFilter.All;
        public string? OwnerName { get; set; }
    }
}
