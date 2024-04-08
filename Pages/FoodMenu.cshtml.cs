using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PRN221_PROJECT_FoodOrder.Business.IService;
using PRN221_PROJECT_FoodOrder.Model;
using PRN221_PROJECT_FoodOrder.Utils;

namespace PRN221_PROJECT_FoodOrder.Pages
{
    public class FoodMenuModel : PageModel
    {
        private readonly IProductService productService;
        private readonly ICustomerService customerService;
        private readonly IShoppingCartService shoppingCartService;
        public ICollection<Product> ListProducts { get; set; }
        public User userInfo { get; set; }
        public Customer currentCustomer { get; set; }

        public FoodMenuModel(IProductService context, ICustomerService customerService, IShoppingCartService shoppingCartService)
        {
            productService = context;
            this.customerService = customerService;
            this.shoppingCartService = shoppingCartService;
        }

        public void OnGet()
        {
            ViewData["Title"] = "Menu";
            ListProducts = productService.GetAllProducts();
            var tokenString = HttpContext.Request.Cookies["Token"];
            if (!string.IsNullOrEmpty(tokenString))
            {
                userInfo = JwtUtils.DecodeJwt(tokenString);
                if (!userInfo.Role.ToLower().Equals("admin"))
                {
                    currentCustomer = customerService.getCustomerByUserId(userInfo.UserId);
                }
            }
        }
        public IActionResult OnPostAddToBasket([FromBody] ShoppingCart addedItem)
        {
            // check user session
            var tokenString = HttpContext.Request.Cookies["Token"];
            if (string.IsNullOrEmpty(tokenString))
            {
                return BadRequest(new { success = false, message = "Error: No authen" });
            }
            userInfo = JwtUtils.DecodeJwt(tokenString);
            currentCustomer = customerService.getCustomerByUserId(userInfo.UserId);
            if (currentCustomer.CustomerId != addedItem.CustomerId)
            {
                return RedirectToPage("Error");
            }
            // Perform any additional logic like checking if the item already exists in the cart, updating quantities, etc.
            ShoppingCart cartItem = shoppingCartService.getCartItem(addedItem.CustomerId, addedItem.ProductId);
            if (cartItem == null)
            {
                // Save the ShoppingCart object to your database or cart storage
                cartItem = new ShoppingCart
                {
                    CustomerId = addedItem.CustomerId,
                    ProductId = addedItem.ProductId,
                    Amount = addedItem.Amount
                };
                shoppingCartService.InsertCart(cartItem);
            }
            else
            {
                // updating amount if existed
                cartItem.Amount += addedItem.Amount;
                shoppingCartService.UpdateCart(cartItem);
            }
            Product productInfo = productService.GetProductById(addedItem.ProductId);
            addedItem.Product = new Product
            {
                ProductId = addedItem.ProductId,
                ProductName = productInfo.ProductName,
                ProductImage = productInfo.ProductImage,
                UnitPrice = productInfo.UnitPrice
            };
            // Return cart item
            return new JsonResult(addedItem);
        }

        public IActionResult OnPostUpdateCart([FromBody] ShoppingCart updatedItem)
        {
            // check user session
            var tokenString = HttpContext.Request.Cookies["Token"];
            if (!string.IsNullOrEmpty(tokenString))
            {
                userInfo = JwtUtils.DecodeJwt(tokenString);
                currentCustomer = customerService.getCustomerByUserId(userInfo.UserId);
            }
            if (currentCustomer.CustomerId != updatedItem.CustomerId)
            {
                return RedirectToPage("Error");
            }
            // Perform any additional logic like checking if the item exists in the cart or not
            ShoppingCart cartItem = shoppingCartService.getCartItem(updatedItem.CustomerId, updatedItem.ProductId);
            if (cartItem == null)
            {
                return BadRequest(new { success = false, message = "Error authen" });
            }
            // delete entity if amount = 0
            if (updatedItem.Amount == 0)
            {
                shoppingCartService.DeleteCart(cartItem);
                return new JsonResult(1);
            }
            // update amount
            cartItem.Amount = updatedItem.Amount;
            shoppingCartService.UpdateCart(cartItem);

            // Return a success response
            return new JsonResult(1);
        }
    }
}
