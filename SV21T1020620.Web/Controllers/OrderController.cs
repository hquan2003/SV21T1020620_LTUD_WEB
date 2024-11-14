using Microsoft.AspNetCore.Mvc;
using SV21T1020620.BusinessLayers;
using SV21T1020620.Web.Models;
using SV21T1020620.Web;
using System.Globalization;

namespace SV21T1020484.Web.Controllers
{
    public class OrderController : Controller
    {
        private const int PAGE_SIZE = 30;
        private const string ORDER_SEARCH_CONDITON = "OrderSearchCondition";
        public IActionResult Index()
        {
            var condition = ApplicationContext.GetSessionData<OrderSearchInput>(ORDER_SEARCH_CONDITON);
            if (condition == null)
            {
                var cultureInfo = new CultureInfo("en-GB");

                condition = new OrderSearchInput
                {
                    Page = 1,
                    PageSize = PAGE_SIZE,
                    SearchValue = "",
                    Status = 0,
                    TimeRange = $"{DateTime.Today.AddDays(-7).ToString("dd/MM/yyyy", cultureInfo)} - {DateTime.Today.ToString("dd/MM/yyyy", cultureInfo)}"
                };
            }
            return View(condition);
        }
        public IActionResult Search(OrderSearchInput condition)
        {
            int rowCount;
            var data = OrderDataService.ListOrders(out rowCount, condition.Page, condition.PageSize, condition.Status,
                                                     condition.FromTime, condition.ToTime, condition.SearchValue ?? "");
            OrderSearchResult model = new OrderSearchResult()
            {
                Page = condition.Page,
                PageSize = condition.PageSize,
                SearchValue = condition.SearchValue ?? "",
                RowCount = rowCount,
                Status = condition.Status,
                TimeRange = condition.TimeRange,
                Data = data
            };
            ApplicationContext.SetSessionData(ORDER_SEARCH_CONDITON, condition);
            return View(model);
        }
        public IActionResult Create()
        {
            return View();
        }
        public IActionResult Details(int id = 0)
        {
            var order = OrderDataService.GetOrder(id);
            if (order == null)
                return RedirectToAction("Index");

            var details = OrderDataService.ListOrderDetails(id);
            var model = new OrderDetailModel()
            { 
                Order = order,
                Details = details
            };
            return View(model);
        }
        public IActionResult EditDetail(int id = 0, int productId = 0)
        {
            return View();
        }
        public IActionResult Shipping()
        {
            return View();
        }
    }
}
