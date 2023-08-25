namespace dotnet_challenge;

public class Order
{
    public int OrderId { get; set; }
    public int CustomerId { get; set; }
    public DateTime ExpectedDeliveryDate { get; set; }
    public int KitId { get; set; }
    public int Amount { get; set; }
    public decimal TotalPrice { get; set; }
}