using CloudinaryDotNet.Actions;
using Microsoft.EntityFrameworkCore;
using PRN221_PROJECT_FoodOrder.Business.IService;
using PRN221_PROJECT_FoodOrder.Model;

namespace PRN221_PROJECT_FoodOrder.Business.Service
{
	public class OrderService : IOrderService
	{
		private readonly FoodOrderContext _context;
		public OrderService(FoodOrderContext context) => _context = context;
		public void DeleteOrder(int orderId)
		{
			throw new NotImplementedException();
		}

		public ICollection<Order> GetAllOrders()
		{
			throw new NotImplementedException();
		}

		public ICollection<Order> GetCustomerOrders(int customerId)
		{
			try
			{
				return _context.Orders.Include(o => o.OrderItems).ThenInclude(i => i.Item).Where(o => o.ReceiverId == customerId).OrderByDescending(o => o.OrderDate).ToList();
			}
			catch (Exception)
			{
				throw;
			}
		}

		public int InsertOrder(Order order)
		{
			try
			{
				_context.Orders.Add(order);
				_context.SaveChanges();
				return order.OrderId;
			}
			catch (Exception)
			{

				throw;
			}
		}

		public void InsertOrderItems(int orderId, OrderItem[] items)
		{
			Order existedOrder = _context.Orders.Find(orderId);
			if (existedOrder == null)
			{
				return;
			}
			_context.OrderItems.AddRange(items);
			_context.SaveChanges();
			decimal total = 0;
			foreach(OrderItem item in items)
			{
				total += (decimal)item.Subtotal;
			}
			existedOrder.Total = total;
			UpdateOrder(existedOrder);
		}

		public void UpdateOrder(Order order)
		{
			Order existedOrder = _context.Orders.Find(order.OrderId);
			if (existedOrder != null)
			{
				existedOrder.Total = order.Total;
				existedOrder.OrderStatus = order.OrderStatus;
				_context.SaveChanges();
			}
		}
	}
}
