using PRN221_PROJECT_FoodOrder.Model;

namespace PRN221_PROJECT_FoodOrder.Business.IService
{
    public interface IOrderService
    {
        int InsertOrder(Order order);
        void UpdateOrder(Order order);
        void DeleteOrder(int orderId);
        ICollection<Order> GetAllOrders();
        ICollection<Order> GetCustomerOrders(int customerId);
        void InsertOrderItems(int orderId, OrderItem[] items);
    }
}
