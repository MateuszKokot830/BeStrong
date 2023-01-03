using Microsoft.EntityFrameworkCore;

namespace Application.Helpers
{
    public class PaginationList<T> : List<T>
    {
        public PaginationList(IEnumerable<T> items, int count, int pageNumber, int pageSize)
        {
            TotalItems = count;
            CurrentPage = pageNumber;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            PageSize = pageSize;
            AddRange(items);
        }

        public PaginationList(int totalItems, int currentPage, int totalPages, int pageSize)
        {
            this.TotalItems = totalItems;
            this.CurrentPage = currentPage;
            this.TotalPages = totalPages;
            this.PageSize = pageSize;

        }
        public int TotalItems { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }

        public static async Task<PaginationList<T>> CreateAsync(IQueryable<T> source, int pageNumber, int pageSize)
        {
            var count = await source.CountAsync();
            var items = await source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
            
            return new PaginationList<T>(items, count, pageNumber, pageSize);
        }
    }
}