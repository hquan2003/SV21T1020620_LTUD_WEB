using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SV21T1020620.BusinessLayers;
using SV21T1020620.DomainModels;

namespace SV21T1020620.Shop.Controllers
{
    public class ProductController : Controller
    {
        private const string SHOPPING_CART = "ShoppingCart";
        [HttpGet]
        [AllowAnonymous]   
        public IActionResult Detail(int productID)
        {
            Product? model = SV21T1020620.BusinessLayers.ProductDataService.GetProduct(productID);
            if (model == null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View(model);
        }
        [Authorize]
        public IActionResult AddtoCart(int ProductID, int quantity)
        {
            var customer = User.GetUserData();
            var Cart = CartDataService.getCartByCustomerID(Convert.ToInt32(customer.UserId));
            if (Cart == null)
            {
                var newCart = new Cart();
                newCart.CustomerID = Convert.ToInt32(customer.UserId);
                newCart.Sum = 0;
                Cart = newCart;
                CartDataService.AddCart(Cart);
            }
            Cart = CartDataService.getCartByCustomerID(Convert.ToInt32(customer.UserId));
            var product = ProductDataService.GetProduct(ProductID);
            if (product != null)
            {
                var CartID = Cart.CartID;
                int productID = product.ProductID;
                var exists = CartDataService.checkProductExists(CartID, productID);
                if (exists == null)
                {
                    var data = new Cartdetail();
                    data.Quantity = quantity;
                    data.Price = product.Price;
                    data.CartID = Cart.CartID;
                    data.ProductID = product.ProductID;
                    int id = CartDataService.AddCartDetail(data);
                    int sum = Cart.Sum + 1;
                    Cart.Sum = sum;
                    bool kq = CartDataService.SaveCart(Cart);
                }
                else
                {
                    int Quantity = exists.Quantity + quantity;
                    bool id = CartDataService.SaveCartdetail(CartID, ProductID, Quantity);
                }

            }
            return RedirectToAction("Index", "Home");
        }


    }
}
