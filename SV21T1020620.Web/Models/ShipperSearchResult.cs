using SV21T1020620.DomainModels;

namespace SV21T1020620.Web.Models
{
    public class ShipperSearchResult : PaginationSearchResult
    {
        public required List<Shipper> Data { get; set; }
    }
}
