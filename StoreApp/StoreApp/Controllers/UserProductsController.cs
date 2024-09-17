using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StoreApp.Models;
using StoreApp.Searvices;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class UserProductsController : ControllerBase
{
    private readonly IProductService _productService;
    private readonly ILogger<UserProductsController> _logger;

    public UserProductsController(IProductService productService, ILogger<UserProductsController> logger)
    {
        _productService = productService;
        _logger = logger;
    }

    // GET: api/UserProducts
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
    {
        _logger.LogInformation("Retrieving all products for the user.");
        var products = await _productService.GetAllProductsAsync();
        _logger.LogInformation("Successfully retrieved {Count} products.", products.Count());
        return Ok(products);
    }

    // GET: api/UserProducts/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Product>> GetProduct(int id)
    {
        _logger.LogInformation("Retrieving product with ID: {Id}", id);
        try
        {
            var product = await _productService.GetProductByIdAsync(id);
            _logger.LogInformation("Successfully retrieved product with ID: {Id}", id);
            return Ok(product);
        }
        catch (KeyNotFoundException)
        {
            _logger.LogWarning("Product with ID: {Id} not found.", id);
            return NotFound();
        }
    }
}
