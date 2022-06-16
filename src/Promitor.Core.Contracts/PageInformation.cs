namespace Promitor.Core.Contracts
{
    public class PageInformation
    {
        public int PageSize { get; set; }
        public int CurrentPage { get; set; }
        public long TotalRecords { get; set; }
    }
}
