using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PRN221_PROJECT_FoodOrder.Model;
using PRN221_PROJECT_FoodOrder.Utils;

namespace PRN221_PROJECT_FoodOrder.Pages
{
    public class ChangePasswordModel : PageModel
    {
        [BindProperty]
        public User us { get; set; }

        public void OnGet()
        {
            
        }

        public IActionResult OnPost()
        {
            using (var db = new FoodOrderContext())
            {
                var tokenstring = Request.Cookies["Token"];
                if (!string.IsNullOrEmpty(tokenstring))
                {
                    User u = JwtUtils.DecodeJwt(tokenstring);
                    // Get information of user by id
                    User userDetails = db.Users.Where(e => e.UserId == u.UserId).Include(e => e.Customer).FirstOrDefault();
                    if (userDetails != null)
                    {
                        userDetails.Password = us.Password;
                        db.Users.Update(userDetails);
                        db.SaveChanges();
                    }
                }
            }
            return RedirectToPage("Index");
        }
    }
}
