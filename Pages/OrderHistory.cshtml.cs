using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PRN221_PROJECT_FoodOrder.Business.IService;
using PRN221_PROJECT_FoodOrder.Model;
using PRN221_PROJECT_FoodOrder.Utils;

namespace PRN221_PROJECT_FoodOrder.Pages
{
    public class OrderHistoryModel : PageModel
    {
		private readonly ICustomerService customerService;
		private readonly IOrderService orderService;
		public User userInfo { get; set; }
		public Customer currentCustomer { get; set; }
		public IEnumerable<Order> orders { get; set; }

		public OrderHistoryModel(ICustomerService customerService, IOrderService orderService)
		{
			this.customerService = customerService;
			this.orderService = orderService;
		}
		public IActionResult OnGet()
        {
			ViewData["Title"] = "Order History";
			var tokenString = HttpContext.Request.Cookies["Token"];
			if (string.IsNullOrEmpty(tokenString))
			{
				return RedirectToPage("/login");
			}
			userInfo = JwtUtils.DecodeJwt(tokenString);
			if (userInfo.Role.ToLower().Equals("admin"))
			{
				return RedirectToPage("/manageorder");
			}
			currentCustomer = customerService.getCustomerByUserId(userInfo.UserId);
			orders = orderService.GetCustomerOrders(currentCustomer.CustomerId);
			return Page();
		}
    }
}
