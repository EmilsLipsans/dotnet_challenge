namespace dotnet_challenge;

public interface IOrderService
{
    Order PlaceOrder(int customerId, DateTime expectedDeliveryDate, int desiredAmount, int kitId);
    List<Order> GetCustomerOrders(int customerId);
}