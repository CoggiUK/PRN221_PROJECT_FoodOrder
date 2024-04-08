using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PRN221_PROJECT_FoodOrder.Model;

namespace PRN221_PROJECT_FoodOrder.Pages
{
    public class OrderDetailsModel : PageModel
    {
        [BindProperty]
        public Order ord { get; set; }


        List<OrderItem> listO = new List<OrderItem>();
        public void OnGet(int orderid)
        {
            using (var db = new FoodOrderContext())
            {
                listO = db.OrderItems.Where(x => x.OrderId == orderid).ToList();
                if (listO.Count != 0)
                {
                    ViewData["item"] = listO;

                }
                ViewData["OID"] = orderid;
            }
        }

        public IActionResult OnPost()
        {
            using (var db = new FoodOrderContext())
            {
                Order od = db.Orders.Where(x => x.OrderId == ord.OrderId).FirstOrDefault();
                if (od != null)
                {
                    od.OrderStatus = "hold";
                    db.Orders.Update(od);
                    db.SaveChanges();
                }
            }
            return RedirectToPage("/ManageOrder");
        }
    }
}
