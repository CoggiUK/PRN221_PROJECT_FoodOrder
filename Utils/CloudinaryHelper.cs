using CloudinaryDotNet.Actions;
using CloudinaryDotNet;
using System.Security.Principal;

namespace PRN221_PROJECT_FoodOrder.Utils
{
    public class CloudinaryHelper
    {
        private readonly Cloudinary _cloudinary;

        public CloudinaryHelper(string cloudName, string apiKey, string apiSecret)
        {
            Account account = new Account(cloudName, apiKey, apiSecret);
            _cloudinary = new Cloudinary(account);
        }

        public async Task<string> UploadImageAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return null;

            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(file.FileName, file.OpenReadStream())
            };

            var uploadResult = await _cloudinary.UploadAsync(uploadParams);
            return uploadResult.SecureUri.AbsoluteUri;
        }
    }
}
