using SV21T1020620.DomainModels;

namespace SV21T1020620.Web.Models
{
    public class EmployeeSearchResult : PaginationSearchResult
    {
        public required List<Employee> Data { get; set; }
    }
}
