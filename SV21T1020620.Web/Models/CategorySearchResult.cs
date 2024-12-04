using SV21T1020620.DomainModels;

namespace SV21T1020620.Web.Models
{
    public class CategorySearchResult : PaginationSearchResult
    {
        public required List<Category> Data { get; set; }
    }
}
