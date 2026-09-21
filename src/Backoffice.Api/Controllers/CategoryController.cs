using Backoffice.Api.Models;
using Backoffice.Api.Repositories;
using Backoffice.Api.Dtos;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;
using System.Text.Json;

namespace Backoffice.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]

public class CategoriesController : ControllerBase
{
   private readonly IRepository<Category> _repository;
   private readonly IConnectionMultiplexer _redis;

   public CategoriesController(IRepository<Category> repository, IConnectionMultiplexer redis)
   {
      _repository = repository;
      _redis = redis;
   }

   [HttpGet]
   public async Task<ActionResult<IEnumerable<CategoryResponse>>> GetAllCategories()
   {
      var db = _redis.GetDatabase();
      var cached = await db.StringGetAsync("categories:all");

      if (cached.HasValue)
      {
         var cachedResponse = JsonSerializer.Deserialize<List<CategoryResponse>>(cached.ToString());
         return Ok(cachedResponse);
      }

      var categories = await _repository.GetAllAsync();
      var response = categories.Select(c => new CategoryResponse
      {
         Id = c.Id,
         Name = c.Name
      }).ToList();

      await db.StringSetAsync("categories:all", JsonSerializer.Serialize(response), TimeSpan.FromMinutes(5));

      return Ok(response);
   }

   [HttpGet("{id}")]
   public async Task<ActionResult<CategoryResponse>> GetCategoryById(int id)
   {
      var category = await _repository.GetByIdAsync(id);
      if (category == null)
      {
         return NotFound();
      }

      var response = new CategoryResponse
      {
         Id = category.Id,
         Name = category.Name,
      };

      return Ok(response);
   }

   [HttpPost]
   public async Task<ActionResult<CategoryResponse>> CreateCategory([FromBody] CreateCategoryRequest request)
   {
      var category = new Category
      {

         Name = request.Name,
      };

      await _repository.AddAsync(category);
      await _repository.SaveChangesAsync();
      var db = _redis.GetDatabase();
      await db.KeyDeleteAsync("categories:all");

      var response = new CategoryResponse
      {
         Id = category.Id,
         Name = category.Name
      };

      return Ok(response);
   }

   [HttpDelete("{id}")]
   public async Task<IActionResult> DeleteCategoryById(int id)
   {
      var category = await _repository.GetByIdAsync(id);
      if (category == null)
      {
         return NotFound();
      }

      try
      {
         _repository.Delete(category);
         await _repository.SaveChangesAsync();
         var db = _redis.GetDatabase();
         await db.KeyDeleteAsync("categories:all");
         return NoContent();
      }
      catch (Exception ex)
      {
         return BadRequest(ex.Message);
      }
   }

   [HttpPut("{id}")]
   public async Task<IActionResult> UpdateCategoryById(int id, [FromBody] CreateCategoryRequest request)
   {
      var category = await _repository.GetByIdAsync(id);

      if (category == null)
      {
         return NotFound();
      }

      category.Name = request.Name;

      await _repository.SaveChangesAsync();
      var db = _redis.GetDatabase();
      await db.KeyDeleteAsync("categories:all");
      return NoContent();
   }

}
