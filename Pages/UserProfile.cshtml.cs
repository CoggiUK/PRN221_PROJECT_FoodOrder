using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PRN221_PROJECT_FoodOrder.Model;
using PRN221_PROJECT_FoodOrder.Utils;

namespace PRN221_PROJECT_FoodOrder.Pages
{
    public class UserProfileModel : PageModel
    {
        [BindProperty]
        public User userUpdate { get; set; }

        public void OnGet()
        {
            using(FoodOrderContext context = new FoodOrderContext())
            {
                var tokenString = Request.Cookies["Token"];
                if(!string.IsNullOrEmpty(tokenString))
                {
                    Model.User u = JwtUtils.DecodeJwt(tokenString);
                    // Get information of user by id
                    User userDetails = context.Users.Where(e => e.UserId == u.UserId).Include(e => e.Customer).FirstOrDefault();
                    ViewData["userDetails"] = userDetails;
                }
            }
        }

        public IActionResult OnPost()
        {
            using(FoodOrderContext context = new FoodOrderContext())
            {
                User userNeedUpdate = context.Users.Where(e => e.UserId == userUpdate.UserId).Include(e => e.Customer).FirstOrDefault();
                if(userNeedUpdate != null)
                {
                    userNeedUpdate.Customer.CustomerName = userUpdate.Customer.CustomerName;
                    userNeedUpdate.Customer.CustomerPhone = userUpdate.Customer.CustomerPhone;
                    userNeedUpdate.Customer.CustomerAddress = userUpdate.Customer.CustomerAddress;
                    userNeedUpdate.Avatar = userUpdate.Avatar;
                    context.SaveChanges();
                }
            }
            return RedirectToPage("UserProfile");
        }
    }
}
