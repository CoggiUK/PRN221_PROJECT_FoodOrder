using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PRN221_PROJECT_FoodOrder.Model;

namespace PRN221_PROJECT_FoodOrder.Pages
{
    public class ManageOrderModel : PageModel
    {

        public void OnGet()
        {
            List<Order> orders = new List<Order>();
            using (var db = new FoodOrderContext())
            {
                orders = db.Orders.Include(o => o.OrderItems).ToList();
                ViewData["listO"] = orders;
            }

        }

        public IActionResult OnGetConfirm(int orderid)
        {
            using (var db = new FoodOrderContext())
            {
                Order order = db.Orders.Where(x => x.OrderId == orderid).FirstOrDefault();
                string temp = "";
                if (order != null)
                {
                    if (order.OrderStatus.ToLower().Equals("new"))
                    {
                        temp += "closed";

                    }
                    else
                    {
                        temp += "new";
                    }
                    order.OrderStatus = temp;
                    db.Orders.Update(order);
                    db.SaveChanges();
                }
            }
            return RedirectToPage("/ManageOrder");
        }
        public void CountI(int productId)
        {
            using (var db = new FoodOrderContext())
            {
                List<OrderItem> listOD = db.OrderItems.ToList();
                int countItem = 0;
                foreach (OrderItem oitem in listOD)
                {

                    if (oitem.ItemId == productId)
                    {

                    }
                }
            }
        }

        public int GetAmount(int orderitemId, int productId)
        {
            int count=0;
            using (var db = new FoodOrderContext())
            {
               
                OrderItem oitem = db.OrderItems.Where(x=>x.OrderItemId==orderitemId&&x.ItemId==productId).FirstOrDefault();
                if (oitem!=null)
                {
                    count += (int)oitem.Amount;
                }
            }
            return count;
        }
    }
}
