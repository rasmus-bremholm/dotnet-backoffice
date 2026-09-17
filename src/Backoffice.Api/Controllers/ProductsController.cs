using Backoffice.Api.Data;
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
   public async Task<ActionResult<IEnumerable<Product>>> GetAllProducts()
   {
      return Ok(await _context.Products.ToListAsync());
   }
}
