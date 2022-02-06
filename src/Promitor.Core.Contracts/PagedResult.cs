
namespace Promitor.Core.Contracts
{
    public class PagedResult<TResult>
    {
        public TResult Result { get; set; }
        public int PageSize { get; set; }
        public int CurrentPage { get; set; }
        public long TotalRecords { get; set; }

        public PagedResult(TResult result, long totalRecords, int currentPage, int pageSize)
        {
            Result = result;
            TotalRecords = totalRecords;
            CurrentPage = currentPage;
            PageSize = pageSize;
        }
    }
}
