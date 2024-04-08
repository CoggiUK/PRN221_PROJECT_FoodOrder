using Microsoft.EntityFrameworkCore;
using PRN221_PROJECT_FoodOrder.Business.IService;
using PRN221_PROJECT_FoodOrder.Model;

namespace PRN221_PROJECT_FoodOrder.Business.Service
{
	public class ShoppingCartService : IShoppingCartService
	{
		private readonly FoodOrderContext _context;
		public ShoppingCartService(FoodOrderContext context) => _context = context;

		public void clearCart(int customerId)
		{
			try
			{
				var carts = _context.ShoppingCarts.Where(cart => cart.CustomerId == customerId).ToArray();
				_context.ShoppingCarts.RemoveRange(carts);
				_context.SaveChanges();
			}
			catch (Exception)
			{
				throw;
			}
		}

		public void DeleteCart(ShoppingCart cart)
		{
			try
			{
				_context.ShoppingCarts.Remove(cart);
				_context.SaveChanges();
			}
			catch (Exception)
			{
				throw;
			}
		}

		public ShoppingCart getCartItem(int customerId, int productId)
		{
			ShoppingCart? cart;
			try
			{

				cart = _context.ShoppingCarts.Include(c => c.Product).FirstOrDefault(c => c.CustomerId == customerId
						&& c.ProductId == productId);
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
			return cart;
		}

		public ICollection<ShoppingCart> getCustomerShoppingCart(int customerId)
		{
			List<ShoppingCart> cart;
			try
			{
				cart = _context.ShoppingCarts.Where(c => c.CustomerId == customerId).ToList();
				foreach (ShoppingCart cartItem in cart)
				{
					cartItem.Product = _context.Products.FirstOrDefault(p => p.ProductId == cartItem.ProductId);
				}
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
			return cart;
		}

		public void InsertCart(ShoppingCart cart)
		{
			try
			{
				_context.ShoppingCarts.Add(cart);
				_context.SaveChanges();
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}

		public void UpdateCart(ShoppingCart cart)
		{
			try
			{
				ShoppingCart currentCart = _context.ShoppingCarts.FirstOrDefault(c => c.CustomerId == cart.CustomerId && c.ProductId == cart.ProductId);
				currentCart.Amount = cart.Amount;
				_context.ShoppingCarts.Update(currentCart);
				_context.SaveChanges();
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}
	}
}
