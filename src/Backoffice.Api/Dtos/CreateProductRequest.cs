namespace Backoffice.Api.Dtos;

public class CreateProductRequest
{
   public string Sku { get; set; } = string.Empty;
   public string Name { get; set; } = string.Empty;
   public string Description { get; set; } = string.Empty;
   public string Shelf { get; set; } = string.Empty;

   public int CategoryId { get; set; }
   public int VatRateId { get; set; }
   public int? BrandId { get; set; }

   public decimal SalesPriceExcludingVat { get; set; }
   public decimal CostPrice { get; set; }

   public float Weight { get; set; }
   public float? Width { get; set; }
   public float? Length { get; set; }
   public float? Height { get; set; }
}
