using SV21T1020620.DomainModels;

namespace SV21T1020620.Web.Models
{
    public class SupplierSearchResult : PaginationSearchResult
    {
        public required List<Supplier> Data { get; set; }
    }
}
