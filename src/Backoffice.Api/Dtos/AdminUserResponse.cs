using Backoffice.Api.Models;

public class CreateAdminUserRequest
{
   public string Name { get; set; } = string.Empty;
   public string Email { get; set; } = string.Empty;
   public string Password { get; set; } = string.Empty;
   public AdminRole Role { get; set; }
}

public class AdminUserResponse
{
   public int Id { get; set; }
   public string Name { get; set; } = string.Empty;
   public string Email { get; set; } = string.Empty;
   public AdminRole Role { get; set; }
}


public class LoginRequest
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
