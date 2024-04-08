using Microsoft.EntityFrameworkCore;
using PRN221_PROJECT_FoodOrder.Business.IService;
using PRN221_PROJECT_FoodOrder.Model;

namespace PRN221_PROJECT_FoodOrder.Business.Service
{
    public class CustomerService : ICustomerService
    {
        private readonly FoodOrderContext _context;
        public CustomerService(FoodOrderContext context) => _context = context;
        public Customer getCustomerByUserId(int userId)
        {
            Customer customer;
            try
            {
                customer = _context.Customers.FirstOrDefault(c => c.UserId == userId);
            }catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return customer;
        }
    }
}
