using Backoffice.Api.Models;
using Backoffice.Api.Repositories;
using Backoffice.Api.Dtos;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;
using System.Text.Json;

namespace Backoffice.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]

public class BrandsController : ControllerBase
{
   private readonly IRepository<Brand> _repository;
   private readonly IConnectionMultiplexer _redis;

   public BrandsController(IRepository<Brand> repository, IConnectionMultiplexer redis)
   {
      _repository = repository;
      _redis = redis;
   }

   [HttpGet]
   public async Task<ActionResult<IEnumerable<BrandResponse>>> GetAllBrands()
   {
      var db = _redis.GetDatabase();
      var cached = await db.StringGetAsync("brands:all");

      if (cached.HasValue)
      {
         var cachedResponse = JsonSerializer.Deserialize<List<BrandResponse>>(cached.ToString());
         return Ok(cachedResponse);
      }

      var brands = await _repository.GetAllAsync();
      var response = brands.Select(b => new BrandResponse
      {
         Id = b.Id,
         Name = b.Name,
         Description = b.Description,
         Logo = b.Logo

      }).ToList();

      await db.StringSetAsync("brands:all", JsonSerializer.Serialize(response), TimeSpan.FromMinutes(5));

      return Ok(response);
   }

   [HttpGet("{id}")]
   public async Task<ActionResult<BrandResponse>> GetBrandById(int id)
   {
      var brand = await _repository.GetByIdAsync(id);
      if (brand == null)
      {
         return NotFound();
      }

      var response = new BrandResponse
      {
         Id = brand.Id,
         Name = brand.Name,
         Description = brand.Description,
         Logo = brand.Logo
      };

      return Ok(response);
   }

   [HttpPost]
   public async Task<ActionResult<BrandResponse>> CreateBrand([FromBody] CreateBrandRequest request)
   {
      var brand = new Brand
      {

         Name = request.Name,
         Description = request.Description,
         Logo = request.Logo
      };

      await _repository.AddAsync(brand);
      await _repository.SaveChangesAsync();
      var db = _redis.GetDatabase();
      await db.KeyDeleteAsync("brands:all");

      var response = new BrandResponse
      {
         Id = brand.Id,
         Name = brand.Name,
         Description = brand.Description,
         Logo = brand.Logo
      };

      return Ok(response);
   }

   [HttpDelete("{id}")]
   public async Task<IActionResult> DeleteBrandById(int id)
   {
      var brand = await _repository.GetByIdAsync(id);
      if (brand == null)
      {
         return NotFound();
      }

      try
      {
         _repository.Delete(brand);
         await _repository.SaveChangesAsync();
         var db = _redis.GetDatabase();
         await db.KeyDeleteAsync("brands:all");
         return NoContent();
      }
      catch (Exception ex)
      {
         return BadRequest(ex.Message);
      }
   }

   [HttpPut("{id}")]
   public async Task<IActionResult> UpdateBrandById(int id, [FromBody] CreateBrandRequest request)
   {
      var brand = await _repository.GetByIdAsync(id);

      if (brand == null)
      {
         return NotFound();
      }

      brand.Name = request.Name;
      brand.Description = request.Description;
      brand.Logo = request.Logo;

      await _repository.SaveChangesAsync();
      var db = _redis.GetDatabase();
      await db.KeyDeleteAsync("brands:all");
      return NoContent();
   }

}
