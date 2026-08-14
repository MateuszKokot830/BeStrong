using Domain.Common;

namespace Application.Helpers.Criteria
{
    public class WorkoutPlanSearchCriteria : PaginationCriteria
    {
        public WorkoutPlanCategory? Category { get; set; }
        public string? Name { get; set; }
        public bool OnlyOwn { get; set; }
    }
}
