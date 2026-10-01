
namespace Backoffice.Api.Services;

public interface ILoginRateLimiter
{
   Task<bool> TryAttemptAsync(string clientId);
   Task ResetAsync(string clientId);

}
