namespace Application.Helpers
{
    public class PaginationHeader
    {
        public PaginationHeader(int currentPag, int itemsPerPage, int totalItems, int totalPages)
        {
            CurrentPag = currentPag;
            ItemsPerPage = itemsPerPage;
            TotalItems = totalItems;
            TotalPages = totalPages;
        }

        public int CurrentPag { get; set; } 
        public int ItemsPerPage { get; set; }   
        public int TotalItems { get; set; }
        public int TotalPages { get; set; }
    }
}