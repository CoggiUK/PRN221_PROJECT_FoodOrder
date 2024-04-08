using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PRN221_PROJECT_FoodOrder.Model;
using PRN221_PROJECT_FoodOrder.Utils;

namespace PRN221_PROJECT_FoodOrder.Pages
{
    public class ResetPasswordModel : PageModel
    {
        [BindProperty]
        public string emailResetPassword { get; set; }

        public void OnGet()
        {
            if (TempData.ContainsKey("MSG"))
            {
                ViewData["MSG"] = TempData["MSG"];
               
            }
        }

        public IActionResult OnPost()
        {
            using (FoodOrderContext context = new FoodOrderContext())
            {
                Model.User u = context.Users.Where(e => e.Email.Equals(emailResetPassword)).FirstOrDefault();
                if(u != null)
                {
                    string token = JwtUtils.GenerateToken("" + u.UserId, emailResetPassword);

                    // Tạo link reset password chứa token
                    string resetLink = Url.Page("/ResetChangePassword", pageHandler: null, values: new { token }, protocol: Request.Scheme);
                    EmailService.SendResetPasswordEmail(emailResetPassword, resetLink);
                    TempData["MSG"] = "Email sent! Please check your email";
                }
                // Tra ve thong bao khong tim thay email hop le
                else
                {
                    TempData["MSG"] = "Your email is not existed in system!";
                    
                }
            }
            return RedirectToPage("ResetPassword");
        }
    }
}
