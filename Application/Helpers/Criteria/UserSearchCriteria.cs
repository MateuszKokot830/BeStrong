using Domain.Common;

namespace Application.Helpers.Criteria
{
    public class UserSearchCriteria : PaginationCriteria
    {
        public string? ExcludeUsername { get; set; }
        public string? Username { get; set; }
        public Gender? Gender { get; set; }
        public string? Country { get; set; }
        public string? City { get; set; }
    }
}
