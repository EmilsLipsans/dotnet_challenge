using dotnet_challenge;

namespace dotnet_challenge_test;

[TestClass]
public class OrderServiceTests
{
    List<Tuple<int, string, decimal>> _testingKits = new List<Tuple<int, string, decimal>>
    {
        Tuple.Create(1, "Basic Kit", 98.99m),
        Tuple.Create(2, "Advanced Kit", 149.99m),
        Tuple.Create(3, "Premium Kit", 199.99m)
    };
    
    // Using https://learn.microsoft.com/en-us/dotnet/core/testing/unit-testing-best-practices as a guide i have implemented unit tests
    [TestMethod]
    public void PlaceOrder_ValidInput_ReturnsOrder()
    {
        // Arrange 
        OrderService orderService = new OrderService();
        orderService.ImportDnaTestKitList(_testingKits);
        
        int customerId = 1;
        
        // order delivery date is in the future
        DateTime expectedDeliveryDate = DateTime.Now.AddDays(5);
        
        // amount is from 1 to 999
        int desiredAmount = 5;
        
        // kitId exists in the dictionary
        int kitId = 1;
        
        // Act
        Order order = orderService.PlaceOrder(customerId: customerId, 
            expectedDeliveryDate: expectedDeliveryDate,
            desiredAmount: desiredAmount,
            kitId: kitId);
        
        // Assert that order has been created with all the passed in data
        Assert.IsNotNull(order);
        Assert.AreEqual(customerId, order.CustomerId);
        Assert.AreEqual(expectedDeliveryDate, order.ExpectedDeliveryDate);
        Assert.AreEqual(desiredAmount, order.Amount);
        Assert.AreEqual(kitId, order.KitId);
    }
    
    [TestMethod]
    public void CalculateTotalPrice_ValidInput_FivePercentDiscount_ReturnsOrderPlacedSuccessfully()
    {
        // ... Your test code here ...
    }
    [TestMethod]
    public void PlaceOrder_ValidInput_FifteenPercentDiscount_ReturnsOrderPlacedSuccessfully()
    {
        // ... Your test code here ...
    }
}