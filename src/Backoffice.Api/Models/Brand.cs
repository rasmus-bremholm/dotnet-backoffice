namespace Backoffice.Api.Models;

public class Brand
{
   public int Id { get; set; }
   public string Name { get; set; } = String.Empty;
   public string Description { get; set; } = String.Empty;
   public string Logo { get; set; } = String.Empty;
   public ICollection<Product> Products { get; set; } = new List<Product>();
}
