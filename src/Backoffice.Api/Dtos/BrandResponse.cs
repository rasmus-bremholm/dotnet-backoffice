namespace Backoffice.Api.Dtos;

public class BrandResponse
{
   public int Id { get; set; }
   public string Name { get; set; } = string.Empty;

   public string Description { get; set; } = string.Empty;

   public string Logo { get; set; } = String.Empty;
}

public class CreateBrandRequest
{
   public string Name { get; set; } = string.Empty;

   public string Description { get; set; } = string.Empty;

   public string Logo { get; set; } = String.Empty;
}
