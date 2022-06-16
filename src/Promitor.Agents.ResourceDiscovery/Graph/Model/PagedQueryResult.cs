using Newtonsoft.Json.Linq;

namespace Promitor.Agents.ResourceDiscovery.Graph.Model
{
    public class PagedQueryResult
    {
        public JObject Result { get; set; }
        public int PageSize { get; set; }
        public int CurrentPage { get; set; }
        public long TotalRecords { get; set; }

        public bool HasMore => (TotalRecords - (PageSize * CurrentPage)) > 0;

        public PagedQueryResult(JObject result, long totalRecords, int currentPage, int pageSize)
        {
            Result = result;
            TotalRecords = totalRecords;
            CurrentPage = currentPage;
            PageSize = pageSize;
        }
    }
}
