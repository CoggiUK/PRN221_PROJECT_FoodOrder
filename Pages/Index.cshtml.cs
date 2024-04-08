using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PRN221_PROJECT_FoodOrder.Business.IService;
using PRN221_PROJECT_FoodOrder.Model;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace PRN221_PROJECT_FoodOrder.Pages
{
    public class IndexModel : PageModel
    {
        [BindProperty]
        public List<Comment> comments { get; set; }

        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
            using var db = new FoodOrderContext();
            int[] _customerIds = (from r in db.Rates.Where(s => s.RateStar == 5)
                                    select r.CustomerId).ToArray();
            var customerIds = _customerIds.Distinct().ToList();
            comments = db.Comments.Include(c => c.Customer).ThenInclude(c => c.User).Where(c => customerIds.Contains((int)c.CustomerId))
                .OrderByDescending(c => c.CreatedAt).Take(3).ToList();

        }
    }
}