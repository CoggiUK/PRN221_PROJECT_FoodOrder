using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PRN221_PROJECT_FoodOrder.Model;

namespace PRN221_PROJECT_FoodOrder.Pages
{
    public class RegisterModel : PageModel
    {
        [BindProperty]
        public User RegisterUser { get; set; }

        public void OnGet()
        {
            if (TempData.ContainsKey("SuccessMessage"))
            {
                ViewData["SuccessMessage"] = TempData["SuccessMessage"];
            }
        }

        public IActionResult OnPost()
        {
            using (FoodOrderContext context = new FoodOrderContext())
            {
                try
                {
                    RegisterUser.Role = "Customer";
                    RegisterUser.UserStatus = true;
                    context.Users.Add(RegisterUser);

                    context.SaveChanges();

                    Customer customer = new Customer()
                    {
                        UserId = RegisterUser.UserId
                    };

                    context.Customers.Add(customer);
                    context.SaveChanges();
                    
                    TempData["SuccessMessage"] = "Register Success";
                } 
                catch (Exception ex)
                {
                    
                }
            }
            return RedirectToPage("/Register");
        }
    }
}
