using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using SV21T1020620.BusinessLayers;
using SV21T1020620.DomainModels;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SV21T1020620.Shop.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            ViewBag.Username = username;
            if (string.IsNullOrEmpty(username))
            {
                ModelState.AddModelError("Error", "Nhập tên đăng nhập");
                return View();
            }
            if (string.IsNullOrEmpty(password))
            {
                ModelState.AddModelError("Error", "Nhập mật khẩu");
                return View();
            }
            var userAccount = UserAccountService.Authorize(UserAccountService.UserTypes.Customer, username, password);
            if (userAccount == null)
            {
                ModelState.AddModelError("Error", "Sai tên đăng nhập hoặc mật khẩu");
                return View();
            }
            var userData = new WebUserData
            {
                UserId = userAccount.UserId,
                UserName = userAccount.UserName,
                DisplayName = userAccount.DisplayName,
                Roles = userAccount.RoleNames.Split(",").ToList()
            };

            //
            await HttpContext.SignInAsync(userData.CreatePrincipal());
            return RedirectToAction("Index", "Home");

        }
        public async Task<IActionResult> Logout()
        {
            HttpContext.Session.Clear();
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }
        public IActionResult Cart()
        {
            var user = User.GetUserData();
            if (user == null || user.UserId == null)
            {
                // Chuyển hướng đến trang đăng nhập nếu user không tồn tại
                return RedirectToAction("Login", "Account");
            }
            int customerID = Convert.ToInt32(user.UserId);
            var model = SV21T1020620.BusinessLayers.CartDataService.ListViewCart(customerID);
            return View(model);
        }
        public IActionResult EditProfile(int customerID = 0)
        {
            ViewBag.Title = "Cập nhật thông tin của khách hàng";
            var data = CommonDataService.GetCustomer(customerID);
            if (data == null)
            {
                return RedirectToAction("Index");
            }
            return View(data);
        }
        public IActionResult SaveProfile(Customer data)
        {
            ViewBag.Title = data.CustomerId == 0 ? "Bổ sung khách hàng mới" : "Cập nhật thông tin khách hàng";
            // kiểm tra nếu dữ liệu đầu vào không hợp lệ thì tạo ra một thông báo lỗi và lưu trữ vào ModelState
            if (string.IsNullOrWhiteSpace(data.CustomerName))
                ModelState.AddModelError(nameof(data.CustomerName), "Tên khách hàng không được để trống");

            if (string.IsNullOrWhiteSpace(data.ContactName))
                ModelState.AddModelError(nameof(data.ContactName), " Tên liên hệ không được để trống");
            if (string.IsNullOrWhiteSpace(data.Phone))
                ModelState.AddModelError(nameof(data.Phone), "Vui lòng nhập điện thoại");
            if (string.IsNullOrWhiteSpace(data.Email))
                ModelState.AddModelError(nameof(data.Email), "vui lòng nhập Email");
            if (string.IsNullOrWhiteSpace(data.Address))
                ModelState.AddModelError(nameof(data.Address), "Vui lòng nhập địa chỉ");
            if (string.IsNullOrWhiteSpace(data.Province))
                ModelState.AddModelError(nameof(data.Province), "Vui lòng chọn tỉnh/thành phố");
            // dựa vào thuộc tính IsVAlid của ModelState để bieetd có tồn tại lỗi hay không?
            if (ModelState.IsValid == false)
            {
                return View("EditProfile", data);
            }
            try
            {
                if (data.CustomerId == 0)
                {
                    int id = CommonDataService.AddCustomer(data);
                    if (id <= 0)
                    {
                        ModelState.AddModelError(nameof(data.Email), "Email bị trùng");
                        return View("EditProfile", data);
                    }
                }
                else
                {
                    bool result = CommonDataService.UpdateCustomer(data);
                    if (result == false)
                    {
                        ModelState.AddModelError(nameof(data.Email), "Email bị trùng");
                        return View("EditProfile", data);
                    }
                }
                return RedirectToAction("Index", "Home");
            }
            catch
            {
                ModelState.AddModelError("Error", " Hệ thống tạm thời gián đoạn");
                return View("Edit", data);
            }
        }
        public IActionResult History()
        {
            return View();
        }
        public IActionResult Register()
        {
            CustomerDTO customer = new CustomerDTO();
            customer.CustomerID = 0;
            customer.IsLocked = false;
            return View(customer);
        }
        public IActionResult Save(CustomerDTO data)
        {
            int id = CommonDataService.AddCustomerDTO(data);
            if (id <= 0)
            {
                ModelState.AddModelError("Error", "Tài khoản đã tồn tại");
                return View("Register", data);
            }

            return RedirectToAction("Login");
        }
        public IActionResult AccessDenined()
        {
            return View();
        }
        public IActionResult ChangePassword(string userName, string oldPassword, string newPassword, string confirmPassword)
        {
            if (Request.Method == "POST")
            {
                if (confirmPassword.Trim().Equals(newPassword.Trim()) == false)
                    ModelState.AddModelError("confirmPass", "Mật khẩu không khớp");
                if (ModelState.IsValid == false)
                {
                    return View();
                }
                else
                {
                    var result = UserAccountService.ChangePassword(UserAccountService.UserTypes.Customer, userName, oldPassword, newPassword);
                    if (result == true)
                    {
                        HttpContext.Session.Clear();
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ModelState.AddModelError("oldPass", "Mật khẩu cũ không đúng");
                        return View();
                    }
                }
            }
            return View();
        }
    }
}
