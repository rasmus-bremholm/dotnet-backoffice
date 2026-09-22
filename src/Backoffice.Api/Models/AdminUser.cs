

namespace Backoffice.Api.Models;

public class AdminUser
{
   public int Id { get; set; }
   public string Email { get; set; } = String.Empty;
   public string Name { get; set; } = String.Empty;
   public string PasswordHash { get; set; } = String.Empty;
   public AdminRole Role { get; set; }
   public DateTime CreatedAt { get; set; }
}
