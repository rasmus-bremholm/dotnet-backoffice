namespace Backoffice.Api.Dtos;

public class VatRateResponse
{
   public int Id { get; set; }
   public decimal Rate { get; set; }
   public string Label { get; set; } = String.Empty;
}

public class CreateVatRateRequest
{
   public decimal Rate { get; set; }
   public string Label { get; set; } = String.Empty;
}
