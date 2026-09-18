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
}
