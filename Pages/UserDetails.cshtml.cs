using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PRN221_PROJECT_FoodOrder.Model;

namespace PRN221_PROJECT_FoodOrder.Pages
{
    public class UserDetailsModel : PageModel
    {
        public void OnGet(int userid)
        {
            using(FoodOrderContext context = new FoodOrderContext())
            {
                User u = context.Users.Where(e => e.UserId == userid).Include(e => e.Customer).FirstOrDefault();
                if(u != null)
                {
                    ViewData["user"] = u;
                }
            }
        }
    }
}
