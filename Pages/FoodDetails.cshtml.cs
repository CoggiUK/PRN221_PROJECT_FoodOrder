using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PRN221_PROJECT_FoodOrder.Business.IService;
using PRN221_PROJECT_FoodOrder.Business.Service;
using PRN221_PROJECT_FoodOrder.Model;
using PRN221_PROJECT_FoodOrder.Utils;
using System.ComponentModel.DataAnnotations;

namespace PRN221_PROJECT_FoodOrder.Pages
{
	public class FoodDetailsModel : PageModel
	{
		private readonly IProductService productService;
		private readonly ICustomerService customerService;
		public User userInfo { get; set; }
		public Customer currentCustomer { get; set; }
		public Product Product { get; set; }

        [Required(ErrorMessage = "Please enter a message.")]
        [BindProperty]
		public Comment comment { get; set; }
		public FoodDetailsModel(IProductService productService, ICustomerService customerService)
		{
			this.productService = productService;
			this.customerService = customerService;
		}

		public IActionResult OnGet(int productId)
		{
			var tokenString = HttpContext.Request.Cookies["Token"];
			if (!string.IsNullOrEmpty(tokenString))
			{
				userInfo = JwtUtils.DecodeJwt(tokenString);
                if (!userInfo.Role.Equals("Admin"))
                {
                    currentCustomer = customerService.getCustomerByUserId(userInfo.UserId);
                }
            }

			if (productId == 0)
			{
				return RedirectToPage("/Index");
			}
			Product product = productService.GetProductById(productId);
			if (product == null)
			{
				TempData["ErrorMessage"] = "Product not found or deleted!";
				return RedirectToPage("/Error");
			}
			Product = product;
            ViewData["Title"] = Product.ProductName;
            return Page();
		}

		public IActionResult OnPostRating([FromBody]Rate rating)
		{
            // check user session
            var tokenString = HttpContext.Request.Cookies["Token"];
            if (string.IsNullOrEmpty(tokenString))
            {
                // cut
                TempData["ErrorMessage"] = "No access";
                return RedirectToPage("error");
            }
            userInfo = JwtUtils.DecodeJwt(tokenString);
            if (userInfo.Role.ToLower().Equals("admin"))
            {
                // admin rate cl
                return Page();
            }
            // get customer by userId
            currentCustomer = customerService.getCustomerByUserId(userInfo.UserId);
            // fake user
            if (currentCustomer.CustomerId !=  rating.CustomerId)
            {
                TempData["ErrorMessage"] = "No access";
                return RedirectToPage("error");
            }
			using (var db = new FoodOrderContext())
			{
				Rate existingRate = db.Rates.FirstOrDefault(r => r.CustomerId == rating.CustomerId && r.ProductId == rating.ProductId);
				if (existingRate != null)
				{
					existingRate.RateStar = rating.RateStar;
					db.Rates.Update(existingRate);
					db.SaveChanges();
				}
				else
				{
					db.Rates.Add(rating);
					db.SaveChanges();
				}
				Product = db.Products.Find(rating.ProductId);
				IEnumerable<Rate> productRates = db.Rates.Where(r => r.ProductId == rating.ProductId);
				decimal totalStar = 0;
                foreach (var product in productRates) {
					totalStar += product.RateStar;
				}
				Product.Rating = totalStar/ (decimal)productRates.Count();
				db.Products.Update(Product);
				db.SaveChanges();
			}
			return new JsonResult(1);
        }

		public IActionResult OnPostComment()
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}
			if (comment.CustomerId == 0)
			{
				return RedirectToPage("/login");
			}
            // check user session
            var tokenString = HttpContext.Request.Cookies["Token"];
            if (string.IsNullOrEmpty(tokenString))
            {
                // cut
                TempData["ErrorMessage"] = "No access";
                return RedirectToPage("error");
            }
            userInfo = JwtUtils.DecodeJwt(tokenString);
            if (userInfo.Role.ToLower().Equals("admin"))
            {
                // admin comment cl
                return Page();
            }
            // get customer by userId
            currentCustomer = customerService.getCustomerByUserId(userInfo.UserId);
            // fake user
            if (currentCustomer.CustomerId != comment.CustomerId)
            {
                TempData["ErrorMessage"] = "No access";
                return RedirectToPage("error");
            }
			using (var db = new FoodOrderContext())
			{
				db.Comments.Add(comment);
				db.SaveChanges();
			}
			return Redirect($"/fooddetails?productId={comment.ProductId}");
        }
    }
}
