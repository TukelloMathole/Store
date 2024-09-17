using Microsoft.EntityFrameworkCore;
using StoreApp.Data;
using StoreApp.DTOs;
using StoreApp.Models;
using Microsoft.Extensions.Logging;
using StoreApp.Searvices;

namespace StoreApp.Services
{
    public class ProductService : IProductService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ProductService> _logger;

        public ProductService(ApplicationDbContext context, ILogger<ProductService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            try
            {
                return await _context.Products.ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all products");
                throw;
            }
        }

        public async Task<Product> GetProductByIdAsync(int id)
        {
            try
            {
                var product = await _context.Products.FindAsync(id);
                if (product == null)
                {
                    _logger.LogWarning("Product not found with ID: {ProductId}", id);
                    throw new KeyNotFoundException("Product not found");
                }
                return product;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving product by ID: {ProductId}", id);
                throw;
            }
        }

        public async Task<Product> CreateProductAsync(ProductDto productDto)
        {
            
            if (productDto == null)
            {
                _logger.LogWarning("Invalid product data received for creation");
                throw new ArgumentNullException(nameof(productDto));
            }

            

            try
            {
                var product = new Product
                {
                    Name = productDto.Name,
                    Description = productDto.Description,
                    Price = productDto.Price,
                    Stock = productDto.Stock
                };

                _context.Products.Add(product);
                await _context.SaveChangesAsync();

                return product;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating product with Name: {ProductName}", productDto.Name);
                throw;
            }
        }

        public async Task UpdateProductAsync(int id, ProductDto productDto)
        {
            
            if (productDto == null)
            {
                _logger.LogWarning("Invalid product data received for update");
                throw new ArgumentNullException(nameof(productDto));
            }

            try
            {
                var product = await _context.Products.FindAsync(id);
                if (product == null)
                {
                    _logger.LogWarning("Product not found with ID: {ProductId}", id);
                    throw new KeyNotFoundException("Product not found");
                }

                product.Name = productDto.Name;
                product.Description = productDto.Description;
                product.Price = productDto.Price;
                product.Stock = productDto.Stock;

                _context.Entry(product).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating product with ID: {ProductId}", id);
                throw;
            }
        }

        public async Task DeleteProductAsync(int id)
        {
            try
            {
                var product = await _context.Products.FindAsync(id);
                if (product == null)
                {
                    _logger.LogWarning("Product not found with ID: {ProductId}", id);
                    throw new KeyNotFoundException("Product not found");
                }

                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting product with ID: {ProductId}", id);
                throw;
            }
        }
    }
}
