using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PRN221_PROJECT_FoodOrder.Model;
using PRN221_PROJECT_FoodOrder.Utils;

namespace PRN221_PROJECT_FoodOrder.Pages
{
    public class ManageUserModel : PageModel
    {
        public IActionResult OnGet()
        {

            // Lấy JWT từ cookies
            var tokenString = HttpContext.Request.Cookies["Token"];
            if (!string.IsNullOrEmpty(tokenString))
            {
                // Giải mã JWT bằng cách gọi hàm trong lớp tiện ích JwtUtils
                var user = JwtUtils.DecodeJwt(tokenString);

                if (user.Role.Equals("Customer"))
                {
                    return RedirectToPage("Index");
                }
            } else
            {
                return RedirectToPage("Login");
            }
            using (FoodOrderContext context = new FoodOrderContext())
            {
                ViewData["userList"] = context.Users.ToList();
            }
            return Page();
        }

        public IActionResult OnGetDisable(int userid)
        {
            using (FoodOrderContext context = new FoodOrderContext())
            {
                User u = context.Users.Where(e => e.UserId == userid).Include(e => e.Customer).FirstOrDefault();
                if (u != null)
                {
                    u.UserStatus = false;
                    context.SaveChanges();
                }
            }
            return RedirectToPage("ManageUser");
        }
    }
}
