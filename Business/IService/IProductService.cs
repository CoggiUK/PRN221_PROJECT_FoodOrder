using PRN221_PROJECT_FoodOrder.Model;

namespace PRN221_PROJECT_FoodOrder.Business.IService
{
    public interface IProductService
    {
        ICollection<Product> GetAllProducts();
        Product GetProductById(int id);
        ICollection<Product> SearchProducts(string productName, string productType);
        void InsertProduct(Product product);
        void UpdateProduct(Product product);
        void DeleteProduct(int id);

    }
}
