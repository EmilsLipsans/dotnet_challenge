using dotnet_challenge;

namespace dotnet_challenge_test;

[TestClass]
public class OrderServiceTests
{
    // test DnaTestKit data the user of the api would import before placing orders
    List<Tuple<int, string, decimal>> _testingKits = new List<Tuple<int, string, decimal>>
    {
        Tuple.Create(1, "Basic Kit", 98.99m),
        Tuple.Create(2, "Basic Kit+", 100.0m),
        Tuple.Create(3, "Advanced Kit", 199.99m)
    };

    // Using https://learn.microsoft.com/en-us/dotnet/core/testing/unit-testing-best-practices as a guide i have implemented unit tests
    [TestMethod]
    public void PlaceOrder_ValidInput_NoDiscount_ReturnsOrder()
    {
        // Arrange 
        OrderService orderService = new OrderService();
        orderService.ImportDnaTestKitList(_testingKits);

        int customerId = 1;

        // valid order delivery date is in the future
        DateTime expectedDeliveryDate = DateTime.Now.AddDays(5);

        // valid amount is from 1 to 999. order where amount is under 10 gets no discount
        int desiredAmount = 5;

        // valid kitId exists in the dictionary
        int kitId = 1;

        // kit price of kit where the kitId = 1
        decimal kitPrice = _testingKits[kitId - 1].Item3;

        // expected total price calculation with no discounts 
        decimal expectedTotalPrice = desiredAmount * kitPrice;

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
        Assert.AreEqual(expectedTotalPrice, order.TotalPrice);
    }

    [TestMethod]
    public void PlaceOrder_ValidInput_5PercentDiscount_ReturnsOrder()
    {
        // Arrange 
        OrderService orderService = new OrderService();
        orderService.ImportDnaTestKitList(_testingKits);

        int customerId = 1;

        // valid order delivery date is in the future
        DateTime expectedDeliveryDate = DateTime.Now.AddDays(5);

        // valid amount is from 1 to 999. order where amount is 10-49  gets 5% discount
        int desiredAmount = 10;

        // valid kitId exists in the dictionary
        int kitId = 2;

        // kit price of kit where the kitId = 2
        decimal kitPrice = _testingKits[kitId - 1].Item3;

        // expected total price calculation with 5% discount 
        decimal expectedTotalPrice = (desiredAmount * kitPrice) * 0.95m;

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
        Assert.AreEqual(expectedTotalPrice, order.TotalPrice);
    }

    [TestMethod]
    public void PlaceOrder_ValidInput_15PercentDiscount_ReturnsOrder()
    {
        // Arrange 
        OrderService orderService = new OrderService();
        orderService.ImportDnaTestKitList(_testingKits);

        int customerId = 1;

        // valid order delivery date is in the future
        DateTime expectedDeliveryDate = DateTime.Now.AddDays(5);

        // valid amount is from 1 to 999. order where amount is 50 or more gets 15% discount
        int desiredAmount = 50;

        // valid kitId exists in the dictionary
        int kitId = 2;

        // kit price of kit where the kitId = 2
        decimal kitPrice = _testingKits[kitId - 1].Item3;

        // expected total price calculation with 15% discount 
        decimal expectedTotalPrice = (desiredAmount * kitPrice) * 0.85m;

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
        Assert.AreEqual(expectedTotalPrice, order.TotalPrice);
    }

    [TestMethod]
    public void GetCustomerOrders_ValidInput_HasOrders_ReturnsOrders()
    {
        // Arrange 
        OrderService orderService = new OrderService();
        orderService.ImportDnaTestKitList(_testingKits);

        int customerId = 1;

        // valid order delivery date is in the future
        DateTime expectedDeliveryDate = DateTime.Now.AddDays(5);

        // valid amount is from 1 to 999. order where amount is 50 or more gets 15% discount
        int desiredAmount = 50;

        // valid kitId exists in the dictionary
        int kitId = 2;

        // kit price of kit where the kitId = 2
        decimal kitPrice = _testingKits[kitId - 1].Item3;

        // expected total price calculation with 15% discount 
        decimal expectedTotalPrice = (desiredAmount * kitPrice) * 0.85m;

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
        Assert.AreEqual(expectedTotalPrice, order.TotalPrice);
    }
}