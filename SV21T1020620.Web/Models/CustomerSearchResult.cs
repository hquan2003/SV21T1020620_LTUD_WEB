using SV21T1020620.DomainModels;

namespace SV21T1020620.Web.Models
{
    public class CustomerSearchResult : PaginationSearchResult
    {
        public required List<Customer> Data { get; set; }
    }
}
