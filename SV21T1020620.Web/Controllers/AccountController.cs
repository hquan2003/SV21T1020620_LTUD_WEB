using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SV21T1020620.BusinessLayers;
using SV21T1020620.DomainModels;
using System.Data;

namespace SV21T1020620.Web.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            // Kiểm tra thông tin đầu vào
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                ModelState.AddModelError("Error", "Vui lòng nhập đầy đủ tên tài khoản và mật khẩu.");
                return View();
            }

            // Kiểm tra tài khoản người dùng
            UserAccount userAccount = null;
            foreach (var userType in Enum.GetValues(typeof(UserTypes)).Cast<UserTypes>())
            {
                userAccount = UserAccountService.Authorize(userType, username, password);
                if (userAccount != null)
                    break;
            }

            if (userAccount == null)
            {
                ModelState.AddModelError("Error", "Đăng nhập thất bại. Vui lòng kiểm tra lại thông tin.");
                return View();
            }

            // Đăng nhập thành công
            WebUserData userData = new WebUserData()
            {
                UserId = userAccount.UserId,
                UserName = userAccount.UserName,
                DisplayName = userAccount.DisplayName,
                Photo = userAccount.Photo,
                Roles = userAccount.RoleNames.Split(',').ToList()
            };

            // Tạo giấy chứng nhận và ghi nhận trạng thái đăng nhập
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, userData.CreatePrincipal());

            // Quay về trang chủ
            return RedirectToAction("Index", "Home");
        }


        public async Task<IActionResult> Logout()
        {
            HttpContext.Session.Clear();
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }
        public IActionResult ChangePassword()
        {

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ChangePassword(string oldPassword, string newPassword, string confirmPassword)
        {
            // Kiểm tra mật khẩu xác nhận có khớp với mật khẩu mới không
            if (newPassword != confirmPassword)
            {
                ModelState.AddModelError("Error", "Mật khẩu xác nhận không khớp.");
                return View();
            }

            var username = User.FindFirst("UserName")?.Value;  // Lấy tên người dùng hiện tại
            ViewBag.Username = username;
            // Gọi phương thức UpdateUserPassword để thực hiện thay đổi mật khẩu
            bool isPasswordUpdated = UserAccountService.UpdateUserPassword(username, oldPassword, newPassword);

            // Nếu việc thay đổi mật khẩu thành công
            if (isPasswordUpdated)
            {

                return RedirectToAction("Index", "Home"); // Quay lại trang chủ hoặc trang bất kỳ
            }

            // Nếu không thành công, hiển thị thông báo lỗi
            ModelState.AddModelError("Error", "Đổi mật khẩu thất bại. Vui lòng kiểm tra lại thông tin.");
            return View();
        }



        public IActionResult AccessDenined()
        {
            return View();
        }
    }
}