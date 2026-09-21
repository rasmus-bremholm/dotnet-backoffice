using Backoffice.Api.Dtos;
using Backoffice.Api.Models;
using Backoffice.Api.Repositories;
using Microsoft.AspNetCore.Mvc;


namespace Backoffice.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]

public class ProductsController : ControllerBase
{
   private readonly IRepository<Product> _repository;

   public ProductsController(IRepository<Product> repository)
   {
      _repository = repository;
   }

   [HttpGet]
   public async Task<ActionResult<IEnumerable<ProductResponse>>> GetAllProducts()
   {
      var products = await _repository.GetAllAsync();
      var response = products.Select(p => new ProductResponse
      {
         Id = p.Id,
         Sku = p.Sku,
         Name = p.Name,
         Description = p.Description,
         SalesPriceExcludingVat = p.SalesPriceExcludingVat
      });

      return Ok(response);
   }

   [HttpGet("{id}")]
   public async Task<ActionResult<ProductResponse>> GetProductById(int id)
   {
      var product = await _repository.GetByIdAsync(id);

      if (product == null)
      {
         return NotFound();
      }

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

      await _repository.AddAsync(product);
      await _repository.SaveChangesAsync();

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
      var product = await _repository.GetByIdAsync(id);
      if (product == null)
      {
         return NotFound();
      }

      try
      {
         _repository.Delete(product);
         await _repository.SaveChangesAsync();
         return NoContent();
      }
      catch (Exception ex)
      {
         return BadRequest(ex.Message);
      }
   }


   [HttpPut("{id}")]
   public async Task<IActionResult> UpdateProductById(int id, [FromBody] CreateProductRequest request)
   {
      var product = await _repository.GetByIdAsync(id);

      if (product == null)
      {
         return NotFound();
      }

      product.Sku = request.Sku;
      product.Name = request.Name;
      product.Description = request.Description;
      product.Shelf = request.Shelf;
      product.CategoryId = request.CategoryId;
      product.VatRateId = request.VatRateId;
      product.BrandId = request.BrandId;
      product.SalesPriceExcludingVat = request.SalesPriceExcludingVat;
      product.CostPrice = request.CostPrice;
      product.Weight = request.Weight;
      product.Width = request.Width;
      product.Length = request.Length;
      product.Height = request.Height;

      await _repository.SaveChangesAsync();
      return NoContent();
   }

}
