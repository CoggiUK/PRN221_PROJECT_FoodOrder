using PRN221_PROJECT_FoodOrder.Model;

namespace PRN221_PROJECT_FoodOrder.Business.IService
{
    public interface ICustomerService
    {
        Customer getCustomerByUserId(int userId);
    }
}
