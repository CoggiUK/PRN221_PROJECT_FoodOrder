using PRN221_PROJECT_FoodOrder.Model;

namespace PRN221_PROJECT_FoodOrder.Business.IService
{
    public interface IShoppingCartService
    {
        ICollection<ShoppingCart> getCustomerShoppingCart(int customerId);
        ShoppingCart getCartItem(int customerId, int productId);
        void InsertCart(ShoppingCart cart);
        void UpdateCart(ShoppingCart cart);
        void DeleteCart(ShoppingCart cart);
		void clearCart(int customerId);
	}
}
