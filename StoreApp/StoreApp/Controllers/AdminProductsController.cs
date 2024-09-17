using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StoreApp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using StoreApp.DTOs;
using StoreApp.Searvices;
using Microsoft.Extensions.Logging;

[Authorize(Roles = "Admin")]
[Route("api/[controller]")]
[ApiController]
public class AdminProductsController : ControllerBase
{
    private readonly IProductService _productService;
    private readonly ILogger<AdminProductsController> _logger;

    public AdminProductsController(IProductService productService, ILogger<AdminProductsController> logger)
    {
        _productService = productService;
        _logger = logger;
    }

    // GET: api/AdminProducts
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
    {
        _logger.LogInformation("Getting all products.");
        var products = await _productService.GetAllProductsAsync();
        _logger.LogInformation("Successfully retrieved {Count} products.", products.Count());
        return Ok(products);
    }

    // GET: api/AdminProducts/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Product>> GetProduct(int id)
    {
        _logger.LogInformation("Getting product with ID: {Id}", id);
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

    // POST: api/AdminProducts
    [HttpPost]
    public async Task<ActionResult<Product>> PostProduct([FromBody] ProductDto productDto)
    {
        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Invalid product data received.");
            return BadRequest(ModelState);
        }

        _logger.LogInformation("Creating new product.");
        var product = await _productService.CreateProductAsync(productDto);
        _logger.LogInformation("Successfully created product with ID: {Id}", product.Id);
        return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
    }

    // PUT: api/AdminProducts/5
    [HttpPut("{id}")]
    public async Task<IActionResult> PutProduct(int id, [FromBody] ProductDto productDto)
    {
        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Invalid product data received for update.");
            return BadRequest(ModelState);
        }

        _logger.LogInformation("Updating product with ID: {Id}", id);
        try
        {
            await _productService.UpdateProductAsync(id, productDto);
            _logger.LogInformation("Successfully updated product with ID: {Id}", id);
            return NoContent();
        }
        catch (KeyNotFoundException)
        {
            _logger.LogWarning("Product with ID: {Id} not found for update.", id);
            return NotFound();
        }
    }

    // DELETE: api/AdminProducts/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        _logger.LogInformation("Deleting product with ID: {Id}", id);
        try
        {
            await _productService.DeleteProductAsync(id);
            _logger.LogInformation("Successfully deleted product with ID: {Id}", id);
            return NoContent();
        }
        catch (KeyNotFoundException)
        {
            _logger.LogWarning("Product with ID: {Id} not found for deletion.", id);
            return NotFound();
        }
    }
}
