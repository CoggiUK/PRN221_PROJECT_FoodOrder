using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using PRN221_PROJECT_FoodOrder.Model;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json.Serialization;
using PRN221_PROJECT_FoodOrder.Utils;

namespace PRN221_PROJECT_FoodOrder.Pages
{
    public class LoginModel : PageModel
    {
        private readonly string jwtSecretKey = "PRN221_QUICKFOODORDER";

        [BindProperty]
        public User UserLogin { get; set; }
        public void OnGet()
        {
            if (TempData.ContainsKey("MsgLogout"))
            {
                ViewData["MSG"] = TempData["MsgLogout"];
                return;
            }
            if (TempData.ContainsKey("MSG"))
            {
                ViewData["MSG"] = TempData["MSG"];
            }

            
/*
            // Lấy JWT từ cookies
            var tokenString = HttpContext.Request.Cookies["Token"];
            if (!string.IsNullOrEmpty(tokenString))
            {
                // Giải mã JWT bằng cách gọi hàm trong lớp tiện ích JwtUtils
                var user = JwtUtils.DecodeJwt(tokenString);

                if (user != null)
                {
                    // Sử dụng thông tin người dùng
                    // ...
                }
            }*/
        }

        public IActionResult OnPost()
        {
            using (FoodOrderContext context = new FoodOrderContext())
            {
                try
                {
                    User user = context.Users.Where(u => u.Username.Equals(UserLogin.Username) && u.Password.Equals(UserLogin.Password)).FirstOrDefault();
                    if (user == null)
                    {
                        TempData["MSG"] = "Login failed, username or password is wrong!";
                        return RedirectToPage("/Login");
                    } 
                    if(user.UserStatus == false)
                    {
                        TempData["MSG"] = "Your account is banned";
                        return RedirectToPage("/Login");
                    }
                    TempData["MSG"] = "Login Success";
                    var userJson = JsonConvert.SerializeObject(user);


                    // Mã hóa chuỗi JSON thành JWT token
                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecretKey));
                    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                    var token = new JwtSecurityToken(
                        expires: DateTime.Now.AddMinutes(60), // Thời gian tồn tại của token (60 phút trong ví dụ này)
                        signingCredentials: creds,
                        claims: new[] { new Claim("user", userJson) } // Thêm claim user chứa chuỗi JSON của người dùng
                    );
                    var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

                    Response.Cookies.Append("Token", tokenString, new CookieOptions
                    {
                        Expires = DateTime.Now.AddMinutes(60), // Thời gian tồn tại của cookies (60 phút trong ví dụ này)
                        HttpOnly = true,
                        Secure = true,
                        SameSite = SameSiteMode.Strict // Điều này giúp bảo mật chống lại các cuộc tấn công CSRF
                    });

                    

                }
                catch (Exception ex)
                {

                }
            }
            return RedirectToPage("/FoodMenu");
        }


    }
    
}
