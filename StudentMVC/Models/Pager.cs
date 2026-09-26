using System.ComponentModel.DataAnnotations;

namespace StudentMVC.Models
{
    public class Pager
    {
        [Key]
        public int TotalItems { get; private set; }
        public int CurrentPage { get; private set; }
        public int PageSize { get; private set; }

        public int TotalPages { get; private set; }
        public int StartPage { get; private set; }
        public int EndPage { get; private set; }

        public Pager()
        {
                
        }
        public Pager(int totalItems, int page, int pageSize = 8)
        {
            int totalPages = (int)Math.Ceiling((decimal)totalItems/(decimal)pageSize);
           int currentPage = page;

            int startPage = currentPage - 5;
            int endPage = currentPage + 4;
            if(startPage <= 0)
            {
                endPage = endPage - (startPage - 1);
                startPage = 1;
            }
            if(endPage>totalPages)
            {
                endPage = totalPages;
                if (endPage > 8)
                {
                    startPage = endPage - 7;
                }
            }
            TotalItems = totalItems; TotalPages = totalPages; StartPage = startPage; EndPage = endPage; CurrentPage = currentPage; PageSize = pageSize;
        }


    }
}
