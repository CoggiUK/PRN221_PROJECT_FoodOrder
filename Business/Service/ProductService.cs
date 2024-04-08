using Microsoft.EntityFrameworkCore;
using PRN221_PROJECT_FoodOrder.Model;

namespace PRN221_PROJECT_FoodOrder.Business.IService
{
    public class ProductService : IProductService
    {
        private readonly FoodOrderContext _context;
        public ProductService(FoodOrderContext context) => _context = context;
        public ICollection<Product> GetAllProducts()
        {
            List<Product> products;
            try
            {
                products = (from p in _context.Products
                            orderby p.ProductName
                           select p).ToList() ;
            }catch (Exception ex)
            {
                throw new Exception(ex.Message); 
            }
            return products;
        }

        public Product GetProductById(int id)
        {
            Product product;
            try
            {
                product = _context.Products.FirstOrDefault(p => p.ProductId == id);
                if (product == null)
                {
                    throw new Exception("Invalid");
                }
                product.Rates = _context.Rates.Where(r => r.ProductId == id).ToList();
                product.Comments = _context.Comments.Include(c => c.Customer).ThenInclude(c => c.User).Where(c => c.ProductId == id).OrderByDescending(c => c.CreatedAt).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return product;
        }

        public ICollection<Product> SearchProducts(string productName, string productType)
        {
            throw new NotImplementedException();
        }

        public void InsertProduct(Product product)
        {
            throw new NotImplementedException();
        }

        public void UpdateProduct(Product product)
        {
            throw new NotImplementedException();
        }

        public void DeleteProduct(int id)
        {
            throw new NotImplementedException();
        }

    }
}
