namespace Backoffice.Api.Dtos;

public class ProductResponse
{
   public int Id { get; set; }
   public string Sku { get; set; } = string.Empty;
   public string Name { get; set; } = string.Empty;
   public string Description { get; set; } = string.Empty;
   public decimal SalesPriceExcludingVat { get; set; }
}
