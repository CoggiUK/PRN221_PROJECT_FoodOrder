using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PRN221_PROJECT_FoodOrder.Business.IService;
using PRN221_PROJECT_FoodOrder.Model;
using PRN221_PROJECT_FoodOrder.Utils;
using System.Security.Cryptography.X509Certificates;

namespace PRN221_PROJECT_FoodOrder.Pages
{
	public class CheckoutModel : PageModel
	{
		private readonly IProductService productService;
		private readonly ICustomerService customerService;
		private readonly IShoppingCartService shoppingCartService;
		private readonly IOrderService orderService;
		public User userInfo { get; set; }
		public Customer currentCustomer { get; set; }
		public ICollection<ShoppingCart> Cart { get; set; }

		public CheckoutModel(IProductService productService, ICustomerService customerService, IShoppingCartService shoppingCartService, IOrderService orderService)
		{
			this.productService = productService;
			this.customerService = customerService;
			this.shoppingCartService = shoppingCartService;
			this.orderService = orderService;
		}

		public void OnGet()
		{
			ViewData["Title"] = "Checkout";
			var tokenString = HttpContext.Request.Cookies["Token"];
			if (!string.IsNullOrEmpty(tokenString))
			{
				userInfo = JwtUtils.DecodeJwt(tokenString);
				if (!userInfo.Role.ToLower().Equals("admin"))
				{
					currentCustomer = customerService.getCustomerByUserId(userInfo.UserId);
					Cart = shoppingCartService.getCustomerShoppingCart(currentCustomer.CustomerId);
				}
			}

		}

		public IActionResult OnPost(int CustomerId, string Name, string Phone, string District, string DetailsAddress)
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
				// admin mua cl
				return Page();
			}
			// get customer by userId
			currentCustomer = customerService.getCustomerByUserId(userInfo.UserId);
			// get cus's carts
			Cart = shoppingCartService.getCustomerShoppingCart(currentCustomer.CustomerId);
			// fake user
			if (currentCustomer.CustomerId != CustomerId)
			{
				TempData["ErrorMessage"] = "No access";
				return RedirectToPage("error");
			}

			if (Cart.Count == 0)
			{
				// ko co thi mua cai gi
				return Page();
			}
			
			Order newOrder = new Order
			{
				ReceiverId = CustomerId,
				ReceiverName = Name,
				ReceiverPhone = Phone,
				OrderStatus = "new",
				DeliveryAddress = DetailsAddress+" - "+District
			};
			orderService.InsertOrder(newOrder);
			List<OrderItem> orderItems = new List<OrderItem>();
			foreach (var c in Cart)
			{
				OrderItem item = new OrderItem
				{
					OrderId = newOrder.OrderId,
					ItemId = c.ProductId,
					ItemName = c.Product.ProductName,
					ItemType = c.Product.ProductType,
					UnitCost = c.Product.UnitPrice,
					Amount = c.Amount,
					Subtotal = c.Product.UnitPrice * c.Amount
				};
				orderItems.Add(item);
			}
			orderService.InsertOrderItems(newOrder.OrderId, orderItems.ToArray());
			shoppingCartService.clearCart(CustomerId);
			return RedirectToPage("/foodmenu");
		}
	}
}
