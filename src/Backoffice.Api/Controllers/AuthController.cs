using Backoffice.Api.Models;
using Backoffice.Api.Repositories;
using Microsoft.AspNetCore.Mvc;
using Backoffice.Api.Dtos;
using Microsoft.AspNetCore.Identity;

namespace Backoffice.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]

public class AuthController : ControllerBase
{
   private readonly IRepository<AdminUser> _repository;
   private readonly IPasswordHasher<AdminUser> _passwordHasher;

   public AuthController(IRepository<AdminUser> repository, IPasswordHasher<AdminUser> passwordHasher)
   {
      _repository = repository;
      _passwordHasher = passwordHasher;
   }

   [HttpPost]
   public async Task<ActionResult<AdminUserResponse>> RegisterAdmin([FromBody] CreateAdminUserRequest request)
   {
      var user = new AdminUser
      {
         Name = request.Name,
         Email = request.Email,
         Role = request.Role,
         CreatedAt = DateTime.UtcNow
      };

      user.PasswordHash = _passwordHasher.HashPassword(user, request.Password);

      await _repository.AddAsync(user);
      await _repository.SaveChangesAsync();

      var response = new AdminUserResponse
      {
         Id = user.Id,
         Name = user.Name,
         Email = user.Email,
         Role = user.Role,
      };

      return Ok(response);
   }

   [HttpPost]
   public async Task<ActionResult> LoginUser([FromBody] LoginRequest request)
   {
      var user = await _repository.FindAsync(u => u.Email == request.Email);

      if (user == null)
      {
         return NotFound();
      }

      var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, request.Password);
      if (result != PasswordVerificationResult.Success)
      {
         return Unauthorized("Invalid email or password");
      }

      return Ok(result);
   }
}
