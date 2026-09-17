namespace Backoffice.Api.Models;

public class VatRate
{
   public int Id { get; set; }
   public decimal Rate { get; set; }
   public string Label { get; set; } = String.Empty;
   public ICollection<Product> Products { get; set; } = new List<Product>();
}
