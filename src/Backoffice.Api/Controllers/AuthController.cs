using Backoffice.Api.Models;
using Backoffice.Api.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Backoffice.Api.Services;
namespace Backoffice.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]

public class AuthController : ControllerBase
{
   private readonly IRepository<AdminUser> _repository;
   private readonly IPasswordHasher<AdminUser> _passwordHasher;
   private readonly ILoginRateLimiter _loginRateLimiter;

   public AuthController(ILoginRateLimiter loginRateLimiter, IRepository<AdminUser> repository, IPasswordHasher<AdminUser> passwordHasher)
   {
      _repository = repository;
      _passwordHasher = passwordHasher;
      _loginRateLimiter = loginRateLimiter;
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

      var existingUser = await _repository.FindAsync(u => u.Email == request.Email);

      if (existingUser != null)
      {
         return Conflict("A user with this email already exists.");
      }

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

   [HttpPost("login")]
   public async Task<ActionResult> LoginUser([FromBody] LoginRequest request)
   {
      var clientIp = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";

      if (!await _loginRateLimiter.TryAttemptAsync(clientIp))
      {
         return StatusCode(429, "Too many login attempts. Smell you later");
      }

      var user = await _repository.FindAsync(u => u.Email == request.Email);

      // No such user
      if (user == null)
      {
         return Unauthorized("Invalid email or password");
      }

      var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, request.Password);


      if (result == PasswordVerificationResult.SuccessRehashNeeded)
      {
         user.PasswordHash = _passwordHasher.HashPassword(user, request.Password);
         await _repository.SaveChangesAsync();
      }
      else if (result != PasswordVerificationResult.Success)
      {
         // Password is rong
         return Unauthorized("Invalid email or password");
      }

      // Deletes the attempts key on sucessful login.
      await _loginRateLimiter.ResetAsync(clientIp);
      // Login sucessful! Woop
      HttpContext.Session.SetInt32("AdminUserId", user.Id);

      var response = new AdminUserResponse
      {
         Id = user.Id,
         Name = user.Name,
         Email = user.Email,
         Role = user.Role,

      };
      return Ok(response);
   }

   [HttpGet("me")]
   public async Task<ActionResult<AdminUserResponse>> GetMe()
   {
      int? userId = HttpContext.Session.GetInt32("AdminUserId");

      if (userId == null)
      {
         return Unauthorized();
      }

      var user = await _repository.FindAsync(u => u.Id == userId);

      if (user == null)
      {
         HttpContext.Session.Clear();
         return Unauthorized();
      }

      var response = new AdminUserResponse
      {
         Id = user.Id,
         Name = user.Name,
         Email = user.Email,
         Role = user.Role,

      };

      return Ok(response);
   }
}
