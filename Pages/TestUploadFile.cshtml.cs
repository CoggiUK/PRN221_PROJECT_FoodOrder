using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PRN221_PROJECT_FoodOrder.Utils;

namespace PRN221_PROJECT_FoodOrder.Pages
{
    public class TestUploadFileModel : PageModel
    {
        public void OnGet()
        {

        }

        public async Task<IActionResult> OnPostAsync(IFormFile imageFile)
        {
            if (imageFile != null && imageFile.Length > 0)
            {
                CloudinaryHelper cloudinary = new CloudinaryHelper("dfkvp7x35", "536593263684142", "29mquvkAuRme0JdrlNhdiI2NG60");
                string url = await cloudinary.UploadImageAsync(imageFile);

            }
            return Page();
        }
    }
}
