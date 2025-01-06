using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SV21T1020620.BusinessLayers;
using SV21T1020620.DomainModels;
using SV21T1020620.Shop.Models;
using System.Globalization;

namespace SV21T1020620.Shop.Controllers
{
    public class ProductController : Controller
    {
        private const string SHOPPING_CART = "ShoppingCart";
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Detail(int productID, string? tab = "specifications")
        {
            var product = ProductDataService.GetProduct(productID);
            if (product == null)
            {
                return RedirectToAction("Index", "Home");
            }

            // Truyền tab cần hiển thị vào ViewBag
            ViewBag.ActiveTab = tab;

            return View(product);
        }
        [Authorize]
        public IActionResult AddToCart(int ProductID, int quantity)
        {
            var customer = User.GetUserData();
            var cart = CartDataService.getCartByCustomerID(Convert.ToInt32(customer.UserId)) ?? new Cart
            {
                CustomerID = Convert.ToInt32(customer.UserId),
                Count = 0
            };

            if (cart.CartID == 0)
            {
                CartDataService.AddCart(cart);
                cart = CartDataService.getCartByCustomerID(Convert.ToInt32(customer.UserId));
            }

            var product = ProductDataService.GetProduct(ProductID);
            if (product != null)
            {
                var exists = CartDataService.checkProductExists(cart.CartID, product.ProductID);
                if (exists == null)
                {
                    var data = new Cartdetail
                    {
                        Quantity = quantity,
                        Price = product.Price,
                        CartID = cart.CartID,
                        ProductID = product.ProductID
                    };
                    CartDataService.AddCartDetail(data);
                    cart.Count += 1;
                    CartDataService.SaveCart(cart);
                }
                else
                {
                    exists.Quantity += quantity;
                    CartDataService.SaveCartdetail(cart.CartID, ProductID, exists.Quantity);
                }
            }

            return RedirectToAction("Index", "Home");
        }

        public IActionResult RemoveCart(int cartDetailID)
        {
            CartDataService.DeleteDetail(cartDetailID);
            return RedirectToAction("Cart", "Account");

        }
        private List<CartItem> GetCartItems()
        {
            var sessionData = HttpContext.Session.GetString(SHOPPING_CART);
            return string.IsNullOrEmpty(sessionData)?new List<CartItem>():JsonConvert.DeserializeObject<List<CartItem>>(sessionData);
        }

        private void SaveCartSession(List<CartItem> cart)
        {
            HttpContext.Session.SetString(SHOPPING_CART, JsonConvert.SerializeObject(cart));
        }
        [HttpPost]
        [Authorize]
        public IActionResult DeleteSelected(List<int> SelectedItems)
        {
            if (SelectedItems != null && SelectedItems.Count > 0)
            {
                foreach (var cartDetailID in SelectedItems)
                {
                    CartDataService.DeleteDetail(cartDetailID);
                }
            }
            return RedirectToAction("Cart", "Account");
        }

    }
}
