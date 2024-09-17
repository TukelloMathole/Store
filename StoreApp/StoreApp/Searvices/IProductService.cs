using StoreApp.DTOs;
using StoreApp.Models;

namespace StoreApp.Searvices
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetAllProductsAsync();
        Task<Product> GetProductByIdAsync(int id);
        Task<Product> CreateProductAsync(ProductDto productDto);
        Task UpdateProductAsync(int id, ProductDto productDto);
        Task DeleteProductAsync(int id);
    }
}
