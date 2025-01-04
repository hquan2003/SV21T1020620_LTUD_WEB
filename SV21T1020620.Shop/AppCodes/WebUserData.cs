using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;

namespace SV21T1020620.Shop
{
    /// <summary>
    /// Lưu thông tin của người dùng, đc ghi trong cookie
    /// </summary>
    public class WebUserData
    {
        public string UserId { get; set; } = "";
        public string UserName { get; set; } = "";
        public string DisplayName { get; set; } = "";
        public string Photo { get; set; } = "";
        public string Count { get; set; } = "";

        public List<string>? Roles { get; set; }

        private List<Claim> Claims
        {
            get
            {
                List<Claim> claims = new List<Claim>();

                // Thêm Claim nếu giá trị không bị null hoặc rỗng
                if (!string.IsNullOrEmpty(UserId))
                    claims.Add(new Claim(nameof(UserId), UserId));
                if (!string.IsNullOrEmpty(UserName))
                    claims.Add(new Claim(nameof(UserName), UserName));
                if (!string.IsNullOrEmpty(DisplayName))
                    claims.Add(new Claim(nameof(DisplayName), DisplayName));
                if (!string.IsNullOrEmpty(Photo))
                    claims.Add(new Claim(nameof(Photo), Photo));
                if (!string.IsNullOrEmpty(Count))
                    claims.Add(new Claim(nameof(Count), Count));

                // Thêm Claim cho vai trò (Role)
                if (Roles != null)
                {
                    foreach (var role in Roles)
                    {
                        if (!string.IsNullOrEmpty(role))
                            claims.Add(new Claim(ClaimTypes.Role, role));
                    }
                }

                return claims;
            }
        }

        public ClaimsPrincipal CreatePrincipal()
        {
            // Tạo ClaimsIdentity dựa trên danh sách Claims đã kiểm tra
            var identity = new ClaimsIdentity(Claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);
            return principal;
        }
    }
}
