using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PRN221_PROJECT_FoodOrder.Model;

namespace PRN221_PROJECT_FoodOrder.Pages
{
    public class ManageProductModel : PageModel
    {
        [BindProperty]
        public Product product { get; set; }



        public void OnGet()

        {
            List<Product> listPro = new List<Product>();
            using (var db = new FoodOrderContext())
            {
                listPro = db.Products.ToList();
                ViewData["listP"] = listPro;
            }

        }

        public IActionResult OnPost()
        {
            using (var db = new FoodOrderContext())
            {
                Product temp = new Product();
                temp.ProductName = product.ProductName;
                temp.ProductType = product.ProductType;
                if (product.UnitPrice == null)
                {
                    temp.UnitPrice = 0;
                }
                else
                {
                    temp.UnitPrice = product.UnitPrice;
                }

                temp.Description = product.Description;
                temp.ProductImage = product.ProductImage;
                db.Products.Add(temp);
                db.SaveChanges();
            }
            return RedirectToPage("/ManageProduct");

        }

        public IActionResult OnGetDisable(int proid)
        {
            using (var db = new FoodOrderContext())
            {
                Product nDel = db.Products.Where(p => p.ProductId == proid).FirstOrDefault();
                bool temp = true;
                if (nDel.Status == temp)
                {
                        temp = false;
                    }
                nDel.Status = temp;
                db.Products.Update(nDel);
                db.SaveChanges();
            }
            return RedirectToPage("/ManageProduct");
        }

        public IActionResult OnPostDelete(int proid)
        {
            using (var db = new FoodOrderContext())
            {
                try
                {
                    var productToDelete = db.Products.FirstOrDefault(p => p.ProductId == proid);
                    if (productToDelete != null)
                    {
                        var orderItemIdsToDelete = db.OrderItems
                            .Where(oi => oi.ItemId == proid)
                            .Select(oi => oi.OrderId)
                            .ToList();

                        db.OrderItems.RemoveRange(db.OrderItems.Where(oi => oi.ItemId == proid));
                        db.SaveChanges();

                        foreach (var orderId in orderItemIdsToDelete)
                        {
                            var order = db.Orders.FirstOrDefault(o => o.OrderId == orderId);
                            if (order != null)
                            {
                                db.Orders.Remove(order);
                            }
                        }
                        db.SaveChanges();

                        db.Products.Remove(productToDelete);
                        db.SaveChanges();
                    }

                    return RedirectToPage("/ManageProduct");
                }
                catch (Exception ex)
                {
                    var innerException = ex.InnerException;
                    Console.WriteLine("Error: " + innerException?.Message);
                    return Page();
                }
            }
        }

    }
}
