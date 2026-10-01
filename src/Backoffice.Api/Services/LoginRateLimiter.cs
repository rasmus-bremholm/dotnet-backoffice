namespace Backoffice.Api.Services;

using StackExchange.Redis;

public class LoginRateLimiter : ILoginRateLimiter
{
   private const int MaxAttempts = 5;

   private static readonly TimeSpan Window = TimeSpan.FromMinutes(5);

   private readonly IConnectionMultiplexer _connectionMultiplexer;

   public LoginRateLimiter(IConnectionMultiplexer connectionMultiplexer)
   {
      _connectionMultiplexer = connectionMultiplexer;
   }

   public async Task<bool> TryAttemptAsync(string clientId)
   {
      return;
   }

   public async Task ResetAsync(string clientId)
   {

   }
}
