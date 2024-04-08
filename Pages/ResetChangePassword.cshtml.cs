using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PRN221_PROJECT_FoodOrder.Model;
using PRN221_PROJECT_FoodOrder.Utils;
using System.Security.Claims;

namespace PRN221_PROJECT_FoodOrder.Pages
{
    public class ResetChangePasswordModel : PageModel
    {
        [BindProperty]
        public string newPassword { get; set; }

        public IActionResult OnGet()
        {
            string token = Request.Query["token"];
            if(string.IsNullOrEmpty(token))
            {
                return RedirectToPage("Index");
            }
            ClaimsPrincipal principal = JwtUtils.GetPrincipalFromToken(token);
            if(principal == null)
            {
                // Xu li token khong hop le
                return RedirectToPage("Index");
            }
            string userid = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            TempData["userid"] = userid;
            
            return Page();
        }

        public IActionResult OnPost()
        {
            if(TempData.ContainsKey("userid"))
            {
                string userid = TempData["userid"] as string;
                using(FoodOrderContext context = new FoodOrderContext())
                {
                    Model.User u = context.Users.Where(e => e.UserId == Int32.Parse(userid)).FirstOrDefault();
                    if(u != null)
                    {
                        u.Password = newPassword;
                    }
                    context.SaveChanges();
                }
            }
            return Page();
        }
    }
}
