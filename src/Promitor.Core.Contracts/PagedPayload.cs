using System.Collections.Generic;

namespace Promitor.Core.Contracts
{
    public class PagedPayload<TResult> where TResult : class
    {
        public List<TResult> Result { get; set; }
        public PageInformation PageInformation { get; set; }
        public bool HasMore => (PageInformation.TotalRecords - (PageInformation.PageSize * PageInformation.CurrentPage)) > 0;

        public PagedPayload()
        {
        }

        public PagedPayload(TResult result, long totalRecords, int currentPage, int pageSize)
            : this(new List<TResult> {result}, totalRecords, currentPage, pageSize)
        {
        }

        public PagedPayload(List<TResult> result, long totalRecords, int currentPage, int pageSize)
        {
            Result = result;
            PageInformation = new PageInformation
            {
                TotalRecords = totalRecords,
                CurrentPage = currentPage,
                PageSize = pageSize
            };
        }
    }
}
