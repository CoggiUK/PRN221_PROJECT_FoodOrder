using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace PRN221_PROJECT_FoodOrder.Pages
{
    public class LogoutModel : PageModel
    {
        public IActionResult OnGet()
        {
            Response.Cookies.Delete("Token");
            TempData["MsgLogout"] = "Log out successful";
            return RedirectToPage("Login");
        }
    }
}
