using Backoffice.Api.Data;
using Backoffice.Api.Dtos;
using Backoffice.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backoffice.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]

public class ProductsController : ControllerBase
{
   private readonly BackofficeDbContext _context;

   public ProductsController(BackofficeDbContext context)
   {
      _context = context;
   }

   [HttpGet]
   public async Task<ActionResult<IEnumerable<ProductResponse>>> GetAllProducts()
   {
      var products = await _context.Products.Select(p => new ProductResponse
      {
         Id = p.Id,
         Sku = p.Sku,
         Name = p.Name,
         Description = p.Description,
         SalesPriceExcludingVat = p.SalesPriceExcludingVat
      }).ToListAsync();
      return Ok(products);
   }

   [HttpGet("{id}")]
   public async Task<ActionResult<ProductResponse>> GetProductById(int id)
   {
      var product = await _context.Products.Where(p => p.Id == id).Select(p => new ProductResponse
      {
         Id = p.Id,
         Sku = p.Sku,
         Name = p.Name,
         Description = p.Description,
         SalesPriceExcludingVat = p.SalesPriceExcludingVat
      }).FirstOrDefaultAsync();
      if (product == null)
      {
         return NotFound();
      }
      return Ok(product);
   }

   [HttpPost]
   public async Task<ActionResult<ProductResponse>> CreateProduct([FromBody] CreateProductRequest request)
   {
      var product = new Product
      {
         Sku = request.Sku,
         Name = request.Name,
         Description = request.Description,
         Shelf = request.Shelf,
         CategoryId = request.CategoryId,
         VatRateId = request.VatRateId,
         BrandId = request.BrandId,
         SalesPriceExcludingVat = request.SalesPriceExcludingVat,
         CostPrice = request.CostPrice,
         Weight = request.Weight,
         Width = request.Width,
         Length = request.Length,
         Height = request.Height
      };

      _context.Products.Add(product);
      await _context.SaveChangesAsync();

      var response = new ProductResponse
      {
         Id = product.Id,
         Sku = product.Sku,
         Name = product.Name,
         Description = product.Description,
         SalesPriceExcludingVat = product.SalesPriceExcludingVat
      };

      return Ok(response);
   }

   [HttpDelete("{id}")]
   public async Task<IActionResult> DeleteProductById(int id)
   {
      var product = await _context.Products.FindAsync(id);
      if (product == null)
      {
         return NotFound();
      }

      try
      {
         _context.Products.Remove(product);
         await _context.SaveChangesAsync();
         return NoContent();
      }
      catch (Exception ex)
      {
         return BadRequest(ex.Message);
      }
   }


}
