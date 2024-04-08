using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PRN221_PROJECT_FoodOrder.Model;

namespace PRN221_PROJECT_FoodOrder.Pages
{
    public class ProductDetailsModel : PageModel
    {

        [BindProperty]
        public Product needEdit { get; set; }


        public void OnGet(int proid)
        {
            using (var db = new FoodOrderContext())
            {
                Product p = db.Products.Where(x => x.ProductId == proid).FirstOrDefault();
                if (p != null)
                {
                    ViewData["product"] = p;
                }
            }
        }

        public IActionResult OnPost()
        {
            using (var db = new FoodOrderContext())
            {
               
                Product nd= db.Products.Where(x => x.ProductId == needEdit.ProductId).FirstOrDefault();
                if (nd!=null)
                {
                    nd.ProductName = needEdit.ProductName;
                    nd.ProductType = needEdit.ProductType;
                    nd.UnitPrice = needEdit.UnitPrice;
                    nd.Description = needEdit.Description;
                    nd.ProductImage = needEdit.ProductImage;
                    nd.Status = needEdit.Status;
                    db.Products.Update(nd);
                    db.SaveChanges();
                }
            }
            return RedirectToPage("/ManageProduct");
        }
    }
}
