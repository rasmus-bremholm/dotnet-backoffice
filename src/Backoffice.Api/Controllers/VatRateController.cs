using Backoffice.Api.Models;
using Backoffice.Api.Repositories;
using Backoffice.Api.Dtos;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;
using System.Text.Json;

namespace Backoffice.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]

public class VatRatesController : ControllerBase
{
   private readonly IRepository<VatRate> _repository;
   private readonly IConnectionMultiplexer _redis;

   public VatRatesController(IRepository<VatRate> repository, IConnectionMultiplexer redis)
   {
      _repository = repository;
      _redis = redis;
   }

   [HttpGet]
   public async Task<ActionResult<IEnumerable<VatRateResponse>>> GetAllVatRates()
   {
      var db = _redis.GetDatabase();
      var cached = await db.StringGetAsync("vats:all");

      if (cached.HasValue)
      {
         var cachedResponse = JsonSerializer.Deserialize<List<VatRateResponse>>(cached.ToString());
         return Ok(cachedResponse);
      }

      var vatRates = await _repository.GetAllAsync();
      var response = vatRates.Select(v => new VatRateResponse
      {
         Id = v.Id,
         Rate = v.Rate,
         Label = v.Label,
      }).ToList();

      await db.StringSetAsync("vats:all", JsonSerializer.Serialize(response), TimeSpan.FromMinutes(5));

      return Ok(response);
   }

   [HttpGet("{id}")]
   public async Task<ActionResult<VatRateResponse>> GetVatRateById(int id)
   {
      var vatRate = await _repository.GetByIdAsync(id);
      if (vatRate == null)
      {
         return NotFound();
      }

      var response = new VatRateResponse
      {
         Id = vatRate.Id,
         Rate = vatRate.Rate,
         Label = vatRate.Label
      };

      return Ok(response);
   }

   [HttpPost]
   public async Task<ActionResult<VatRateResponse>> CreateVatRate([FromBody] CreateVatRateRequest request)
   {
      var vatRate = new VatRate
      {

         Rate = request.Rate,
         Label = request.Label,

      };

      await _repository.AddAsync(vatRate);
      await _repository.SaveChangesAsync();
      var db = _redis.GetDatabase();
      await db.KeyDeleteAsync("vats:all");

      var response = new VatRateResponse
      {
         Id = vatRate.Id,
         Rate = vatRate.Rate,
         Label = vatRate.Label,
      };

      return Ok(response);
   }

   [HttpDelete("{id}")]
   public async Task<IActionResult> DeleteVatRateById(int id)
   {
      var vatRate = await _repository.GetByIdAsync(id);
      if (vatRate == null)
      {
         return NotFound();
      }

      try
      {
         _repository.Delete(vatRate);
         await _repository.SaveChangesAsync();
         var db = _redis.GetDatabase();
         await db.KeyDeleteAsync("vats:all");
         return NoContent();
      }
      catch (Exception ex)
      {
         return BadRequest(ex.Message);
      }
   }

   [HttpPut("{id}")]
   public async Task<IActionResult> UpdateVatRateById(int id, [FromBody] CreateVatRateRequest request)
   {
      var vatRate = await _repository.GetByIdAsync(id);

      if (vatRate == null)
      {
         return NotFound();
      }

      vatRate.Rate = request.Rate;
      vatRate.Label = request.Label;

      await _repository.SaveChangesAsync();
      var db = _redis.GetDatabase();
      await db.KeyDeleteAsync("vats:all");
      return NoContent();
   }

}
