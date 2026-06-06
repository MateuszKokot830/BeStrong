namespace Application.Helpers
{
    public class PaginationHeader(int currentPag, int itemsPerPage, int totalItems, int totalPages)
    {
        public int CurrentPag { get; set; } = currentPag;
        public int ItemsPerPage { get; set; } = itemsPerPage;
        public int TotalItems { get; set; } = totalItems;
        public int TotalPages { get; set; } = totalPages;
    }
}