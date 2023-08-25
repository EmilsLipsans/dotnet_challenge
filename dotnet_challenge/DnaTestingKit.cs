namespace dotnet_challenge;

public class DnaTestingKit
{
    public DnaTestingKit(string kitVariant, decimal basePrice, int kitId)
    {
        KitVariant = kitVariant;
        BasePrice = basePrice;
        KitId = kitId;
    }

    public int KitId { get; set; }
    public string KitVariant { get; set; }
    public decimal BasePrice { get; set; }
}