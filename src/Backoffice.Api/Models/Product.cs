
namespace Backoffice.Api.Models;

public class Product
{
   public int Id { get; set; }
   public string Sku { get; set; } = String.Empty;
   public string Shelf { get; set; } = String.Empty;
   public int CategoryId { get; set; }
   public Category Category { get; set; } = null!;

   public decimal SalesPriceExcludingVat { get; set; }
   public decimal CostPrice { get; set; }
   public int VatRateId { get; set; }
   public VatRate VatRate { get; set; } = null!;

   public float Weight { get; set; }

   public float? Width { get; set; }

   public float? Length { get; set; }

   public float? Height { get; set; }

   public string Name { get; set; } = String.Empty;

   public string Description { get; set; } = String.Empty;

}
