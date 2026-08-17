namespace Application.Helpers.Criteria
{
    public class WorkoutSearchCriteria : PaginationCriteria
    {
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public string? Name { get; set; }
        public int? ExerciseId { get; set; }
    }
}
