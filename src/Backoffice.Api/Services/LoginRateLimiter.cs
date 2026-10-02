namespace Backoffice.Api.Services;

using StackExchange.Redis;

public class LoginRateLimiter : ILoginRateLimiter
{
   private const int MaxAttempts = 5;

   private static readonly TimeSpan Window = TimeSpan.FromMinutes(5);

   private readonly IConnectionMultiplexer _connectionMultiplexer;
   private static string BuildKey(string clientId) => $"login-attempts:{clientId}";

   public LoginRateLimiter(IConnectionMultiplexer connectionMultiplexer)
   {
      _connectionMultiplexer = connectionMultiplexer;

   }

   public async Task<bool> TryAttemptAsync(string clientId)
   {
      var db = _connectionMultiplexer.GetDatabase();
      var key = BuildKey(clientId);
      var attempts = await db.StringIncrementAsync(key);
      await db.KeyExpireAsync(key, Window, ExpireWhen.HasNoExpiry);

      return attempts <= MaxAttempts;
   }

   public async Task ResetAsync(string clientId)
   {
      var db = _connectionMultiplexer.GetDatabase();
      var key = BuildKey(clientId);
      await db.KeyDeleteAsync(key);
   }
}
